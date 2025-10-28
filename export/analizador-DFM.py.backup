#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
KPI Suite Completa - PinkButterfly CoreBrain
Análisis completo de rentabilidad y calibración del DFM
Incluye: Trade Book, Cancelaciones, Desglose de Confianza
"""

import csv
import re
from datetime import datetime
from collections import defaultdict

def clean_number(s):
    """Limpia y convierte strings a números"""
    if not s or s == '-' or s == 'N/A' or s == '':
        return 0.0
    try:
        # Si ya es un número, devolverlo
        if isinstance(s, (int, float)):
            return float(s)
        # Convertir string: reemplazar coma por punto
        cleaned = str(s).strip().replace(',', '.')
        return float(cleaned)
    except:
        return 0.0

def analyze_log_file(log_file):
    """Analiza el archivo de log para extraer scoring breakdowns"""
    
    print(f"Analizando log: {log_file}")
    
    scoring_breakdowns = []
    
    with open(log_file, 'r', encoding='utf-8', errors='ignore') as f:
        current_breakdown = {}
        in_breakdown = False
        
        for line in f:
            # Detectar inicio de breakdown
            if 'DESGLOSE COMPLETO DE SCORING' in line:
                in_breakdown = True
                current_breakdown = {}
                continue
            
            if in_breakdown:
                # Extraer datos del breakdown
                if 'HeatZone ID:' in line:
                    match = re.search(r'HeatZone ID:\s*(\S+)', line)
                    if match:
                        current_breakdown['zone_id'] = match.group(1)
                
                elif 'Direction:' in line:
                    match = re.search(r'Direction:\s*(\w+)', line)
                    if match:
                        current_breakdown['direction'] = match.group(1)
                
                elif 'Input: CoreScore' in line:
                    match = re.search(r'CoreScore.*?=\s*([\d,]+)', line)
                    if match:
                        current_breakdown['core_score'] = clean_number(match.group(1))
                
                elif 'Input: ProximityScore' in line:
                    match = re.search(r'ProximityScore.*?=\s*([\d,]+)', line)
                    if match:
                        current_breakdown['proximity_score'] = clean_number(match.group(1))
                
                elif 'Output: CoreScoreContribution' in line:
                    match = re.search(r'=\s*([\d,]+)', line)
                    if match:
                        current_breakdown['core_contribution'] = clean_number(match.group(1))
                
                elif 'Output: ProximityContribution' in line:
                    match = re.search(r'=\s*([\d,]+)', line)
                    if match:
                        current_breakdown['proximity_contribution'] = clean_number(match.group(1))
                
                elif 'Output: ConfluenceContribution' in line:
                    match = re.search(r'=\s*([\d,]+)', line)
                    if match:
                        current_breakdown['confluence_contribution'] = clean_number(match.group(1))
                
                elif 'Output: TypeContribution' in line:
                    match = re.search(r'=\s*([\d,]+)', line)
                    if match:
                        current_breakdown['type_contribution'] = clean_number(match.group(1))
                
                elif 'Output: BiasContribution' in line:
                    match = re.search(r'=\s*([\d,]+)', line)
                    if match:
                        current_breakdown['bias_contribution'] = clean_number(match.group(1))
                
                elif 'Output: MomentumContribution' in line:
                    match = re.search(r'=\s*([\d,]+)', line)
                    if match:
                        current_breakdown['momentum_contribution'] = clean_number(match.group(1))
                
                elif 'FinalConfidence' in line:
                    match = re.search(r'FinalConfidence.*?=\s*([\d,]+)', line)
                    if match:
                        current_breakdown['final_confidence'] = clean_number(match.group(1))
                
                elif '¿Supera umbral?' in line:
                    if 'SÍ' in line:
                        current_breakdown['signal_generated'] = True
                    else:
                        current_breakdown['signal_generated'] = False
                    
                    # Fin del breakdown
                    if current_breakdown:
                        scoring_breakdowns.append(current_breakdown.copy())
                    in_breakdown = False
                    current_breakdown = {}
    
    print(f"OK Scoring breakdowns encontrados: {len(scoring_breakdowns)}\n")
    return scoring_breakdowns

def analyze_trades_csv(csv_file):
    """Analiza el CSV y agrupa por Trade ID"""
    
    print("\n" + "="*70)
    print("TRADE BOOK GENERATOR - PinkButterfly CoreBrain")
    print("="*70 + "\n")
    print(f"Analizando: {csv_file}\n")
    
    # Leer CSV y agrupar por Trade ID
    trades_by_id = defaultdict(list)
    
    with open(csv_file, 'r', encoding='utf-8') as f:
        lines = f.readlines()
        
        if not lines:
            print("⚠️ CSV vacío")
            return "", []
        
        # Procesar cada línea manualmente
        for line_num, line in enumerate(lines[1:], start=2):  # Saltar header
            if not line.strip():
                continue
            
            parts = line.strip().split(',')
            
            if len(parts) < 10:
                continue
            
            # Construir el registro
            try:
                # Campos básicos (siempre presentes)
                trade_id = parts[0]
                timestamp = parts[1]
                action = parts[2]
                direction = parts[3]
                
                # Entry siempre tiene formato XXXX,YY (2 partes) - mantener coma
                entry = f"{parts[4]},{parts[5]}" if len(parts) > 5 else parts[4]
                
                # Detectar el tipo de evento por el Action
                if action == 'REGISTERED':
                    # Formato REGISTERED (20 campos después del split por comas decimales):
                    # TradeID,Timestamp,Action,Direction,Entry(2),SL(2),TP(2),RiskPoints(2),RewardPoints(2),RR(2),Bar,EntryBarTime,StructureID,Status,ExitReason,ExitBar,ExitBarTime,ExitPrice,PnLPoints,PnLDollars
                    sl = f"{parts[6]},{parts[7]}" if len(parts) > 7 and parts[6] != '-' else '-'
                    tp = f"{parts[8]},{parts[9]}" if len(parts) > 9 and parts[8] != '-' else '-'
                    risk_points = f"{parts[10]},{parts[11]}" if len(parts) > 11 and parts[10] != '-' else '-'
                    reward_points = f"{parts[12]},{parts[13]}" if len(parts) > 13 and parts[12] != '-' else '-'
                    rr = f"{parts[14]},{parts[15]}" if len(parts) > 15 and parts[14] != '-' else '-'
                    bar = parts[16] if len(parts) > 16 else '-'
                    entry_bar_time = parts[17] if len(parts) > 17 else '-'
                    structure_id = parts[18] if len(parts) > 18 else '-'
                    status = parts[19] if len(parts) > 19 else '-'
                    exit_reason = '-'
                    exit_bar = '-'
                    exit_bar_time = '-'
                    exit_price = '-'
                    pnl_points = '-'
                    pnl_dollars = '-'
                    
                elif action in ['CANCELLED', 'EXPIRED']:
                    # Formato CANCELLED/EXPIRED (20 campos):
                    # TradeID,Timestamp,Action,Direction,Entry(2),-,-,-,-,-,Bar,EntryBarTime,-,Status,ExitReason,-,-,-,-,-
                    sl = '-'
                    tp = '-'
                    risk_points = '-'
                    reward_points = '-'
                    rr = '-'
                    bar = parts[11] if len(parts) > 11 else '-'  # Bar de cancelación/expiración
                    entry_bar_time = parts[12] if len(parts) > 12 else '-'  # Timestamp de entrada
                    structure_id = '-'
                    status = parts[14] if len(parts) > 14 else '-'  # CANCELLED o EXPIRED
                    exit_reason = parts[15] if len(parts) > 15 else '-'  # Razón
                    exit_bar = bar  # El bar de cancelación ES el exit_bar
                    exit_bar_time = entry_bar_time  # Para canceladas, exit_bar_time = entry_bar_time
                    exit_price = '-'
                    pnl_points = '-'
                    pnl_dollars = '-'
                    
                elif action == 'CLOSED':
                    # Formato CLOSED (25 campos):
                    # TradeID,Timestamp,Action,Direction,Entry(2),SL(2),-,-,-,-,Bar,EntryBarTime,-,Status,ExitReason,ExitBar,ExitBarTime,ExitPrice(2),PnLPoints(2),PnLDollars(2)
                    sl = f"{parts[6]},{parts[7]}" if len(parts) > 7 and parts[6] != '-' else '-'
                    tp = '-'
                    risk_points = '-'
                    reward_points = '-'
                    rr = '-'
                    bar = parts[12] if len(parts) > 12 else '-'  # Bar de entrada
                    entry_bar_time = parts[13] if len(parts) > 13 else '-'  # Timestamp de entrada
                    structure_id = parts[14] if len(parts) > 14 else '-'  # StructureID
                    status = parts[15] if len(parts) > 15 else '-'  # SL_HIT o TP_HIT
                    exit_reason = parts[16] if len(parts) > 16 else '-'  # SL o TP
                    exit_bar = parts[17] if len(parts) > 17 else '-'  # Bar de salida
                    exit_bar_time = parts[18] if len(parts) > 18 else '-'  # Timestamp de salida
                    exit_price = f"{parts[19]},{parts[20]}" if len(parts) > 20 and parts[19] != '-' else '-'
                    pnl_points = f"{parts[21]},{parts[22]}" if len(parts) > 22 and parts[21] != '-' else '-'
                    pnl_dollars = f"{parts[23]},{parts[24]}" if len(parts) > 24 and parts[23] != '-' else '-'
                
                else:
                    # Formato desconocido, saltar
                    continue
                
                row = {
                    'TradeID': trade_id,
                    'Timestamp': timestamp,
                    'Action': action,
                    'Direction': direction,
                    'Entry': entry,
                    'SL': sl,
                    'TP': tp,
                    'RiskPoints': risk_points,
                    'RewardPoints': reward_points,
                    'RR': rr,
                    'Bar': bar,
                    'EntryBarTime': entry_bar_time,
                    'StructureID': structure_id,
                    'Status': status,
                    'ExitReason': exit_reason,
                    'ExitBar': exit_bar,
                    'ExitBarTime': exit_bar_time,
                    'ExitPrice': exit_price,
                    'PnLPoints': pnl_points,
                    'PnLDollars': pnl_dollars
                }
                
                if trade_id and trade_id != 'N/A' and trade_id != '':
                    trades_by_id[trade_id].append(row)
            
            except Exception as e:
                print(f"⚠️ Error en línea {line_num}: {e}")
                print(f"   Contenido: {line[:100]}")
                continue
    
    print(f"OK Trade IDs unicos: {len(trades_by_id)}\n")
    
    # Procesar cada trade
    trade_book = []
    
    for trade_id, events in trades_by_id.items():
        # Buscar evento REGISTERED
        registered = next((e for e in events if e['Action'] == 'REGISTERED'), None)
        if not registered:
            continue
        
        # Buscar evento final (CLOSED, CANCELLED, EXPIRED)
        final = next((e for e in events if e['Action'] in ['CLOSED', 'CANCELLED', 'EXPIRED']), None)
        
        # Construir registro del trade
        trade = {
            'trade_id': trade_id,
            'direction': registered.get('Direction', 'N/A'),
            'entry': clean_number(registered.get('Entry', '0')),
            'sl': clean_number(registered.get('SL', '0')),
            'tp': clean_number(registered.get('TP', '0')),
            'risk_points': clean_number(registered.get('RiskPoints', '0')),
            'reward_points': clean_number(registered.get('RewardPoints', '0')),
            'rr': clean_number(registered.get('RR', '0')),
            'entry_bar': registered.get('Bar', 'N/A'),
            'entry_bar_time': registered.get('EntryBarTime', 'N/A'),
            'structure_id': registered.get('StructureID', 'N/A'),
            'status': 'PENDING',
            'exit_reason': 'N/A',
            'exit_bar': 'N/A',
            'exit_bar_time': 'N/A',
            'exit_price': 0.0,
            'pnl_points': 0.0,
            'pnl_dollars': 0.0
        }
        
        if final:
            # El campo 'Status' en el CSV contiene SL_HIT, CANCELLED, EXPIRED, etc.
            final_status = final.get('Status', 'N/A')
            trade['status'] = final_status
            trade['exit_reason'] = final.get('ExitReason', 'N/A')
            # ExitBar ya está correctamente parseado en el evento final
            trade['exit_bar'] = final.get('ExitBar', 'N/A')
            trade['exit_bar_time'] = final.get('ExitBarTime', 'N/A')
            trade['exit_price'] = clean_number(final.get('ExitPrice', '0'))
            trade['pnl_points'] = clean_number(final.get('PnLPoints', '0'))
            trade['pnl_dollars'] = clean_number(final.get('PnLDollars', '0'))
        
        trade_book.append(trade)
    
    # Clasificar trades
    closed = [t for t in trade_book if t['status'] in ['SL_HIT', 'TP_HIT', 'CLOSED']]
    cancelled = [t for t in trade_book if t['status'] == 'CANCELLED']
    expired = [t for t in trade_book if t['status'] == 'EXPIRED']
    pending = [t for t in trade_book if t['status'] == 'PENDING']
    
    tp_hits = [t for t in closed if t['exit_reason'] == 'TP']
    sl_hits = [t for t in closed if t['exit_reason'] == 'SL']
    
    print(f"Resumen de Operaciones:")
    print(f"  - Total registradas: {len(trade_book)}")
    print(f"  - Cerradas: {len(closed)} (TP: {len(tp_hits)}, SL: {len(sl_hits)})")
    print(f"  - Canceladas: {len(cancelled)}")
    print(f"  - Expiradas: {len(expired)}")
    print(f"  - Pendientes: {len(pending)}\n")
    
    # Calcular métricas
    win_rate = (len(tp_hits) / len(closed) * 100) if len(closed) > 0 else 0
    
    total_pnl_points = sum([t['pnl_points'] for t in closed])
    total_pnl_dollars = sum([t['pnl_dollars'] for t in closed])
    
    gross_profit = sum([t['pnl_dollars'] for t in tp_hits])
    gross_loss = abs(sum([t['pnl_dollars'] for t in sl_hits]))
    
    profit_factor = (gross_profit / gross_loss) if gross_loss > 0 else 0
    
    avg_win = (gross_profit / len(tp_hits)) if len(tp_hits) > 0 else 0
    avg_loss = (gross_loss / len(sl_hits)) if len(sl_hits) > 0 else 0
    
    # Razones de cancelación y expiración
    cancel_reasons = defaultdict(int)
    for t in cancelled:
        cancel_reasons[t['exit_reason']] += 1
    
    expire_reasons = defaultdict(int)
    for t in expired:
        expire_reasons[t['exit_reason']] += 1
    
    # Encontrar la última operación ejecutada (cerrada)
    last_closed_trade = None
    if closed:
        # Ordenar por exit_bar_time si está disponible, sino por exit_bar
        closed_sorted = sorted(closed, key=lambda x: x.get('exit_bar_time', 'N/A'), reverse=True)
        last_closed_trade = closed_sorted[0]
    
    # Generar reporte
    report = f"""# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}  
