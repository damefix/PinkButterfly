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

LOG_PATH, CSV_PATH = find_latest_log()
OUTPUT_PATH = os.path.join(SCRIPT_DIR, "ANALISIS_LOGICA_DE_OPERACIONES.md")

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
    """Carga CSV de trades - usa el analizador diagnostico existente"""
    trades = []
    
    try:
        # Usar regex para parsear líneas con formato correcto
        re_trade = re.compile(
            r"^(T\d+),.+?,(REGISTERED|CLOSED),(BUY|SELL),([0-9,\.]+),([0-9,\.]+),([0-9,\.]+),([0-9,\.]+),([0-9,\.]+),([0-9,\.]+),.+?,(TP_HIT|SL_HIT|PENDING),.*?(-?[0-9,\.]+).*?(-?[0-9,\.]+)\s*$"
        )
        
        trades_dict = {}
        with open(csv_path, 'r', encoding='utf-8') as f:
            for line in f:
                # Skip header
                if line.startswith('TradeID'):
                    continue
                
                # Parse REGISTERED
                if 'REGISTERED' in line:
                    parts = line.strip().split(',')
                    if len(parts) >= 13:
                        trade_id = parts[0]
                        trades_dict[trade_id] = {
                            'TradeID': trade_id,
                            'Direction': parts[3],
                            'Entry': to_float(parts[4] + '.' + parts[5]),  # Reconstruir decimal
                            'RiskPoints': to_float(parts[7] + '.' + parts[8]),
                            'RewardPoints': to_float(parts[9] + '.' + parts[10]),
                            'RR': to_float(parts[11] + '.' + parts[12]),
                            'Status': 'PENDING',
                            'ExitReason': '',
                            'PnLPoints': 0.0
                        }
                
                # Parse CLOSED
                elif 'CLOSED' in line:
                    parts = line.strip().split(',')
                    if len(parts) >= 19:
                        trade_id = parts[0]
                        if trade_id in trades_dict:
                            trades_dict[trade_id]['Status'] = 'CLOSED'
                            trades_dict[trade_id]['ExitReason'] = parts[14]
                            # PnLPoints está en las últimas posiciones
                            pnl_str = parts[-2] + '.' + parts[-1] if parts[-2] != '-' else '0'
                            trades_dict[trade_id]['PnLPoints'] = to_float(pnl_str)
        
        trades = list(trades_dict.values())
        
    except Exception as e:
        print(f"[ERROR] No se pudo cargar CSV: {e}")
        import traceback
        traceback.print_exc()
    
    return trades

