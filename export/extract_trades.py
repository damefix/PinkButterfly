#!/usr/bin/env python3
"""
Script para extraer y analizar todas las operaciones del log de PinkButterfly
"""

import re
from collections import defaultdict

def extract_all_trades(log_file):
    """Extrae todas las operaciones del log"""
    
    trades = []
    trade_results = {}
    
    with open(log_file, 'r', encoding='utf-8', errors='ignore') as f:
        for line in f:
            # Extraer órdenes registradas (números con coma)
            match_order = re.search(r'ORDEN REGISTRADA.*?(BUY|SELL) LIMIT @ ([\d,]+).*?SL=([\d,]+), TP=([\d,]+).*?Bar=(\d+)', line)
            if match_order:
                action = match_order.group(1)
                entry = float(match_order.group(2).replace(',', '.'))
                sl = float(match_order.group(3).replace(',', '.'))
                tp = float(match_order.group(4).replace(',', '.'))
                bar = int(match_order.group(5))
                
                trade_id = f"{action}_{entry}_{bar}"
                trades.append({
                    'id': trade_id,
                    'action': action,
                    'entry': entry,
                    'sl': sl,
                    'tp': tp,
                    'bar': bar,
                    'result': 'PENDING'
                })
            
            # Extraer resultados (TP/SL) - números con coma
            match_tp = re.search(r'CERRADA POR TP: (BUY|SELL) @ ([\d,]+) en barra (\d+)', line)
            if match_tp:
                action = match_tp.group(1)
                entry = float(match_tp.group(2).replace(',', '.'))
                bar = int(match_tp.group(3))
                trade_id = f"{action}_{entry}"
                trade_results[trade_id] = ('TP', bar)
            
            match_sl = re.search(r'CERRADA POR SL: (BUY|SELL) @ ([\d,]+) en barra (\d+)', line)
            if match_sl:
                action = match_sl.group(1)
                entry = float(match_sl.group(2).replace(',', '.'))
                bar = int(match_sl.group(3))
                trade_id = f"{action}_{entry}"
                trade_results[trade_id] = ('SL', bar)
    
    # Asociar resultados con trades
    for trade in trades:
        trade_key = f"{trade['action']}_{trade['entry']}"
        if trade_key in trade_results:
            result, exit_bar = trade_results[trade_key]
            trade['result'] = result
            trade['exit_bar'] = exit_bar
    
    return trades

def calculate_pnl(trades):
    """Calcula P&L de todas las operaciones"""
    
    stats = {
        'total': 0,
        'executed': 0,
        'closed': 0,
        'tp_hit': 0,
        'sl_hit': 0,
        'pending': 0,
        'total_points_won': 0,
        'total_points_lost': 0,
        'pnl_mes': 0,
        'pnl_es': 0,
        'rr_list': [],
        'trades_detail': []
    }
    
    for trade in trades:
        stats['total'] += 1
        
        if trade['result'] == 'PENDING':
            stats['pending'] += 1
            continue
        
        stats['closed'] += 1
        
        # Calcular puntos de riesgo y recompensa
        if trade['action'] == 'BUY':
            risk = trade['entry'] - trade['sl']
            reward = trade['tp'] - trade['entry']
        else:  # SELL
            risk = trade['sl'] - trade['entry']
            reward = trade['entry'] - trade['tp']
        
        rr = reward / risk if risk > 0 else 0
        
        # Calcular P&L
        if trade['result'] == 'TP':
            stats['tp_hit'] += 1
            points = reward
            stats['total_points_won'] += points
            pnl_mes = points * 5
            pnl_es = points * 50
        else:  # SL
            stats['sl_hit'] += 1
            points = -risk
            stats['total_points_lost'] += abs(points)
            pnl_mes = points * 5
            pnl_es = points * 50
        
        stats['pnl_mes'] += pnl_mes
        stats['pnl_es'] += pnl_es
        stats['rr_list'].append(rr)
        
        stats['trades_detail'].append({
            'action': trade['action'],
            'entry': trade['entry'],
            'sl': trade['sl'],
            'tp': trade['tp'],
            'result': trade['result'],
            'points': points,
            'risk': risk,
            'reward': reward,
            'rr': rr,
            'pnl_mes': pnl_mes,
            'pnl_es': pnl_es
        })
    
    # Calcular métricas
    if stats['closed'] > 0:
        stats['win_rate'] = (stats['tp_hit'] / stats['closed']) * 100
    else:
        stats['win_rate'] = 0
    
    if stats['total_points_lost'] > 0:
        stats['profit_factor'] = stats['total_points_won'] / stats['total_points_lost']
    else:
        stats['profit_factor'] = float('inf') if stats['total_points_won'] > 0 else 0
    
    if len(stats['rr_list']) > 0:
        stats['avg_rr'] = sum(stats['rr_list']) / len(stats['rr_list'])
    else:
        stats['avg_rr'] = 0
    
    return stats

