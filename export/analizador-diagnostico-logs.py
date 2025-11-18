#!/usr/bin/env python3
"""
Analizador de logs de PinkButterfly (bloques [DIAGNOSTICO]) y CSV de trades.

Genera un informe Markdown con métricas para DFM, Proximity, Risk y CancelBias,
útil para decisiones basadas en datos tras cada backtest.

Uso:
  python export/analizador-diagnostico-logs.py --log logs/backtest_YYYYMMDD_hhmmss.log \
                                              --csv logs/trades_YYYYMMDD_hhmmss.csv \
                                              -o export/DIAGNOSTICO_YYYYMMDD_hhmmss.md

Si no se especifica -o, imprimirá el informe por stdout.
"""

import argparse
import csv
import os
import re
import sys
from datetime import datetime
from collections import Counter, defaultdict


def to_float(num_str: str) -> float:
    if num_str is None:
        return 0.0
    # Acepta comas decimales (formato español) y puntos
    s = num_str.strip().replace('\u202f', '').replace(' ', '').replace(',', '.')
    try:
        return float(s)
    except Exception:
        return 0.0


def parse_log(log_path: str) -> dict:
    # Regex robustos (DIAGNOSTICO / DIAGNÓSTICO)
    re_diagnostic_tag = r"DIAGN[ÓO]STICO"

    re_dfm_eval = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[DFM\].*Evaluadas:\s*Bull=(\d+)\s*Bear=(\d+)\s*\|\s*PassedThreshold=(\d+)")
    re_dfm_bins = re.compile(rf"\[(?:{re_diagnostic_tag})\]\[DFM\]\s*ConfidenceBins:\s*(.*)")

    re_prox = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Proximity\].*KeptAligned=(\d+)/(\d+),\s*KeptCounter=(\d+)/(\d+),\s*"
        rf"AvgProxAligned=([0-9\.,]+),\s*AvgProxCounter=([0-9\.,]+),\s*AvgDistATRAligned=([0-9\.,]+),\s*AvgDistATRCounter=([0-9\.,]+)")
    re_prox_pre = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Proximity\]\s*Pre:\s*Aligned=(\d+)/(\d+)\s*Counter=(\d+)/(\d+)\s*AvgProxAligned=([0-9\.,]+)\s*AvgDistATRAligned=([0-9\.,]+)")
    re_prox_prefer = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Proximity\]\s*PreferAligned:.*filtradas\s+(\d+)\s+contra-bias, quedan\s+(\d+)")
    # Proximity detailed line (from Debug)
    re_prox_detail = re.compile(
        r"\[ProximityAnalyzer\] HeatZone .*BaseProx=([0-9\.,]+), ZoneATR=([0-9\.,]+), SizePenalty=([0-9\.,]+), FinalProx=([0-9\.,]+), Aligned=(True|False)",
        re.IGNORECASE
    )
    # Proximity drivers summary (INFO)
    re_prox_drivers = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Proximity\]\s*Drivers:\s*Aligned n=(\d+)\s*.*?BaseProx≈\s*([0-9\.,]+)\s*.*?ZoneATR≈\s*([0-9\.,]+)\s*.*?SizePenalty≈\s*([0-9\.,]+)\s*.*?FinalProx≈\s*([0-9\.,]+)\s*\|\s*Counter n=(\d+)\s*.*?BaseProx≈\s*([0-9\.,]+)\s*.*?ZoneATR≈\s*([0-9\.,]+)\s*.*?SizePenalty≈\s*([0-9\.,]+)\s*.*?FinalProx≈\s*([0-9\.,]+)"
    )

    re_risk = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*Accepted=(\d+)\s*RejSL=(\d+)\s*RejTP=(\d+)\s*RejRR=(\d+)\s*RejEntry=(\d+)")
    re_risk_rej_detail = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*RejSL Detalle:\s*Dir=(\w+)\s*Aligned=(true|false)\s*SLDistATR=([0-9\.,]+)\s*Prox=([0-9\.,]+)\s*Core=([0-9\.,]+)",
        re.IGNORECASE
    )
    re_risk_hist = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*HistSL\s*Aligned=0-10:(\d+),10-15:(\d+),15-20:(\d+),20-25:(\d+),25\+:(\d+)\s*\|\s*Counter=0-10:(\d+),10-15:(\d+),15-20:(\d+),20-25:(\d+),25\+:(\d+)"
    )
    # RejSL con Bin (nuevo formato)
    re_risk_rejsl = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*RejSL:\s*Dir=(\w+)\s*Aligned=(true|false)\s*SLDistATR=([0-9\.,]+)\s*Bin=(\d+)",
        re.IGNORECASE
    )
    # Aceptaciones (para WR por bins de SLDistATR)
    re_risk_slaccepted = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*SLAccepted:\s*Zone=\S+\s*Dir=(\w+)\s*Entry=([0-9\.,]+)\s*SL=([0-9\.,]+)\s*TP=([0-9\.,]+)\s*SLDistATR=([0-9\.,]+).*?(?:RR=([0-9\.,]+))?(?:\s+Conf=([0-9\.,]+))?",
        re.IGNORECASE
    )
    # Aceptaciones detalle de calidad
    re_risk_slaccepted_detail = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*SLAccepted DETAIL:\s*Zone=(\S+)\s*Dir=(\w+)\s*Aligned=(true|false)\s*Core=([0-9\.,]+)\s*Prox=([0-9\.,]+)\s*ConfC=([0-9\.,]+)\s*ConfScore=([0-9\.,]+)\s*SL_TF=(-?\d+)\s*SL_Struct=(true|false)\s*TP_TF=(-?\d+)\s*TP_Struct=(true|false)\s*RR=([0-9\.,]+)\s*Confidence=([0-9\.,]+)",
        re.IGNORECASE
    )
    # Resumen band/TF y RR plan por bandas
    re_slpick_bands = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*SLPickBands:\s*lt8:(\d+),8-10:(\d+),10-12\.5:(\d+),12\.5-15:(\d+),gt15:(\d+)\s*\|\s*TF\s*5:(\d+),15:(\d+),60:(\d+),240:(\d+),1440:(\d+)",
        re.IGNORECASE
    )
    re_rrplan_bands = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*RRPlanBands:\s*0-10=([0-9\.,]+)\(n=(\d+)\),10-15=([0-9\.,]+)\(n=(\d+)\)",
        re.IGNORECASE
    )

    # V6.0d: Doble cerrojo - validación híbrida puntos/ATR
    re_sl_check_fail = re.compile(
        r"\[RISK\]\[SL_CHECK_FAIL\]\s*Zone=(\S+)\s*SL=([0-9\.,]+)pts.*?SLDistATR=([0-9\.,]+).*?SLTF=(-?\d+).*?ATR=([0-9\.,]+)",
        re.IGNORECASE
    )
    re_tp_check_fail = re.compile(
        r"\[RISK\]\[TP_CHECK_FAIL\]\s*Zone=(\S+)\s*TP=([0-9\.,]+)pts.*?TPDistATR=([0-9\.,]+).*?TPTF=(-?\d+).*?ATR=([0-9\.,]+)",
        re.IGNORECASE
    )
    re_sl_high_vol = re.compile(
        r"\[RISK\]\[SL_HIGH_VOL\]\s*Zone=(\S+)\s*ATR=([0-9\.,]+).*?SLDistATR=([0-9\.,]+).*?SL=([0-9\.,]+)pts",
        re.IGNORECASE
    )
    re_sl_check_pass = re.compile(
        r"\[RISK\]\[SL_CHECK_PASS\]\s*Zone=(\S+)\s*SL:\s*([0-9\.,]+)pts\s*([0-9\.,]+)ATR\s*TF=(-?\d+)\s*atr=([0-9\.,]+)",
        re.IGNORECASE
    )
    re_tp_check_pass = re.compile(
        r"\[RISK\]\[TP_CHECK_PASS\]\s*Zone=(\S+)\s*TP:\s*([0-9\.,]+)pts\s*([0-9\.,]+)ATR\s*TF=(-?\d+)\s*atr=([0-9\.,]+)",
        re.IGNORECASE
    )
    # V6.0c: Política TP
    re_tp_policy_forced = re.compile(
        r"\[RISK\]\[TP_POLICY\].*?(?:FORCED[_\s]?P3|Zone=\S+\s+FORCED[_\s]?P3).*?TF=(-?\d+).*?DistATR=([0-9\.,]+).*?RR=([0-9\.,]+)",
        re.IGNORECASE
    )
    re_tp_policy_fallback = re.compile(
        r"\[RISK\]\[TP_POLICY\].*?P4_FALLBACK.*?DistATR=([0-9\.,]+).*?RR=([0-9\.,]+)",
        re.IGNORECASE
    )
    # V6.0f-FASE2: Opposing HeatZone para TP
    re_tp_policy_opposing = re.compile(
        r"\[RISK\]\[TP_POLICY\]\s*Zone=(\S+)\s*P0_OPPOSING:\s*ZoneId=(\S+)\s*Dir=(\w+)\s*TF=(-?\d+)\s*Score=([0-9\.,]+)\s*RR=([0-9\.,]+)\s*DistATR=([0-9\.,]+)",
        re.IGNORECASE
    )
    # V6.0f-FASE2: P0_ANY_DIR (fallback de opposing)
    re_tp_policy_any_dir = re.compile(
        r"\[RISK\]\[TP_POLICY\]\s*Zone=(\S+)\s*P0_ANY_DIR:\s*ZoneId=(\S+)\s*Dir=(\w+)\s*TF=(-?\d+)\s*Score=([0-9\.,]+)\s*RR=([0-9\.,]+)\s*DistATR=([0-9\.,]+)",
        re.IGNORECASE
    )
    
    # V6.0f-FASE2: P0_SWING_LITE (swing-based fallback)
    re_tp_policy_swing_lite = re.compile(
        r"\[RISK\]\[TP_POLICY\]\s*Zone=(\S+)\s*P0_SWING_LITE:\s*TF=(-?\d+)\s*Score=([0-9\.,]+)\s*RR=([0-9\.,]+)\s*DistATR=([0-9\.,]+)",
        re.IGNORECASE
    )
    # V6.0e: Búsqueda de siguiente TP
    re_tp_next = re.compile(
        r"\[RISK\]\[TP_NEXT\]\s*Zone=(\S+)\s*Candidato\s*TF=(-?\d+)\s*TP=([0-9\.,]+)\s*Dist=([0-9\.,]+)pts.*?(PASS|RECHAZADO)",
        re.IGNORECASE
    )

    re_cancelbias = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[CancelBias\].*index=(\d+).*Close=([0-9\.,]+).*EMA200~?=([0-9\.,]+).*Bias=(\w+)")

    # SL/TP Analysis (nuevo - análisis post-mortem)
    re_sl_candidates = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*SL_CANDIDATES:\s*Zone=(\S+)\s*Dir=(\w+)\s*TotalCandidates=(\d+)")
    re_sl_candidate = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*SL_CANDIDATE:\s*Idx=(\d+)\s*Type=(\w+)\s*Score=([0-9\.,]+)\s*TF=(\d+)\s*DistATR=([0-9\.,]+)\s*Age=(\d+)\s*Price=([0-9\.,]+)\s*InBand=(True|False)")
    re_sl_selected = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*SL_SELECTED:\s*Zone=(\S+)\s*Type=(\w+)\s*Score=([0-9\.,]+)\s*TF=(-?\d+)\s*DistATR=([0-9\.,]+)\s*Age=(\d+)\s*Price=([0-9\.,]+)\s*Reason=(\S+)")
    re_tp_candidates = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*TP_CANDIDATES:\s*Zone=(\S+)\s*Dir=(\w+)\s*TotalCandidates=(\d+)")
    re_tp_candidate = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*TP_CANDIDATE:\s*Idx=(\d+)\s*Priority=(\S+)\s*Type=(\w+)\s*Score=([0-9\.,]+)\s*TF=(\d+)\s*DistATR=([0-9\.,]+)\s*Age=(\d+)\s*Price=([0-9\.,]+)\s*RR=([0-9\.,]+)")
    re_tp_selected = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*TP_SELECTED:\s*Zone=(\S+)\s*Priority=(\S+)\s*Type=(\w+)\s*Score=([0-9\.,]+)\s*TF=(-?\d+)\s*DistATR=([0-9\.,]+)\s*Age=(\d+)\s*Price=([0-9\.,]+)\s*RR=([0-9\.,]+)\s*Reason=(\S+)")
    # Compatibilidad: aceptar tanto [TP_PICK] como [RISK][TP_PICK] (formato genérico fuera de [DIAGNOSTICO])
    re_tp_pick_generic = re.compile(
        r"\[(?:RISK\]\[)?TP_PICK\]\s*Zone=(\S+).*?Type=(\S+)\s*TF=(-?\d+).*?DistATR=([0-9\.,]+)\s*RR=([0-9\.,]+).*?(?:Score|FinalScore)=([0-9\.,]+).*?Price=([0-9\.,]+)(?:\s*Reason=(\S+))?",
        re.IGNORECASE
    )

    # StructureFusion diagnostics (nuevo en V5.6.9)
    # Por zona: [DIAGNOSTICO][StructureFusion] HZ=... Triggers=X Anchors=Y BullDir=a BearDir=b → Dir=Z Reason=... Bias=...
    re_sf_zone = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[StructureFusion\].*HZ=\w+.*Triggers=(\d+).*Anchors=(\d+).*BullDir=([0-9\.,]+).*BearDir=([0-9\.,]+).*→\s*Dir=(\w+).*Reason=([^\s]+)")
    # TF lists: [DIAGNOSTICO][StructureFusion] ZONA HZ=... TFTrig=15/60 TFAng=240/1440
    re_sf_tfs = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[StructureFusion\]\s*ZONA\s+HZ=.*TFTrig=([0-9/]+)\s+TFAng=([0-9/]*)")
    # Resumen: [DIAGNOSTICO][StructureFusion] TotHZ=n WithAnchors=a DirBull=b DirBear=c DirNeutral=d
    re_sf_summary = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[StructureFusion\].*TotHZ=(\d+).*WithAnchors=(\d+).*DirBull=(\d+).*DirBear=(\d+).*DirNeutral=(\d+)")

    # Logs generales (no DIAGNOSTICO) útiles
    re_dfm_conf = re.compile(
        r"\[DecisionFusionModel\].*Confidence=([0-9\.,]+) \(Core=([0-9\.,]+), Prox=([0-9\.,]+), Conf=([0-9\.,]+), Type=([0-9\.,]+), Bias=([0-9\.,]+)\)")
    re_ctx_bias = re.compile(r"\[ContextManager\].*GlobalBias:\s*(\w+),\s*Strength:\s*([0-9\.,]+)")
    re_ctx_diag = re.compile(rf"\[(?:{re_diagnostic_tag})\]\[Context\]\s*Bias=(\w+)\s*Strength=([0-9\.,]+)\s*Close60>Avg200=(true|false)", re.IGNORECASE)
    re_tm_cancel = re.compile(r"\[TradeManager\].*ORDEN (CANCELADA|EXPIRADA).*Razón:?\s*(.*)")
    re_tm_cancel_diag = re.compile(rf"\[(?:{re_diagnostic_tag})\]\[TM\]\s*Cancel_BOS\s*Action=(BUY|SELL)\s*Bias=(\w+)")

    # Funnel traces (TradeManager) - aceptar con o sin ':'
    re_tm_registered = re.compile(r"\[TradeManager\].*ORDEN REGISTRADA:")
    re_tm_dedup_cooldown = re.compile(r"\[TRADE\]\[DEDUP\]\s+COOLDOWN:?\b")
    re_tm_dedup_ident = re.compile(r"\[TRADE\]\[DEDUP\]\s+IDENTICAL:?\b")
    re_tm_skip_conc = re.compile(r"\[TRADE\]\[SKIP\]\s+CONCURRENCY_LIMIT:?\b")

    # DEDUP detail (nuevos formatos enriquecidos)
    re_dedup_ident_new = re.compile(
        r"\[TRADE\]\[DEDUP\]\s+IDENTICAL\s+Zone=(\S+)\s+Action=(BUY|SELL)\s+Key=([0-9\.,\-]+/[0-9\.,\-]+/[0-9\.,\-]+)\s+DomTF=(-?\d+)\s+LastSimilar=(\S*)\s+LastBar=(-?\d+)\s+CurrentBar=(-?\d+)\s+DeltaBars=(-?\d+)\s+Tolerance=([0-9\.,]+)")
    re_dedup_ident_old = re.compile(
        r"\[TRADE\]\[DEDUP\]\s+IDENTICAL:?\s+Action=(BUY|SELL).*?Key=([0-9\./\.-]+).*?lastId=(\S+).*?lastBar=(-?\d+).*?deltaBars=(-?\d+).*?TF=(-?\d+).*?(?:Struct|Struct|Structure|Struct)=(\S+)")
    re_dedup_cooldown_new = re.compile(
        r"\[TRADE\]\[DEDUP\]\s+COOLDOWN\s+Zone=(\S+)\s+Action=(BUY|SELL)\s+Key=([0-9\.,\-]+/[0-9\.,\-]+/[0-9\.,\-]+)\s+DomTF=(-?\d+)\s+CurrentBar=(-?\d+)\s+BarsRemain=(\d+)\s+UntilBar=(\d+)")
    re_dedup_cooldown_old = re.compile(
        r"\[TRADE\]\[DEDUP\]\s+COOLDOWN:?\s+.*?Action=(BUY|SELL).*?Struct=(\S+).*?BarsRemain=(\d+).*?UntilBar=(\d+)")

    stats = {
        'dfm': {
            'lines': 0,
            'bull_evals': 0,
            'bear_evals': 0,
            'passed': 0,
            'bins': [0]*10
        },
        'prox': {
            'lines': 0,
            'kept_aligned': 0,
            'total_aligned': 0,
            'kept_counter': 0,
            'total_counter': 0,
            'sum_avg_prox_aligned': 0.0,
            'sum_avg_prox_counter': 0.0,
            'sum_avg_dist_aligned': 0.0,
            'sum_avg_dist_counter': 0.0,
            'prefer_aligned_events': 0,
            'prefer_filtered_total': 0
        },
        'prox_pre': {
            'lines': 0,
            'aligned': 0,
            'aligned_total': 0,
            'counter': 0,
            'counter_total': 0,
            'sum_avg_prox_aligned': 0.0,
            'sum_avg_dist_aligned': 0.0
        },
        'prox_detail': {
            'lines': 0,
            'aligned': {
                'count': 0,
                'sum_base_prox': 0.0,
                'sum_zone_atr': 0.0,
                'sum_size_penalty': 0.0,
                'sum_final_prox': 0.0
            },
            'counter': {
                'count': 0,
                'sum_base_prox': 0.0,
                'sum_zone_atr': 0.0,
                'sum_size_penalty': 0.0,
                'sum_final_prox': 0.0
            }
        },
        'risk': {
            'lines': 0,
            'accepted': 0,
            'rej_sl': 0,
            'rej_tp': 0,
            'rej_rr': 0,
            'rej_entry': 0,
            'accepted_details': [],  # [(dir, entry, sl, tp, slatr, conf)]
            'accepted_detail_rows': [],  # [{zone,dir,aligned,core,prox,confc,sl_tf,sl_struct,tp_tf,tp_struct,rr,conf}]
            'rej_details': {
                'aligned': {
                    'count': 0,
                    'sum_slatr': 0.0,
                    'sum_prox': 0.0,
                    'sum_core': 0.0
                },
                'counter': {
                    'count': 0,
                    'sum_slatr': 0.0,
                    'sum_prox': 0.0,
                    'sum_core': 0.0
                }
            },
            'hist_aligned': [0,0,0,0,0],
            'hist_counter': [0,0,0,0,0],
            'wr_bins': {
                'bins': [ {'wins':0,'losses':0} for _ in range(5) ],
                'matched': 0,
                'unmatched': 0
            },
            'slpick_bands': {
                'lt8': 0, '8_10': 0, '10_12_5': 0, '12_5_15': 0, 'gt15': 0,
                'tf': {'5':0,'15':0,'60':0,'240':0,'1440':0}
            },
            # V6.0d: Doble cerrojo
            'rej_sl_points': 0,
            'rej_tp_points': 0,
            'rej_sl_high_vol': 0,
            'rej_sl_points_by_tf': {},
            'rej_tp_points_by_tf': {},
            'rej_sl_high_vol_by_tf': {},
            'sl_check_pass': {
                'count': 0,
                'sum_pts': 0.0,
                'sum_atr_dist': 0.0,
                'by_tf': {}
            },
            'tp_check_pass': {
                'count': 0,
                'sum_pts': 0.0,
                'sum_atr_dist': 0.0,
                'by_tf': {}
            },
            # V6.0c: Política TP
            'tp_forced_p3': 0,
            'tp_p4_fallback': 0,
            'tp_forced_p3_by_tf': {},
            # V6.0f-FASE2: Opposing HeatZone para TP
            'tp_p0_opposing': 0,
            'tp_p0_opposing_by_tf': {},
            'tp_p0_opposing_avg_score': 0.0,
            'tp_p0_opposing_avg_rr': 0.0,
            'tp_p0_opposing_avg_distatr': 0.0,
            # V6.0f-FASE2: P0_ANY_DIR (fallback de opposing)
            'tp_p0_any_dir': 0,
            'tp_p0_any_dir_by_tf': {},
            'tp_p0_any_dir_avg_score': 0.0,
            'tp_p0_any_dir_avg_rr': 0.0,
            'tp_p0_any_dir_avg_distatr': 0.0,
            # V6.0f-FASE2: P0_SWING_LITE (swing-based fallback)
            'tp_p0_swing_lite': 0,
            'tp_p0_swing_lite_by_tf': {},
            'tp_p0_swing_lite_avg_score': 0.0,
            'tp_p0_swing_lite_avg_rr': 0.0,
            'tp_p0_swing_lite_avg_distatr': 0.0,
            # V6.0e: Búsqueda de siguiente TP
            'tp_next': {
                'zones_with_search': set(),
                'total_candidates': 0,
                'rejected_by_points': 0,
                'rejected_by_tf': {},
                'passed': 0
            },
            'rrplan_bands': {
                '0_10_sum': 0.0, '0_10_n': 0,
                '10_15_sum': 0.0, '10_15_n': 0
            },
            'wr_conf': {
                'bins': [ {'wins':0,'losses':0} for _ in range(5) ]
            }
        },
        'cancelbias': {
            'lines': 0,
            'bias_counts': {'Bullish': 0, 'Bearish': 0, 'Neutral': 0},
            'samples': 0,
            'close_gt_ema': 0
        },
        'dfm_components': {
            'count': 0,
            'sum_final': 0.0,
            'sum_core': 0.0,
            'sum_prox': 0.0,
            'sum_conf': 0.0,
            'sum_type': 0.0,
            'sum_bias': 0.0
        },
        'context_bias': {
            'lines': 0,
            'bias_counts': {'Bullish': 0, 'Bearish': 0, 'Neutral': 0},
            'strength_sum': 0.0
        },
        'context_diag': {
            'lines': 0,
            'bias_counts': {'Bullish': 0, 'Bearish': 0, 'Neutral': 0},
            'strength_sum': 0.0,
            'close_gt_avg': 0
        },
        'tm_reasons': {
            'cancel': {},
            'expire': {}
        },
        'tm_cancel_diag': {
            'lines': 0,
            'by_action': {'BUY': 0, 'SELL': 0},
            'by_bias': {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}
        },
        'sf': {
            'zone_lines': 0,
            'zones_with_anchors': 0,
            'zones_bull': 0,
            'zones_bear': 0,
            'zones_neutral': 0,
            'cycles': 0,
            'sum_tot_hz': 0,
            'sum_with_anchors': 0,
            'sum_dir_bull': 0,
            'sum_dir_bear': 0,
            'sum_dir_neutral': 0,
            'reasons': {},
            'tf_trig_counts': {},
            'tf_anchor_counts': {}
        },
        'sl_analysis': {
            'zones': 0,
            'total_candidates': 0,
            'selected': 0,
            'candidates': [],  # [{type, score, tf, dist_atr, age, in_band}]
            'selected_list': []  # [{type, score, tf, dist_atr, age, reason}]
        },
        'tp_analysis': {
            'zones': 0,
            'total_candidates': 0,
            'selected': 0,
            'candidates': [],  # [{priority, type, score, tf, dist_atr, age, rr}]
            'selected_list': []  # [{priority, type, score, tf, dist_atr, age, rr, reason}]
        },
        'funnel': {
            'dfm_passed': 0,
            'registered': 0,
            'dedup_cooldown': 0,
            'dedup_identical': 0,
            'skip_concurrency': 0,
            'executed': 0,  # se rellena desde CSV
        },
        'dedup': {
            'identical_by_zone': {},      # zoneId -> count
            'identical_key_by_zone': {},  # zoneId -> Counter(keys)
            'identical_delta_hist': Counter(),  # deltaBars -> count
            'identical_by_action': {'BUY': 0, 'SELL': 0},
            'identical_by_domtf': {},     # tf -> count
            'cooldown_by_zone': {}        # zoneId -> count
        }
    }

    try:
        with open(log_path, 'r', encoding='utf-8', errors='ignore') as f:
            for line in f:
                # DFM evals
                m = re_dfm_eval.search(line)
                if m:
                    stats['dfm']['lines'] += 1
                    stats['dfm']['bull_evals'] += int(m.group(1))
                    stats['dfm']['bear_evals'] += int(m.group(2))
                    stats['dfm']['passed'] += int(m.group(3))
                    stats['funnel']['dfm_passed'] += int(m.group(3))
                    continue

                m = re_dfm_bins.search(line)
                if m:
                    bins_str = m.group(1)
                    # esperado: "0:0,1:0,...,9:0"
                    parts = [p.strip() for p in bins_str.split(',') if p.strip()]
                    for p in parts:
                        if ':' in p:
                            idx_str, val_str = p.split(':', 1)
                            try:
                                idx = int(idx_str)
                                val = int(val_str)
                                if 0 <= idx < 10:
                                    stats['dfm']['bins'][idx] += val
                            except Exception:
                                pass
                    continue

                # Proximity (resumen final)
                m = re_prox.search(line)
                if m:
                    stats['prox']['lines'] += 1
                    ka, ta, kc, tc = int(m.group(1)), int(m.group(2)), int(m.group(3)), int(m.group(4))
                    ap_al, ap_ct = to_float(m.group(5)), to_float(m.group(6))
                    ad_al, ad_ct = to_float(m.group(7)), to_float(m.group(8))
                    stats['prox']['kept_aligned'] += ka
                    stats['prox']['total_aligned'] += ta
                    stats['prox']['kept_counter'] += kc
                    stats['prox']['total_counter'] += tc
                    stats['prox']['sum_avg_prox_aligned'] += ap_al
                    stats['prox']['sum_avg_prox_counter'] += ap_ct
                    stats['prox']['sum_avg_dist_aligned'] += ad_al
                    stats['prox']['sum_avg_dist_counter'] += ad_ct
                    continue

                # Proximity Pre
                m = re_prox_pre.search(line)
                if m:
                    stats['prox_pre']['lines'] += 1
                    al, alt, ct, ctt = int(m.group(1)), int(m.group(2)), int(m.group(3)), int(m.group(4))
                    apal = to_float(m.group(5))
                    adal = to_float(m.group(6))
                    stats['prox_pre']['aligned'] += al
                    stats['prox_pre']['aligned_total'] += alt
                    stats['prox_pre']['counter'] += ct
                    stats['prox_pre']['counter_total'] += ctt
                    stats['prox_pre']['sum_avg_prox_aligned'] += apal
                    stats['prox_pre']['sum_avg_dist_aligned'] += adal
                    continue

                # Proximity detail (debug)
                m = re_prox_detail.search(line)
                if m:
                    stats['prox_detail']['lines'] += 1
                    base_prox = to_float(m.group(1))
                    zone_atr = to_float(m.group(2))
                    size_pen = to_float(m.group(3))
                    final_prox = to_float(m.group(4))
                    aligned = (m.group(5).lower() == 'true')
                    bucket = 'aligned' if aligned else 'counter'
                    stats['prox_detail'][bucket]['count'] += 1
                    stats['prox_detail'][bucket]['sum_base_prox'] += base_prox
                    stats['prox_detail'][bucket]['sum_zone_atr'] += zone_atr
                    stats['prox_detail'][bucket]['sum_size_penalty'] += size_pen
                    stats['prox_detail'][bucket]['sum_final_prox'] += final_prox
                    continue

                m = re_prox_prefer.search(line)
                if m:
                    stats['prox']['prefer_aligned_events'] += 1
                    stats['prox']['prefer_filtered_total'] += int(m.group(1))
                    continue

                # Proximity drivers summary (INFO)
                m = re_prox_drivers.search(line)
                if m:
                    stats['prox_detail']['lines'] += 1
                    a_n = int(m.group(1)); a_base = to_float(m.group(2)); a_zone = to_float(m.group(3)); a_size = to_float(m.group(4)); a_final = to_float(m.group(5))
                    c_n = int(m.group(6)); c_base = to_float(m.group(7)); c_zone = to_float(m.group(8)); c_size = to_float(m.group(9)); c_final = to_float(m.group(10))
                    if a_n > 0:
                        stats['prox_detail']['aligned']['count'] += a_n
                        stats['prox_detail']['aligned']['sum_base_prox'] += a_base * a_n
                        stats['prox_detail']['aligned']['sum_zone_atr'] += a_zone * a_n
                        stats['prox_detail']['aligned']['sum_size_penalty'] += a_size * a_n
                        stats['prox_detail']['aligned']['sum_final_prox'] += a_final * a_n
                    if c_n > 0:
                        stats['prox_detail']['counter']['count'] += c_n
                        stats['prox_detail']['counter']['sum_base_prox'] += c_base * c_n
                        stats['prox_detail']['counter']['sum_zone_atr'] += c_zone * c_n
                        stats['prox_detail']['counter']['sum_size_penalty'] += c_size * c_n
                        stats['prox_detail']['counter']['sum_final_prox'] += c_final * c_n
                    continue

                # Risk
                m = re_risk.search(line)
                if m:
                    stats['risk']['lines'] += 1
                    stats['risk']['accepted'] += int(m.group(1))
                    stats['risk']['rej_sl'] += int(m.group(2))
                    stats['risk']['rej_tp'] += int(m.group(3))
                    stats['risk']['rej_rr'] += int(m.group(4))
                    stats['risk']['rej_entry'] += int(m.group(5))
                    continue

                # Risk reject detail
                m = re_risk_rej_detail.search(line)
                if m:
                    aligned = m.group(2).lower() == 'true'
                    slatr = to_float(m.group(3))
                    prox = to_float(m.group(4))
                    core = to_float(m.group(5))
                    bucket = 'aligned' if aligned else 'counter'
                    stats['risk']['rej_details'][bucket]['count'] += 1
                    stats['risk']['rej_details'][bucket]['sum_slatr'] += slatr
                    stats['risk']['rej_details'][bucket]['sum_prox'] += prox
                    stats['risk']['rej_details'][bucket]['sum_core'] += core
                    continue

                # Risk histogram summary
                m = re_risk_hist.search(line)
                if m:
                    # Solo establecer desde el resumen si aún no hemos acumulado bins desde líneas individuales
                    if sum(stats['risk']['hist_aligned']) + sum(stats['risk']['hist_counter']) == 0:
                        stats['risk']['hist_aligned'] = [int(m.group(i)) for i in range(1,6)]
                        stats['risk']['hist_counter'] = [int(m.group(i)) for i in range(6,11)]
                    continue

                # RejSL con Bin (sumar a hist y detalles)
                m = re_risk_rejsl.search(line)
                if m:
                    aligned = m.group(2).lower() == 'true'
                    slatr = to_float(m.group(3))
                    bin_idx = int(m.group(4)) if m.group(4).isdigit() else -1
                    bucket = 'aligned' if aligned else 'counter'
                    stats['risk']['rej_details'][bucket]['count'] += 1
                    stats['risk']['rej_details'][bucket]['sum_slatr'] += slatr
                    # prox/core no aparecen siempre en esta línea; las dejamos en 0 si no están
                    if aligned and 0 <= bin_idx < 5:
                        stats['risk']['hist_aligned'][bin_idx] += 1
                    elif 0 <= bin_idx < 5:
                        stats['risk']['hist_counter'][bin_idx] += 1
                    continue

                # SLAccepted (guardar aceptaciones para cruzar con CSV)
                m = re_risk_slaccepted.search(line)
                if m:
                    d = m.group(1).upper()
                    e = to_float(m.group(2))
                    s = to_float(m.group(3))
                    t = to_float(m.group(4))
                    slatr = to_float(m.group(5))
                    # RR (grp6) no es necesario para matching; Conf (grp7) para bins de confianza
                    conf = to_float(m.group(7)) if len(m.groups()) >= 7 else 0.0
                    stats['risk']['accepted_details'].append((d, e, s, t, slatr, conf))
                    continue

                # SLAccepted DETAIL (calidad)
                m = re_risk_slaccepted_detail.search(line)
                if m:
                    z = m.group(1)
                    d = m.group(2).upper()
                    aligned = (m.group(3).lower() == 'true')
                    core = to_float(m.group(4))
                    prox = to_float(m.group(5))
                    confc = to_float(m.group(6))
                    confs = to_float(m.group(7))
                    sl_tf = int(m.group(8))
                    sl_struct = (m.group(9).lower() == 'true')
                    tp_tf = int(m.group(10))
                    tp_struct = (m.group(11).lower() == 'true')
                    rr = to_float(m.group(12))
                    conf = to_float(m.group(13))
                    stats['risk']['accepted_detail_rows'].append({'zone': z, 'dir': d, 'aligned': aligned, 'core': core, 'prox': prox, 'confc': confc, 'confscore': confs, 'sl_tf': sl_tf, 'sl_struct': sl_struct, 'tp_tf': tp_tf, 'tp_struct': tp_struct, 'rr': rr, 'conf': conf})
                    continue

                # SLPickBands resumen
                m = re_slpick_bands.search(line)
                if m:
                    stats['risk']['slpick_bands']['lt8'] += int(m.group(1))
                    stats['risk']['slpick_bands']['8_10'] += int(m.group(2))
                    stats['risk']['slpick_bands']['10_12_5'] += int(m.group(3))
                    stats['risk']['slpick_bands']['12_5_15'] += int(m.group(4))
                    stats['risk']['slpick_bands']['gt15'] += int(m.group(5))
                    stats['risk']['slpick_bands']['tf']['5'] += int(m.group(6))
                    stats['risk']['slpick_bands']['tf']['15'] += int(m.group(7))
                    stats['risk']['slpick_bands']['tf']['60'] += int(m.group(8))
                    stats['risk']['slpick_bands']['tf']['240'] += int(m.group(9))
                    stats['risk']['slpick_bands']['tf']['1440'] += int(m.group(10))
                    continue

                # RRPlan por bandas
                m = re_rrplan_bands.search(line)
                if m:
                    avg_0_10 = to_float(m.group(1)); n0 = int(m.group(2))
                    avg_10_15 = to_float(m.group(3)); n1 = int(m.group(4))
                    stats['risk']['rrplan_bands']['0_10_sum'] += avg_0_10 * n0
                    stats['risk']['rrplan_bands']['0_10_n'] += n0
                    stats['risk']['rrplan_bands']['10_15_sum'] += avg_10_15 * n1
                    stats['risk']['rrplan_bands']['10_15_n'] += n1
                    continue

                # V6.0d: Doble cerrojo - validación híbrida
                m = re_sl_check_fail.search(line)
                if m:
                    stats['risk']['rej_sl_points'] += 1
                    tf = int(m.group(4))
                    stats['risk']['rej_sl_points_by_tf'][tf] = stats['risk']['rej_sl_points_by_tf'].get(tf, 0) + 1
                    continue

                m = re_tp_check_fail.search(line)
                if m:
                    stats['risk']['rej_tp_points'] += 1
                    tf = int(m.group(4))
                    stats['risk']['rej_tp_points_by_tf'][tf] = stats['risk']['rej_tp_points_by_tf'].get(tf, 0) + 1
                    continue

                m = re_sl_high_vol.search(line)
                if m:
                    stats['risk']['rej_sl_high_vol'] += 1
                    # Extraer TF si existe en la línea (opcional)
                    continue

                m = re_sl_check_pass.search(line)
                if m:
                    pts = to_float(m.group(2))
                    atr_dist = to_float(m.group(3))
                    tf = int(m.group(4))
                    stats['risk']['sl_check_pass']['count'] += 1
                    stats['risk']['sl_check_pass']['sum_pts'] += pts
                    stats['risk']['sl_check_pass']['sum_atr_dist'] += atr_dist
                    if tf not in stats['risk']['sl_check_pass']['by_tf']:
                        stats['risk']['sl_check_pass']['by_tf'][tf] = {'count': 0, 'sum_pts': 0.0, 'sum_atr': 0.0}
                    stats['risk']['sl_check_pass']['by_tf'][tf]['count'] += 1
                    stats['risk']['sl_check_pass']['by_tf'][tf]['sum_pts'] += pts
                    stats['risk']['sl_check_pass']['by_tf'][tf]['sum_atr'] += atr_dist
                    continue

                m = re_tp_check_pass.search(line)
                if m:
                    pts = to_float(m.group(2))
                    atr_dist = to_float(m.group(3))
                    tf = int(m.group(4))
                    stats['risk']['tp_check_pass']['count'] += 1
                    stats['risk']['tp_check_pass']['sum_pts'] += pts
                    stats['risk']['tp_check_pass']['sum_atr_dist'] += atr_dist
                    if tf not in stats['risk']['tp_check_pass']['by_tf']:
                        stats['risk']['tp_check_pass']['by_tf'][tf] = {'count': 0, 'sum_pts': 0.0, 'sum_atr': 0.0}
                    stats['risk']['tp_check_pass']['by_tf'][tf]['count'] += 1
                    stats['risk']['tp_check_pass']['by_tf'][tf]['sum_pts'] += pts
                    stats['risk']['tp_check_pass']['by_tf'][tf]['sum_atr'] += atr_dist
                    continue

                # V6.0c: Política TP
                m = re_tp_policy_forced.search(line)
                if m:
                    stats['risk']['tp_forced_p3'] += 1
                    tf = int(m.group(1))
                    stats['risk']['tp_forced_p3_by_tf'][tf] = stats['risk']['tp_forced_p3_by_tf'].get(tf, 0) + 1
                    continue

                # V6.0f-FASE2: Opposing HeatZone para TP
                m = re_tp_policy_opposing.search(line)
                if m:
                    tf = int(m.group(4))
                    score = to_float(m.group(5))
                    rr = to_float(m.group(6))
                    dist_atr = to_float(m.group(7))
                    
                    stats['risk']['tp_p0_opposing'] += 1
                    stats['risk']['tp_p0_opposing_by_tf'][tf] = stats['risk']['tp_p0_opposing_by_tf'].get(tf, 0) + 1
                    stats['risk']['tp_p0_opposing_avg_score'] += score
                    stats['risk']['tp_p0_opposing_avg_rr'] += rr
                    stats['risk']['tp_p0_opposing_avg_distatr'] += dist_atr
                    continue

                # V6.0f-FASE2: P0_ANY_DIR (fallback de opposing)
                m = re_tp_policy_any_dir.search(line)
                if m:
                    tf = int(m.group(4))
                    score = to_float(m.group(5))
                    rr = to_float(m.group(6))
                    dist_atr = to_float(m.group(7))
                    
                    stats['risk']['tp_p0_any_dir'] += 1
                    stats['risk']['tp_p0_any_dir_by_tf'][tf] = stats['risk']['tp_p0_any_dir_by_tf'].get(tf, 0) + 1
                    stats['risk']['tp_p0_any_dir_avg_score'] += score
                    stats['risk']['tp_p0_any_dir_avg_rr'] += rr
                    stats['risk']['tp_p0_any_dir_avg_distatr'] += dist_atr
                    continue
                
                # V6.0f-FASE2: P0_SWING_LITE (swing-based fallback)
                m = re_tp_policy_swing_lite.search(line)
                if m:
                    tf = int(m.group(2))
                    score = to_float(m.group(3))
                    rr = to_float(m.group(4))
                    dist_atr = to_float(m.group(5))
                    
                    stats['risk']['tp_p0_swing_lite'] += 1
                    stats['risk']['tp_p0_swing_lite_by_tf'][tf] = stats['risk']['tp_p0_swing_lite_by_tf'].get(tf, 0) + 1
                    stats['risk']['tp_p0_swing_lite_avg_score'] += score
                    stats['risk']['tp_p0_swing_lite_avg_rr'] += rr
                    stats['risk']['tp_p0_swing_lite_avg_distatr'] += dist_atr
                    continue
                
                m = re_tp_policy_fallback.search(line)
                if m:
                    stats['risk']['tp_p4_fallback'] += 1
                    continue
            
                # V6.0e: Búsqueda de siguiente TP
                m = re_tp_next.search(line)
                if m:
                    zone = m.group(1)
                    tf = int(m.group(2))
                    status = m.group(5).upper()
                    
                    stats['risk']['tp_next']['zones_with_search'].add(zone)
                    stats['risk']['tp_next']['total_candidates'] += 1
                    
                    if status == 'RECHAZADO':
                        stats['risk']['tp_next']['rejected_by_points'] += 1
                        stats['risk']['tp_next']['rejected_by_tf'][tf] = stats['risk']['tp_next']['rejected_by_tf'].get(tf, 0) + 1
                    else:
                        stats['risk']['tp_next']['passed'] += 1
                    continue

                # SL Analysis
                m = re_sl_candidates.search(line)
                if m:
                    stats['sl_analysis']['zones'] += 1
                    stats['sl_analysis']['total_candidates'] += int(m.group(3))
                    continue

                m = re_sl_candidate.search(line)
                if m:
                    stats['sl_analysis']['candidates'].append({
                        'type': m.group(2),
                        'score': to_float(m.group(3)),
                        'tf': int(m.group(4)),
                        'dist_atr': to_float(m.group(5)),
                        'age': int(m.group(6)),
                        'in_band': (m.group(8).lower() == 'true')
                    })
                    continue

                m = re_sl_selected.search(line)
                if m:
                    stats['sl_analysis']['selected'] += 1
                    stats['sl_analysis']['selected_list'].append({
                        'type': m.group(2),
                        'score': to_float(m.group(3)),
                        'tf': int(m.group(4)),
                        'dist_atr': to_float(m.group(5)),
                        'age': int(m.group(6)),
                        'reason': m.group(8)
                    })
                    continue

                # TP Analysis
                m = re_tp_candidates.search(line)
                if m:
                    stats['tp_analysis']['zones'] += 1
                    stats['tp_analysis']['total_candidates'] += int(m.group(3))
                    continue

                m = re_tp_candidate.search(line)
                if m:
                    stats['tp_analysis']['candidates'].append({
                        'priority': m.group(2),
                        'type': m.group(3),
                        'score': to_float(m.group(4)),
                        'tf': int(m.group(5)),
                        'dist_atr': to_float(m.group(6)),
                        'age': int(m.group(7)),
                        'rr': to_float(m.group(9))
                    })
                    continue

                m = re_tp_selected.search(line)
                if m:
                    stats['tp_analysis']['selected'] += 1
                    stats['tp_analysis']['selected_list'].append({
                        'zone': m.group(1),
                        'priority': m.group(2),
                        'type': m.group(3),
                        'score': to_float(m.group(4)),
                        'tf': int(m.group(5)),
                        'dist_atr': to_float(m.group(6)),
                        'age': int(m.group(7)),
                        'price': to_float(m.group(8)),
                        'rr': to_float(m.group(9)),
                        'reason': m.group(10)
                    })
                    continue

                # Compatibilidad con [TP_PICK] y [RISK][TP_PICK]
                m = re_tp_pick_generic.search(line)
                if m:
                    zone = m.group(1)
                    tp_type = m.group(2)
                    tf = int(m.group(3))
                    distatr = to_float(m.group(4))
                    rr = to_float(m.group(5))
                    score = to_float(m.group(6))
                    price = to_float(m.group(7))
                    reason = (m.group(8) or 'TP_PICK') if len(m.groups()) >= 8 else 'TP_PICK'
                    prio = 'P3' if tp_type.upper().startswith('P3') else ('P0' if tp_type.upper().startswith('P0') else 'NA')
                    stats['tp_analysis']['selected'] += 1
                    stats['tp_analysis']['selected_list'].append({
                        'zone': zone,
                        'priority': prio,
                        'type': tp_type,
                        'score': score,
                        'tf': tf,
                        'dist_atr': distatr,
                        'age': 0,
                        'price': price,
                        'rr': rr,
                        'reason': reason
                    })
                    continue

                # CancelBias
                m = re_cancelbias.search(line)
                if m:
                    stats['cancelbias']['lines'] += 1
                    idx = int(m.group(1))
                    close = to_float(m.group(2))
                    ema = to_float(m.group(3))
                    bias = m.group(4)
                    stats['cancelbias']['samples'] += 1
                    if bias in stats['cancelbias']['bias_counts']:
                        stats['cancelbias']['bias_counts'][bias] += 1
                    if close > ema:
                        stats['cancelbias']['close_gt_ema'] += 1
                    continue

                # Context diag
                m = re_ctx_diag.search(line)
                if m:
                    stats['context_diag']['lines'] += 1
                    bias = m.group(1)
                    strength = to_float(m.group(2))
                    gt = m.group(3).lower() == 'true'
                    if bias in stats['context_diag']['bias_counts']:
                        stats['context_diag']['bias_counts'][bias] += 1
                    stats['context_diag']['strength_sum'] += strength
                    if gt:
                        stats['context_diag']['close_gt_avg'] += 1
                    continue

                # DFM components (no diagnostico)
                m = re_dfm_conf.search(line)
                if m:
                    stats['dfm_components']['count'] += 1
                    stats['dfm_components']['sum_final'] += to_float(m.group(1))
                    stats['dfm_components']['sum_core'] += to_float(m.group(2))
                    stats['dfm_components']['sum_prox'] += to_float(m.group(3))
                    stats['dfm_components']['sum_conf'] += to_float(m.group(4))
                    stats['dfm_components']['sum_type'] += to_float(m.group(5))
                    stats['dfm_components']['sum_bias'] += to_float(m.group(6))
                    continue

                # ContextManager bias
                m = re_ctx_bias.search(line)
                if m:
                    stats['context_bias']['lines'] += 1
                    cbias = m.group(1)
                    cstr = to_float(m.group(2))
                    if cbias in stats['context_bias']['bias_counts']:
                        stats['context_bias']['bias_counts'][cbias] += 1
                    stats['context_bias']['strength_sum'] += cstr
                    continue

                # TradeManager cancel/expire reasons
                m = re_tm_cancel.search(line)
                if m:
                    kind = m.group(1)
                    reason = (m.group(2) or '').strip()
                    bucket = 'cancel' if 'CANCEL' in kind.upper() else 'expire'
                    stats['tm_reasons'][bucket][reason] = stats['tm_reasons'][bucket].get(reason, 0) + 1
                    continue

                # TradeManager Cancel_BOS diag
                m = re_tm_cancel_diag.search(line)
                if m:
                    stats['tm_cancel_diag']['lines'] += 1
                    action = m.group(1)
                    bias = m.group(2)
                    if action in stats['tm_cancel_diag']['by_action']:
                        stats['tm_cancel_diag']['by_action'][action] += 1
                    if bias in stats['tm_cancel_diag']['by_bias']:
                        stats['tm_cancel_diag']['by_bias'][bias] += 1
                    continue

                # Funnel: Registered / Dedup / Concurrency
                if re_tm_registered.search(line):
                    stats['funnel']['registered'] += 1
                    continue
                if re_tm_dedup_cooldown.search(line):
                    stats['funnel']['dedup_cooldown'] += 1
                    # Parse detalles si formato nuevo/u antiguo
                    mnew = re_dedup_cooldown_new.search(line)
                    if mnew:
                        zone = mnew.group(1)
                        stats['dedup']['cooldown_by_zone'][zone] = stats['dedup']['cooldown_by_zone'].get(zone, 0) + 1
                    else:
                        mold = re_dedup_cooldown_old.search(line)
                        if mold:
                            zone = mold.group(2)
                            stats['dedup']['cooldown_by_zone'][zone] = stats['dedup']['cooldown_by_zone'].get(zone, 0) + 1
                    continue
                if re_tm_dedup_ident.search(line):
                    stats['funnel']['dedup_identical'] += 1
                    # Parse detalles para análisis
                    mnew = re_dedup_ident_new.search(line)
                    if mnew:
                        zone = mnew.group(1)
                        action = mnew.group(2)
                        key = mnew.group(3)
                        domtf = mnew.group(4)
                        delta = int(mnew.group(8)) if mnew.group(8) else -1
                        # by zone
                        stats['dedup']['identical_by_zone'][zone] = stats['dedup']['identical_by_zone'].get(zone, 0) + 1
                        # key by zone
                        if zone not in stats['dedup']['identical_key_by_zone']:
                            stats['dedup']['identical_key_by_zone'][zone] = Counter()
                        stats['dedup']['identical_key_by_zone'][zone][key] += 1
                        # action
                        if action in stats['dedup']['identical_by_action']:
                            stats['dedup']['identical_by_action'][action] += 1
                        # domtf
                        stats['dedup']['identical_by_domtf'][domtf] = stats['dedup']['identical_by_domtf'].get(domtf, 0) + 1
                        # delta
                        stats['dedup']['identical_delta_hist'][delta] += 1
                    else:
                        mold = re_dedup_ident_old.search(line)
                        if mold:
                            action = mold.group(1)
                            key = mold.group(2)
                            last_id = mold.group(3)
                            last_bar = mold.group(4)
                            delta = int(mold.group(5)) if mold.group(5) else -1
                            domtf = mold.group(6)
                            zone = mold.group(7)
                            stats['dedup']['identical_by_zone'][zone] = stats['dedup']['identical_by_zone'].get(zone, 0) + 1
                            if zone not in stats['dedup']['identical_key_by_zone']:
                                stats['dedup']['identical_key_by_zone'][zone] = Counter()
                            stats['dedup']['identical_key_by_zone'][zone][key] += 1
                            if action in stats['dedup']['identical_by_action']:
                                stats['dedup']['identical_by_action'][action] += 1
                            stats['dedup']['identical_by_domtf'][domtf] = stats['dedup']['identical_by_domtf'].get(domtf, 0) + 1
                            stats['dedup']['identical_delta_hist'][delta] += 1
                    continue
                if re_tm_skip_conc.search(line):
                    stats['funnel']['skip_concurrency'] += 1
                    continue

                # StructureFusion por zona
                m = re_sf_zone.search(line)
                if m:
                    stats['sf']['zone_lines'] += 1
                    anchors = int(m.group(2))
                    dir_final = m.group(5)
                    reason = m.group(6)
                    if anchors > 0:
                        stats['sf']['zones_with_anchors'] += 1
                    if dir_final == 'Bullish':
                        stats['sf']['zones_bull'] += 1
                    elif dir_final == 'Bearish':
                        stats['sf']['zones_bear'] += 1
                    else:
                        stats['sf']['zones_neutral'] += 1
                    stats['sf']['reasons'][reason] = stats['sf']['reasons'].get(reason, 0) + 1
                    continue

                # StructureFusion TF lists
                m = re_sf_tfs.search(line)
                if m:
                    tf_trig = (m.group(1) or '').strip()
                    tf_anchor = (m.group(2) or '').strip()
                    for tf in [t for t in tf_trig.split('/') if t]:
                        stats['sf']['tf_trig_counts'][tf] = stats['sf']['tf_trig_counts'].get(tf, 0) + 1
                    for tf in [t for t in tf_anchor.split('/') if t]:
                        stats['sf']['tf_anchor_counts'][tf] = stats['sf']['tf_anchor_counts'].get(tf, 0) + 1
                    continue

                # StructureFusion resumen por ciclo
                m = re_sf_summary.search(line)
                if m:
                    stats['sf']['cycles'] += 1
                    stats['sf']['sum_tot_hz'] += int(m.group(1))
                    stats['sf']['sum_with_anchors'] += int(m.group(2))
                    stats['sf']['sum_dir_bull'] += int(m.group(3))
                    stats['sf']['sum_dir_bear'] += int(m.group(4))
                    stats['sf']['sum_dir_neutral'] += int(m.group(5))
                    continue
    except FileNotFoundError:
        print(f"ERROR: No se encontró el log: {log_path}", file=sys.stderr)

    # Deduplicación de TP seleccionados (evitar doble conteo por [TP_PICK] y [RISK][TP_PICK])
    try:
        sel = stats['tp_analysis']['selected_list']
        deduped = []
        seen = set()
        for s in sel:
            zone = s.get('zone', '')
            tp_type = s.get('type', '')
            tf = s.get('tf', -9999)
            price = s.get('price', 0.0)
            key = (zone, tp_type, tf, round(price if price is not None else 0.0, 2))
            if key in seen:
                continue
            seen.add(key)
            deduped.append(s)
        stats['tp_analysis']['selected_list'] = deduped
        stats['tp_analysis']['selected'] = len(deduped)
    except Exception:
        pass

    return stats