**CSV File:** `{csv_file}`  
**Trades Analizados:** {len(trade_book)}
"""
    
    # Añadir información de última operación
    if last_closed_trade:
        last_exit_date = last_closed_trade.get('exit_bar_time', 'N/A')
        if last_exit_date == 'N/A':
            last_exit_date = f"Bar {last_closed_trade.get('exit_bar', 'N/A')}"
        report += f"""  
**Última Operación Cerrada:** {last_closed_trade['trade_id']} - {last_closed_trade['direction']} - {last_exit_date}
"""
    
    report += f"""
---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | {len(trade_book)} |
| **Operaciones Ejecutadas (Cerradas)** | {len(closed)} |
| **Operaciones Canceladas** | {len(cancelled)} |
| **Operaciones Expiradas** | {len(expired)} |
| **Operaciones Pendientes** | {len(pending)} |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | {win_rate:.1f}% ({len(tp_hits)}/{len(closed)}) |
| **Profit Factor** | {profit_factor:.2f} |
| **P&L Total (Puntos)** | {total_pnl_points:+.2f} |
| **P&L Total (USD)** | ${total_pnl_dollars:+.2f} |
| **Gross Profit** | ${gross_profit:.2f} |
| **Gross Loss** | ${gross_loss:.2f} |
| **Avg Win** | ${avg_win:.2f} |
| **Avg Loss** | ${avg_loss:.2f} |
| **Avg R:R (Planned)** | {sum([t['rr'] for t in closed]) / len(closed) if len(closed) > 0 else 0:.2f} |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas ({len(closed)} total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
"""
    
    # Detalles de operaciones cerradas
    for trade in closed:
        result_icon = "[TP]" if trade['exit_reason'] == 'TP' else "[SL]"
        entry_date = trade['entry_bar_time'] if trade['entry_bar_time'] != 'N/A' else f"Bar {trade['entry_bar']}"
        exit_date = trade['exit_bar_time'] if trade['exit_bar_time'] != 'N/A' else f"Bar {trade['exit_bar']}"
        report += f"| {trade['trade_id']} | {trade['direction']} | {trade['entry']:.2f} | {trade['sl']:.2f} | {trade['tp']:.2f} | {trade['exit_price']:.2f} | {result_icon} {trade['exit_reason']} | {trade['pnl_points']:+.2f} | ${trade['pnl_dollars']:+.2f} | {trade['rr']:.2f} | {entry_date} | {exit_date} |\n"
    
    # Operaciones canceladas (resumen)
    report += f"""

### Operaciones Canceladas ({len(cancelled)} total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
"""
    
    # Top 10 canceladas por R:R
    cancelled_sorted = sorted(cancelled, key=lambda x: x['rr'], reverse=True)[:10]
    for trade in cancelled_sorted:
        entry_date = trade['entry_bar_time'] if trade['entry_bar_time'] != 'N/A' else f"Bar {trade['entry_bar']}"
        report += f"| {trade['trade_id']} | {trade['direction']} | {trade['entry']:.2f} | {trade['sl']:.2f} | {trade['tp']:.2f} | {trade['rr']:.2f} | {entry_date} | {trade['exit_reason']} |\n"
    
    # Operaciones expiradas (resumen)
    report += f"""

### Operaciones Expiradas ({len(expired)} total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
"""
    
    # Top 10 expiradas por R:R
    expired_sorted = sorted(expired, key=lambda x: x['rr'], reverse=True)[:10]
    for trade in expired_sorted:
        entry_date = trade['entry_bar_time'] if trade['entry_bar_time'] != 'N/A' else f"Bar {trade['entry_bar']}"
        report += f"| {trade['trade_id']} | {trade['direction']} | {trade['entry']:.2f} | {trade['sl']:.2f} | {trade['tp']:.2f} | {trade['rr']:.2f} | {entry_date} | {trade['exit_reason']} |\n"
    
    # Razones de cancelación
    report += f"""

## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones ({len(cancelled)} total)

| Razón | Cantidad | % |
|-------|----------|---|
"""
    
    for reason, count in sorted(cancel_reasons.items(), key=lambda x: x[1], reverse=True):
        pct = (count / len(cancelled) * 100) if len(cancelled) > 0 else 0
        report += f"| {reason} | {count} | {pct:.1f}% |\n"
    
    report += f"""

### Expiraciones ({len(expired)} total)

| Razón | Cantidad | % |
|-------|----------|---|
"""
    
    for reason, count in sorted(expire_reasons.items(), key=lambda x: x[1], reverse=True):
        pct = (count / len(expired) * 100) if len(expired) > 0 else 0
        report += f"| {reason} | {count} | {pct:.1f}% |\n"
    
    # Conclusiones
    report += f"""

---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

"""
    
    if win_rate < 30:
        report += f"""- ⚠️ **CRÍTICO:** Win Rate muy bajo ({win_rate:.1f}% < 30%)
- **Problema:** El sistema está generando señales de baja calidad
- **Acción requerida:** Revisar pesos del DFM y criterios de entrada
"""
    elif win_rate < 50:
        report += f"""- ⚠️ **ADVERTENCIA:** Win Rate bajo ({win_rate:.1f}% < 50%)
- **Acción sugerida:** Calibrar pesos del DFM
"""
    else:
        report += f"""- [OK] **OK:** Win Rate aceptable ({win_rate:.1f}%)
"""
    
    if profit_factor < 1.0:
        report += f"""
- ⚠️ **CRÍTICO:** Profit Factor < 1.0 (sistema perdedor: {profit_factor:.2f})
- **Problema:** Las pérdidas superan las ganancias
- **Acción requerida:** 
  1. Revisar R:R de las operaciones
  2. Analizar cancelaciones por BOS
  3. Aumentar `MinConfidenceForEntry`
"""
    elif profit_factor < 1.5:
        report += f"""
- ⚠️ **ADVERTENCIA:** Profit Factor bajo ({profit_factor:.2f} < 1.5)
"""
    else:
        report += f"""
- [OK] **OK:** Profit Factor aceptable ({profit_factor:.2f})
"""
    
    # Análisis de cancelaciones
    if len(cancelled) > len(closed) * 10:
        report += f"""

### Gestión de Operaciones

- ⚠️ **CRÍTICO:** Ratio cancelaciones/cerradas muy alto ({len(cancelled)}/{len(closed)} = {len(cancelled)/len(closed) if len(closed) > 0 else 0:.1f}x)
- **Problema:** El sistema está cancelando demasiadas operaciones
- **Análisis:** {len(cancelled) / len(trade_book) * 100:.1f}% de operaciones registradas son canceladas
- **Acción sugerida:** Revisar lógica de cancelación por BOS
"""
    
    report += f"""

## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las {len(sl_hits)} operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: {sum([t['rr'] for t in closed]) / len(closed) if len(closed) > 0 else 0:.2f}
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}*
"""
    
    return report, trade_book

if __name__ == "__main__":
    import sys
    
    if len(sys.argv) < 3:
        print("Uso: python analizador-DFM.py <log_file> <csv_file>")
        sys.exit(1)
    
    log_file = sys.argv[1]
    csv_file = sys.argv[2]
    output_file = "export/KPI_SUITE_COMPLETA.md"
    
    # Analizar LOG (scoring breakdowns)
    print("\n" + "="*70)
    print("PASO 1: Analizando LOG para scoring breakdowns")
    print("="*70 + "\n")
    scoring_data = analyze_log_file(log_file)
    
    # Analizar CSV (trades)
    print("="*70)
    print("PASO 2: Analizando CSV para trade book")
    print("="*70)
    report, trade_book = analyze_trades_csv(csv_file)
    
    # Añadir análisis de scoring al reporte
    if scoring_data:
        # Calcular promedios
        avg_core = sum([s.get('core_contribution', 0) for s in scoring_data]) / len(scoring_data)
        avg_proximity = sum([s.get('proximity_contribution', 0) for s in scoring_data]) / len(scoring_data)
        avg_confluence = sum([s.get('confluence_contribution', 0) for s in scoring_data]) / len(scoring_data)
        avg_type = sum([s.get('type_contribution', 0) for s in scoring_data]) / len(scoring_data)
        avg_bias = sum([s.get('bias_contribution', 0) for s in scoring_data]) / len(scoring_data)
        avg_momentum = sum([s.get('momentum_contribution', 0) for s in scoring_data]) / len(scoring_data)
        avg_confidence = sum([s.get('final_confidence', 0) for s in scoring_data]) / len(scoring_data)
        
        signals_generated = len([s for s in scoring_data if s.get('signal_generated', False)])
        
        scoring_section = f"""

## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de {len(scoring_data)} evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | {avg_core:.4f} | {(avg_core/avg_confidence*100) if avg_confidence > 0 else 0:.1f}% |
| **Proximity** | {avg_proximity:.4f} | {(avg_proximity/avg_confidence*100) if avg_confidence > 0 else 0:.1f}% |
| **Confluence** | {avg_confluence:.4f} | {(avg_confluence/avg_confidence*100) if avg_confidence > 0 else 0:.1f}% |
| **Type** | {avg_type:.4f} | {(avg_type/avg_confidence*100) if avg_confidence > 0 else 0:.1f}% |
| **Bias** | {avg_bias:.4f} | {(avg_bias/avg_confidence*100) if avg_confidence > 0 else 0:.1f}% |
| **Momentum** | {avg_momentum:.4f} | {(avg_momentum/avg_confidence*100) if avg_confidence > 0 else 0:.1f}% |
| **TOTAL (Avg Confidence)** | {avg_confidence:.4f} | 100% |

### Resumen de Señales

- **Evaluaciones totales:** {len(scoring_data)}
- **Señales generadas:** {signals_generated} ({(signals_generated/len(scoring_data)*100):.1f}%)
- **Señales rechazadas (WAIT):** {len(scoring_data) - signals_generated} ({((len(scoring_data) - signals_generated)/len(scoring_data)*100):.1f}%)

