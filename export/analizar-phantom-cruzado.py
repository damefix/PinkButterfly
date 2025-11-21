#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
ANALISIS PHANTOM CRUZADO - Identificar factores de calidad
Cruza datos de DFM (Confluence, Core) con Phantom Opportunities (MFE/MAE)
para identificar qué hace buenas a las zonas 3-5 ATR
"""

import re
from collections import defaultdict
from datetime import datetime

LOG_FILE = r"C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_174403.log"

print("=" * 80)
print("ANALISIS PHANTOM CRUZADO - Investigacion de factores de calidad")
print("=" * 80)
print("")

# ============================================================================
# PASO 1: Parsear datos DFM (Confluence, Core, Distancia)
# ============================================================================

print("[1/4] Parseando datos DFM (AdaptiveConf)...")

dfm_data = {}  # zone_id -> {confl, core, dist_atr, decision}

regex_dfm = re.compile(
    r"\[AdaptiveConf\] Zone=(\S+) DistATR=([0-9,\.]+).*ConflRaw=([0-9,\.]+).*CoreRaw=([0-9,\.]+).*Decision=(\w+)"
)

with open(LOG_FILE, 'r', encoding='utf-8', errors='ignore') as f:
    for line in f:
        m = regex_dfm.search(line)
        if m:
            zone_id = m.group(1)
            dist_atr = float(m.group(2).replace(',', '.'))
            confl_raw = float(m.group(3).replace(',', '.'))
            core_raw = float(m.group(4).replace(',', '.'))
            decision = m.group(5)
            
            dfm_data[zone_id] = {
                'dist_atr': dist_atr,
                'confl_raw': confl_raw,
                'core_raw': core_raw,
                'decision': decision
            }

print(f"  OK: {len(dfm_data)} zonas con datos DFM parseadas")

# ============================================================================
# PASO 2: Parsear Phantom Opportunities (con MFE/MAE)
# ============================================================================

print("[2/4] Parseando Phantom Opportunities...")

# Primero parsear todos los phantoms
phantoms = []
regex_phantom = re.compile(
    r"\[PHANTOM_OPPORTUNITY\] Zone=(\S+) Dir=(\w+) Entry=([0-9,\.]+) SL=([0-9,\.]+) TP=([0-9,\.]+) RR=([0-9,\.]+) DistATR=([0-9,\.]+)"
)

with open(LOG_FILE, 'r', encoding='utf-8', errors='ignore') as f:
    for line in f:
        m = regex_phantom.search(line)
        if m:
            zone_id = m.group(1)
            direction = m.group(2)
            entry = float(m.group(3).replace(',', '.'))
            sl = float(m.group(4).replace(',', '.'))
            tp = float(m.group(5).replace(',', '.'))
            rr = float(m.group(6).replace(',', '.'))
            dist_atr = float(m.group(7).replace(',', '.'))
            
            phantoms.append({
                'zone_id': zone_id,
                'direction': direction,
                'entry': entry,
                'sl': sl,
                'tp': tp,
                'rr': rr,
                'dist_atr': dist_atr
            })

print(f"  OK: {len(phantoms)} phantom opportunities parseadas")

# ============================================================================
# PASO 3: Cruzar DFM + Phantom
# ============================================================================

print("[3/4] Cruzando datos DFM + Phantom...")

crossed_data = []
for phantom in phantoms:
    zone_id = phantom['zone_id']
    if zone_id in dfm_data:
        crossed = {
            **phantom,
            **dfm_data[zone_id]
        }
        crossed_data.append(crossed)

print(f"  OK: {len(crossed_data)} zonas cruzadas (DFM + Phantom)")

# ============================================================================
# PASO 4: Analizar por combinaciones
# ============================================================================

print("[4/4] Analizando combinaciones de factores...")
print("")

# Foco: Rango 3-5 ATR (el que tenia WR 69.6%)
focus_range = [z for z in crossed_data if 3.0 <= z['dist_atr'] < 5.0]

print(f"ANALISIS: Rango 3-5 ATR ({len(focus_range)} zonas)")
print("=" * 80)
print("")

# Analisis 1: Confluence Alto vs Bajo
print("### FACTOR 1: CONFLUENCE")
print("-" * 80)

confl_alto = [z for z in focus_range if z['confl_raw'] >= 0.5]
confl_bajo = [z for z in focus_range if z['confl_raw'] < 0.5]

print(f"Confluence ALTO (>=0.5): {len(confl_alto)} zonas ({len(confl_alto)/len(focus_range)*100:.1f}%)")
if confl_alto:
    avg_confl = sum(z['confl_raw'] for z in confl_alto) / len(confl_alto)
    avg_core = sum(z['core_raw'] for z in confl_alto) / len(confl_alto)
    avg_rr = sum(z['rr'] for z in confl_alto) / len(confl_alto)
    print(f"  - Confluence promedio: {avg_confl:.2f}")
    print(f"  - CoreRaw promedio: {avg_core:.2f}")
    print(f"  - R:R promedio: {avg_rr:.2f}")

print("")
print(f"Confluence BAJO (<0.5): {len(confl_bajo)} zonas ({len(confl_bajo)/len(focus_range)*100:.1f}%)")
if confl_bajo:
    avg_confl = sum(z['confl_raw'] for z in confl_bajo) / len(confl_bajo)
    avg_core = sum(z['core_raw'] for z in confl_bajo) / len(confl_bajo)
    avg_rr = sum(z['rr'] for z in confl_bajo) / len(confl_bajo)
    print(f"  - Confluence promedio: {avg_confl:.2f}")
    print(f"  - CoreRaw promedio: {avg_core:.2f}")
    print(f"  - R:R promedio: {avg_rr:.2f}")

print("")
print("")

# Analisis 2: Core Alto vs Bajo
print("### FACTOR 2: CORE QUALITY")
print("-" * 80)

core_alto = [z for z in focus_range if z['core_raw'] >= 0.5]
core_bajo = [z for z in focus_range if z['core_raw'] < 0.5]

print(f"Core ALTO (>=0.5): {len(core_alto)} zonas ({len(core_alto)/len(focus_range)*100:.1f}%)")
if core_alto:
    avg_confl = sum(z['confl_raw'] for z in core_alto) / len(core_alto)
    avg_core = sum(z['core_raw'] for z in core_alto) / len(core_alto)
    avg_rr = sum(z['rr'] for z in core_alto) / len(core_alto)
    print(f"  - Confluence promedio: {avg_confl:.2f}")
    print(f"  - CoreRaw promedio: {avg_core:.2f}")
    print(f"  - R:R promedio: {avg_rr:.2f}")

print("")
print(f"Core BAJO (<0.5): {len(core_bajo)} zonas ({len(core_bajo)/len(focus_range)*100:.1f}%)")
if core_bajo:
    avg_confl = sum(z['confl_raw'] for z in core_bajo) / len(core_bajo)
    avg_core = sum(z['core_raw'] for z in core_bajo) / len(core_bajo)
    avg_rr = sum(z['rr'] for z in core_bajo) / len(core_bajo)
    print(f"  - Confluence promedio: {avg_confl:.2f}")
    print(f"  - CoreRaw promedio: {avg_core:.2f}")
    print(f"  - R:R promedio: {avg_rr:.2f}")

print("")
print("")

# Analisis 3: Combinacion Alta Calidad (Confl + Core altos)
print("### FACTOR 3: ALTA CALIDAD (Confluence >=0.5 AND Core >=0.5)")
print("-" * 80)

alta_calidad = [z for z in focus_range if z['confl_raw'] >= 0.5 and z['core_raw'] >= 0.5]
baja_calidad = [z for z in focus_range if z['confl_raw'] < 0.5 and z['core_raw'] < 0.5]
mixta = [z for z in focus_range if z not in alta_calidad and z not in baja_calidad]

print(f"ALTA CALIDAD: {len(alta_calidad)} zonas ({len(alta_calidad)/len(focus_range)*100:.1f}%)")
if alta_calidad:
    avg_confl = sum(z['confl_raw'] for z in alta_calidad) / len(alta_calidad)
    avg_core = sum(z['core_raw'] for z in alta_calidad) / len(alta_calidad)
    avg_rr = sum(z['rr'] for z in alta_calidad) / len(alta_calidad)
    print(f"  - Confluence promedio: {avg_confl:.2f}")
    print(f"  - CoreRaw promedio: {avg_core:.2f}")
    print(f"  - R:R promedio: {avg_rr:.2f}")

print("")
print(f"BAJA CALIDAD: {len(baja_calidad)} zonas ({len(baja_calidad)/len(focus_range)*100:.1f}%)")
if baja_calidad:
    avg_confl = sum(z['confl_raw'] for z in baja_calidad) / len(baja_calidad)
    avg_core = sum(z['core_raw'] for z in baja_calidad) / len(baja_calidad)
    avg_rr = sum(z['rr'] for z in baja_calidad) / len(baja_calidad)
    print(f"  - Confluence promedio: {avg_confl:.2f}")
    print(f"  - CoreRaw promedio: {avg_core:.2f}")
    print(f"  - R:R promedio: {avg_rr:.2f}")

print("")
print(f"CALIDAD MIXTA: {len(mixta)} zonas ({len(mixta)/len(focus_range)*100:.1f}%)")

print("")
print("")

# Analisis 4: Distribucion de valores
print("### FACTOR 4: DISTRIBUCION DE VALORES (Rango 3-5 ATR)")
print("-" * 80)

if focus_range:
    confl_values = [z['confl_raw'] for z in focus_range]
    core_values = [z['core_raw'] for z in focus_range]
    rr_values = [z['rr'] for z in focus_range]
    
    print(f"Confluence:")
    print(f"  - Promedio: {sum(confl_values)/len(confl_values):.2f}")
    print(f"  - Min/Max: {min(confl_values):.2f} / {max(confl_values):.2f}")
    print(f"  - Mediana: {sorted(confl_values)[len(confl_values)//2]:.2f}")
    
    print("")
    print(f"CoreRaw:")
    print(f"  - Promedio: {sum(core_values)/len(core_values):.2f}")
    print(f"  - Min/Max: {min(core_values):.2f} / {max(core_values):.2f}")
    print(f"  - Mediana: {sorted(core_values)[len(core_values)//2]:.2f}")
    
    print("")
    print(f"R:R:")
    print(f"  - Promedio: {sum(rr_values)/len(rr_values):.2f}")
    print(f"  - Min/Max: {min(rr_values):.2f} / {max(rr_values):.2f}")

print("")
print("")

# ============================================================================
# CONCLUSIONES
# ============================================================================

print("=" * 80)
print("CONCLUSIONES")
print("=" * 80)
print("")

if len(confl_alto) < len(focus_range) * 0.2:
    print("X Confluence Alto (<20% de zonas): NO es el factor clave")
    print("   La mayoria de zonas 3-5 ATR tienen baja confluence")
    print("")

if len(core_alto) < len(focus_range) * 0.2:
    print("X Core Alto (<20% de zonas): NO es el factor clave")
    print("   La mayoria de zonas 3-5 ATR tienen bajo core quality")
    print("")

if len(alta_calidad) < len(focus_range) * 0.15:
    print("X Alta Calidad combinada (<15% de zonas): NO es el factor clave")
    print("   Muy pocas zonas tienen AMBOS factores altos")
    print("")

print("? HIPOTESIS ALTERNATIVAS A INVESTIGAR:")
print("   1. Edad/Tiempo de formacion (zonas mas viejas = mas validadas)")
print("   2. Timeframe dominante (TF mayor = estructura mas fuerte)")
print("   3. Momentum de mercado en formacion")
print("   4. Proximidad a estructura mayor (S/R clave)")
print("")

print("=" * 80)
print("Análisis completado")
print("=" * 80)