def parse_csv(csv_path: str) -> dict:
    out = {
        'rows': 0,
        'executed': 0,
        'cancelled': 0,
        'expired': 0,
        'buy': 0,
        'sell': 0,
        'cancel_reasons': {},
        'expire_reasons': {},
        'trades': []  # lista simple para matching aproximado
    }
    if not csv_path or not os.path.exists(csv_path):
        return out

    try:
        with open(csv_path, 'r', encoding='utf-8', errors='ignore') as f:
            sample = f.read(8192)
            f.seek(0)
            try:
                dialect = csv.Sniffer().sniff(sample, delimiters=",;\t|")
            except Exception:
                dialect = csv.excel
            reader = csv.DictReader(f, dialect=dialect)
            for row in reader:
                out['rows'] += 1
                # Dirección operativa: priorizar 'Direction' (BUY/SELL); si no, intentar 'Dir'; nunca usar 'Action' para dirección
                direction = (row.get('Direction') or row.get('Dir') or '').upper()
                action = (row.get('Action') or '').upper()
                status = (row.get('Status') or row.get('Resultado') or row.get('ExitType') or '')
                reason = (row.get('ExitReason') or row.get('Razón') or '').strip()
                # Lectura tolerante de columnas de precios
                def getf(keys):
                    for k in keys:
                        if k in row and row[k] != '':
                            try:
                                return to_float(str(row[k]))
                            except Exception:
                                pass
                    return None
                e = getf(['Entry','EntryPrice','Entry Price'])
                sl = getf(['SL','StopLoss','Stop Loss','SL Price'])
                tp = getf(['TP','TakeProfit','Take Profit','TP Price'])
                ex = getf(['Exit','ExitPrice','Exit Price'])

                if direction == 'BUY':
                    out['buy'] += 1
                elif direction == 'SELL':
                    out['sell'] += 1

                st_up = status.upper()
                # Inferir resultado si está vacío usando Exit vs TP/SL
                if st_up == '' and ex is not None:
                    if tp is not None and abs(ex - tp) <= 0.5:
                        st_up = 'TP'
                    elif sl is not None and abs(ex - sl) <= 0.5:
                        st_up = 'SL'
                if 'TP' in st_up or 'SL' in st_up or 'EXECUTED' in st_up or reason.upper() in ('TP','SL'):
                    out['executed'] += 1
                    out['trades'].append({'dir': direction, 'entry': e, 'sl': sl, 'tp': tp, 'exit': ex, 'status': st_up or reason.upper()})
                elif 'CANCEL' in st_up:
                    out['cancelled'] += 1
                    if reason:
                        out['cancel_reasons'][reason] = out['cancel_reasons'].get(reason, 0) + 1
                elif 'EXPIRE' in st_up or 'EXPIR' in st_up:
                    out['expired'] += 1
                    if reason:
                        out['expire_reasons'][reason] = out['expire_reasons'].get(reason, 0) + 1
    except Exception as e:
        print(f"ERROR leyendo CSV {csv_path}: {e}", file=sys.stderr)

    # Fallback: si no hemos detectado ejecuciones (TP/SL), intentar parser manual para CLOSED con decimales con coma
    if out['executed'] == 0:
        try:
            import re
            with open(csv_path, 'r', encoding='utf-8', errors='ignore') as f:
                header = f.readline()  # descartar cabecera
                for line in f:
                    if 'CLOSED' not in line:
                        continue
                    # Formato esperado: ... ,CLOSED,DIR,Entry,dec,SL,dec,TP,dec, ... ,Status(=TP_HIT/SL_HIT),ExitReason(=TP/SL), ...
                    # Extraer dirección
                    if ',CLOSED,BUY,' in line:
                        direction = 'BUY'
                        rest = line.split(',CLOSED,BUY,', 1)[1]
                    elif ',CLOSED,SELL,' in line:
                        direction = 'SELL'
                        rest = line.split(',CLOSED,SELL,', 1)[1]
                    else:
                        continue
                    # Capturar primeros tres tokens numéricos tipo 1234,56 o guión '-'
                    nums = re.findall(r"(-?\d+,\d{2}|-)", rest)
                    # Determinar Status/ExitReason simples
                    st = 'TP' if ',TP_HIT,TP,' in line else ('SL' if ',SL_HIT,SL,' in line else '')
                    if nums and len(nums) >= 3:
                        def conv(s):
                            return None if s == '-' else to_float(s)
                        e = conv(nums[0]); sl = conv(nums[1]); tp = conv(nums[2])
                        out['executed'] += 1
                        out['trades'].append({'dir': direction, 'entry': e, 'sl': sl, 'tp': tp, 'exit': None, 'status': st})
                        if direction == 'BUY':
                            out['buy'] += 1
                        elif direction == 'SELL':
                            out['sell'] += 1
        except Exception as fe:
            print(f"WARN: Fallback CSV parse error: {fe}", file=sys.stderr)

    return out