### Diagnóstico de Calibración

"""
        
        # Diagnóstico automático
        contributions = [
            ('CoreScore', avg_core),
            ('Proximity', avg_proximity),
            ('Confluence', avg_confluence),
            ('Type', avg_type),
            ('Bias', avg_bias),
            ('Momentum', avg_momentum)
        ]
        contributions_sorted = sorted(contributions, key=lambda x: x[1], reverse=True)
        
        scoring_section += f"**Componentes ordenados por contribución:**\n\n"
        for i, (name, value) in enumerate(contributions_sorted, 1):
            scoring_section += f"{i}. **{name}**: {value:.4f}\n"
        
        scoring_section += f"\n**Recomendaciones de calibración:**\n\n"
        
        if avg_proximity < 0.05:
            scoring_section += f"- ⚠️ **Proximity muy bajo ({avg_proximity:.4f})**: Las zonas están muy lejos del precio\n"
            scoring_section += f"  - Acción: Reducir `ProximityThresholdATR` o aumentar `Weight_Proximity`\n"
        
        if avg_core < 0.20:
            scoring_section += f"- ⚠️ **CoreScore bajo ({avg_core:.4f})**: Las estructuras base tienen poca calidad\n"
            scoring_section += f"  - Acción: Revisar detectores o aumentar `Weight_CoreScore`\n"
        
        if avg_bias < 0.05:
            scoring_section += f"- ⚠️ **Bias muy bajo ({avg_bias:.4f})**: El sesgo de mercado no está contribuyendo\n"
            scoring_section += f"  - Acción: Revisar `ContextManager` o aumentar `Weight_Bias`\n"
        
        # Insertar la sección de scoring antes de las conclusiones
        report = report.replace("---\n\n# 🎓 CONCLUSIONES", scoring_section + "\n---\n\n# 🎓 CONCLUSIONES")
    
    # Guardar reporte
    with open(output_file, 'w', encoding='utf-8') as f:
        f.write(report)
    
    # Guardar Trade Book CSV maestro
    csv_master = "export/TRADE_BOOK_MASTER.csv"
    with open(csv_master, 'w', encoding='utf-8', newline='') as f:
        fieldnames = ['trade_id', 'direction', 'entry', 'sl', 'tp', 'risk_points', 'reward_points', 'rr', 
                      'entry_bar', 'entry_bar_time', 'exit_bar', 'exit_bar_time', 'status', 'exit_reason', 
                      'exit_price', 'pnl_points', 'pnl_dollars', 'structure_id']
        writer = csv.DictWriter(f, fieldnames=fieldnames)
        writer.writeheader()
        writer.writerows(trade_book)
    
    print("\n" + "="*70)
    print(f"[OK] Reporte completo: {output_file}")
    print(f"[OK] Trade Book CSV: {csv_master}")
    print("="*70 + "\n")

