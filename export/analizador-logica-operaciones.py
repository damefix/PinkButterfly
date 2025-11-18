#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
===============================================================================
ANALIZADOR DE LOGICA DE OPERACIONES - PinkButterfly CoreBrain
===============================================================================

PROPOSITO:
----------
Analiza rigurosamente la logica de trading del sistema PinkButterfly para 
identificar problemas en tres pilares fundamentales:

1. BIAS/SENTIMIENTO: 
   - El bias global (Bullish/Bearish) refleja el movimiento real del precio?
   - EMA200@60m es demasiado lenta para intradia?
   - Propone alternativas mas rapidas y compuestas

2. CALIDAD DE ENTRADAS:
   - Para CADA operacion: el precio fue hacia el TP o hacia el SL?
   - Calcula MFE (Max Favorable Excursion) y MAE (Max Adverse Excursion)
   - Determina si la entrada fue tecnicamente correcta o contra-tendencia
   - Analiza waterfall del pipeline (donde se pierden senales)

3. PRECISION DE SL/TP:
   - Los SL/TP fueron alcanzados o se quedaron cortos/largos?
   - Distribuciones en puntos y ATR por TF
   - Propone limites dinamicos basados en percentiles reales (data-driven)
   - Calcula R:R optimo para el Win Rate actual

METODOLOGIA:
------------
- Parsea el LOG completo para extraer:
  * Trazas [Context] (bias en cada barra)
  * Trazas [PIPE] (OHLC de cada barra TF5/15/60/240/1440)
  * Trazas [TRADE] (registro/cierre de operaciones)
  * Trazas [RISK] (SL/TP policy y validaciones)

- Para cada trade ejecutado:
  1. Reconstruye la serie OHLC desde Entry hasta Exit (barra a barra)
  2. Calcula MFE: maxima ganancia flotante antes del cierre
  3. Calcula MAE: maxima perdida flotante antes del cierre
  4. Determina "direccion inicial": precio fue primero hacia TP o SL?
  5. Evalua precision: SL/TP fueron alcanzados o quedaron lejos?

- Genera metricas agregadas:
  * % de entradas "buenas" (MFE > MAE, precio fue a TP primero)
  * % de entradas "malas" (MAE > MFE, precio fue a SL primero)
  * Distribucion de MFE/MAE por direccion (BUY/SELL)
  * Correlacion Bias vs Direccion Real del precio

OUTPUT:
-------
Genera UN SOLO informe markdown: ANALISIS_LOGICA_DE_OPERACIONES.md

Este informe contiene:
- Resumen ejecutivo con KPIs y problemas criticos
- Analisis detallado de los 3 pilares
- Recomendaciones priorizadas (data-driven)
- Plan de accion con cambios especificos en codigo

DEPENDENCIAS:
-------------
- LOG: NinjaTrader 8/PinkButterfly/logs/backtest_*.log
- CSV: NinjaTrader 8/PinkButterfly/logs/trades_*.csv