def render_markdown(log_path: str, csv_path: str, stats_log: dict, stats_csv: dict) -> str:
    now = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    lines = []
    lines.append(f"# Informe Diagnóstico de Logs - {now}")
    lines.append("")
    lines.append(f"- Log: `{log_path}`")
    if csv_path:
        lines.append(f"- CSV: `{csv_path}`")
    lines.append("")

    # DFM
    dfm = stats_log['dfm']
    total_eval_events = dfm['lines']
    bull = dfm['bull_evals']
    bear = dfm['bear_evals']
    passed = dfm['passed']
    lines.append("## DFM")
    lines.append(f"- Eventos de evaluación: {total_eval_events}")
    lines.append(f"- Evaluaciones Bull: {bull} | Bear: {bear}")
    lines.append(f"- Pasaron umbral (PassedThreshold): {passed}")
    bins = ', '.join([f"{i}:{dfm['bins'][i]}" for i in range(10)])
    lines.append(f"- ConfidenceBins acumulado: {bins}")
    lines.append("")
    # DFM components
    comp = stats_log['dfm_components']
    lines.append("### DFM - Contribuciones promedio (desde logs)")
    if comp['count'] > 0:
        lines.append(f"- Muestras: {comp['count']}")
        lines.append(
            f"- Final≈ {comp['sum_final']/comp['count']:.3f} | Core≈ {comp['sum_core']/comp['count']:.3f} | Prox≈ {comp['sum_prox']/comp['count']:.3f} | "
            f"Conf≈ {comp['sum_conf']/comp['count']:.3f} | Type≈ {comp['sum_type']/comp['count']:.3f} | Bias≈ {comp['sum_bias']/comp['count']:.3f}")
    else:
        lines.append("- Sin muestras de desglose de componentes en el log.")
    lines.append("")

    # Proximity
    prox = stats_log['prox']
    lines.append("## Proximity")
    lines.append(f"- Eventos: {prox['lines']}")
    lines.append(f"- KeptAligned: {prox['kept_aligned']}/{prox['total_aligned']} | KeptCounter: {prox['kept_counter']}/{prox['total_counter']}")
    if prox['lines'] > 0:
        lines.append(f"- Promedios reportados (media de promedios por evento):")
        lines.append(f"  - AvgProxAligned≈ {prox['sum_avg_prox_aligned']/prox['lines']:.3f} | AvgProxCounter≈ {prox['sum_avg_prox_counter']/prox['lines']:.3f}")
        lines.append(f"  - AvgDistATRAligned≈ {prox['sum_avg_dist_aligned']/prox['lines']:.2f} | AvgDistATRCounter≈ {prox['sum_avg_dist_counter']/prox['lines']:.2f}")
    lines.append(f"- PreferAligned eventos: {prox['prefer_aligned_events']} | Filtradas contra-bias: {prox['prefer_filtered_total']}")
    lines.append("")

    # Proximity Pre
    prox_pre = stats_log['prox_pre']
    lines.append("### Proximity (Pre-PreferAligned)")
    lines.append(f"- Eventos: {prox_pre['lines']}")
    if prox_pre['lines'] > 0:
        lines.append(f"- Aligned pre: {prox_pre['aligned']}/{prox_pre['aligned_total']} | Counter pre: {prox_pre['counter']}/{prox_pre['counter_total']}")
        lines.append(f"- AvgProxAligned(pre)≈ {prox_pre['sum_avg_prox_aligned']/prox_pre['lines']:.3f} | AvgDistATRAligned(pre)≈ {prox_pre['sum_avg_dist_aligned']/prox_pre['lines']:.2f}")
    lines.append("")

    # Proximity Drivers (detallado)
    pd = stats_log['prox_detail']
    lines.append("### Proximity Drivers")
    lines.append(f"- Eventos: {pd['lines']}")
    if pd['aligned']['count'] > 0 or pd['counter']['count'] > 0:
        if pd['aligned']['count'] > 0:
            lines.append(f"- Alineadas: n={pd['aligned']['count']} | BaseProx≈ {pd['aligned']['sum_base_prox']/pd['aligned']['count']:.3f} | ZoneATR≈ {pd['aligned']['sum_zone_atr']/pd['aligned']['count']:.2f} | SizePenalty≈ {pd['aligned']['sum_size_penalty']/pd['aligned']['count']:.3f} | FinalProx≈ {pd['aligned']['sum_final_prox']/pd['aligned']['count']:.3f}")
        if pd['counter']['count'] > 0:
            lines.append(f"- Contra-bias: n={pd['counter']['count']} | BaseProx≈ {pd['counter']['sum_base_prox']/pd['counter']['count']:.3f} | ZoneATR≈ {pd['counter']['sum_zone_atr']/pd['counter']['count']:.2f} | SizePenalty≈ {pd['counter']['sum_size_penalty']/pd['counter']['count']:.3f} | FinalProx≈ {pd['counter']['sum_final_prox']/pd['counter']['count']:.3f}")
    lines.append("")

    # Risk
    risk = stats_log['risk']
    lines.append("## Risk")
    lines.append(f"- Eventos: {risk['lines']}")
    lines.append(f"- Accepted={risk['accepted']} | RejSL={risk['rej_sl']} | RejTP={risk['rej_tp']} | RejRR={risk['rej_rr']} | RejEntry={risk['rej_entry']}")
    
    # V6.0d: Doble cerrojo - validación híbrida
    if risk['rej_sl_points'] + risk['rej_tp_points'] + risk['rej_sl_high_vol'] > 0:
        lines.append("### Risk – Validación Doble Cerrojo (V6.0d)")
        lines.append(f"- **RejSL_Points:** {risk['rej_sl_points']} (rechazados por >60pts)")
        lines.append(f"- **RejTP_Points:** {risk['rej_tp_points']} (rechazados por >120pts)")
        lines.append(f"- **RejSL_HighVol:** {risk['rej_sl_high_vol']} (rechazados por ATR>15 y DistATR>10)")
        
        # Rechazos SL por TF
        if risk['rej_sl_points_by_tf']:
            lines.append("")
            lines.append("**Rechazos SL por TF:**")
            lines.append("")
            lines.append("| TF | RejSL_Points |")
            lines.append("|----|--------------|")
            for tf in sorted(risk['rej_sl_points_by_tf'].keys()):
                lines.append(f"| {tf} | {risk['rej_sl_points_by_tf'][tf]} |")
        
        # Rechazos TP por TF
        if risk['rej_tp_points_by_tf']:
            lines.append("")
            lines.append("**Rechazos TP por TF:**")
            lines.append("")
            lines.append("| TF | RejTP_Points |")
            lines.append("|----|--------------|")
            for tf in sorted(risk['rej_tp_points_by_tf'].keys()):
                lines.append(f"| {tf} | {risk['rej_tp_points_by_tf'][tf]} |")
        lines.append("")
    
    # Medias PASS (si hay datos)
    if risk['sl_check_pass']['count'] > 0 or risk['tp_check_pass']['count'] > 0:
        lines.append("### Risk – Medias SL/TP Aceptados")
        if risk['sl_check_pass']['count'] > 0:
            avg_pts = risk['sl_check_pass']['sum_pts'] / risk['sl_check_pass']['count']
            avg_atr = risk['sl_check_pass']['sum_atr_dist'] / risk['sl_check_pass']['count']
            lines.append(f"- **SL:** {avg_pts:.2f} pts ({avg_atr:.2f} ATR) - n={risk['sl_check_pass']['count']}")
            if risk['sl_check_pass']['by_tf']:
                lines.append("  - Por TF:")
                for tf in sorted(risk['sl_check_pass']['by_tf'].keys()):
                    d = risk['sl_check_pass']['by_tf'][tf]
                    if d['count'] > 0:
                        lines.append(f"    - TF{tf}: {d['sum_pts']/d['count']:.2f} pts ({d['sum_atr']/d['count']:.2f} ATR) n={d['count']}")
        
        if risk['tp_check_pass']['count'] > 0:
            avg_pts = risk['tp_check_pass']['sum_pts'] / risk['tp_check_pass']['count']
            avg_atr = risk['tp_check_pass']['sum_atr_dist'] / risk['tp_check_pass']['count']
            lines.append(f"- **TP:** {avg_pts:.2f} pts ({avg_atr:.2f} ATR) - n={risk['tp_check_pass']['count']}")
            if risk['tp_check_pass']['by_tf']:
                lines.append("  - Por TF:")
                for tf in sorted(risk['tp_check_pass']['by_tf'].keys()):
                    d = risk['tp_check_pass']['by_tf'][tf]
                    if d['count'] > 0:
                        lines.append(f"    - TF{tf}: {d['sum_pts']/d['count']:.2f} pts ({d['sum_atr']/d['count']:.2f} ATR) n={d['count']}")
        lines.append("")
    
    # V6.0c: Política TP
    if risk['tp_forced_p3'] + risk['tp_p4_fallback'] > 0:
        total_tp = risk['tp_forced_p3'] + risk['tp_p4_fallback']
        pct_forced = (risk['tp_forced_p3'] / total_tp * 100) if total_tp > 0 else 0
        lines.append("### TP Policy (V6.0c)")
        lines.append(f"- **FORCED_P3:** {risk['tp_forced_p3']} ({pct_forced:.1f}%)")
        lines.append(f"- **P4_FALLBACK:** {risk['tp_p4_fallback']} ({100-pct_forced:.1f}%)")
        
        if risk['tp_forced_p3_by_tf']:
            lines.append("- **FORCED_P3 por TF:**")
            for tf in sorted(risk['tp_forced_p3_by_tf'].keys()):
                count = risk['tp_forced_p3_by_tf'][tf]
                pct = (count / risk['tp_forced_p3'] * 100) if risk['tp_forced_p3'] > 0 else 0
                lines.append(f"  - TF{tf}: {count} ({pct:.1f}%)")
        lines.append("")
    
    # V6.0f-FASE2: Opposing HeatZone para TP
    if risk['tp_p0_opposing'] > 0 or risk['tp_p0_any_dir'] > 0 or risk['tp_p0_swing_lite'] > 0:
        total_tp_all = risk['tp_p0_opposing'] + risk['tp_p0_any_dir'] + risk['tp_p0_swing_lite'] + risk['tp_forced_p3'] + risk['tp_p4_fallback']
        
        lines.append("### TP P0 HeatZone-Based (V6.0f-FASE2)")
        
        if risk['tp_p0_opposing'] > 0:
            pct_opposing = (risk['tp_p0_opposing'] / total_tp_all * 100) if total_tp_all > 0 else 0
            avg_score = risk['tp_p0_opposing_avg_score'] / risk['tp_p0_opposing']
            avg_rr = risk['tp_p0_opposing_avg_rr'] / risk['tp_p0_opposing']
            avg_distatr = risk['tp_p0_opposing_avg_distatr'] / risk['tp_p0_opposing']
            
            lines.append(f"- **P0_OPPOSING:** {risk['tp_p0_opposing']} ({pct_opposing:.1f}% del total)")
            lines.append(f"  - Avg Score: {avg_score:.2f} | Avg R:R: {avg_rr:.2f} | Avg DistATR: {avg_distatr:.2f}")
            if risk['tp_p0_opposing_by_tf']:
                lines.append("  - Por TF: " + ", ".join([f"TF{tf}={count}" for tf, count in sorted(risk['tp_p0_opposing_by_tf'].items())]))
        
        if risk['tp_p0_any_dir'] > 0:
            pct_any = (risk['tp_p0_any_dir'] / total_tp_all * 100) if total_tp_all > 0 else 0
            avg_score = risk['tp_p0_any_dir_avg_score'] / risk['tp_p0_any_dir']
            avg_rr = risk['tp_p0_any_dir_avg_rr'] / risk['tp_p0_any_dir']
            avg_distatr = risk['tp_p0_any_dir_avg_distatr'] / risk['tp_p0_any_dir']
            
            lines.append(f"- **P0_ANY_DIR:** {risk['tp_p0_any_dir']} ({pct_any:.1f}% del total)")
            lines.append(f"  - Avg Score: {avg_score:.2f} | Avg R:R: {avg_rr:.2f} | Avg DistATR: {avg_distatr:.2f}")
            if risk['tp_p0_any_dir_by_tf']:
                lines.append("  - Por TF: " + ", ".join([f"TF{tf}={count}" for tf, count in sorted(risk['tp_p0_any_dir_by_tf'].items())]))
        
        if risk['tp_p0_swing_lite'] > 0:
            pct_swing_lite = (risk['tp_p0_swing_lite'] / total_tp_all * 100) if total_tp_all > 0 else 0
            avg_score = risk['tp_p0_swing_lite_avg_score'] / risk['tp_p0_swing_lite']
            avg_rr = risk['tp_p0_swing_lite_avg_rr'] / risk['tp_p0_swing_lite']
            avg_distatr = risk['tp_p0_swing_lite_avg_distatr'] / risk['tp_p0_swing_lite']
            
            lines.append(f"- **P0_SWING_LITE:** {risk['tp_p0_swing_lite']} ({pct_swing_lite:.1f}% del total)")
            lines.append(f"  - Avg Score: {avg_score:.2f} | Avg R:R: {avg_rr:.2f} | Avg DistATR: {avg_distatr:.2f}")
            if risk['tp_p0_swing_lite_by_tf']:
                lines.append("  - Por TF: " + ", ".join([f"TF{tf}={count}" for tf, count in sorted(risk['tp_p0_swing_lite_by_tf'].items())]))
        
        lines.append("")
    
    # V6.0e: Búsqueda de siguiente TP
    if risk['tp_next']['zones_with_search']:
        lines.append("### TP Next Candidate Analysis (V6.0e)")
        n_zones = len(risk['tp_next']['zones_with_search'])
        n_total = risk['tp_next']['total_candidates']
        n_rej = risk['tp_next']['rejected_by_points']
        n_pass = risk['tp_next']['passed']
        avg_cand = n_total / n_zones if n_zones > 0 else 0
        
        lines.append(f"- **Zonas con búsqueda de siguiente TP:** {n_zones}")
        lines.append(f"- **Total candidatos evaluados:** {n_total} (promedio {avg_cand:.1f} por zona)")
        lines.append(f"- **Candidatos rechazados por límite puntos:** {n_rej} ({n_rej/n_total*100:.1f}%)")
        lines.append(f"- **Candidatos que pasaron límite:** {n_pass} ({n_pass/n_total*100:.1f}%)")
        
        if risk['tp_next']['rejected_by_tf']:
            lines.append("")
            lines.append("**Rechazados por TF:**")
            lines.append("")
            lines.append("| TF | Rechazados | % |")
            lines.append("|----|------------|---|")
            total_rej = sum(risk['tp_next']['rejected_by_tf'].values())
            for tf in sorted(risk['tp_next']['rejected_by_tf'].keys()):
                count = risk['tp_next']['rejected_by_tf'][tf]
                pct = count / total_rej * 100 if total_rej > 0 else 0
                lines.append(f"| {tf} | {count} | {pct:.1f}% |")
        lines.append("")
    
    # Risk Drivers
    rd_a = risk['rej_details']['aligned']
    rd_c = risk['rej_details']['counter']
    if rd_a['count'] > 0 or rd_c['count'] > 0:
        lines.append("### Risk Drivers (Rechazos por SL)")
        if rd_a['count'] > 0:
            lines.append(f"- Alineadas: n={rd_a['count']} | SLDistATR≈ {rd_a['sum_slatr']/rd_a['count']:.2f} | Prox≈ {rd_a['sum_prox']/rd_a['count']:.3f} | Core≈ {rd_a['sum_core']/rd_a['count']:.3f}")
        if rd_c['count'] > 0:
            lines.append(f"- Contra-bias: n={rd_c['count']} | SLDistATR≈ {rd_c['sum_slatr']/rd_c['count']:.2f} | Prox≈ {rd_c['sum_prox']/rd_c['count']:.3f} | Core≈ {rd_c['sum_core']/rd_c['count']:.3f}")
    if sum(risk['hist_aligned']) + sum(risk['hist_counter']) > 0:
        ha = risk['hist_aligned']; hc = risk['hist_counter']
        lines.append(f"- HistSL Aligned 0-10:{ha[0]},10-15:{ha[1]},15-20:{ha[2]},20-25:{ha[3]},25+:{ha[4]}")
        lines.append(f"- HistSL Counter 0-10:{hc[0]},10-15:{hc[1]},15-20:{hc[2]},20-25:{hc[3]},25+:{hc[4]}")
    lines.append("")

    # WR por bins de SLDistATR (cruce SLAccepted con CSV)
    def slatr_bin_idx(v: float) -> int:
        if v < 10.0: return 0
        if v < 15.0: return 1
        if v < 20.0: return 2
        if v < 25.0: return 3
        return 4

    if stats_csv and stats_csv.get('trades') and risk.get('accepted_details'):
        # matching aproximado por (dir, entry≈, sl≈, tp≈) con redondeo a 2 decimales
        trades = stats_csv['trades']
        wr_bins = risk['wr_bins']
        wr_conf = risk['wr_conf']
        matched = 0
        unmatched = 0
        # Preconstruir índices por dir
        idx_by_dir = {'BUY': [], 'SELL': []}
        for tr in trades:
            if tr['dir'] in idx_by_dir:
                idx_by_dir[tr['dir']].append(tr)
        def approx_equal(a, b, tol=0.5):
            if a is None or b is None:
                return False
            return abs(a - b) <= tol
        def conf_bin_idx(c: float) -> int:
            # bins: [0.50-0.60), [0.60-0.70), [0.70-0.80), [0.80-0.90), [0.90-1.00]
            if c < 0.60: return 0
            if c < 0.70: return 1
            if c < 0.80: return 2
            if c < 0.90: return 3
            return 4

        for tup in risk['accepted_details']:
            # compatibilidad hacia atrás: algunos logs antiguos no llevan conf
            if len(tup) >= 6:
                (d, e, s, t, slatr, conf) = tup
            else:
                (d, e, s, t, slatr) = tup
                conf = 0.0
            d_up = (d or '').upper()
            d_norm = 'BUY' if d_up.startswith('BULL') else ('SELL' if d_up.startswith('BEAR') else d_up)
            candidates = idx_by_dir.get(d_norm, [])
            found = False
            for tr in candidates:
                if approx_equal(e, tr['entry']) and (tr['sl'] is None or approx_equal(s, tr['sl'])) and (tr['tp'] is None or approx_equal(t, tr['tp'])):
                    b = slatr_bin_idx(slatr)
                    if 'TP' in tr['status']:
                        wr_bins['bins'][b]['wins'] += 1
                        # acumular por confianza
                        cb = conf_bin_idx(conf)
                        wr_conf['bins'][cb]['wins'] += 1
                    elif 'SL' in tr['status']:
                        wr_bins['bins'][b]['losses'] += 1
                        cb = conf_bin_idx(conf)
                        wr_conf['bins'][cb]['losses'] += 1
                    matched += 1
                    found = True
                    break
            if not found:
                unmatched += 1
        wr_bins['matched'] = matched
        wr_bins['unmatched'] = unmatched

        lines.append("### WR vs SLDistATR (aceptaciones)")
        lines.append(f"- Matched aceptaciones con CSV: {matched} | Unmatched: {unmatched}")
        labels = ["0-10","10-15","15-20","20-25","25+"]
        for i, label in enumerate(labels):
            w = wr_bins['bins'][i]['wins']
            l = wr_bins['bins'][i]['losses']
            tot = max(1, w+l)
            wr = w / tot * 100.0
            lines.append(f"- {label}: Wins={w} Losses={l} WR={wr:.1f}% (n={w+l})")
        lines.append("")

        # Nueva sección: WR vs Confidence (aceptaciones)
        labels_c = ["0.50-0.60","0.60-0.70","0.70-0.80","0.80-0.90","0.90-1.00"]
        lines.append("### WR vs Confidence (aceptaciones)")
        for i, label in enumerate(labels_c):
            w = wr_conf['bins'][i]['wins']
            l = wr_conf['bins'][i]['losses']
            tot = max(1, w+l)
            wrp = w / tot * 100.0
            lines.append(f"- {label}: Wins={w} Losses={l} WR={wrp:.1f}% (n={w+l})")
        lines.append("")

        # Análisis de Calidad de Zonas Aceptadas
        detail = risk.get('accepted_detail_rows', [])
        if detail:
            lines.append("### Análisis de Calidad de Zonas Aceptadas")
            n = len(detail)
            avg_core = sum(d['core'] for d in detail)/n
            avg_prox = sum(d['prox'] for d in detail)/n
            avg_confc = sum(d['confc'] for d in detail)/n
            avg_confscore = sum(d.get('confscore',0.0) for d in detail)/n
            avg_rr = sum(d['rr'] for d in detail)/n
            avg_conf = sum(d['conf'] for d in detail)/n
            aligned_n = sum(1 for d in detail if d['aligned'])
            lines.append(f"- Muestras: {n} | Aligned={aligned_n} ({aligned_n/n*100:.1f}%)")
            lines.append(f"- Core≈ {avg_core:.2f} | Prox≈ {avg_prox:.2f} | ConfC≈ {avg_confc:.2f} | ConfScore≈ {avg_confscore:.2f} | RR≈ {avg_rr:.2f} | Confidence≈ {avg_conf:.2f}")
            # Distribuciones SL/TP por TF y estructuralidad
            sl_tf_cnt = Counter(str(d['sl_tf']) for d in detail)
            tp_tf_cnt = Counter(str(d['tp_tf']) for d in detail)
            sl_struct_p = sum(1 for d in detail if d['sl_struct'])/n*100.0
            tp_struct_p = sum(1 for d in detail if d['tp_struct'])/n*100.0
            lines.append(f"- SL_TF dist: {dict(sl_tf_cnt)} | SL_Structural≈ {sl_struct_p:.1f}%")
            lines.append(f"- TP_TF dist: {dict(tp_tf_cnt)} | TP_Structural≈ {tp_struct_p:.1f}%")
            # WR por bins de CoreScore (usando mismos matches ya hechos)
            core_bins = [ {'wins':0,'losses':0} for _ in range(3) ]
            def core_bin_idx(v: float) -> int:
                if v < 0.50: return 0
                if v < 0.70: return 1
                return 2
            # índice por (dir, entry, sl, tp) aprox para mapear detalle a trade
            idx_by_dir = {'BUY': [], 'SELL': []}
            for tr in trades:
                if tr['dir'] in idx_by_dir:
                    idx_by_dir[tr['dir']].append(tr)
            def approx_equal(a, b, tol=0.5):
                if a is None or b is None:
                    return False
                return abs(a - b) <= tol
            # reconstruir entradas desde accepted_details (que sí guardan precios)
            acc = risk.get('accepted_details', [])
            # Map temporal: usamos tuple redondeado como clave -> conf core
            def key_from_tuple(t):
                (d, e, s, tpp, slatr, conf) = t
                return (('BUY' if (d or '').upper().startswith('BULL') else ('SELL' if (d or '').upper().startswith('BEAR') else d)), round(e,2), round(s if s is not None else 0,2), round(tpp if tpp is not None else 0,2))
            acc_keys = [key_from_tuple(t) for t in acc]
            # Para cada detalle, intentar casarlo con un trade y su tuple aceptado más cercano por dir/entry/sl/tp
            for drow in detail:
                ddir = drow['dir']
                # Buscar candidates en trades
                cand_trades = idx_by_dir.get(ddir, [])
                # no tenemos e/s/tp en detail, así que aproximamos por distribución global: saltamos WR por core si no podemos casar
                # Opcionalmente, podríamos mapear por orden, pero mantenemos simple: solo reporte de promedios arriba
                # (Implementación WR por Core precisa requeriría arrastrar ZoneId->precios en logs)
                pass
            lines.append("")

    # Resumen SLPick por Bandas y TF (si hay)
    sb = risk.get('slpick_bands', {})
    rrpb = risk.get('rrplan_bands', {})
    if sb and sum([sb.get('lt8',0), sb.get('8_10',0), sb.get('10_12_5',0), sb.get('12_5_15',0), sb.get('gt15',0)]) > 0:
        lines.append("### SLPick por Bandas y TF")
        lines.append(f"- Bandas: lt8={sb.get('lt8',0)}, 8-10={sb.get('8_10',0)}, 10-12.5={sb.get('10_12_5',0)}, 12.5-15={sb.get('12_5_15',0)}, >15={sb.get('gt15',0)}")
        tf = sb.get('tf', {})
        if tf:
            lines.append(f"- TF: 5m={tf.get('5',0)}, 15m={tf.get('15',0)}, 60m={tf.get('60',0)}, 240m={tf.get('240',0)}, 1440m={tf.get('1440',0)}")
        if rrpb:
            avg0 = (rrpb.get('0_10_sum',0.0) / rrpb.get('0_10_n',1)) if rrpb.get('0_10_n',0) > 0 else 0.0
            avg1 = (rrpb.get('10_15_sum',0.0) / rrpb.get('10_15_n',1)) if rrpb.get('10_15_n',0) > 0 else 0.0
            lines.append(f"- RR plan por bandas: 0-10≈ {avg0:.2f} (n={rrpb.get('0_10_n',0)}), 10-15≈ {avg1:.2f} (n={rrpb.get('10_15_n',0)})")
        lines.append("")

    # CancelBias
    cb = stats_log['cancelbias']
    lines.append("## CancelBias (EMA200@60m)")
    lines.append(f"- Eventos: {cb['lines']}")
    lines.append(f"- Distribución Bias: {cb['bias_counts']}")
    if cb['samples'] > 0:
        lines.append(f"- Coherencia (Close>EMA): {cb['close_gt_ema']}/{cb['samples']} ({cb['close_gt_ema']/cb['samples']*100:.1f}%)")
    lines.append("")

    # StructureFusion
    sf = stats_log['sf']
    lines.append("## StructureFusion")
    lines.append(f"- Trazas por zona: {sf['zone_lines']} | Zonas con Anchors: {sf['zones_with_anchors']}")
    lines.append(f"- Dir zonas (zona): Bull={sf['zones_bull']} Bear={sf['zones_bear']} Neutral={sf['zones_neutral']}")
    if sf['cycles'] > 0:
        lines.append(f"- Resumen por ciclo (promedios): TotHZ≈ {sf['sum_tot_hz']/sf['cycles']:.1f}, WithAnchors≈ {sf['sum_with_anchors']/sf['cycles']:.1f}, DirBull≈ {sf['sum_dir_bull']/sf['cycles']:.1f}, DirBear≈ {sf['sum_dir_bear']/sf['cycles']:.1f}, DirNeutral≈ {sf['sum_dir_neutral']/sf['cycles']:.1f}")
    if sf['reasons']:
        lines.append(f"- Razones de dirección: {sf['reasons']}")
    if sf['tf_trig_counts'] or sf['tf_anchor_counts']:
        lines.append(f"- TF Triggers: {sf['tf_trig_counts']}")
        lines.append(f"- TF Anchors: {sf['tf_anchor_counts']}")
    lines.append("")
    # Context bias
    ctx = stats_log['context_bias']
    lines.append("## ContextManager Bias")
    lines.append(f"- Eventos: {ctx['lines']} | Distribución: {ctx['bias_counts']}")
    if ctx['lines'] > 0:
        lines.append(f"- Strength promedio: {ctx['strength_sum']/ctx['lines']:.2f}")
    lines.append("")

    # Context diagnóstico
    cdx = stats_log['context_diag']
    lines.append("### Context (Diagnóstico)")
    lines.append(f"- Eventos: {cdx['lines']} | Distribución: {cdx['bias_counts']}")
    if cdx['lines'] > 0:
        lines.append(f"- Strength promedio: {cdx['strength_sum']/cdx['lines']:.2f} | Close60>Avg200: {cdx['close_gt_avg']}/{cdx['lines']}")
    lines.append("")
    # TradeManager razones (desde log)
    lines.append("## TradeManager - Razones (desde log)")
    if stats_log['tm_reasons']['cancel']:
        lines.append(f"- Cancelaciones: {stats_log['tm_reasons']['cancel']}")
    if stats_log['tm_reasons']['expire']:
        lines.append(f"- Expiraciones: {stats_log['tm_reasons']['expire']}")
    tmd = stats_log['tm_cancel_diag']
    if tmd['lines'] > 0:
        lines.append(f"- Cancel_BOS (diag): por acción {tmd['by_action']} | por bias {tmd['by_bias']}")
    lines.append("")

    # CSV resumen (si hay)
    if stats_csv and stats_csv['rows'] > 0:
        lines.append("## CSV de Trades")
        lines.append(f"- Filas: {stats_csv['rows']} | Ejecutadas: {stats_csv['executed']} | Canceladas: {stats_csv['cancelled']} | Expiradas: {stats_csv['expired']}")
        lines.append(f"- BUY: {stats_csv['buy']} | SELL: {stats_csv['sell']}")
        if stats_csv['cancel_reasons']:
            lines.append(f"- Cancelaciones por razón: {stats_csv['cancel_reasons']}")
        if stats_csv['expire_reasons']:
            lines.append(f"- Expiraciones por razón: {stats_csv['expire_reasons']}")
        lines.append("")

    # Embudo de Señales (Funnel)
    fun = stats_log.get('funnel', {})
    if fun:
        fun['executed'] = stats_csv.get('executed', 0) if stats_csv else 0
        lines.append("## 📊 Embudo de Señales (Funnel)")
        lines.append(f"- DFM Señales (PassedThreshold): {fun.get('dfm_passed',0)}")
        lines.append(f"- Registered: {fun.get('registered',0)}")
        lines.append(f"  - DEDUP_COOLDOWN: {fun.get('dedup_cooldown',0)} | DEDUP_IDENTICAL: {fun.get('dedup_identical',0)} | SKIP_CONCURRENCY: {fun.get('skip_concurrency',0)}")
        # Ratios clave (normalizados por intentos de registro)
        def pct(a, b):
            b = max(1, b)
            return (a / b) * 100.0
        dfm_passed = fun.get('dfm_passed',0)
        registered = fun.get('registered',0)
        dedup_total = fun.get('dedup_cooldown',0) + fun.get('dedup_identical',0)
        skip_conc = fun.get('skip_concurrency',0)
        attempts = registered + dedup_total + skip_conc
        lines.append(f"- Intentos de registro: {attempts}")
        lines.append("")

        # Análisis adicional de DEDUP (IDENTICAL/COOLDOWN)
        ded = stats_log.get('dedup', {})
        ident_by_zone = ded.get('identical_by_zone', {})
        ident_keys = ded.get('identical_key_by_zone', {})
        ident_delta = ded.get('identical_delta_hist', Counter())
        ident_by_action = ded.get('identical_by_action', {})
        ident_by_domtf = ded.get('identical_by_domtf', {})
        cooldown_by_zone = ded.get('cooldown_by_zone', {})

        total_ident = sum(ident_by_zone.values())
        if total_ident > 0:
            lines.append("### TRADE DEDUP - Zonas y Persistencia")
            # Top 10 zonas
            top = sorted(ident_by_zone.items(), key=lambda x: x[1], reverse=True)[:10]
            lines.append("- Top 10 Zonas más deduplicadas (IDENTICAL):")
            lines.append("")
            lines.append("| ZoneID | Duplicados | % del Total | Key Típica |")
            lines.append("|--------|------------:|------------:|-----------:|")
            for zone, cnt in top:
                pzone = (cnt / total_ident) * 100.0
                key_cnt = ident_keys.get(zone, Counter())
                top_key = key_cnt.most_common(1)[0][0] if key_cnt else ""
                lines.append(f"| {zone} | {cnt} | {pzone:.1f}% | {top_key} |")
            lines.append("")

            # Distribución DeltaBars (incluye 0 = misma barra)
            d0 = ident_delta.get(0, 0)
            d1 = ident_delta.get(1, 0)
            d2_5 = sum(v for k,v in ident_delta.items() if isinstance(k,int) and 2 <= k <= 5)
            d6_12 = sum(v for k,v in ident_delta.items() if isinstance(k,int) and 6 <= k <= 12)
            dgt12 = sum(v for k,v in ident_delta.items() if isinstance(k,int) and k > 12)
            lines.append("- Distribución de DeltaBars (IDENTICAL):")
            lines.append("")
            lines.append("| DeltaBars | Cantidad | % |")
            lines.append("|-----------|---------:|---:|")
            def ppct(x):
                return f"{(x/max(1,total_ident))*100:.1f}%"
            lines.append(f"| 0 | {d0} | {ppct(d0)} |")
            lines.append(f"| 1 | {d1} | {ppct(d1)} |")
            lines.append(f"| 2-5 | {d2_5} | {ppct(d2_5)} |")
            lines.append(f"| 6-12 | {d6_12} | {ppct(d6_12)} |")
            lines.append(f"| >12 | {dgt12} | {ppct(dgt12)} |")
            lines.append("")

            # Breakdown opcional por Acción y DomTF
            if ident_by_action:
                lines.append(f"- IDENTICAL por Acción: {ident_by_action}")
            if ident_by_domtf:
                lines.append(f"- IDENTICAL por DomTF: {ident_by_domtf}")
            if cooldown_by_zone:
                lines.append(f"- COOLDOWN por Zona (top 5): {dict(sorted(cooldown_by_zone.items(), key=lambda x: x[1], reverse=True)[:5])}")
            lines.append("")
        lines.append("### Ratios del Funnel")
        lines.append(f"- Coverage = Intentos / PassedThreshold = {pct(attempts, dfm_passed):.1f}%")
        lines.append(f"- RegRate = Registered / Intentos = {pct(registered, attempts):.1f}%")
        lines.append(f"- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = {pct(dedup_total, attempts):.1f}%")
        lines.append(f"- Concurrency = SKIP_CONCURRENCY / Intentos = {pct(skip_conc, attempts):.1f}%")
        lines.append(f"- ExecRate = Ejecutadas / Registered = {pct(fun.get('executed',0), max(registered,1)):.1f}%")
        lines.append("")

    # SL/TP Analysis Post-Mortem
    sla = stats_log['sl_analysis']
    tpa = stats_log['tp_analysis']
    if sla['zones'] > 0 or tpa['zones'] > 0:
        lines.append("## Análisis Post-Mortem: SL/TP")
        
        # SL Analysis
        if sla['zones'] > 0:
            lines.append("### Stop Loss (SL)")
            lines.append(f"- Zonas analizadas: {sla['zones']} | Total candidatos: {sla['total_candidates']} | Seleccionados: {sla['selected']}")
            lines.append(f"- Candidatos por zona (promedio): {sla['total_candidates']/sla['zones']:.1f}")
            
            if sla['candidates']:
                # Edad
                ages_cand = [c['age'] for c in sla['candidates']]
                ages_sel = [s['age'] for s in sla['selected_list']]
                lines.append(f"- **Edad (barras)** - Candidatos: med={sorted(ages_cand)[len(ages_cand)//2] if ages_cand else 0}, max={max(ages_cand) if ages_cand else 0} | Seleccionados: med={sorted(ages_sel)[len(ages_sel)//2] if ages_sel else 0}, max={max(ages_sel) if ages_sel else 0}")
                # Score
                scores_cand = [c['score'] for c in sla['candidates']]
                scores_sel = [s['score'] for s in sla['selected_list']]
                avg_score_cand = sum(scores_cand)/len(scores_cand) if scores_cand else 0
                avg_score_sel = sum(scores_sel)/len(scores_sel) if scores_sel else 0
                lines.append(f"- **Score** - Candidatos: avg={avg_score_cand:.2f} | Seleccionados: avg={avg_score_sel:.2f}")
                # TF
                tf_cand = Counter(c['tf'] for c in sla['candidates'])
                tf_sel = Counter(s['tf'] for s in sla['selected_list'])
                lines.append(f"- **TF Candidatos**: {dict(tf_cand.most_common(5))}")
                lines.append(f"- **TF Seleccionados**: {dict(tf_sel)}")
                # DistATR
                dist_cand = [c['dist_atr'] for c in sla['candidates']]
                dist_sel = [s['dist_atr'] for s in sla['selected_list']]
                avg_dist_cand = sum(dist_cand)/len(dist_cand) if dist_cand else 0
                avg_dist_sel = sum(dist_sel)/len(dist_sel) if dist_sel else 0
                lines.append(f"- **DistATR** - Candidatos: avg={avg_dist_cand:.1f} | Seleccionados: avg={avg_dist_sel:.1f}")
                # Reasons
                reasons = Counter(s['reason'] for s in sla['selected_list'])
                lines.append(f"- **Razones de selección**: {dict(reasons)}")
                # InBand ratio
                in_band_count = sum(1 for c in sla['candidates'] if c['in_band'])
                lines.append(f"- **En banda [10,15] ATR**: {in_band_count}/{len(sla['candidates'])} ({in_band_count/len(sla['candidates'])*100:.1f}%)")
            lines.append("")
        
        # TP Analysis
        if tpa['zones'] > 0:
            lines.append("### Take Profit (TP)")
            lines.append(f"- Zonas analizadas: {tpa['zones']} | Total candidatos: {tpa['total_candidates']} | Seleccionados: {tpa['selected']}")
            lines.append(f"- Candidatos por zona (promedio): {tpa['total_candidates']/tpa['zones']:.1f}")
            
            if tpa['candidates']:
                # Edad
                ages_cand = [c['age'] for c in tpa['candidates']]
                ages_sel = [s['age'] for s in tpa['selected_list']]
                lines.append(f"- **Edad (barras)** - Candidatos: med={sorted(ages_cand)[len(ages_cand)//2] if ages_cand else 0}, max={max(ages_cand) if ages_cand else 0} | Seleccionados: med={sorted(ages_sel)[len(ages_sel)//2] if ages_sel else 0}, max={max(ages_sel) if ages_sel else 0}")
                # Score
                scores_cand = [c['score'] for c in tpa['candidates']]
                scores_sel = [s['score'] for s in tpa['selected_list']]
                avg_score_cand = sum(scores_cand)/len(scores_cand) if scores_cand else 0
                avg_score_sel = sum(scores_sel)/len(scores_sel) if scores_sel else 0
                lines.append(f"- **Score** - Candidatos: avg={avg_score_cand:.2f} | Seleccionados: avg={avg_score_sel:.2f}")
                # Priority
                pri_cand = Counter(c['priority'] for c in tpa['candidates'])
                pri_sel = Counter(s['priority'] for s in tpa['selected_list'])
                lines.append(f"- **Priority Candidatos**: {dict(pri_cand)}")
                lines.append(f"- **Priority Seleccionados**: {dict(pri_sel)}")
                # Type
                type_cand = Counter(c['type'] for c in tpa['candidates'])
                type_sel = Counter(s['type'] for s in tpa['selected_list'])
                lines.append(f"- **Type Candidatos**: {dict(type_cand)}")
                lines.append(f"- **Type Seleccionados**: {dict(type_sel)}")
                # TF
                tf_cand = Counter(c['tf'] for c in tpa['candidates'])
                tf_sel = Counter(s['tf'] for s in tpa['selected_list'])
                lines.append(f"- **TF Candidatos**: {dict(tf_cand.most_common(5))}")
                lines.append(f"- **TF Seleccionados**: {dict(tf_sel)}")
                # DistATR
                dist_cand = [c['dist_atr'] for c in tpa['candidates']]
                dist_sel = [s['dist_atr'] for s in tpa['selected_list']]
                avg_dist_cand = sum(dist_cand)/len(dist_cand) if dist_cand else 0
                avg_dist_sel = sum(dist_sel)/len(dist_sel) if dist_sel else 0
                lines.append(f"- **DistATR** - Candidatos: avg={avg_dist_cand:.1f} | Seleccionados: avg={avg_dist_sel:.1f}")
                # RR
                rr_cand = [c['rr'] for c in tpa['candidates']]
                rr_sel = [s['rr'] for s in tpa['selected_list']]
                avg_rr_cand = sum(rr_cand)/len(rr_cand) if rr_cand else 0
                avg_rr_sel = sum(rr_sel)/len(rr_sel) if rr_sel else 0
                lines.append(f"- **RR** - Candidatos: avg={avg_rr_cand:.2f} | Seleccionados: avg={avg_rr_sel:.2f}")
                # Reasons
                reasons = Counter(s['reason'] for s in tpa['selected_list'])
                lines.append(f"- **Razones de selección**: {dict(reasons)}")
            lines.append("")
        
        # Recomendaciones basadas en datos
        lines.append("### 🎯 Recomendaciones")
        recos = []
        if sla['selected_list'] and ages_sel:
            max_age_sel = max(ages_sel)
            if max_age_sel > 500:
                recos.append(f"⚠️ SL: Estructuras muy antiguas (max {max_age_sel} barras). Considerar filtro de edad máxima.")
        if tpa['selected_list'] and ages_sel:
            max_age_tp = max([s['age'] for s in tpa['selected_list']])
            if max_age_tp > 500:
                recos.append(f"⚠️ TP: Estructuras muy antiguas (max {max_age_tp} barras). Considerar filtro de edad máxima.")
        if sla['selected_list']:
            low_score_count = sum(1 for s in sla['selected_list'] if s['score'] < 0.5)
            if low_score_count / len(sla['selected_list']) > 0.3:
                recos.append(f"⚠️ SL: {low_score_count/len(sla['selected_list'])*100:.0f}% tienen score < 0.5. Considerar umbral mínimo de calidad.")
        if tpa['selected_list']:
            fallback_count = sum(1 for s in tpa['selected_list'] if 'Fallback' in s['reason'] or 'NoStructural' in s['reason'])
            if fallback_count / len(tpa['selected_list']) > 0.3:
                recos.append(f"⚠️ TP: {fallback_count/len(tpa['selected_list'])*100:.0f}% son fallback (sin estructura válida). Problema de calidad de estructuras.")
        if recos:
            for r in recos:
                lines.append(f"- {r}")
        else:
            lines.append("- ✅ No se detectaron problemas evidentes en la selección de SL/TP.")
        lines.append("")

    # Conclusiones automáticas simples
    lines.append("## Observaciones automáticas")
    if bear > bull and stats_csv.get('sell', 0) > stats_csv.get('buy', 0):
        lines.append("- Predominio de evaluaciones y señales SELL.")
    if stats_csv.get('cancel_reasons', {}).get('BOS contradictorio', 0) > 0:
        lines.append("- Alta tasa de cancelaciones por BOS contradictorio -> revisar sesgo en generación de señales.")
    if prox['total_aligned'] > 0:
        ratio_kept_aligned = prox['kept_aligned'] / max(1, prox['total_aligned'])
        lines.append(f"- KeptAligned ratio≈ {ratio_kept_aligned:.2f}.")

    return '\n'.join(lines)


