#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Comparación Quirúrgica: Fix N1 vs Fix N2
Objetivo: Identificar qué operaciones cambiaron y su impacto en calidad
"""

import csv
from pathlib import Path

# Rutas
CSV_N1 = Path(r"C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_153317.csv")
CSV_N2 = Path(r"C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_155348.csv")

print("="*80)
print("ANALISIS QUIRURGICO: FIX N1 vs FIX N2")
print("="*80)

# Leer CSV
def read_csv_to_list(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        reader = csv.DictReader(f)
        return [row for row in reader]

df_n1 = read_csv_to_list(CSV_N1)
df_n2 = read_csv_to_list(CSV_N2)

# Filtrar solo REGISTERED (primera línea de cada trade)
reg_n1 = [r for r in df_n1 if r['Action'] == 'REGISTERED']
reg_n2 = [r for r in df_n2 if r['Action'] == 'REGISTERED']

print(f"\nRESUMEN CUANTITATIVO:")
print(f"   - Operaciones Registradas N1: {len(reg_n1)}")
print(f"   - Operaciones Registradas N2: {len(reg_n2)}")
print(f"   - Diferencia: {len(reg_n1) - len(reg_n2)} operaciones")

# Crear claves únicas por EntryBarTime + Direction + Entry
for r in reg_n1:
    r['Key'] = r['EntryBarTime'] + '_' + r['Direction'] + '_' + r['Entry']
for r in reg_n2:
    r['Key'] = r['EntryBarTime'] + '_' + r['Direction'] + '_' + r['Entry']

set_n1 = set(r['Key'] for r in reg_n1)
set_n2 = set(r['Key'] for r in reg_n2)

solo_n1 = set_n1 - set_n2
solo_n2 = set_n2 - set_n1
comunes = set_n1 & set_n2

print(f"\nDIFERENCIAS:")
print(f"   - Operaciones SOLO en N1: {len(solo_n1)}")
print(f"   - Operaciones SOLO en N2: {len(solo_n2)}")
print(f"   - Operaciones COMUNES: {len(comunes)}")

# === ANÁLISIS 1: OPERACIONES PERDIDAS (Solo en N1) ===
if solo_n1:
    print(f"\n{'='*80}")
    print(f"OPERACIONES PERDIDAS (Solo en Fix N1):")
    print(f"{'='*80}")
    perdidas = [r for r in reg_n1 if r['Key'] in solo_n1]
    perdidas = sorted(perdidas, key=lambda x: int(x['Bar']))
    
    for row in perdidas:
        tid = row['TradeID']
        entry = float(row['Entry'].replace(',', '.'))
        sl = float(row['SL'].replace(',', '.'))
        tp = float(row['TP'].replace(',', '.'))
        rr = float(row['RR'].replace(',', '.'))
        dir = row['Direction']
        bar = row['EntryBarTime']
        
        print(f"\n   {tid} | {dir} @ {entry:.2f} | SL={sl:.2f} | TP={tp:.2f} | R:R={rr:.2f}")
        print(f"      Bar: {bar}")
        
        # Buscar resultado en CSV cerradas
        closed_n1 = [r for r in df_n1 if r['TradeID'] == tid and r['Action'] == 'CLOSED']
        if closed_n1:
            result = closed_n1[0]
            pnl = float(result.get('PnLDollars', '0').replace(',', '.'))
            reason = result.get('ExitReason', 'N/A')
            print(f"      Resultado N1: {reason} | P&L: ${pnl:.2f}")
        else:
            # Buscar expiradas
            expired_n1 = [r for r in df_n1 if r['TradeID'] == tid and r['Action'] == 'EXPIRED']
            if expired_n1:
                reason = expired_n1[0].get('ExitReason', 'N/A')
                print(f"      Expirada N1: {reason}")
            else:
                print(f"      Aun PENDING en N1")

# === ANÁLISIS 2: OPERACIONES GANADAS (Solo en N2) ===
if solo_n2:
    print(f"\n{'='*80}")
    print(f"🟢 OPERACIONES GANADAS (Solo en Fix N2):")
    print(f"{'='*80}")
    ganadas = reg_n2[reg_n2['Key'].isin(solo_n2)].sort_values('Bar')
    
    for idx, row in ganadas.iterrows():
        tid = row['TradeID']
        entry = row['Entry']
        sl = row['SL']
        tp = row['TP']
        rr = row['RR']
        dir = row['Direction']
        bar = row['EntryBarTime']
        
        print(f"\n   {tid} | {dir} @ {entry:.2f} | SL={sl:.2f} | TP={tp:.2f} | R:R={rr:.2f}")
        print(f"      Bar: {bar}")
        
        # Buscar resultado en CSV cerradas
        closed_n2 = df_n2[(df_n2['TradeID'] == tid) & (df_n2['Action'] == 'CLOSED')]
        if not closed_n2.empty:
            result = closed_n2.iloc[0]
            pnl = result.get('PnLDollars', 0.0)
            reason = result.get('ExitReason', 'N/A')
            print(f"      ✅ Resultado N2: {reason} | P&L: ${pnl:.2f}")
        else:
            # Buscar expiradas
            expired_n2 = df_n2[(df_n2['TradeID'] == tid) & (df_n2['Action'] == 'EXPIRED')]
            if not expired_n2.empty:
                reason = expired_n2.iloc[0].get('ExitReason', 'N/A')
                print(f"      ⏱️ Expirada N2: {reason}")
            else:
                print(f"      ⏳ Aún PENDING en N2")

# === ANÁLISIS 3: COMPARATIVA DE CALIDAD ===
print(f"\n{'='*80}")
print(f"📈 ANÁLISIS DE CALIDAD COMPARATIVO")
print(f"{'='*80}")

# Calcular P&L de operaciones cerradas
closed_n1 = df_n1[df_n1['Action'] == 'CLOSED'].copy()
closed_n2 = df_n2[df_n2['Action'] == 'CLOSED'].copy()

pnl_n1 = closed_n1['PnLDollars'].sum()
pnl_n2 = closed_n2['PnLDollars'].sum()

wins_n1 = (closed_n1['PnLDollars'] > 0).sum()
losses_n1 = (closed_n1['PnLDollars'] < 0).sum()
wr_n1 = (wins_n1 / len(closed_n1) * 100) if len(closed_n1) > 0 else 0

wins_n2 = (closed_n2['PnLDollars'] > 0).sum()
losses_n2 = (closed_n2['PnLDollars'] < 0).sum()
wr_n2 = (wins_n2 / len(closed_n2) * 100) if len(closed_n2) > 0 else 0

print(f"\n🔵 FIX N1 (StructureFusion con GUID):")
print(f"   - Operaciones Cerradas: {len(closed_n1)}")
print(f"   - P&L Total: ${pnl_n1:.2f}")
print(f"   - Wins: {wins_n1} | Losses: {losses_n1}")
print(f"   - Win Rate: {wr_n1:.1f}%")

print(f"\n🟣 FIX N2 (ProximityAnalyzer sin GUID):")
print(f"   - Operaciones Cerradas: {len(closed_n2)}")
print(f"   - P&L Total: ${pnl_n2:.2f}")
print(f"   - Wins: {wins_n2} | Losses: {losses_n2}")
print(f"   - Win Rate: {wr_n2:.1f}%")

print(f"\n⚖️ DIFERENCIA:")
print(f"   - P&L: ${pnl_n2 - pnl_n1:.2f} ({'+' if pnl_n2 > pnl_n1 else ''}{pnl_n2 - pnl_n1:.2f})")
print(f"   - Win Rate: {wr_n2 - wr_n1:+.1f}pp")
print(f"   - Operaciones Cerradas: {len(closed_n2) - len(closed_n1):+d}")

# === ANÁLISIS 4: CALIDAD DE PERDIDAS/GANANCIAS ===
if solo_n1:
    print(f"\n📊 CALIDAD DE OPERACIONES PERDIDAS (Solo en N1):")
    perdidas_ids = reg_n1[reg_n1['Key'].isin(solo_n1)]['TradeID'].tolist()
    perdidas_closed = closed_n1[closed_n1['TradeID'].isin(perdidas_ids)]
    
    if not perdidas_closed.empty:
        pnl_perdidas = perdidas_closed['PnLDollars'].sum()
        wins_perdidas = (perdidas_closed['PnLDollars'] > 0).sum()
        losses_perdidas = (perdidas_closed['PnLDollars'] < 0).sum()
        
        print(f"   - Cerradas: {len(perdidas_closed)} de {len(perdidas_ids)}")
        print(f"   - P&L: ${pnl_perdidas:.2f}")
        print(f"   - Wins: {wins_perdidas} | Losses: {losses_perdidas}")
        
        if pnl_perdidas > 0:
            print(f"   ✅ CONCLUSIÓN: Las operaciones perdidas eran RENTABLES (+${pnl_perdidas:.2f})")
        else:
            print(f"   ⚠️ CONCLUSIÓN: Las operaciones perdidas eran NO RENTABLES (${pnl_perdidas:.2f})")
    else:
        print(f"   - Ninguna operación perdida llegó a cerrarse en N1")

if solo_n2:
    print(f"\n📊 CALIDAD DE OPERACIONES GANADAS (Solo en N2):")
    ganadas_ids = reg_n2[reg_n2['Key'].isin(solo_n2)]['TradeID'].tolist()
    ganadas_closed = closed_n2[closed_n2['TradeID'].isin(ganadas_ids)]
    
    if not ganadas_closed.empty:
        pnl_ganadas = ganadas_closed['PnLDollars'].sum()
        wins_ganadas = (ganadas_closed['PnLDollars'] > 0).sum()
        losses_ganadas = (ganadas_closed['PnLDollars'] < 0).sum()
        
        print(f"   - Cerradas: {len(ganadas_closed)} de {len(ganadas_ids)}")
        print(f"   - P&L: ${pnl_ganadas:.2f}")
        print(f"   - Wins: {wins_ganadas} | Losses: {losses_ganadas}")
        
        if pnl_ganadas > 0:
            print(f"   ✅ CONCLUSIÓN: Las operaciones ganadas son RENTABLES (+${pnl_ganadas:.2f})")
        else:
            print(f"   ⚠️ CONCLUSIÓN: Las operaciones ganadas son NO RENTABLES (${pnl_ganadas:.2f})")
    else:
        print(f"   - Ninguna operación ganada llegó a cerrarse en N2")

# === RECOMENDACIÓN FINAL ===
print(f"\n{'='*80}")
print(f"🎯 RECOMENDACIÓN FINAL")
print(f"{'='*80}")

if pnl_n2 > pnl_n1:
    print(f"\n✅ MANTENER FIX N2")
    print(f"   - Mejora de ${pnl_n2 - pnl_n1:.2f} en P&L")
    print(f"   - Win Rate: {wr_n2:.1f}% (vs {wr_n1:.1f}%)")
else:
    print(f"\n❌ REVERTIR FIX N2 → Volver a FIX N1")
    print(f"   - Pérdida de ${pnl_n2 - pnl_n1:.2f} en P&L")
    print(f"   - Win Rate: {wr_n2:.1f}% (vs {wr_n1:.1f}%)")
    print(f"   - Fix N2 solo eliminaba dependencia de GUID, no era crítico para determinismo")

print(f"\n{'='*80}")
print("FIN DEL ANÁLISIS")
print(f"{'='*80}\n")

