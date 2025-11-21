#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Comparación Quirúrgica Simple: Fix N1 vs Fix N2
"""

import csv

CSV_N1 = r"C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_153317.csv"
CSV_N2 = r"C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_155348.csv"

def read_csv(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        return list(csv.DictReader(f))

# Leer archivos
print("="*80)
print("ANALISIS QUIRURGICO: FIX N1 vs FIX N2")
print("="*80)

df_n1 = read_csv(CSV_N1)
df_n2 = read_csv(CSV_N2)

# Operaciones registradas
reg_n1 = [r for r in df_n1 if r['Action'] == 'REGISTERED']
reg_n2 = [r for r in df_n2 if r['Action'] == 'REGISTERED']

# Operaciones cerradas
closed_n1 = [r for r in df_n1 if r['Action'] == 'CLOSED']
closed_n2 = [r for r in df_n2 if r['Action'] == 'CLOSED']

# Calcular métricas
def safe_float(s):
    if not s or s == '-':
        return 0.0
    return float(s.replace(',', '.'))

pnl_n1 = sum(safe_float(r.get('PnLDollars', '0')) for r in closed_n1)
pnl_n2 = sum(safe_float(r.get('PnLDollars', '0')) for r in closed_n2)

wins_n1 = sum(1 for r in closed_n1 if safe_float(r.get('PnLDollars', '0')) > 0)
wins_n2 = sum(1 for r in closed_n2 if safe_float(r.get('PnLDollars', '0')) > 0)

losses_n1 = sum(1 for r in closed_n1 if safe_float(r.get('PnLDollars', '0')) < 0)
losses_n2 = sum(1 for r in closed_n2 if safe_float(r.get('PnLDollars', '0')) < 0)

wr_n1 = (wins_n1 / len(closed_n1) * 100) if closed_n1 else 0
wr_n2 = (wins_n2 / len(closed_n2) * 100) if closed_n2 else 0

# Crear claves únicas
for r in reg_n1:
    r['Key'] = f"{r['EntryBarTime']}_{r['Direction']}_{r['Entry']}"
for r in reg_n2:
    r['Key'] = f"{r['EntryBarTime']}_{r['Direction']}_{r['Entry']}"

set_n1 = set(r['Key'] for r in reg_n1)
set_n2 = set(r['Key'] for r in reg_n2)

solo_n1 = set_n1 - set_n2
solo_n2 = set_n2 - set_n1

# IMPRIMIR RESULTADOS
print(f"\nRESUMEN CUANTITATIVO:")
print(f"   - Operaciones Registradas N1: {len(reg_n1)}")
print(f"   - Operaciones Registradas N2: {len(reg_n2)}")
print(f"   - Diferencia: {len(reg_n1) - len(reg_n2):+d} operaciones")

print(f"\nDIFERENCIAS:")
print(f"   - Operaciones SOLO en N1: {len(solo_n1)}")
print(f"   - Operaciones SOLO en N2: {len(solo_n2)}")

print(f"\n{'='*80}")
print(f"METRICAS DE CALIDAD")
print(f"{'='*80}")

print(f"\nFIX N1 (StructureFusion con GUID):")
print(f"   - Operaciones Cerradas: {len(closed_n1)}")
print(f"   - P&L Total: ${pnl_n1:.2f}")
print(f"   - Wins: {wins_n1} | Losses: {losses_n1}")
print(f"   - Win Rate: {wr_n1:.1f}%")

print(f"\nFIX N2 (ProximityAnalyzer sin GUID):")
print(f"   - Operaciones Cerradas: {len(closed_n2)}")
print(f"   - P&L Total: ${pnl_n2:.2f}")
print(f"   - Wins: {wins_n2} | Losses: {losses_n2}")
print(f"   - Win Rate: {wr_n2:.1f}%")

print(f"\nDIFERENCIA:")
diff_pnl = pnl_n2 - pnl_n1
diff_wr = wr_n2 - wr_n1
diff_ops = len(closed_n2) - len(closed_n1)

print(f"   - P&L: {diff_pnl:+.2f} USD ({'+' if diff_pnl > 0 else ''}{diff_pnl:.2f})")
print(f"   - Win Rate: {diff_wr:+.1f}pp")
print(f"   - Operaciones Cerradas: {diff_ops:+d}")

# ANÁLISIS DE OPERACIONES PERDIDAS/GANADAS
if solo_n1:
    print(f"\n{'='*80}")
    print(f"OPERACIONES PERDIDAS (Solo en N1)")
    print(f"{'='*80}")
    perdidas = [r for r in reg_n1 if r['Key'] in solo_n1]
    perdidas = sorted(perdidas, key=lambda x: int(x['Bar']))
    
    print(f"\nTotal: {len(perdidas)} operaciones")
    print(f"\nTop 5:")
    for row in perdidas[:5]:
        tid = row['TradeID']
        entry = safe_float(row['Entry'])
        sl = safe_float(row['SL'])
        tp = safe_float(row['TP'])
        rr = safe_float(row['RR'])
        dir = row['Direction']
        bar = row['EntryBarTime']
        
        print(f"   {tid} | {dir} @ {entry:.2f} | SL={sl:.2f} | TP={tp:.2f} | R:R={rr:.2f}")
        
        # Buscar resultado
        closed = [r for r in df_n1 if r['TradeID'] == tid and r['Action'] == 'CLOSED']
        if closed:
            pnl = safe_float(closed[0].get('PnLDollars', '0'))
            reason = closed[0].get('ExitReason', 'N/A')
            print(f"        -> N1: {reason} | P&L: ${pnl:.2f}")

    # Calcular calidad de perdidas
    perdidas_ids = [r['TradeID'] for r in perdidas]
    perdidas_closed = [r for r in closed_n1 if r['TradeID'] in perdidas_ids]
    if perdidas_closed:
        pnl_perdidas = sum(safe_float(r.get('PnLDollars', '0')) for r in perdidas_closed)
        wins_perdidas = sum(1 for r in perdidas_closed if safe_float(r.get('PnLDollars', '0')) > 0)
        losses_perdidas = sum(1 for r in perdidas_closed if safe_float(r.get('PnLDollars', '0')) < 0)
        
        print(f"\n   CALIDAD: {len(perdidas_closed)} cerradas | P&L: ${pnl_perdidas:.2f} | Wins: {wins_perdidas} | Losses: {losses_perdidas}")

if solo_n2:
    print(f"\n{'='*80}")
    print(f"OPERACIONES GANADAS (Solo en N2)")
    print(f"{'='*80}")
    ganadas = [r for r in reg_n2 if r['Key'] in solo_n2]
    ganadas = sorted(ganadas, key=lambda x: int(x['Bar']))
    
    print(f"\nTotal: {len(ganadas)} operaciones")
    print(f"\nTop 5:")
    for row in ganadas[:5]:
        tid = row['TradeID']
        entry = safe_float(row['Entry'])
        sl = safe_float(row['SL'])
        tp = safe_float(row['TP'])
        rr = safe_float(row['RR'])
        dir = row['Direction']
        bar = row['EntryBarTime']
        
        print(f"   {tid} | {dir} @ {entry:.2f} | SL={sl:.2f} | TP={tp:.2f} | R:R={rr:.2f}")
        
        # Buscar resultado
        closed = [r for r in df_n2 if r['TradeID'] == tid and r['Action'] == 'CLOSED']
        if closed:
            pnl = safe_float(closed[0].get('PnLDollars', '0'))
            reason = closed[0].get('ExitReason', 'N/A')
            print(f"        -> N2: {reason} | P&L: ${pnl:.2f}")

    # Calcular calidad de ganadas
    ganadas_ids = [r['TradeID'] for r in ganadas]
    ganadas_closed = [r for r in closed_n2 if r['TradeID'] in ganadas_ids]
    if ganadas_closed:
        pnl_ganadas = sum(safe_float(r.get('PnLDollars', '0')) for r in ganadas_closed)
        wins_ganadas = sum(1 for r in ganadas_closed if safe_float(r.get('PnLDollars', '0')) > 0)
        losses_ganadas = sum(1 for r in ganadas_closed if safe_float(r.get('PnLDollars', '0')) < 0)
        
        print(f"\n   CALIDAD: {len(ganadas_closed)} cerradas | P&L: ${pnl_ganadas:.2f} | Wins: {wins_ganadas} | Losses: {losses_ganadas}")

# RECOMENDACIÓN FINAL
print(f"\n{'='*80}")
print(f"RECOMENDACION FINAL")
print(f"{'='*80}")

if pnl_n2 > pnl_n1:
    print(f"\nMANTENER FIX N2")
    print(f"   - Mejora de ${diff_pnl:.2f} en P&L")
    print(f"   - Win Rate: {wr_n2:.1f}% (vs {wr_n1:.1f}%)")
else:
    print(f"\nREVERTIR FIX N2 -> Volver a FIX N1")
    print(f"   - Perdida de ${abs(diff_pnl):.2f} en P&L")
    print(f"   - Win Rate: {wr_n2:.1f}% (vs {wr_n1:.1f}%)")
    print(f"   - Fix N2 solo eliminaba dependencia de GUID, no era critico para determinismo")

print(f"\n{'='*80}\n")