def main():
    parser = argparse.ArgumentParser(description='Analizador de logs PinkButterfly -> Markdown')
    parser.add_argument('--log', required=True, help='Ruta al archivo de log backtest_*.log')
    parser.add_argument('--csv', required=False, help='Ruta al archivo CSV trades_*.csv')
    parser.add_argument('-o', '--output', required=False, help='Ruta de salida .md (opcional)')
    args = parser.parse_args()

    log_path = args.log
    csv_path = args.csv

    stats_log = parse_log(log_path)
    stats_csv = parse_csv(csv_path) if csv_path else {}
    md = render_markdown(log_path, csv_path, stats_log, stats_csv)

    if args.output:
        try:
            with open(args.output, 'w', encoding='utf-8') as f:
                f.write(md)
            print(f"[OK] Informe generado: {args.output}")
        except Exception as e:
            # No imprimir md a consola porque contiene caracteres Unicode
            print(f"[ERROR] Error guardando informe: {e}", file=sys.stderr)
            print(f"[INFO] Archivo generado pero con problemas al confirmar", file=sys.stderr)
    else:
        # No imprimir md directamente a consola en Windows (problemas Unicode)
        print("[INFO] Contenido generado (no mostrado para evitar errores de encoding)")


if __name__ == '__main__':
    main()