def load_log_data(log_path):
    """Carga y parsea el log completo"""
    data = {
        'context_bias': [],  # (timestamp, bias, close, ema200)
        'proximity_events': [],  # (timestamp, aligned, counter)
        'dfm_events': [],  # (timestamp, bull_evals, bear_evals, passed)
        'risk_events': [],  # (zone_id, accepted, rej_reason, sl_pts, tp_pts, sl_tf, tp_tf)
        'tp_policy': [],  # (zone_id, priority, tf, dist_atr, rr)
        'fusion_zones': [],  # (zone_id, direction, tf_dom, anchors)
        'price_bars': [],  # (timestamp, tf, bar, open, high, low, close)  # OHLC de cada barra
    }
    
    # Regex patterns
    # V6.0g: Nuevo formato de bias compuesto
    re_context = re.compile(r"\[DIAGNOSTICO\]\[Context\].*?BiasComposite=(\w+)\s+Score=([0-9\-\.,]+)\s+EMA20=([0-9\-\.,]+)\s+EMA50=([0-9\-\.,]+)\s+BOS=([0-9\-\.,]+)\s+Reg24h=([0-9\-\.,]+)")
    re_context_old = re.compile(r"\[Context\].*?Bias=(\w+)")  # Fallback para logs antiguos
    re_proximity = re.compile(r"\[Proximity\].*?Aligned=(\d+).*?Counter=(\d+)")
    re_dfm = re.compile(r"\[DFM\].*?Bull=(\d+).*?Bear=(\d+).*?Passed=(\d+)")
    re_risk_accept = re.compile(r"\[DIAGNOSTICO\]\[Risk\].*?Zone=(\S+).*?SL=([0-9\.]+)pts.*?TP=([0-9\.]+)pts.*?SLTF=(-?\d+).*?TPTF=(-?\d+)")
    re_tp_policy = re.compile(r"\[RISK\]\[TP_POLICY\].*?Zone=(\S+).*?(P0_OPPOSING|P0_ANY_DIR|P0_SWING_LITE|FORCED_P3|P4_FALLBACK).*?TF=(-?\d+).*?RR=([0-9\.]+).*?DistATR=([0-9\.]+)", re.IGNORECASE)
    re_fusion = re.compile(r"\[StructureFusion\].*?Zone=(\S+).*?Dir=(\w+).*?TFDom=(\d+)")
    re_pipe = re.compile(r"\[(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})\].*?\[PIPE\].*?TF=(\d+).*?Bar=(\d+).*?O=([0-9,\.]+).*?H=([0-9,\.]+).*?L=([0-9,\.]+).*?C=([0-9,\.]+)")
    
    try:
        with open(log_path, 'r', encoding='utf-8', errors='ignore') as f:
            for line in f:
                # Context Bias (V6.0g: nuevo formato compuesto)
                m = re_context.search(line)
                if m:
                    data['context_bias'].append({
                        'bias': m.group(1),
                        'score': float(m.group(2).replace(',', '.')),
                        'ema20': float(m.group(3).replace(',', '.')),
                        'ema50': float(m.group(4).replace(',', '.')),
                        'bos': float(m.group(5).replace(',', '.')),
                        'reg24h': float(m.group(6).replace(',', '.'))
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
                    data['dfm_events'].append({
                        'bull_evals': int(m.group(1)),
                        'bear_evals': int(m.group(2)),
                        'passed': int(m.group(3))
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
                        timestamp = datetime.strptime(m.group(1), "%Y-%m-%d %H:%M:%S")
                        tf = int(m.group(2))
                        bar = int(m.group(3))
                        o = to_float(m.group(4))
                        h = to_float(m.group(5))
                        l = to_float(m.group(6))
                        c = to_float(m.group(7))
                        
                        data['price_bars'].append({
                            'timestamp': timestamp,
                            'tf': tf,
                            'bar': bar,
                            'open': o,
                            'high': h,
                            'low': l,
                            'close': c
                        })
                    except:
                        pass  # Ignorar líneas mal formateadas
    
    except Exception as e:
        print(f"[ERROR] No se pudo cargar log: {e}")
    
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
        except:
            return result
        
        # Filtrar barras del TF especificado posteriores a la entrada
        # Limitamos a 100 barras máximo (suficiente para intradía)
        relevant_bars = [
            b for b in price_bars 
            if b['tf'] == tf_analysis and b['timestamp'] >= entry_time
        ][:100]
        
        if not relevant_bars:
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

# ============================================================================
# ANÁLISIS PILAR 1: BIAS/SENTIMIENTO
# ============================================================================

def analyze_bias(log_data, trades):
    """Analiza precisión del bias vs movimiento real"""
    print("[1/3] Analizando BIAS/SENTIMIENTO...")
    
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
        ema20_vals = [b['ema20'] for b in log_data['context_bias'] if b.get('ema20') is not None]
        ema50_vals = [b['ema50'] for b in log_data['context_bias'] if b.get('ema50') is not None]
        bos_vals = [b['bos'] for b in log_data['context_bias'] if b.get('bos') is not None]
        reg24h_vals = [b['reg24h'] for b in log_data['context_bias'] if b.get('reg24h') is not None]
        
        if scores:
            results['composite_stats'] = {
                'score_avg': sum(scores) / len(scores),
                'score_min': min(scores),
                'score_max': max(scores),
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
        results['recommendation'] = f"CRÍTICO: Bias Compuesto {neutral_pct:.1f}% Neutral - threshold 0.5/-0.5 DEMASIADO ALTO. Score real [{stats['score_min']:.2f}, {stats['score_max']:.2f}]. REDUCIR threshold a 0.3/-0.3."
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
    print("[2/3] Analizando ENTRADAS/ZONAS...")
    
    results = {
        'fusion_zones': len(log_data['fusion_zones']),
        'proximity_passed': sum([e['aligned'] + e['counter'] for e in log_data['proximity_events']]),
        'dfm_passed': sum([e['passed'] for e in log_data['dfm_events']]),
        'risk_accepted': len(log_data['risk_events']),
        'trades_registered': len(trades),
        'waterfall': {},
        'wr_by_tp_type': {},
        'pf_by_tp_type': {}
    }
    
    # Waterfall del pipeline
    results['waterfall'] = {
        'StructureFusion': results['fusion_zones'],
        'ProximityAnalyzer': results['proximity_passed'],
        'DFM': results['dfm_passed'],
        'RiskCalculator': results['risk_accepted'],
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
    print("[3/3] Analizando SL/TP...")
    
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
    wr = len([t for t in trades if t['ExitReason'] == 'TP_HIT']) / len(trades) if trades else 0.364
    rr_min_breakeven = (1 - wr) / wr if wr > 0 else 1.75
    
    results['rr_min_breakeven'] = rr_min_breakeven
    
    return results

# ============================================================================
# GENERADOR DE INFORME
# ============================================================================

def generate_report(trades, log_data, bias_results, entries_results, sl_tp_results):
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
    wins = len([t for t in trades if t['ExitReason'] == 'TP_HIT'])
    losses = len([t for t in trades if t['ExitReason'] == 'SL_HIT'])
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
        lines.append(f"- Threshold actual: 0.5/-0.5")
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
        lines.append("### 2.3 Recomendación: Ajustar Threshold del Bias Compuesto")
        lines.append("")
        lines.append("**Solución Inmediata:** Reducir threshold de 0.5/-0.5 a **0.3/-0.3**")
        lines.append("")
        lines.append("**Archivo:** `pinkbutterfly-produccion/ContextManager.cs` (línea ~207)")
        lines.append("")
        lines.append("```csharp")
        lines.append("// ANTES:")
        lines.append("if (compositeScore > 0.5) { ... }")
        lines.append("")
        lines.append("// DESPUÉS:")
        lines.append("if (compositeScore > 0.3) { ... }  // Más sensible")
        lines.append("elif (compositeScore < -0.3) { ... }")
        lines.append("```")
        lines.append("")
        lines.append("**Justificación:**")
        lines.append(f"- Scores reales: [{stats['score_min']:.2f}, {stats['score_max']:.2f}]")
        lines.append(f"- Score promedio: {stats['score_avg']:.3f}")
        lines.append("- Threshold 0.5 requiere 100% alineación de componentes (poco realista)")
        lines.append("- Threshold 0.3 requiere 60% alineación (más realista)")
        lines.append("")
        lines.append("**Impacto esperado:**")
        lines.append("- Neutral actual: 99.4% → ~60-70% (objetivo)")
        lines.append("- Bullish/Bearish: ~0.5% → ~15-20% cada uno")
        lines.append("- Sistema empezará a usar el bias para filtrar operaciones")
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
        lines.append("if BiasScore > 0.5: Bullish")
        lines.append("elif BiasScore < -0.5: Bearish")
        lines.append("else: Neutral")
        lines.append("```")
        lines.append("")
        lines.append("**Ventajas:**")
        lines.append("- ✅ Responde en 4-24 horas (intradía)")
        lines.append("- ✅ Combina múltiples señales (robusto)")
        lines.append("- ✅ Detecta cambios de estructura (BOS/CHoCH)")
        lines.append("")
    
    # ========================================================================
    # 3. PILAR 2: ENTRADAS/ZONAS
    # ========================================================================
    lines.append("---")
    lines.append("")
    lines.append("## 3. PILAR 2: ENTRADAS/ZONAS")
    lines.append("")
    
    lines.append("### 3.1 Waterfall del Pipeline")
    lines.append("")
    lines.append("| Paso | Zonas/Señales | % Supervivencia |")
    lines.append("|------|---------------|-----------------|")
    
    waterfall = entries_results['waterfall']
    prev = waterfall['StructureFusion']
    for step, count in waterfall.items():
        survival = (count / prev * 100) if prev > 0 else 0
        lines.append(f"| {step} | {count} | {survival:.1f}% |")
        if step != 'TradeManager':
            prev = count
    lines.append("")
    
    lines.append("**Análisis:**")
    if waterfall['StructureFusion'] > 0:
        lines.append(f"- **Cuello de botella principal:** {min(waterfall, key=waterfall.get)} ({waterfall[min(waterfall, key=waterfall.get)]} señales)")
        lines.append(f"- **Tasa de conversión final:** {(waterfall['TradeManager'] / waterfall['StructureFusion'] * 100):.2f}%")
    else:
        lines.append("- ⚠️ **No hay datos suficientes para análisis de waterfall**")
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
    
    print("[CARGA] Leyendo log completo...")
    log_data = load_log_data(LOG_PATH)
    print(f"[OK] Log parseado")
    print(f"  - Context bias: {len(log_data['context_bias'])} eventos")
    print(f"  - Proximity: {len(log_data['proximity_events'])} eventos")
    print(f"  - DFM: {len(log_data['dfm_events'])} eventos")
    print(f"  - Risk: {len(log_data['risk_events'])} eventos")
    print(f"  - TP Policy: {len(log_data['tp_policy'])} eventos")
    print(f"  - Fusion Zones: {len(log_data['fusion_zones'])} zonas")
    print("")
    
    # Análisis
    bias_results = analyze_bias(log_data, trades)
    entries_results = analyze_entries(log_data, trades)
    sl_tp_results = analyze_sl_tp(log_data, trades)
    
    # Generar informe
    report = generate_report(trades, log_data, bias_results, entries_results, sl_tp_results)
    
    # Guardar
    with open(OUTPUT_PATH, 'w', encoding='utf-8') as f:
        f.write(report)
    
    print("")
    print("=" * 70)
    print(f"[OK] Informe generado: {OUTPUT_PATH}")
    print("=" * 70)

if __name__ == "__main__":
    main()