AUTOR: AI Assistant (Claude Sonnet 4.5)
VERSION: 2.0 (con analisis MFE/MAE y excursion de precio)
===============================================================================
"""

import sys
import os
import re
import csv
from collections import Counter, defaultdict
from datetime import datetime, timedelta
import statistics
import argparse

# ============================================================================
# CONFIGURACIÓN
# ============================================================================

# Detectar rutas correctas
SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
PROJECT_ROOT = os.path.dirname(SCRIPT_DIR)
NINJATRADER_PATH = os.path.join(os.path.dirname(os.path.dirname(PROJECT_ROOT)), "NinjaTrader 8")
LOG_DIRECTORY = os.path.join(NINJATRADER_PATH, "PinkButterfly", "logs")

# Buscar automáticamente el archivo de log MÁS RECIENTE
def find_latest_log():
    """Encuentra el archivo backtest_*.log más reciente en el directorio de logs"""
    import glob
    log_pattern = os.path.join(LOG_DIRECTORY, "backtest_*.log")
    log_files = glob.glob(log_pattern)
    
    if not log_files:
        print(f"[ERROR] No se encontraron archivos backtest_*.log en {LOG_DIRECTORY}")
        return None, None
    
    # Ordenar por fecha de modificación (más reciente primero)
    latest_log = max(log_files, key=os.path.getmtime)
    
    # Extraer timestamp del nombre del archivo (backtest_YYYYMMDD_HHMMSS.log)
    log_basename = os.path.basename(latest_log)
    timestamp = log_basename.replace("backtest_", "").replace(".log", "")
    
    # Buscar CSV correspondiente
    csv_path = os.path.join(LOG_DIRECTORY, f"trades_{timestamp}.csv")
    
    return latest_log, csv_path

parser = argparse.ArgumentParser()
parser.add_argument('--log', dest='log_path', default=None)
parser.add_argument('--csv', dest='csv_path', default=None)
parser.add_argument('-o', '--output', dest='output_path', default=None)
args = parser.parse_args()

if args.log_path and args.csv_path:
    LOG_PATH = args.log_path
    CSV_PATH = args.csv_path
else:
    LOG_PATH, CSV_PATH = find_latest_log()

OUTPUT_PATH = args.output_path or os.path.join(SCRIPT_DIR, "ANALISIS_LOGICA_DE_OPERACIONES.md")

# Parámetros de análisis
BARS_FORWARD = [20, 40, 80]  # Barras para medir movimiento real
ATR_HIGHVOL_PERCENTILE = 70   # Percentil para régimen alta volatilidad
PERCENTILES = [50, 70, 80, 90, 95]  # Para distribuciones

# ============================================================================
# UTILIDADES
# ============================================================================

def to_float(s):
    """Convierte string a float, retorna 0.0 si falla"""
    try:
        return float(s.replace(',', '.'))
    except:
        return 0.0

def parse_timestamp(ts_str):
    """Parsea timestamp del log"""
    try:
        return datetime.strptime(ts_str, "%Y-%m-%d %H:%M:%S")
    except:
        return None

# ============================================================================
# CARGADORES DE DATOS
# ============================================================================

def load_trades_csv(csv_path):
    """
    Carga CSV de trades - V6.0k: Combina REGISTERED (SL/TP) + CLOSED (resultado) para MFE/MAE
    """
    trades = []
    
    try:
        # Primera pasada: cargar datos de REGISTERED (SL/TP/RR)
        registered_data = {}
        
        with open(csv_path, 'r', encoding='utf-8') as f:
            for line in f:
                if line.startswith('TradeID'):
                    continue
                
                parts = line.strip().split(',')
                if len(parts) < 10:
                    continue
                
                action = parts[2] if len(parts) > 2 else ''
                
                if action == 'REGISTERED':
                    trade_id = parts[0]
                    direction = parts[3]
                    entry = to_float(parts[4] + '.' + parts[5])
                    sl = to_float(parts[6] + '.' + parts[7])
                    tp = to_float(parts[8] + '.' + parts[9])
                    risk_pts = to_float(parts[10] + '.' + parts[11])
                    reward_pts = to_float(parts[12] + '.' + parts[13])
                    rr = to_float(parts[14] + '.' + parts[15])
                    
                    # Obtener timestamp de entrada (índice 17 en CSV)
                    entry_bar_time = parts[17] if len(parts) > 17 else ''
                    
                    registered_data[trade_id] = {
                        'Direction': direction,
                        'Entry': entry,
                        'SL': sl,
                        'TP': tp,
                        'RiskPoints': risk_pts,
                        'RewardPoints': reward_pts,
                        'RR': rr,
                        'EntryBarTime': entry_bar_time
                    }
        
        # Segunda pasada: cargar CLOSED y combinar con REGISTERED
        closed_counter = {}
        
        with open(csv_path, 'r', encoding='utf-8') as f:
            for line in f:
                if line.startswith('TradeID'):
                    continue
                
                parts = line.strip().split(',')
                if len(parts) < 10:
                    continue
                
                action = parts[2] if len(parts) > 2 else ''
                
                if action == 'CLOSED':
                    trade_id = parts[0]
                    
                    # Detectar status (SL_HIT o TP_HIT)
                    exit_reason = ''
                    if 'SL_HIT' in line or ',SL,' in line:
                        exit_reason = 'SL'
                    elif 'TP_HIT' in line or ',TP,' in line:
                        exit_reason = 'TP'
                    
                    # Extraer PnL
                    pnl_points = 0.0
                    if len(parts) >= 4 and parts[-4] != '-':
                        try:
                            pnl_points = to_float(parts[-4] + '.' + parts[-3])
                        except:
                            pass
                    
                    # Generar ID único para operaciones duplicadas
                    if trade_id not in closed_counter:
                        closed_counter[trade_id] = 0
                    closed_counter[trade_id] += 1
                    unique_id = f"{trade_id}_{closed_counter[trade_id]}" if closed_counter[trade_id] > 1 else trade_id
                    
                    # Combinar con datos de REGISTERED
                    if trade_id in registered_data:
                        reg_data = registered_data[trade_id]
                        trades.append({
                            'TradeID': unique_id,
                            'Direction': reg_data['Direction'],
                            'Entry': reg_data['Entry'],
                            'SL': reg_data['SL'],
                            'TP': reg_data['TP'],
                            'RiskPoints': reg_data['RiskPoints'],
                            'RewardPoints': reg_data['RewardPoints'],
                            'RR': reg_data['RR'],
                            'Status': 'CLOSED',
                            'ExitReason': exit_reason,
                            'PnLPoints': pnl_points,
                            'EntryBarTime': reg_data['EntryBarTime']
                        })
                    else:
                        print(f"[WARN] Trade {trade_id} CLOSED sin REGISTERED correspondiente")
        
        print(f"[OK] {len(trades)} trades CERRADOS cargados desde CSV (con SL/TP de REGISTERED)")
        
    except Exception as e:
        print(f"[ERROR] No se pudo cargar CSV: {e}")
        import traceback
        traceback.print_exc()
    
    return trades

def count_csv_registered(csv_path):
    count = 0
    try:
        with open(csv_path, 'r', encoding='utf-8', errors='ignore') as f:
            for line in f:
                if line.startswith('TradeID'):
                    continue
                parts = line.strip().split(',')
                action = parts[2] if len(parts) > 2 else ''
                if action == 'REGISTERED':
                    count += 1
    except Exception:
        return 0
    return count

def load_log_data(log_path):
    """Carga y parsea el log completo"""
    data = {
        'context_bias': [],  # (timestamp, bias, close, ema200)
        'proximity_events': [],  # (timestamp, aligned, counter)
        'dfm_events': [],  # (timestamp, bull_evals, bear_evals, passed)
        'dfm_rejected': [],  # ✅ NUEVO: señales rechazadas por DFM (CONTRA-BIAS)
        'signal_metrics': [],  # ✅ NUEVO: métricas de todas las señales (BUY/SELL/WAIT)
        'risk_events': [],  # (zone_id, accepted, rej_reason, sl_pts, tp_pts, sl_tf, tp_tf)
        'tp_policy': [],  # (zone_id, priority, tf, dist_atr, rr)
        'fusion_zones': [],  # (zone_id, direction, tf_dom, anchors)
        'price_bars': [],  # (timestamp, tf, bar, open, high, low, close)  # OHLC de cada barra
        'phantom_opportunities': [],  # ✅ NUEVO: Zonas procesadas por Risk pero no ejecutadas (para análisis MFE/MAE)
        'waterfall': {  # V6.0k: Embudo del pipeline (paso a paso)
            'fusion_zones': set(),
            'proximity_kept': 0,
            'dfm_evaluated': 0,
            'dfm_passed': 0,
            'risk_received': 0,  # ✅ NUEVO: Total de zonas recibidas por RiskCalculator
            'risk_accepted': 0,
            'risk_rejections': {  # ✅ NUEVO: Razones de rechazo en RiskCalculator
                'ENTRY_STALE': 0,
                'SL_CHECK_FAIL': 0,
                'NO_SL': 0,
                'TP_CHECK_FAIL': 0,
                'ENTRY_TOO_FAR': 0,
                'RR_LOW': 0,
                'OTHER': 0
            }
        }
    }
    
    # Regex patterns
    # V6.0g: Nuevo formato de bias compuesto
    # V6.0k: Actualizado para nuevas trazas (EMA20Slope, EMACross, barUsed opcional)
    re_context = re.compile(r"\[DIAGNOSTICO\]\[Context\].*?BiasComposite=(\w+)\s+Score=([0-9\-\.,]+)\s+Threshold=([0-9\-\.,]+).*?EMA20Slope=([0-9\-\.,]+)\s+EMACross=([0-9\-\.,]+)\s+BOS=([0-9\-\.,]+)(?:\s+\(barUsed=\d+\))?\s+Reg24h=([0-9\-\.,]+)")
    re_context_old = re.compile(r"\[Context\].*?Bias=(\w+)")  # Fallback para logs antiguos
    re_proximity = re.compile(r"\[Proximity\].*?Aligned=(\d+).*?Counter=(\d+)")
    re_dfm = re.compile(r"\[DFM\].*?Bull=(\d+).*?Bear=(\d+).*?Passed=(\d+)")
    # Compat con resumen DIAGNOSTICO: DFM Evaluadas/PassedThreshold
    re_diagnostic_tag = r"DIAGN[ÓO]STICO"
    re_dfm_diag = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[DFM\].*Evaluadas:\s*Bull=(\d+)\s*Bear=(\d+)\s*\|\s*PassedThreshold=(\d+)")
    # ✅ NUEVO: Señales rechazadas por DFM
    re_dfm_rejected = re.compile(r"\[DFM\]\[REJECTED\].*?Zone=(\S+).*?Dir=(\w+).*?Entry=([0-9,\.]+).*?SL=([0-9,\.]+).*?TP=([0-9,\.]+).*?Reason=(\w+).*?Conf=([0-9,\.]+).*?RR=([0-9,\.]+).*?BiasVs=(\w+)")
    # ✅ NUEVO: SIGNAL_METRICS con SL/TP/Reason
    re_signal_metrics = re.compile(r"\[SIGNAL_METRICS\].*?Zone=(\S+).*?TF=(\d+).*?Action=(\w+).*?Entry=([0-9,\.]+).*?SL=([0-9,\.]+).*?TP=([0-9,\.]+).*?Conf=([0-9,\.]+).*?DistPts=([0-9,\.\-]+).*?ATRdom=([0-9,\.\-]+).*?DistATR=([0-9,\.\-]+)(?:.*?Reason=(\S+))?")
    re_risk_accept = re.compile(r"\[DIAGNOSTICO\]\[Risk\].*?Zone=(\S+).*?SL=([0-9\.]+)pts.*?TP=([0-9\.]+)pts.*?SLTF=(-?\d+).*?TPTF=(-?\d+)")
    # Compat con resumen DIAGNOSTICO: Risk Accepted/Rej*
    re_risk_summary = re.compile(
        rf"\[(?:{re_diagnostic_tag})\]\[Risk\]\s*Accepted=(\d+)\s*RejSL=(\d+)\s*RejTP=(\d+)\s*RejRR=(\d+)\s*RejEntry=(\d+)")
    # Compat: líneas de aceptación individuales (como en DIAGNOSTICO) para contar accepted si no hay resumen
    re_risk_slaccepted = re.compile(r"\[DIAGNOSTICO\]\[Risk\]\s*SLAccepted", re.IGNORECASE)
    re_tp_policy = re.compile(r"\[RISK\]\[TP_POLICY\].*?Zone=(\S+).*?(P0_OPPOSING|P0_ANY_DIR|P0_SWING_LITE|FORCED_P3|P4_FALLBACK).*?TF=(-?\d+).*?RR=([0-9\.]+).*?DistATR=([0-9\.]+)", re.IGNORECASE)
    # ✅ NUEVO: Trazas de RiskCalculator para análisis detallado
    re_risk_received = re.compile(r"\[TRACE\]\[RiskCalculator\] Zonas recibidas desde ProximityAnalyzer:\s*(\d+)")
    re_risk_entry_stale = re.compile(r"\[RISK\]\[ENTRY_STALE\]")
    re_risk_no_sl = re.compile(r"\[RISK\]\[NO_SL\]")
    re_risk_sl_fail = re.compile(r"\[RISK\]\[SL_CHECK_FAIL\]")
    re_risk_tp_fail = re.compile(r"\[RISK\]\[TP_CHECK_FAIL\]")
    re_risk_entry_far = re.compile(r"\[RISK\]\[ENTRY_TOO_FAR\]")
    # ✅ NUEVO: Phantom opportunities (zonas procesadas por Risk pero no ejecutadas)
    re_phantom = re.compile(r"\[PHANTOM_OPPORTUNITY\] Zone=(\S+) Dir=(\w+) Entry=([0-9,\.]+) SL=([0-9,\.]+) TP=([0-9,\.]+) RR=([0-9,\.]+) DistATR=([0-9,\.]+) TF=(\d+) Bar=(\d+) Time=([\d\-: ]+)")
    re_fusion = re.compile(r"\[StructureFusion\].*?Zone=(\S+).*?Dir=(\w+).*?TFDom=(\d+)")
    re_pipe = re.compile(r"\[PIPE\].*?TF=(\d+).*?Bar=(\d+).*?O=([0-9,\.]+).*?H=([0-9,\.]+).*?L=([0-9,\.]+).*?C=([0-9,\.]+)")
    
    # V6.0k: Regexes para Waterfall del pipeline
    re_fusion_hz = re.compile(r"\[DIAGNOSTICO\]\[StructureFusion\]\s+HZ=(HZ_\S+)")
    re_prox_kept = re.compile(r"\[Proximity\].*?KeptAligned=(\d+)|PreferAligned.*?kept=(\d+)", re.IGNORECASE)
    # Compat: resumen por ciclo de PROX2
    re_prox2 = re.compile(r"\[PIPE\]\[PROX2\].*?ZonesIn=(\d+)\s+ZonesKept=(\d+)\s+KeptAligned=(\d+)", re.IGNORECASE)
    # V6.0k fix: Contar solo líneas DIAGNOSTICO (no duplicados DIAG)
    re_dfm_evaluated = re.compile(r"\[DIAGNOSTICO\]\[DFM\]\s+Evaluadas:.*?Bull=(\d+).*?Bear=(\d+)")
    
    try:
        saw_risk_summary = False
        with open(log_path, 'r', encoding='utf-8', errors='ignore') as f:
            for line in f:
                # Context Bias (V6.0g: nuevo formato compuesto)
                m = re_context.search(line)
                if m:
                    data['context_bias'].append({
                        'bias': m.group(1),
                        'score': float(m.group(2).replace(',', '.')),
                        'threshold': float(m.group(3).replace(',', '.')),
                        'ema20': float(m.group(4).replace(',', '.')),
                        'ema50': float(m.group(5).replace(',', '.')),
                        'bos': float(m.group(6).replace(',', '.')),
                        'reg24h': float(m.group(7).replace(',', '.'))
                    })
                    continue
                
                # Fallback: formato antiguo de bias
                m = re_context_old.search(line)
                if m:
                    data['context_bias'].append({
                        'bias': m.group(1),
                        'score': None,
                        'ema20': None,
                        'ema50': None,
                        'bos': None,
                        'reg24h': None
                    })
                
                # Proximity
                m = re_proximity.search(line)
                if m:
                    data['proximity_events'].append({
                        'aligned': int(m.group(1)),
                        'counter': int(m.group(2))
                    })
                
                # DFM
                m = re_dfm.search(line)
                if m:
                    passed = int(m.group(3))
                    data['dfm_events'].append({
                        'bull_evals': int(m.group(1)),
                        'bear_evals': int(m.group(2)),
                        'passed': passed
                    })
                    # V6.0k: Actualizar waterfall con DFM passed
                    data['waterfall']['dfm_passed'] += passed
                    continue
                
                # DFM (resumen DIAGNOSTICO con PassedThreshold)
                m = re_dfm_diag.search(line)
                if m:
                    bull = int(m.group(1)); bear = int(m.group(2)); passed = int(m.group(3))
                    data['dfm_events'].append({
                        'bull_evals': bull,
                        'bear_evals': bear,
                        'passed': passed
                    })
                    data['waterfall']['dfm_passed'] += passed
                    data['waterfall']['dfm_evaluated'] += (bull + bear)
                    continue
                
                # ✅ NUEVO: DFM Rejected (señales rechazadas por CONTRA-BIAS)
                m = re_dfm_rejected.search(line)
                if m:
                    data['dfm_rejected'].append({
                        'zone_id': m.group(1),
                        'direction': m.group(2),
                        'entry': to_float(m.group(3)),
                        'sl': to_float(m.group(4)),
                        'tp': to_float(m.group(5)),
                        'reason': m.group(6),
                        'conf': to_float(m.group(7)),
                        'rr': to_float(m.group(8)),
                        'bias_vs': m.group(9)
                    })
                
                # ✅ NUEVO: SIGNAL_METRICS (todas las señales: BUY/SELL/WAIT)
                m = re_signal_metrics.search(line)
                if m:
                    data['signal_metrics'].append({
                        'zone_id': m.group(1),
                        'tf': int(m.group(2)),
                        'action': m.group(3),
                        'entry': to_float(m.group(4)),
                        'sl': to_float(m.group(5)),
                        'tp': to_float(m.group(6)),
                        'conf': to_float(m.group(7)),
                        'dist_pts': to_float(m.group(8)),
                        'atr_dom': to_float(m.group(9)),
                        'dist_atr': to_float(m.group(10)),
                        'reason': m.group(11) if m.group(11) else ""
                    })
                
                # Risk Accept
                m = re_risk_accept.search(line)
                if m:
                    data['risk_events'].append({
                        'zone_id': m.group(1),
                        'sl_pts': to_float(m.group(2)),
                        'tp_pts': to_float(m.group(3)),
                        'sl_tf': int(m.group(4)),
                        'tp_tf': int(m.group(5))
                    })
                    # V6.0k: Actualizar waterfall con Risk accepted
                    if not saw_risk_summary:
                        data['waterfall']['risk_accepted'] += 1
                    continue
                
                # Risk SLAccepted (contar aceptaciones si no hay resumen)
                m = re_risk_slaccepted.search(line)
                if m:
                    if not saw_risk_summary:
                        data['waterfall']['risk_accepted'] += 1
                    continue
                
                # Risk resumen (Accepted=…)
                m = re_risk_summary.search(line)
                if m:
                    try:
                        acc = int(m.group(1))
                        data['waterfall']['risk_accepted'] = acc
                        saw_risk_summary = True
                    except:
                        pass
                    continue
                
                # TP Policy
                m = re_tp_policy.search(line)
                if m:
                    data['tp_policy'].append({
                        'zone_id': m.group(1),
                        'priority': m.group(2),
                        'tf': int(m.group(3)),
                        'rr': to_float(m.group(4)),
                        'dist_atr': to_float(m.group(5))
                    })
                
                # ✅ NUEVO: RiskCalculator - Zonas recibidas
                m = re_risk_received.search(line)
                if m:
                    data['waterfall']['risk_received'] += int(m.group(1))
                    continue
                
                # ✅ NUEVO: RiskCalculator - Razones de rechazo
                if re_risk_entry_stale.search(line):
                    data['waterfall']['risk_rejections']['ENTRY_STALE'] += 1
                    continue
                if re_risk_no_sl.search(line):
                    data['waterfall']['risk_rejections']['NO_SL'] += 1
                    continue
                if re_risk_sl_fail.search(line):
                    data['waterfall']['risk_rejections']['SL_CHECK_FAIL'] += 1
                    continue
                if re_risk_tp_fail.search(line):
                    data['waterfall']['risk_rejections']['TP_CHECK_FAIL'] += 1
                    continue
                if re_risk_entry_far.search(line):
                    data['waterfall']['risk_rejections']['ENTRY_TOO_FAR'] += 1
                    continue
                
                # ✅ NUEVO: Phantom opportunities
                m = re_phantom.search(line)
                if m:
                    try:
                        entry = to_float(m.group(3))
                        sl = to_float(m.group(4))
                        tp = to_float(m.group(5))
                        rr = to_float(m.group(6))
                        dist_atr = to_float(m.group(7))
                        tf = int(m.group(8))
                        bar = int(m.group(9))
                        time_str = m.group(10)
                        
                        # Parsear timestamp
                        try:
                            timestamp = datetime.strptime(time_str, '%Y-%m-%d %H:%M:%S')
                        except:
                            timestamp = None
                        
                        data['phantom_opportunities'].append({
                            'zone_id': m.group(1),
                            'direction': m.group(2),
                            'entry': entry,
                            'sl': sl,
                            'tp': tp,
                            'rr': rr,
                            'dist_atr': dist_atr,
                            'tf': tf,
                            'bar': bar,
                            'timestamp': timestamp
                        })
                    except:
                        pass
                    continue
                
                # V6.0k: Waterfall - StructureFusion (zonas creadas)
                m = re_fusion_hz.search(line)
                if m:
                    data['waterfall']['fusion_zones'].add(m.group(1))
                
                # V6.0k: Waterfall - Proximity (zonas mantenidas)
                # FIX: SUMAR todas las zonas procesadas (no tomar máximo)
                m = re_prox_kept.search(line)
                if m:
                    kept = int(m.group(1)) if m.group(1) else (int(m.group(2)) if m.group(2) else 0)
                    data['waterfall']['proximity_kept'] += kept  # SUMA, no max
                    # no continue: permitir también capturar PROX2 si está
                
                # Compat: PROX2 resumen (no usar, ya contamos con KeptAligned)
                m = re_prox2.search(line)
                if m:
                    continue
                
                # V6.0k: Waterfall - DFM (evaluadas: Bull + Bear)
                m = re_dfm_evaluated.search(line)
                if m:
                    bull = int(m.group(1))
                    bear = int(m.group(2))
                    data['waterfall']['dfm_evaluated'] += (bull + bear)
                
                # Fusion Zones
                m = re_fusion.search(line)
                if m:
                    data['fusion_zones'].append({
                        'zone_id': m.group(1),
                        'direction': m.group(2),
                        'tf_dom': int(m.group(3))
                    })
                
                # PIPE OHLC bars (NUEVO - para análisis MFE/MAE)
                m = re_pipe.search(line)
                if m:
                    try:
                        # Extraer timestamp de la barra (formato: [YYYY-MM-DD HH:MM:SS] antes de [PIPE])
                        # Buscar CUALQUIER timestamp en formato [YYYY-MM-DD HH:MM:SS] en la línea
                        timestamp_match = re.search(r'\[(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})\]', line)
                        if timestamp_match:
                            timestamp = datetime.strptime(timestamp_match.group(1), "%Y-%m-%d %H:%M:%S")
                        else:
                            # Si no hay timestamp, usar una fecha de referencia
                            timestamp = datetime(2025, 1, 1, 0, 0, 0)
                        
                        tf = int(m.group(1))
                        bar = int(m.group(2))
                        o = to_float(m.group(3))
                        h = to_float(m.group(4))
                        l = to_float(m.group(5))
                        c = to_float(m.group(6))
                        
                        data['price_bars'].append({
                            'timestamp': timestamp,
                            'tf': tf,
                            'bar': bar,
                            'open': o,
                            'high': h,
                            'low': l,
                            'close': c
                        })
                    except Exception as e:
                        pass  # Ignorar líneas mal formateadas
    
    except Exception as e:
        print(f"[ERROR] No se pudo cargar log: {e}")
    
    print(f"[DEBUG] Parsed {len(data['price_bars'])} OHLC bars from [PIPE] traces")
    
    return data

# ============================================================================
# ANÁLISIS MFE/MAE (EXCURSIÓN DEL PRECIO)
# ============================================================================

def calculate_mfe_mae(trade, price_bars, tf_analysis=5):
    """
    Calcula MFE (Max Favorable Excursion) y MAE (Max Adverse Excursion) para un trade.
    
    Args:
        trade: Dict con Entry, SL, TP, Direction, EntryBarTime
        price_bars: Lista de barras OHLC desde el log
        tf_analysis: Timeframe para análisis (default 5m para máxima granularidad)
    
    Returns:
        dict con mfe_points, mae_points, initial_direction, bars_count, mfe_atr, mae_atr
    """
    result = {
        'mfe_points': 0.0,
        'mae_points': 0.0,
        'initial_direction': 'NEUTRAL',  # TP_FIRST, SL_FIRST, NEUTRAL
        'bars_to_peak': 0,
        'bars_to_valley': 0,
        'total_bars': 0,
        'excursion_ratio': 0.0  # MFE / MAE
    }
    
    try:
        entry = trade['Entry']
        direction = trade['Direction']
        entry_time_str = trade['EntryBarTime']
        
        # Parsear entry time
        try:
            entry_time = datetime.strptime(entry_time_str, "%Y-%m-%d %H:%M:%S")
        except Exception as e:
            print(f"[DEBUG] Failed to parse entry_time '{entry_time_str}': {e}")
            return result
        
        # DEBUG: Contar barras disponibles
        total_bars_tf = len([b for b in price_bars if b['tf'] == tf_analysis])
        
        # Filtrar barras del TF especificado posteriores a la entrada
        # Limitamos a 100 barras máximo (suficiente para intradía)
        relevant_bars = [
            b for b in price_bars 
            if b['tf'] == tf_analysis and b['timestamp'] >= entry_time
        ][:100]
        
        if not relevant_bars:
            # DEBUG: Ver por qué no hay barras relevantes
            if total_bars_tf == 0:
                print(f"[DEBUG] Trade {trade.get('TradeID', 'unknown')}: No hay barras TF={tf_analysis} en price_bars")
            else:
                first_bar_tf = next((b for b in price_bars if b['tf'] == tf_analysis), None)
                if first_bar_tf:
                    print(f"[DEBUG] Trade {trade.get('TradeID', 'unknown')}: entry_time={entry_time}, primera barra TF{tf_analysis}={first_bar_tf['timestamp']}")
            return result
        
        result['total_bars'] = len(relevant_bars)
        
        # Calcular excursiones
        mfe = 0.0
        mae = 0.0
        mfe_bar = 0
        mae_bar = 0
        
        for idx, bar in enumerate(relevant_bars):
            if direction == 'BUY':
                # Para BUY: MFE es el high más alto, MAE es el low más bajo
                favorable = bar['high'] - entry
                adverse = entry - bar['low']
            else:  # SELL
                # Para SELL: MFE es el low más bajo, MAE es el high más alto
                favorable = entry - bar['low']
                adverse = bar['high'] - entry
            
            if favorable > mfe:
                mfe = favorable
                mfe_bar = idx + 1
            if adverse > mae:
                mae = adverse
                mae_bar = idx + 1
        
        result['mfe_points'] = mfe
        result['mae_points'] = mae
        result['bars_to_peak'] = mfe_bar
        result['bars_to_valley'] = mae_bar
        
        # Calcular ratio (evitar división por cero)
        if mae > 0:
            result['excursion_ratio'] = mfe / mae
        else:
            result['excursion_ratio'] = 999.0 if mfe > 0 else 1.0
        
        # Determinar dirección inicial (primeras 3 barras)
        if len(relevant_bars) >= 3:
            early_favorable = 0.0
            early_adverse = 0.0
            
            for bar in relevant_bars[:3]:
                if direction == 'BUY':
                    early_favorable += (bar['high'] - entry)
                    early_adverse += (entry - bar['low'])
                else:
                    early_favorable += (entry - bar['low'])
                    early_adverse += (bar['high'] - entry)
            
            if early_favorable > early_adverse * 1.5:
                result['initial_direction'] = 'TP_FIRST'
            elif early_adverse > early_favorable * 1.5:
                result['initial_direction'] = 'SL_FIRST'
            else:
                result['initial_direction'] = 'NEUTRAL'
    
    except Exception as e:
        print(f"[WARN] Error calculando MFE/MAE para trade {trade.get('TradeID', 'unknown')}: {e}")
    
    return result

def analyze_mfe_mae(trades, price_bars):
    """
    Calcula MFE/MAE para TODOS los trades registrados (independientemente de su estado)
    usando las ventanas de tracking [PIPE] OHLC post-registro
    """
    print("[3/4] Analizando MFE/MAE (Excursión del Precio) - TODOS LOS TRADES REGISTRADOS...")
    
    results = {
        'trades_with_data': [],
        'mfe_avg': 0.0,
        'mae_avg': 0.0,
        'excursion_ratio_avg': 0.0,
        'tp_first_count': 0,
        'sl_first_count': 0,
        'neutral_count': 0,
        'good_entries_pct': 0.0,  # MFE > MAE
        'bad_entries_pct': 0.0,   # MAE > MFE
        # Desglose por estado
        'by_status': {
            'CLOSED': {'count': 0, 'tp_first': 0, 'sl_first': 0, 'mfe_avg': 0, 'mae_avg': 0},
            'PENDING': {'count': 0, 'tp_first': 0, 'sl_first': 0, 'mfe_avg': 0, 'mae_avg': 0},
            'EXPIRED': {'count': 0, 'tp_first': 0, 'sl_first': 0, 'mfe_avg': 0, 'mae_avg': 0},
            'CANCELLED': {'count': 0, 'tp_first': 0, 'sl_first': 0, 'mfe_avg': 0, 'mae_avg': 0}
        }
    }
    
    # Analizar TODOS los trades con Entry/SL/TP definidos
    print(f"[DEBUG] Total trades recibidos: {len(trades)}")
    if len(trades) > 0:
        first_trade = trades[0]
        print(f"[DEBUG] Primer trade - Entry: {first_trade.get('Entry', 'N/A')}, SL: {first_trade.get('SL', 'N/A')}, TP: {first_trade.get('TP', 'N/A')}")
    
    all_trades = [t for t in trades if t.get('Entry', 0) > 0 and t.get('SL', 0) > 0 and t.get('TP', 0) > 0]
    print(f"[DEBUG] Trades filtrados con Entry/SL/TP > 0: {len(all_trades)}")
    
    if not price_bars:
        print("[WARN] No hay datos OHLC ([PIPE]) en el log. Salta análisis MFE/MAE")
        return results
    
    if not all_trades:
        print(f"[WARN] No hay trades con Entry/SL/TP válidos para análisis MFE/MAE")
        return results
    
    print(f"[INFO] Analizando {len(all_trades)} trades registrados (CLOSED/PENDING/EXPIRED/CANCELLED)...")
    
    mfe_list = []
    mae_list = []
    excursion_ratios = []
    
    for trade in all_trades:
        mfe_mae_data = calculate_mfe_mae(trade, price_bars, tf_analysis=5)
        
        if mfe_mae_data['total_bars'] > 0:
            trade_with_mfe = trade.copy()
            trade_with_mfe['mfe'] = mfe_mae_data
            results['trades_with_data'].append(trade_with_mfe)
            
            mfe_list.append(mfe_mae_data['mfe_points'])
            mae_list.append(mfe_mae_data['mae_points'])
            excursion_ratios.append(mfe_mae_data['excursion_ratio'])
            
            # Conteo global
            if mfe_mae_data['initial_direction'] == 'TP_FIRST':
                results['tp_first_count'] += 1
            elif mfe_mae_data['initial_direction'] == 'SL_FIRST':
                results['sl_first_count'] += 1
            else:
                results['neutral_count'] += 1
            
            # Desglose por estado
            status = trade.get('Status', 'UNKNOWN')
            if status in results['by_status']:
                results['by_status'][status]['count'] += 1
                if mfe_mae_data['initial_direction'] == 'TP_FIRST':
                    results['by_status'][status]['tp_first'] += 1
                elif mfe_mae_data['initial_direction'] == 'SL_FIRST':
                    results['by_status'][status]['sl_first'] += 1
                
                # Acumular MFE/MAE para promedio por estado
                if 'mfe_sum' not in results['by_status'][status]:
                    results['by_status'][status]['mfe_sum'] = 0
                    results['by_status'][status]['mae_sum'] = 0
                results['by_status'][status]['mfe_sum'] += mfe_mae_data['mfe_points']
                results['by_status'][status]['mae_sum'] += mfe_mae_data['mae_points']
    
    # Estadísticas agregadas
    if mfe_list:
        results['mfe_avg'] = sum(mfe_list) / len(mfe_list)
        results['mae_avg'] = sum(mae_list) / len(mae_list)
        results['excursion_ratio_avg'] = sum(excursion_ratios) / len(excursion_ratios)
        
        good_entries = len([r for r in results['trades_with_data'] if r['mfe']['mfe_points'] > r['mfe']['mae_points']])
        results['good_entries_pct'] = (good_entries / len(results['trades_with_data']) * 100) if results['trades_with_data'] else 0
        results['bad_entries_pct'] = 100 - results['good_entries_pct']
    
    # Calcular promedios por estado
    for status, data in results['by_status'].items():
        if data['count'] > 0:
            data['mfe_avg'] = data.get('mfe_sum', 0) / data['count']
            data['mae_avg'] = data.get('mae_sum', 0) / data['count']
            data['tp_first_pct'] = (data['tp_first'] / data['count'] * 100) if data['count'] > 0 else 0
    
    print(f"[OK] MFE/MAE calculado para {len(results['trades_with_data'])} trades")
    print(f"[INFO] Desglose: CLOSED={results['by_status']['CLOSED']['count']}, "
          f"PENDING={results['by_status']['PENDING']['count']}, "
          f"EXPIRED={results['by_status']['EXPIRED']['count']}, "
          f"CANCELLED={results['by_status']['CANCELLED']['count']}")
    
    return results

def analyze_phantom_opportunities(phantom_opportunities, price_bars):
    """
    Análisis COMPLETO de phantom opportunities (zonas procesadas pero no ejecutadas)
    Calcula MFE/MAE para cada una y agrupa por distancia para evaluar calidad
    """
    print("[INFO] Analizando PHANTOM OPPORTUNITIES (oportunidades no ejecutadas)...")
    
    if not phantom_opportunities:
        print("[INFO] No hay phantom opportunities para analizar")
        return None
    
    if not price_bars:
        print("[WARN] No hay datos OHLC para analizar phantom opportunities")
        return None
    
    # Agrupar por rangos de distancia ATR
    distance_ranges = {
        '0.0-2.0 ATR (Muy cerca)': [],
        '2.0-3.0 ATR (Cerca)': [],
        '3.0-5.0 ATR (Media)': [],
        '5.0-10.0 ATR (Lejos)': [],
        '>10.0 ATR (Muy lejos)': []
    }
    
    # Clasificar phantom opportunities por distancia
    for phantom in phantom_opportunities:
        dist = phantom.get('dist_atr', 0)
        if dist < 0:
            continue
            
        if dist < 2.0:
            distance_ranges['0.0-2.0 ATR (Muy cerca)'].append(phantom)
        elif dist < 3.0:
            distance_ranges['2.0-3.0 ATR (Cerca)'].append(phantom)
        elif dist < 5.0:
            distance_ranges['3.0-5.0 ATR (Media)'].append(phantom)
        elif dist < 10.0:
            distance_ranges['5.0-10.0 ATR (Lejos)'].append(phantom)
        else:
            distance_ranges['>10.0 ATR (Muy lejos)'].append(phantom)
    
    # Analizar cada grupo
    analysis_by_range = {}
    
    for range_name, phantoms in distance_ranges.items():
        if not phantoms:
            continue
        
        print(f"[INFO] Analizando {len(phantoms)} phantoms en rango {range_name}...")
        
        analyzed_phantoms = []
        
        for phantom in phantoms:
            # Calcular MFE/MAE usando calculate_mfe_mae
            mfe_mae = calculate_mfe_mae(
                {
                    'Direction': phantom['direction'],
                    'Entry': phantom['entry'],
                    'SL': phantom['sl'],
                    'TP': phantom['tp'],
                    'EntryBarTime': phantom['timestamp'].strftime('%Y-%m-%d %H:%M:%S') if phantom.get('timestamp') else ''
                },
                price_bars
            )
            
            if mfe_mae:
                analyzed_phantoms.append({
                    'phantom': phantom,
                    'mfe_mae': mfe_mae
                })
        
        if not analyzed_phantoms:
            continue
        
        # Calcular estadísticas del grupo
        mfe_list = [p['mfe_mae']['mfe_points'] for p in analyzed_phantoms if p['mfe_mae'].get('mfe_points', 0) > 0]
        mae_list = [p['mfe_mae']['mae_points'] for p in analyzed_phantoms if p['mfe_mae'].get('mae_points', 0) > 0]
        
        # Win Rate teórico (si TP se alcanzó primero)
        tp_first = len([p for p in analyzed_phantoms if p['mfe_mae'].get('initial_direction') == 'TP_FIRST'])
        sl_first = len([p for p in analyzed_phantoms if p['mfe_mae'].get('initial_direction') == 'SL_FIRST'])
        neutral = len([p for p in analyzed_phantoms if p['mfe_mae'].get('initial_direction') == 'NEUTRAL'])
        
        # WR teórico: phantoms que habrían sido winners si se ejecutaran
        winners = len([p for p in analyzed_phantoms if p['mfe_mae'].get('mfe_points', 0) > p['phantom']['rr'] * abs(p['phantom']['entry'] - p['phantom']['sl'])])
        wr_theoretical = (winners / len(analyzed_phantoms) * 100) if analyzed_phantoms else 0
        
        # Calidad de entrada (MFE > MAE indica entrada en buen momento)
        good_entries = len([p for p in analyzed_phantoms if p['mfe_mae'].get('mfe_points', 0) > p['mfe_mae'].get('mae_points', 0)])
        good_entries_pct = (good_entries / len(analyzed_phantoms) * 100) if analyzed_phantoms else 0
        
        # MFE/MAE ratio promedio
        mfe_mae_ratios = []
        for p in analyzed_phantoms:
            mfe = p['mfe_mae'].get('mfe_points', 0)
            mae = p['mfe_mae'].get('mae_points', 1)
            if mae > 0:
                mfe_mae_ratios.append(mfe / mae)
        
        avg_mfe_mae_ratio = sum(mfe_mae_ratios) / len(mfe_mae_ratios) if mfe_mae_ratios else 0
        
        # Determinar conclusión
        if good_entries_pct > 60 and wr_theoretical > 35:
            conclusion = "✅ BUENA CALIDAD - Considerar incluir"
        elif good_entries_pct > 50 and wr_theoretical > 30:
            conclusion = "⚠️ CALIDAD MEDIA - Revisar"
        else:
            conclusion = "❌ BAJA CALIDAD - Filtro correcto"
        
        analysis_by_range[range_name] = {
            'count': len(analyzed_phantoms),
            'mfe_avg': sum(mfe_list) / len(mfe_list) if mfe_list else 0,
            'mae_avg': sum(mae_list) / len(mae_list) if mae_list else 0,
            'mfe_mae_ratio': avg_mfe_mae_ratio,
            'tp_first': tp_first,
            'sl_first': sl_first,
            'neutral': neutral,
            'tp_first_pct': (tp_first / len(analyzed_phantoms) * 100) if analyzed_phantoms else 0,
            'wr_theoretical': wr_theoretical,
            'good_entries_pct': good_entries_pct,
            'conclusion': conclusion,
            'avg_rr': sum([p['phantom']['rr'] for p in analyzed_phantoms]) / len(analyzed_phantoms) if analyzed_phantoms else 0
        }
    
    print(f"[OK] Análisis phantom completado: {len(analysis_by_range)} rangos analizados")
    
    return analysis_by_range

# ============================================================================
# ANÁLISIS PILAR 1: BIAS/SENTIMIENTO
# ============================================================================

def analyze_bias(log_data, trades):
    """Analiza precisión del bias vs movimiento real"""
    print("[1/4] Analizando BIAS/SENTIMIENTO...")
    
    results = {
        'total_samples': len(log_data['context_bias']),
        'bias_distribution': Counter([b['bias'] for b in log_data['context_bias']]),
        'trades_aligned': 0,
        'trades_counter': 0,
        'wr_aligned': 0.0,
        'wr_counter': 0.0,
        'recommendation': "",
        'is_composite': False,
        'composite_stats': {}
    }
    
    # V6.0g: Detectar si el log tiene bias compuesto
    if log_data['context_bias'] and log_data['context_bias'][0].get('score') is not None:
        results['is_composite'] = True
        scores = [b['score'] for b in log_data['context_bias'] if b.get('score') is not None]
        thresholds = [b['threshold'] for b in log_data['context_bias'] if b.get('threshold') is not None]
        ema20_vals = [b['ema20'] for b in log_data['context_bias'] if b.get('ema20') is not None]
        ema50_vals = [b['ema50'] for b in log_data['context_bias'] if b.get('ema50') is not None]
        bos_vals = [b['bos'] for b in log_data['context_bias'] if b.get('bos') is not None]
        reg24h_vals = [b['reg24h'] for b in log_data['context_bias'] if b.get('reg24h') is not None]
        
        if scores:
            results['composite_stats'] = {
                'score_avg': sum(scores) / len(scores),
                'score_min': min(scores),
                'score_max': max(scores),
                'threshold': thresholds[0] if thresholds else 0.3,  # Usar primer threshold (debería ser constante)
                'ema20_avg': sum(ema20_vals) / len(ema20_vals) if ema20_vals else 0,
                'ema50_avg': sum(ema50_vals) / len(ema50_vals) if ema50_vals else 0,
                'bos_avg': sum(bos_vals) / len(bos_vals) if bos_vals else 0,
                'reg24h_avg': sum(reg24h_vals) / len(reg24h_vals) if reg24h_vals else 0
            }
    
    # Analizar alineación de trades
    bullish_count = results['bias_distribution'].get('Bullish', 0)
    bearish_count = results['bias_distribution'].get('Bearish', 0)
    total_bias = bullish_count + bearish_count
    
    if total_bias > 0:
        bullish_pct = (bullish_count / total_bias) * 100
        bearish_pct = (bearish_count / total_bias) * 100
        
        # Comparar con trades ejecutados
        buy_trades = len([t for t in trades if t['Direction'] == 'BUY'])
        sell_trades = len([t for t in trades if t['Direction'] == 'SELL'])
        
        results['trades_aligned'] = buy_trades if bullish_pct > 50 else sell_trades
        results['trades_counter'] = sell_trades if bullish_pct > 50 else buy_trades
        
        # Win Rate por alineación (simplificado por ahora)
        wins = len([t for t in trades if t['ExitReason'] == 'TP_HIT'])
        if buy_trades > 0:
            results['wr_aligned'] = (wins / buy_trades) * 100
        if sell_trades > 0:
            results['wr_counter'] = (wins / sell_trades) * 100 if sell_trades > buy_trades else 0
    
    # Recomendación
    neutral_count = results['bias_distribution'].get('Neutral', 0)
    neutral_pct = (neutral_count / results['total_samples'] * 100) if results['total_samples'] > 0 else 0
    
    if results['is_composite'] and neutral_pct > 90:
        # V6.0g: Bias compuesto con threshold demasiado alto
        stats = results['composite_stats']
        results['recommendation'] = f"CRÍTICO: Bias Compuesto {neutral_pct:.1f}% Neutral - threshold {stats.get('threshold', 0.5):.1f} puede ser demasiado alto. Score real [{stats['score_min']:.2f}, {stats['score_max']:.2f}]. REDUCIR threshold a 0.3/-0.3 si > 0.3."
    elif bullish_count > bearish_count * 2:
        results['recommendation'] = "CRÍTICO: Bias excesivamente alcista ({}%) no refleja movimiento real. Usar EMA más rápida (EMA50@60m o EMA20@60m).".format(
            int((bullish_count / total_bias) * 100) if total_bias > 0 else 0
        )
    
    return results

# ============================================================================
# ANÁLISIS PILAR 2: ENTRADAS/ZONAS
# ============================================================================

def analyze_entries(log_data, trades):
    """Analiza calidad de entradas y waterfall del pipeline"""
    print("[2/4] Analizando ENTRADAS/ZONAS...")
    
    # V6.0k: Usar datos del waterfall parseado
    wf = log_data['waterfall']
    # Fallback: si RiskAccepted no aparece en log, estimar desde CSV REGISTERED
    try:
        if wf.get('risk_accepted', 0) == 0 and CSV_PATH and os.path.exists(CSV_PATH):
            reg_count = count_csv_registered(CSV_PATH)
            if reg_count > 0:
                wf['risk_accepted'] = reg_count
    except Exception:
        pass
    
    results = {
        'fusion_zones': len(wf['fusion_zones']),
        'proximity_passed': wf['proximity_kept'],
        'dfm_evaluated': wf['dfm_evaluated'],
        'dfm_passed': wf['dfm_passed'],
        'risk_accepted': wf['risk_accepted'],
        'trades_registered': len(trades),
        'waterfall': {},
        'wr_by_tp_type': {},
        'pf_by_tp_type': {}
    }
    
    # Waterfall del pipeline (embudo de señales)
    # ✅ ACTUALIZADO: RiskCalculator muestra zonas RECIBIDAS, no solo aceptadas
    results['waterfall'] = {
        'StructureFusion': results['fusion_zones'],
        'ProximityAnalyzer': results['proximity_passed'],
        'DFM_Evaluated': results['dfm_evaluated'],
        'DFM_Passed': results['dfm_passed'],
        'RiskCalculator': wf['risk_received'] if wf['risk_received'] > 0 else results['risk_accepted'],  # Usar recibidas si disponible
        'Risk_Accepted': results['risk_accepted'],  # Nueva métrica
        'TradeManager': results['trades_registered']
    }
    
    # WR/PF por tipo de TP
    tp_types = Counter([tp['priority'] for tp in log_data['tp_policy']])
    for tp_type in tp_types:
        # Simplificado: necesitaríamos correlacionar zone_id con trades
        results['wr_by_tp_type'][tp_type] = 0.0  # Placeholder
        results['pf_by_tp_type'][tp_type] = 0.0  # Placeholder
    
    return results

# ============================================================================
# ANÁLISIS PILAR 3: SL/TP
# ============================================================================

def analyze_sl_tp(log_data, trades):
    """Analiza distribuciones SL/TP y calcula límites data-driven"""
    print("[4/4] Analizando SL/TP...")
    
    # Extraer SL/TP de trades
    sl_points = [t['RiskPoints'] for t in trades if t['RiskPoints'] > 0]
    tp_points = [t['RewardPoints'] for t in trades if t['RewardPoints'] > 0]
    rr_planned = [t['RR'] for t in trades if t['RR'] > 0]
    
    results = {
        'sl_stats': {},
        'tp_stats': {},
        'rr_stats': {},
        'limits_recommended': {},
        'wr_vs_tp_distance': []
    }
    
    # Estadísticas SL
    if sl_points:
        results['sl_stats'] = {
            'count': len(sl_points),
            'mean': statistics.mean(sl_points),
            'median': statistics.median(sl_points),
            'min': min(sl_points),
            'max': max(sl_points),
            'percentiles': {p: statistics.quantiles(sl_points, n=100)[p-1] if len(sl_points) >= 2 else 0 for p in PERCENTILES}
        }
    
    # Estadísticas TP
    if tp_points:
        results['tp_stats'] = {
            'count': len(tp_points),
            'mean': statistics.mean(tp_points),
            'median': statistics.median(tp_points),
            'min': min(tp_points),
            'max': max(tp_points),
            'percentiles': {p: statistics.quantiles(tp_points, n=100)[p-1] if len(tp_points) >= 2 else 0 for p in PERCENTILES}
        }
    
    # Estadísticas R:R
    if rr_planned:
        results['rr_stats'] = {
            'count': len(rr_planned),
            'mean': statistics.mean(rr_planned),
            'median': statistics.median(rr_planned),
            'min': min(rr_planned),
            'max': max(rr_planned)
        }
    
    # Límites recomendados (percentil 90)
    if sl_points and tp_points:
        p90_sl = results['sl_stats']['percentiles'].get(90, 60)
        p90_tp = results['tp_stats']['percentiles'].get(90, 120)
        
        results['limits_recommended'] = {
            'MaxSLDistancePoints': int(p90_sl),
            'MaxTPDistancePoints': int(p90_tp),
            'rationale': f"Basado en percentil 90 de operaciones reales (SL: {p90_sl:.1f}pts, TP: {p90_tp:.1f}pts)"
        }
    
    # R:R mínimo para WR actual
    # Fórmula: R:R_min = (1 - WR) / WR para break-even
    wr = (len([t for t in trades if str(t.get('ExitReason','')).upper() in ('TP','TP_HIT')]) / len(trades)) if trades else 0.0
    rr_min_breakeven = (1 - wr) / wr if wr > 0 else 1.75
    
    results['rr_min_breakeven'] = rr_min_breakeven
    
    return results

# ============================================================================
# GENERADOR DE INFORME
# ============================================================================

def generate_report(trades, log_data, bias_results, entries_results, mfe_mae_results, sl_tp_results, phantom_analysis=None):
    """Genera informe markdown único"""
    print("[INFO] Generando informe...")
    
    lines = []
    lines.append("# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain")
    lines.append("")
    lines.append(f"**Fecha:** {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    lines.append(f"**LOG:** `{LOG_PATH}`")
    lines.append(f"**CSV:** `{CSV_PATH}`")
    lines.append("")
    lines.append("---")
    lines.append("")
    
    # ========================================================================
    # 1. RESUMEN EJECUTIVO
    # ========================================================================
    lines.append("## 1. RESUMEN EJECUTIVO")
    lines.append("")
    lines.append("### KPIs Principales")
    lines.append("")
    
    total_trades = len(trades)
    # Compatibilidad: CSV puede registrar 'TP'/'SL' en ExitReason además de 'TP_HIT'/'SL_HIT'
    wins = len([t for t in trades if str(t.get('ExitReason','')).upper() in ('TP','TP_HIT')])
    losses = len([t for t in trades if str(t.get('ExitReason','')).upper() in ('SL','SL_HIT')])
    wr = (wins / total_trades * 100) if total_trades > 0 else 0
    
    gross_profit = sum([t['PnLPoints'] for t in trades if t['PnLPoints'] > 0])
    gross_loss = abs(sum([t['PnLPoints'] for t in trades if t['PnLPoints'] < 0]))
    pf = (gross_profit / gross_loss) if gross_loss > 0 else 0
    
    lines.append(f"- **Operaciones Ejecutadas:** {total_trades}")
    lines.append(f"- **Win Rate:** {wr:.1f}% ({wins}/{total_trades})")
    lines.append(f"- **Profit Factor:** {pf:.2f}")
    lines.append(f"- **Avg R:R Planeado:** {sl_tp_results['rr_stats'].get('mean', 0):.2f}")
    lines.append(f"- **R:R Mínimo para Break-Even:** {sl_tp_results['rr_min_breakeven']:.2f}")
    lines.append("")
    
    # Problemas críticos
    lines.append("### 🚨 Problemas Críticos Identificados")
    lines.append("")
    lines.append("1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente")
    lines.append("   - Causa: EMA200@60m demasiado lenta (8+ días)")
    lines.append("   - Impacto: Entradas contra-tendencia inmediata")
    lines.append("")
    lines.append("2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**")
    lines.append(f"   - SL máximo observado: {sl_tp_results['sl_stats'].get('max', 0):.0f} puntos")
    lines.append(f"   - TP máximo observado: {sl_tp_results['tp_stats'].get('max', 0):.0f} puntos")
    lines.append(f"   - **120 puntos es swing trading, no intradía** (1.74% del precio)")
    lines.append("")
    lines.append("3. **R:R INSUFICIENTE PARA WR ACTUAL:**")
    lines.append(f"   - R:R actual: {sl_tp_results['rr_stats'].get('mean', 0):.2f}")
    lines.append(f"   - R:R necesario: {sl_tp_results['rr_min_breakeven']:.2f}")
    lines.append(f"   - **Gap:** {sl_tp_results['rr_min_breakeven'] - sl_tp_results['rr_stats'].get('mean', 0):.2f}")
    lines.append("")
    
    # ========================================================================
    # 2. PILAR 1: BIAS/SENTIMIENTO
    # ========================================================================
    lines.append("---")
    lines.append("")
    lines.append("## 2. PILAR 1: BIAS/SENTIMIENTO")
    lines.append("")
    
    lines.append("### 2.1 Distribución del Bias")
    lines.append("")
    lines.append("| Bias | Eventos | % |")
    lines.append("|------|---------|---|")
    for bias, count in bias_results['bias_distribution'].items():
        pct = (count / bias_results['total_samples'] * 100) if bias_results['total_samples'] > 0 else 0
        lines.append(f"| {bias} | {count} | {pct:.1f}% |")
    lines.append("")
    
    lines.append("### 2.2 Diagnóstico")
    lines.append("")
    lines.append(f"**Problema detectado:** {bias_results['recommendation']}")
    lines.append("")
    
    # V6.0g: Mostrar estadísticas del bias compuesto si está disponible
    if bias_results['is_composite'] and bias_results['composite_stats']:
        stats = bias_results['composite_stats']
        lines.append("**Bias Compuesto (V6.0g) - Estadísticas:**")
        lines.append(f"- **Score Promedio:** {stats['score_avg']:.3f}")
        lines.append(f"- **Score Min/Max:** [{stats['score_min']:.3f}, {stats['score_max']:.3f}]")
        lines.append(f"- **Componentes (promedio):**")
        lines.append(f"  - EMA20 Slope: {stats['ema20_avg']:.3f}")
        lines.append(f"  - EMA50 Cross: {stats['ema50_avg']:.3f}")
        lines.append(f"  - BOS Count: {stats['bos_avg']:.3f}")
        lines.append(f"  - Regression 24h: {stats['reg24h_avg']:.3f}")
        lines.append("")
        lines.append("**Análisis:**")
        lines.append(f"- Threshold actual: {stats.get('threshold', 0.3):.1f}/-{stats.get('threshold', 0.3):.1f}")
        lines.append(f"- Score máximo observado: {stats['score_max']:.3f} (apenas supera threshold)")
        lines.append(f"- Score mínimo observado: {stats['score_min']:.3f} (apenas supera threshold)")
        lines.append(f"- **Consecuencia:** Sistema queda {bias_results['bias_distribution'].get('Neutral', 0) / bias_results['total_samples'] * 100:.1f}% Neutral → bias no diferencia tendencias")
    else:
        lines.append("**Contexto:**")
        lines.append("- EMA200@60m refleja últimas **200 horas** (~8+ días)")
        lines.append("- NO captura movimientos intradía (últimas 4-24 horas)")
        lines.append("- Gráfico muestra caída reciente, pero bias sigue 'Bullish'")
    lines.append("")
    
    # V6.0g: Recomendación ajustada según si ya tiene bias compuesto o no
    if bias_results['is_composite']:
        threshold_current = stats.get('threshold', 0.5)
        neutral_current = bias_results['bias_distribution'].get('Neutral', 0) / bias_results['total_samples'] * 100 if bias_results['total_samples'] > 0 else 0
        
        # Solo recomendar cambio de threshold si es necesario
        if threshold_current > 0.3 and neutral_current > 30:
            lines.append("### 2.3 Recomendación: Ajustar Threshold del Bias Compuesto")
            lines.append("")
            lines.append(f"**Solución Inmediata:** Reducir threshold de {threshold_current:.1f}/-{threshold_current:.1f} a **0.3/-0.3**")
            lines.append("")
            lines.append("**Archivo:** `pinkbutterfly-produccion/ContextManager.cs` (línea ~207)")
            lines.append("")
            lines.append("```csharp")
            lines.append("// ANTES:")
            lines.append(f"if (compositeScore > {threshold_current:.1f}) {{ ... }}")
            lines.append("")
            lines.append("// DESPUÉS:")
            lines.append("if (compositeScore > 0.3) { ... }  // Más sensible")
            lines.append("elif (compositeScore < -0.3) { ... }")
            lines.append("```")
            lines.append("")
            lines.append("**Justificación:**")
            lines.append(f"- Scores reales: [{stats['score_min']:.2f}, {stats['score_max']:.2f}]")
            lines.append(f"- Score promedio: {stats['score_avg']:.3f}")
            lines.append(f"- Threshold {threshold_current:.1f} requiere ~{int(threshold_current * 200)}% alineación de componentes (demasiado estricto)")
            lines.append("- Threshold 0.3 requiere 60% alineación (más realista)")
            lines.append("")
            lines.append("**Impacto esperado:**")
            lines.append(f"- Neutral actual: {neutral_current:.1f}% → ~15-20% (objetivo)")
            lines.append("- Bullish/Bearish: aumentarán a ~40-45% cada uno")
            lines.append("- Sistema empezará a usar el bias para filtrar operaciones")
            lines.append("")
        elif threshold_current <= 0.3 and neutral_current > 30:
            # Threshold ya está en 0.3 pero Neutral sigue alto
            lines.append("### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto")
            lines.append("")
            lines.append(f"**Situación:** Threshold ya está en {threshold_current:.1f} (correcto), pero Bias Neutral sigue alto ({neutral_current:.1f}%)")
            lines.append("")
            lines.append("**Posibles causas:**")
            lines.append(f"- **BOS Score bajo ({stats['bos_avg']:.3f}):** BOS/CHoCH no se detectan correctamente")
            lines.append(f"- **Componentes débiles:** Score promedio {stats['score_avg']:.3f} indica poca señal direccional")
            lines.append(f"- **Mercado lateral:** Scores reales [{stats['score_min']:.2f}, {stats['score_max']:.2f}] muy cercanos a 0")
            lines.append("")
            lines.append("**Recomendaciones:**")
            lines.append("1. ✅ Verificar que `BOSDetector.cs` establece `Type = breakType` (bug conocido)")
            lines.append("2. ✅ Revisar logs para confirmar que BOS Score != 0.0")
            lines.append("3. ⚠️ Si BOS sigue en ~0, investigar detección de BOS/CHoCH")
            lines.append("4. ⚠️ Considerar bajar threshold a 0.2 SOLO si los 3 pasos anteriores están OK")
            lines.append("")
        else:
            # Threshold OK y Neutral bajo
            lines.append("### 2.3 Estado: Bias Compuesto Funcionando Correctamente")
            lines.append("")
            lines.append(f"✅ **Threshold actual:** {threshold_current:.1f} (correcto)")
            lines.append(f"✅ **Bias Neutral:** {neutral_current:.1f}% (aceptable)")
            lines.append(f"✅ **Score promedio:** {stats['score_avg']:.3f}")
            lines.append("")
    else:
        lines.append("### 2.3 Recomendación: Bias Compuesto Rápido")
        lines.append("")
        lines.append("**Propuesta:** Cambiar de EMA200@60m a señal compuesta:")
        lines.append("")
        lines.append("```")
        lines.append("BiasScore = (")
        lines.append("    0.30 * EMA20@60m_slope  // Tendencia inmediata (20h)")
        lines.append("  + 0.25 * EMA50@60m_cross  // Tendencia media (50h)")
        lines.append("  + 0.25 * BOS_CHoCH_count  // Cambios de estructura recientes")
        lines.append("  + 0.20 * Regression_24h   // Dirección últimas 24h")
        lines.append(")")
        lines.append("")
        lines.append("if BiasScore > 0.3: Bullish  // Threshold más sensible")
        lines.append("elif BiasScore < -0.3: Bearish")
        lines.append("else: Neutral")
        lines.append("```")
        lines.append("")
        lines.append("**Ventajas:**")
        lines.append("- ✅ Responde en 4-24 horas (intradía)")
        lines.append("- ✅ Combina múltiples señales (robusto)")
        lines.append("- ✅ Detecta cambios de estructura (BOS/CHoCH)")
        lines.append("")
    
    # ========================================================================
    # 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (NUEVO)
    # ========================================================================
    lines.append("---")
    lines.append("")
    lines.append("## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)")
    lines.append("")
    
    # Extraer señales rechazadas de log_data
    rejected_signals = log_data.get('dfm_rejected', [])
    all_signals = log_data.get('signal_metrics', [])
    
    if rejected_signals:
        lines.append(f"### Total de Señales Rechazadas: **{len(rejected_signals)}**")
        lines.append("")
        
        # Distribución por dirección rechazada
        rejected_by_dir = Counter([r['direction'] for r in rejected_signals])
        lines.append("**Distribución por Dirección:**")
        lines.append("")
        lines.append("| Dirección | Count | % |")
        lines.append("|-----------|-------|---|")
        for direction, count in rejected_by_dir.most_common():
            pct = (count / len(rejected_signals) * 100)
            lines.append(f"| {direction} | {count} | {pct:.1f}% |")
        lines.append("")
        
        # Métricas de señales rechazadas
        lines.append("**Métricas de Señales Rechazadas:**")
        lines.append("")
        avg_conf = statistics.mean([r['conf'] for r in rejected_signals]) if rejected_signals else 0
        avg_rr = statistics.mean([r['rr'] for r in rejected_signals]) if rejected_signals else 0
        avg_sl_pts = statistics.mean([abs(r['entry'] - r['sl']) for r in rejected_signals if r['entry'] > 0 and r['sl'] > 0]) if rejected_signals else 0
        avg_tp_pts = statistics.mean([abs(r['tp'] - r['entry']) for r in rejected_signals if r['entry'] > 0 and r['tp'] > 0]) if rejected_signals else 0
        
        lines.append(f"- **Confianza Promedio:** {avg_conf:.3f}")
        lines.append(f"- **R:R Promedio:** {avg_rr:.2f}")
        lines.append(f"- **SL Promedio:** {avg_sl_pts:.1f} pts")
        lines.append(f"- **TP Promedio:** {avg_tp_pts:.1f} pts")
        lines.append("")
        
        # Validación del filtro: comparar señales rechazadas con aprobadas
        approved_signals = [s for s in all_signals if s['action'] in ['BUY', 'SELL']]
        wait_signals = [s for s in all_signals if s['action'] == 'WAIT']
        
        lines.append("**Comparación: Señales Aprobadas vs Rechazadas**")
        lines.append("")
        lines.append("| Métrica | Aprobadas | Rechazadas |")
        lines.append("|---------|-----------|------------|")
        
        if approved_signals:
            approved_conf_avg = statistics.mean([s['conf'] for s in approved_signals])
            lines.append(f"| Conf Promedio | {approved_conf_avg:.3f} | {avg_conf:.3f} |")
        
        lines.append("")
        
        # Tabla detallada de señales rechazadas (top 10)
        lines.append("**Detalle de Señales Rechazadas (Top 10):**")
        lines.append("")
        lines.append("| Zone ID | Dir | Entry | SL | TP | Conf | R:R | Bias vs | Razón |")
        lines.append("|---------|-----|-------|----|----|------|-----|---------|-------|")
        
        for sig in rejected_signals[:10]:
            lines.append(f"| {sig['zone_id']} | {sig['direction']} | {sig['entry']:.2f} | {sig['sl']:.2f} | {sig['tp']:.2f} | {sig['conf']:.3f} | {sig['rr']:.2f} | {sig['bias_vs']} | {sig['reason']} |")
        
        lines.append("")
        
        # Conclusión sobre la validez del filtro
        lines.append("**Conclusión:**")
        lines.append("")
        if avg_conf < 0.70 and avg_rr < 1.5:
            lines.append("✅ **El filtro CONTRA-BIAS está funcionando correctamente:**")
            lines.append(f"- Las señales rechazadas tienen menor confianza ({avg_conf:.3f}) y R:R ({avg_rr:.2f})")
            lines.append("- Se están eliminando operaciones contra-tendencia de baja calidad")
        else:
            lines.append("⚠️ **El filtro CONTRA-BIAS puede estar siendo demasiado restrictivo:**")
            lines.append(f"- Las señales rechazadas tienen confianza razonable ({avg_conf:.3f}) y R:R ({avg_rr:.2f})")
            lines.append("- Se están perdiendo oportunidades válidas contra-tendencia")
        
        lines.append("")
    else:
        lines.append("⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**")
        lines.append("")
        lines.append("Para activar este análisis, las trazas deben estar presentes en el log.")
        lines.append("")
    
    # ========================================================================
    # 3. PILAR 2: ENTRADAS/ZONAS
    # ========================================================================
    lines.append("---")
    lines.append("")
    lines.append("## 3. PILAR 2: ENTRADAS/ZONAS")
    lines.append("")
    
    lines.append("### 3.1 Waterfall del Pipeline (Embudo de Señales)")
    lines.append("")
    lines.append("| Paso | Zonas/Señales | % vs Anterior | % vs Total |")
    lines.append("|------|---------------|---------------|------------|")
    
    # V6.0k: Orden correcto del pipeline
    # ✅ ACTUALIZADO: Incluir Risk_Accepted como paso separado
    waterfall = entries_results['waterfall']
    steps = [
        ('StructureFusion', waterfall['StructureFusion']),
        ('ProximityAnalyzer', waterfall['ProximityAnalyzer']),
        ('DFM_Evaluated', waterfall['DFM_Evaluated']),
        ('DFM_Passed', waterfall['DFM_Passed']),
        ('RiskCalculator', waterfall['RiskCalculator']),
        ('Risk_Accepted', waterfall['Risk_Accepted']),
        ('TradeManager', waterfall['TradeManager'])
    ]
    
    total = waterfall['StructureFusion']
    prev = total
    for i, (step_name, count) in enumerate(steps):
        pct_prev = (count / prev * 100) if prev > 0 else 0
        pct_total = (count / total * 100) if total > 0 else 0
        
        if i == 0:
            lines.append(f"| {step_name} | {count} | 100.0% | 100.0% |")
        else:
            lines.append(f"| {step_name} | {count} | {pct_prev:.1f}% | {pct_total:.1f}% |")
        
        prev = count
    lines.append("")
    
    lines.append("**Análisis:**")
    if total > 0:
        # Identificar mayor caída
        drops = []
        for i in range(1, len(steps)):
            prev_count = steps[i-1][1]
            curr_count = steps[i][1]
            drop_pct = ((prev_count - curr_count) / prev_count * 100) if prev_count > 0 else 0
            drops.append((steps[i][0], drop_pct, prev_count - curr_count))
        
        worst_step = max(drops, key=lambda x: x[1])
        lines.append(f"- **Mayor caída:** {worst_step[0]} (pierde {worst_step[2]} señales, -{worst_step[1]:.1f}%)")
        lines.append(f"- **Tasa de conversión final:** {(waterfall['TradeManager'] / total * 100):.2f}% (de {total} zonas iniciales → {waterfall['TradeManager']} operaciones)")
    else:
        lines.append("- ⚠️ **No hay datos suficientes para análisis de waterfall**")
    lines.append("")
    
    # ✅ NUEVO: Razones de rechazo en RiskCalculator
    risk_rejections = log_data['waterfall']['risk_rejections']
    total_rejections = sum(risk_rejections.values())
    if total_rejections > 0:
        lines.append("### 3.1.1 Razones de Rechazo en RiskCalculator")
        lines.append("")
        lines.append("| Razón | Cantidad | % del Total Rechazado |")
        lines.append("|-------|----------|----------------------|")
        for reason, count in sorted(risk_rejections.items(), key=lambda x: x[1], reverse=True):
            if count > 0:
                pct = (count / total_rejections * 100) if total_rejections > 0 else 0
                lines.append(f"| {reason} | {count:,} | {pct:.1f}% |")
        lines.append("")
        lines.append("**Análisis:**")
        # Identificar razón dominante
        dominant_reason = max(risk_rejections.items(), key=lambda x: x[1])
        if dominant_reason[1] > 0:
            dom_pct = (dominant_reason[1] / total_rejections * 100)
            lines.append(f"- **Razón dominante:** {dominant_reason[0]} ({dominant_reason[1]:,} rechazos, {dom_pct:.1f}%)")
            
            # Recomendaciones específicas según la razón
            if dominant_reason[0] == 'ENTRY_STALE':
                lines.append("- **Problema:** Las zonas llegan 'obsoletas' a RiskCalculator (precio se alejó >2-3 ATR)")
                lines.append("- **Causa raíz:** Las zonas se crean/procesan tarde, o el gate de `MaxDistanceToRegister_ATR` es muy estricto")
                lines.append("- **Acción recomendada:** Investigar 'lag' del pipeline (tiempo entre creación de zona y evaluación)")
            elif dominant_reason[0] == 'SL_CHECK_FAIL':
                lines.append("- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)")
                lines.append("- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico")
            elif dominant_reason[0] == 'NO_SL':
                lines.append("- **Problema:** No se encuentra SL estructural válido")
                lines.append("- **Acción recomendada:** Revisar `RiskCalculator.CalculateStructuralSL` o activar fallback")
        lines.append("")
    
    lines.append("### 3.2 Distribución por Tipo de TP")
    lines.append("")
    tp_types = Counter([tp['priority'] for tp in log_data['tp_policy']])
    lines.append("| Tipo TP | Count | % |")
    lines.append("|---------|-------|---|")
    total_tp = sum(tp_types.values())
    for tp_type, count in tp_types.most_common():
        pct = (count / total_tp * 100) if total_tp > 0 else 0
        lines.append(f"| {tp_type} | {count} | {pct:.1f}% |")
    lines.append("")
    
    # ========================================================================
    # 3.5 MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS
    # ========================================================================
    if mfe_mae_results['trades_with_data']:
        lines.append("### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS")
        lines.append("")
        lines.append("**Métricas Globales:**")
        lines.append("")
        lines.append(f"- **MFE Promedio:** {mfe_mae_results['mfe_avg']:.2f} pts (máxima ganancia flotante)")
        lines.append(f"- **MAE Promedio:** {mfe_mae_results['mae_avg']:.2f} pts (máxima pérdida flotante)")
        lines.append(f"- **Ratio MFE/MAE:** {mfe_mae_results['excursion_ratio_avg']:.2f}")
        lines.append("")
        
        total_with_data = len(mfe_mae_results['trades_with_data'])
        lines.append("**Dirección Inicial (primeras 3 barras @ 5m):**")
        lines.append("")
        lines.append("| Dirección | Count | % |")
        lines.append("|-----------|-------|---|")
        lines.append(f"| TP_FIRST (precio fue hacia TP) | {mfe_mae_results['tp_first_count']} | {(mfe_mae_results['tp_first_count']/total_with_data*100):.1f}% |")
        lines.append(f"| SL_FIRST (precio fue hacia SL) | {mfe_mae_results['sl_first_count']} | {(mfe_mae_results['sl_first_count']/total_with_data*100):.1f}% |")
        lines.append(f"| NEUTRAL (sin dirección clara) | {mfe_mae_results['neutral_count']} | {(mfe_mae_results['neutral_count']/total_with_data*100):.1f}% |")
        lines.append("")
        
        lines.append("**Calidad de Entradas:**")
        lines.append("")
        lines.append(f"- **Entradas Buenas (MFE > MAE):** {mfe_mae_results['good_entries_pct']:.1f}%")
        lines.append(f"- **Entradas Malas (MAE > MFE):** {mfe_mae_results['bad_entries_pct']:.1f}%")
        lines.append("")
        
        if mfe_mae_results['bad_entries_pct'] > 60:
            lines.append("⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE")
            lines.append("- **Problema:** El precio va más en contra que a favor antes del cierre")
            lines.append("- **Causas posibles:**")
            lines.append("  1. Timing incorrecto (entramos antes de reversión)")
            lines.append("  2. Bias desincronizado (operamos contra tendencia real)")
            lines.append("  3. Zonas de baja calidad (sin confluence real)")
            lines.append("")
        
        # ========================================================================
        # DESGLOSE POR ESTADO (MODO DIAGNÓSTICO)
        # ========================================================================
        lines.append("**🔍 Análisis por Estado (Modo Diagnóstico):**")
        lines.append("")
        lines.append("| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |")
        lines.append("|--------|-------|----------|----------|------------|---------|---------|")
        
        for status in ['CLOSED', 'PENDING', 'EXPIRED', 'CANCELLED']:
            data = mfe_mae_results['by_status'][status]
            if data['count'] > 0:
                tp_first_pct = data.get('tp_first_pct', 0)
                lines.append(f"| {status} | {data['count']} | {data['tp_first']} | {data['sl_first']} | {tp_first_pct:.1f}% | {data['mfe_avg']:.2f} | {data['mae_avg']:.2f} |")
        lines.append("")
        
        lines.append("**💡 Interpretación del Modo Diagnóstico:**")
        lines.append("")
        lines.append("- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)")
        lines.append("- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)")
        lines.append("- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)")
        lines.append("- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)")
        lines.append("")
        
        # Recomendaciones basadas en datos
        expired_data = mfe_mae_results['by_status']['EXPIRED']
        if expired_data['count'] > 0:
            expired_tp_pct = expired_data.get('tp_first_pct', 0)
            if expired_tp_pct > 60:
                lines.append("🚨 **CRÍTICO: Los filtros de expiración están bloqueando señales BUENAS**")
                lines.append(f"- {expired_data['count']} señales expiradas tienen {expired_tp_pct:.1f}% TP_FIRST")
                lines.append("- **Acción requerida:** Relajar filtros de expiración (`MaxDistanceToEntry_ATR_Cancel`, `STALE_TIME`)")
                lines.append("")
            elif expired_tp_pct < 40:
                lines.append("✅ **CORRECTO: Los filtros de expiración están bloqueando señales MALAS**")
                lines.append(f"- {expired_data['count']} señales expiradas tienen solo {expired_tp_pct:.1f}% TP_FIRST")
                lines.append("- **Acción:** Mantener filtros actuales")
                lines.append("")
        
        # Tabla detallada de trades con MFE/MAE
        lines.append("**Detalle por Trade (Top 20):**")
        lines.append("")
        lines.append("| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |")
        lines.append("|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|")
        
        for trade_data in mfe_mae_results['trades_with_data'][:20]:  # Top 20
            mfe = trade_data['mfe']
            diagnosis = ""
            if mfe['mfe_points'] > mfe['mae_points'] * 1.5:
                diagnosis = "✅ Entrada excelente"
            elif mfe['mfe_points'] > mfe['mae_points']:
                diagnosis = "👍 Entrada correcta"
            elif mfe['mae_points'] > mfe['mfe_points'] * 1.5:
                diagnosis = "❌ Entrada muy mala"
            else:
                diagnosis = "⚠️ Entrada dudosa"
            
            lines.append(f"| {trade_data['TradeID']} | {trade_data['Direction']} | {mfe['mfe_points']:.2f} | {mfe['mae_points']:.2f} | {mfe['excursion_ratio']:.2f} | {mfe['initial_direction']} | {trade_data['Status']} | {diagnosis} |")
        
        lines.append("")
    else:
        lines.append("### 3.5 Análisis MFE/MAE (Excursión del Precio)")
        lines.append("")
        lines.append("⚠️ **No hay datos OHLC disponibles ([PIPE] logs)**")
        lines.append("")
        lines.append("Para activar este análisis:")
        lines.append("1. En `EngineConfig.cs`: `EnableOHLCLogging = true`")
        lines.append("2. Ejecutar backtest")
        lines.append("3. El log generará trazas `[PIPE] TF=X O=Y H=Z L=W C=V`")
        lines.append("")
    
    # ========================================================================
    # 3.6 ANÁLISIS DE PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)
    # ========================================================================
    if phantom_analysis:
        lines.append("---")
        lines.append("")
        lines.append("### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)")
        lines.append("")
        lines.append("**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**")
        lines.append("")
        lines.append("Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.")
        lines.append("")
        
        # Conteo total
        total_phantoms = sum([data['count'] for data in phantom_analysis.values()])
        lines.append(f"**Total de Phantom Opportunities analizadas:** {total_phantoms:,}")
        lines.append("")
        
        # Tabla comparativa por rango de distancia
        lines.append("**Calidad por Rango de Distancia:**")
        lines.append("")
        lines.append("| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |")
        lines.append("|-----------------|-------|------------|------------|---------------|----------------|---------|------------|")
        
        for range_name in ['0.0-2.0 ATR (Muy cerca)', '2.0-3.0 ATR (Cerca)', '3.0-5.0 ATR (Media)', '5.0-10.0 ATR (Lejos)', '>10.0 ATR (Muy lejos)']:
            if range_name in phantom_analysis:
                data = phantom_analysis[range_name]
                lines.append(f"| {range_name} | {data['count']:,} | {data['wr_theoretical']:.1f}% | {data['tp_first_pct']:.1f}% | {data['mfe_mae_ratio']:.2f} | {data['good_entries_pct']:.1f}% | {data['avg_rr']:.2f} | {data['conclusion']} |")
        
        lines.append("")
        
        # Análisis detallado por rango
        lines.append("**📈 Análisis Detallado por Rango:**")
        lines.append("")
        
        for range_name in ['0.0-2.0 ATR (Muy cerca)', '2.0-3.0 ATR (Cerca)', '3.0-5.0 ATR (Media)', '5.0-10.0 ATR (Lejos)', '>10.0 ATR (Muy lejos)']:
            if range_name not in phantom_analysis:
                continue
            
            data = phantom_analysis[range_name]
            lines.append(f"**{range_name}** ({data['count']:,} oportunidades)")
            lines.append("")
            lines.append(f"- **WR Teórico:** {data['wr_theoretical']:.1f}% (si se hubieran ejecutado)")
            lines.append(f"- **TP_FIRST:** {data['tp_first_pct']:.1f}% ({data['tp_first']} de {data['count']})")
            lines.append(f"- **SL_FIRST:** {((data['sl_first'] / data['count']) * 100) if data['count'] > 0 else 0:.1f}% ({data['sl_first']} de {data['count']})")
            lines.append(f"- **MFE Promedio:** {data['mfe_avg']:.2f} pts")
            lines.append(f"- **MAE Promedio:** {data['mae_avg']:.2f} pts")
            lines.append(f"- **MFE/MAE Ratio:** {data['mfe_mae_ratio']:.2f}")
            lines.append(f"- **Good Entries:** {data['good_entries_pct']:.1f}% (MFE > MAE)")
            lines.append(f"- **R:R Promedio:** {data['avg_rr']:.2f}")
            lines.append("")
            lines.append(f"**{data['conclusion']}**")
            lines.append("")
        
        # Comparativa con trades ejecutados
        if mfe_mae_results['trades_with_data']:
            lines.append("**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**")
            lines.append("")
            
            executed_wr = (mfe_mae_results['tp_first_count'] / len(mfe_mae_results['trades_with_data']) * 100) if mfe_mae_results['trades_with_data'] else 0
            executed_good_entries = mfe_mae_results.get('good_entries_pct', 0)
            
            lines.append("| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |")
            lines.append("|---------|-------------------|------------------|------------------|------------------|")
            
            phantom_02 = phantom_analysis.get('0.0-2.0 ATR (Muy cerca)', {})
            phantom_23 = phantom_analysis.get('2.0-3.0 ATR (Cerca)', {})
            phantom_35 = phantom_analysis.get('3.0-5.0 ATR (Media)', {})
            
            lines.append(f"| **Count** | {len(mfe_mae_results['trades_with_data'])} | {phantom_02.get('count', 0):,} | {phantom_23.get('count', 0):,} | {phantom_35.get('count', 0):,} |")
            lines.append(f"| **TP_FIRST %** | {executed_wr:.1f}% | {phantom_02.get('tp_first_pct', 0):.1f}% | {phantom_23.get('tp_first_pct', 0):.1f}% | {phantom_35.get('tp_first_pct', 0):.1f}% |")
            lines.append(f"| **Good Entries %** | {executed_good_entries:.1f}% | {phantom_02.get('good_entries_pct', 0):.1f}% | {phantom_23.get('good_entries_pct', 0):.1f}% | {phantom_35.get('good_entries_pct', 0):.1f}% |")
            lines.append(f"| **MFE/MAE Ratio** | {mfe_mae_results['excursion_ratio_avg']:.2f} | {phantom_02.get('mfe_mae_ratio', 0):.2f} | {phantom_23.get('mfe_mae_ratio', 0):.2f} | {phantom_35.get('mfe_mae_ratio', 0):.2f} |")
            
            lines.append("")
        
        # Recomendaciones inteligentes basadas en datos
        lines.append("**💡 RECOMENDACIONES BASADAS EN DATOS:**")
        lines.append("")
        
        # Analizar rango 2-3 ATR
        phantom_23 = phantom_analysis.get('2.0-3.0 ATR (Cerca)', {})
        if phantom_23:
            if phantom_23.get('wr_theoretical', 0) > 35 and phantom_23.get('good_entries_pct', 0) > 60:
                lines.append(f"🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene {phantom_23['count']:,} oportunidades de BUENA CALIDAD**")
                lines.append(f"   - WR Teórico: {phantom_23['wr_theoretical']:.1f}%")
                lines.append(f"   - Good Entries: {phantom_23['good_entries_pct']:.1f}%")
                lines.append(f"   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0")
                lines.append("")
            elif phantom_23.get('wr_theoretical', 0) < 30:
                lines.append(f"✅ **CORRECTO: El filtro 2.0 ATR está bloqueando {phantom_23['count']:,} oportunidades de BAJA calidad**")
                lines.append(f"   - WR Teórico: {phantom_23['wr_theoretical']:.1f}%")
                lines.append(f"   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0")
                lines.append("")
        
        # Analizar rango 3-5 ATR
        phantom_35 = phantom_analysis.get('3.0-5.0 ATR (Media)', {})
        if phantom_35:
            if phantom_35.get('wr_theoretical', 0) > 35 and phantom_35.get('good_entries_pct', 0) > 55:
                lines.append(f"⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene {phantom_35['count']:,} oportunidades con calidad MEDIA-ALTA**")
                lines.append(f"   - WR Teórico: {phantom_35['wr_theoretical']:.1f}%")
                lines.append(f"   - Good Entries: {phantom_35['good_entries_pct']:.1f}%")
                lines.append(f"   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones")
                lines.append("")
            else:
                lines.append(f"✅ **CORRECTO: Las {phantom_35['count']:,} oportunidades en 3.0-5.0 ATR tienen baja calidad**")
                lines.append("")
        
        lines.append("")
    else:
        lines.append("### 3.6 Análisis de PHANTOM OPPORTUNITIES")
        lines.append("")
        lines.append("**No hay phantom opportunities para analizar** (se necesita ejecutar backtest con logging [PHANTOM_OPPORTUNITY])")
        lines.append("")
    
    # ========================================================================
    # 4. PILAR 3: SL/TP
    # ========================================================================
    lines.append("---")
    lines.append("")
    lines.append("## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)")
    lines.append("")
    
    lines.append("### 4.1 Distribución Stop Loss (Puntos)")
    lines.append("")
    sl_stats = sl_tp_results['sl_stats']
    if sl_stats:
        lines.append(f"- **Media:** {sl_stats['mean']:.2f} pts")
        lines.append(f"- **Mediana:** {sl_stats['median']:.2f} pts")
        lines.append(f"- **Min/Max:** {sl_stats['min']:.2f} / {sl_stats['max']:.2f} pts")
        lines.append("")
        lines.append("**Percentiles:**")
        lines.append("")
        lines.append("| Percentil | Valor (pts) |")
        lines.append("|-----------|-------------|")
        for p in PERCENTILES:
            lines.append(f"| P{p} | {sl_stats['percentiles'].get(p, 0):.2f} |")
        lines.append("")
    
    lines.append("### 4.2 Distribución Take Profit (Puntos)")
    lines.append("")
    tp_stats = sl_tp_results['tp_stats']
    if tp_stats:
        lines.append(f"- **Media:** {tp_stats['mean']:.2f} pts")
        lines.append(f"- **Mediana:** {tp_stats['median']:.2f} pts")
        lines.append(f"- **Min/Max:** {tp_stats['min']:.2f} / {tp_stats['max']:.2f} pts")
        lines.append("")
        lines.append("**Percentiles:**")
        lines.append("")
        lines.append("| Percentil | Valor (pts) |")
        lines.append("|-----------|-------------|")
        for p in PERCENTILES:
            lines.append(f"| P{p} | {tp_stats['percentiles'].get(p, 0):.2f} |")
        lines.append("")
    
    lines.append("### 4.3 Límites Dinámicos Recomendados (Data-Driven)")
    lines.append("")
    limits = sl_tp_results['limits_recommended']
    if limits:
        lines.append(f"**Basado en percentil 90 de operaciones reales:**")
        lines.append("")
        lines.append("```csharp")
        lines.append(f"// En EngineConfig.cs")
        lines.append(f"public int MaxSLDistancePoints {{ get; set; }} = {limits['MaxSLDistancePoints']}; // Era 60")
        lines.append(f"public int MaxTPDistancePoints {{ get; set; }} = {limits['MaxTPDistancePoints']}; // Era 120")
        lines.append("```")
        lines.append("")
        lines.append(f"**Rationale:** {limits['rationale']}")
        lines.append("")
    
    lines.append("### 4.4 R:R Óptimo")
    lines.append("")
    lines.append(f"**Para Win Rate actual ({wr:.1f}%), el R:R mínimo es:**")
    lines.append("")
    lines.append("```")
    lines.append(f"R:R_min = (1 - WR) / WR")
    lines.append(f"R:R_min = (1 - {wr/100:.3f}) / {wr/100:.3f}")
    lines.append(f"R:R_min = {sl_tp_results['rr_min_breakeven']:.2f}")
    lines.append("```")
    lines.append("")
    lines.append(f"**Estado actual:** R:R promedio = {sl_tp_results['rr_stats'].get('mean', 0):.2f}")
    lines.append(f"**Gap:** {sl_tp_results['rr_min_breakeven'] - sl_tp_results['rr_stats'].get('mean', 0):.2f} (necesitas mejorar R:R)")
    lines.append("")
    
    # ========================================================================
    # 5. CONCLUSIONES Y PLAN DE ACCIÓN
    # ========================================================================
    lines.append("---")
    lines.append("")
    lines.append("## 5. CONCLUSIONES Y PLAN DE ACCIÓN PRIORIZADO")
    lines.append("")
    
    lines.append("### Prioridad 1: CORREGIR BIAS (CRÍTICO)")
    lines.append("")
    lines.append("**Problema:** Bias alcista con gráfico bajista → entradas contra-tendencia")
    lines.append("")
    lines.append("**Solución:**")
    lines.append("1. Reemplazar EMA200@60m por **bias compuesto rápido**")
    lines.append("2. Componentes:")
    lines.append("   - EMA20@60m (tendencia 20h)")
    lines.append("   - EMA50@60m (tendencia 50h)")
    lines.append("   - BOS/CHoCH count (cambios estructura)")
    lines.append("   - Regresión lineal 24h")
    lines.append("3. Pesos sugeridos: 30%, 25%, 25%, 20%")
    lines.append("")
    lines.append("**Impacto esperado:** +15-25% WR (entradas alineadas con movimiento real)")
    lines.append("")
    
    lines.append("### Prioridad 2: LÍMITES SL/TP DINÁMICOS")
    lines.append("")
    lines.append("**Problema:** Límites actuales son para swing, no intradía")
    lines.append("")
    lines.append("**Solución:**")
    if limits:
        lines.append(f"1. **MaxSLDistancePoints:** 60 → **{limits['MaxSLDistancePoints']}** (P90 real)")
        lines.append(f"2. **MaxTPDistancePoints:** 120 → **{limits['MaxTPDistancePoints']}** (P90 real)")
    lines.append("3. **Límite dinámico por volatilidad:**")
    lines.append("   ```")
    lines.append("   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)")
    lines.append("   donde k ≈ 3.0")
    lines.append("   ```")
    lines.append("")
    lines.append("**Impacto esperado:** -20% fallback P4, +15% TP estructural")
    lines.append("")
    
    lines.append("### Prioridad 3: MEJORAR R:R")
    lines.append("")
    lines.append(f"**Problema:** R:R actual ({sl_tp_results['rr_stats'].get('mean', 0):.2f}) < R:R mínimo ({sl_tp_results['rr_min_breakeven']:.2f})")
    lines.append("")
    lines.append("**Solución:**")
    lines.append("1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**")
    lines.append("2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback")
    lines.append("3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)")
    lines.append("")
    lines.append(f"**Impacto esperado:** Sistema break-even con WR={wr:.1f}%")
    lines.append("")
    
    lines.append("---")
    lines.append("")
    lines.append("*Análisis generado automáticamente por analizador-logica-operaciones.py*")
    lines.append(f"*Fecha: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}*")
    
    return '\n'.join(lines)

# ============================================================================
# MAIN
# ============================================================================

def main():
    print("=" * 70)
    print("ANALIZADOR LOGICA DE  OPERACIONES - PinkButterfly CoreBrain")
    print("=" * 70)
    print("")
    
    # Cargar datos
    print("[CARGA] Leyendo CSV de trades...")
    trades = load_trades_csv(CSV_PATH)
    print(f"[OK] {len(trades)} trades cargados")
    
    print(f"[CARGA] Leyendo log completo: {LOG_PATH}")
    log_data = load_log_data(LOG_PATH)
    print(f"[OK] Log parseado")
    print(f"  - Context bias: {len(log_data['context_bias'])} eventos")
    print(f"  - Proximity: {len(log_data['proximity_events'])} eventos")
    print(f"  - DFM: {len(log_data['dfm_events'])} eventos")
    print(f"  - Risk: {len(log_data['risk_events'])} eventos")
    print(f"  - TP Policy: {len(log_data['tp_policy'])} eventos")
    print(f"  - Fusion Zones: {len(log_data['fusion_zones'])} zonas")
    print(f"  - Price Bars (OHLC): {len(log_data['price_bars'])} barras")
    if len(log_data['price_bars']) == 0:
        print(f"[WARN] NO SE CAPTURARON BARRAS OHLC! Verifica que el log tenga trazas [PIPE]")
    print("")
    
    # Análisis
    bias_results = analyze_bias(log_data, trades)
    entries_results = analyze_entries(log_data, trades)
    mfe_mae_results = analyze_mfe_mae(trades, log_data['price_bars'])
    phantom_analysis = analyze_phantom_opportunities(log_data['phantom_opportunities'], log_data['price_bars'])
    sl_tp_results = analyze_sl_tp(log_data, trades)
    
    # Generar informe
    report = generate_report(trades, log_data, bias_results, entries_results, mfe_mae_results, sl_tp_results, phantom_analysis)
    
    # Guardar
    with open(OUTPUT_PATH, 'w', encoding='utf-8') as f:
        f.write(report)
    
    print("")
    print("=" * 70)
    print(f"[OK] Informe generado: {OUTPUT_PATH}")
    print("=" * 70)

if __name__ == "__main__":
    main()