def generate_report(stats):
    """Genera reporte detallado"""
    
    report = []
    report.append("=" * 80)
    report.append("ANÁLISIS COMPLETO DE OPERACIONES - PINKBUTTERFLY")
    report.append("=" * 80)
    report.append("")
    
    # Resumen
    report.append("📊 RESUMEN EJECUTIVO")
    report.append("-" * 80)
    report.append(f"Total Señales Generadas:     {stats['total']}")
    report.append(f"Operaciones Cerradas:        {stats['closed']}")
    report.append(f"Operaciones Pendientes:      {stats['pending']}")
    report.append(f"")
    if stats['closed'] > 0:
        report.append(f"Ganadoras (TP):              {stats['tp_hit']} ({stats['tp_hit']/stats['closed']*100:.1f}%)")
        report.append(f"Perdedoras (SL):             {stats['sl_hit']} ({stats['sl_hit']/stats['closed']*100:.1f}%)")
    else:
        report.append(f"Ganadoras (TP):              {stats['tp_hit']}")
        report.append(f"Perdedoras (SL):             {stats['sl_hit']}")
    report.append(f"Win Rate:                    {stats['win_rate']:.2f}%")
    report.append(f"")
    report.append(f"Puntos Ganados:              +{stats['total_points_won']:.2f}")
    report.append(f"Puntos Perdidos:             -{stats['total_points_lost']:.2f}")
    report.append(f"Neto (Puntos):               {stats['total_points_won'] - stats['total_points_lost']:.2f}")
    report.append(f"")
    report.append(f"P&L MES ($5/punto):          ${stats['pnl_mes']:.2f}")
    report.append(f"P&L ES ($50/punto):          ${stats['pnl_es']:.2f}")
    report.append(f"")
    report.append(f"Profit Factor:               {stats['profit_factor']:.2f}")
    report.append(f"R:R Promedio:                {stats['avg_rr']:.2f}")
    report.append("")
    
    # Detalle de operaciones
    report.append("=" * 80)
    report.append("📋 DETALLE DE TODAS LAS OPERACIONES CERRADAS")
    report.append("=" * 80)
    report.append("")
    
    for i, trade in enumerate(stats['trades_detail'], 1):
        result_emoji = "🟢" if trade['result'] == 'TP' else "🔴"
        report.append(f"OPERACIÓN {i}: {trade['action']} @ {trade['entry']:.2f}")
        report.append(f"  SL: {trade['sl']:.2f} | TP: {trade['tp']:.2f}")
        report.append(f"  Riesgo: {trade['risk']:.2f} pts | Recompensa: {trade['reward']:.2f} pts")
        report.append(f"  R:R: {trade['rr']:.2f}")
        report.append(f"  Resultado: {result_emoji} {trade['result']} ({trade['points']:+.2f} pts)")
        report.append(f"  P&L MES: ${trade['pnl_mes']:+.2f} | P&L ES: ${trade['pnl_es']:+.2f}")
        report.append("")
    
    # Análisis de R:R
    report.append("=" * 80)
    report.append("📊 ANÁLISIS DE RISK:REWARD")
    report.append("=" * 80)
    report.append("")
    
    rr_ranges = {
        '< 0.5': 0,
        '0.5-1.0': 0,
        '1.0-2.0': 0,
        '2.0-3.0': 0,
        '> 3.0': 0
    }
    
    for rr in stats['rr_list']:
        if rr < 0.5:
            rr_ranges['< 0.5'] += 1
        elif rr < 1.0:
            rr_ranges['0.5-1.0'] += 1
        elif rr < 2.0:
            rr_ranges['1.0-2.0'] += 1
        elif rr < 3.0:
            rr_ranges['2.0-3.0'] += 1
        else:
            rr_ranges['> 3.0'] += 1
    
    for range_name, count in rr_ranges.items():
        pct = (count / len(stats['rr_list']) * 100) if len(stats['rr_list']) > 0 else 0
        report.append(f"R:R {range_name}: {count} operaciones ({pct:.1f}%)")
    
    return "\n".join(report)

if __name__ == "__main__":
    print("Extrayendo operaciones del log...")
    trades = extract_all_trades("tests/output.txt")
    
    print(f"Encontradas {len(trades)} operaciones")
    print("Calculando P&L...")
    
    stats = calculate_pnl(trades)
    
    print("Generando reporte...")
    report = generate_report(stats)
    
    # Guardar reporte
    with open("export/ANALISIS_COMPLETO_OPERACIONES.txt", "w", encoding="utf-8") as f:
        f.write(report)
    
    print("✅ Reporte generado: export/ANALISIS_COMPLETO_OPERACIONES.txt")
    print("")
    print(report)

