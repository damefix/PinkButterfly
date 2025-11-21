#!/usr/bin/env python3
"""
Script para generar informes automáticamente usando los logs más recientes
Ejecuta 3 analizadores:
1. analizador-diagnostico-logs.py (métricas detalladas del sistema)
2. analizador-DFM.py (KPI Suite completa)
3. analizador-logica-operaciones.py (análisis de bias, entradas, SL/TP con MFE/MAE)
"""

import os
import sys
from pathlib import Path
from datetime import datetime

def find_latest_files(logs_dir):
    """
    Busca los archivos .log y .csv más recientes en el directorio de logs
    Retorna (log_path, csv_path, timestamp) o (None, None, None) si no encuentra
    """
    logs_dir = Path(logs_dir)
    
    if not logs_dir.exists():
        print(f"ERROR: Directorio de logs no existe: {logs_dir}")
        return None, None, None
    
    # Buscar todos los archivos backtest_*.log
    log_files = list(logs_dir.glob("backtest_*.log"))
    
    if not log_files:
        print(f"ERROR: No se encontraron archivos backtest_*.log en {logs_dir}")
        return None, None, None
    
    # Ordenar por fecha de modificación (más reciente primero)
    log_files.sort(key=lambda f: f.stat().st_mtime, reverse=True)
    latest_log = log_files[0]
    
    # Extraer timestamp del nombre del archivo (formato: backtest_YYYYMMDD_HHMMSS.log)
    log_name = latest_log.stem  # backtest_YYYYMMDD_HHMMSS
    timestamp = log_name.replace("backtest_", "")  # YYYYMMDD_HHMMSS
    
    # Buscar el CSV correspondiente
    csv_name = f"trades_{timestamp}.csv"
    csv_path = logs_dir / csv_name
    
    if not csv_path.exists():
        print(f"ADVERTENCIA: No se encontró CSV correspondiente: {csv_path}")
        print(f"Buscando cualquier CSV reciente...")
        csv_files = list(logs_dir.glob("trades_*.csv"))
        if csv_files:
            csv_files.sort(key=lambda f: f.stat().st_mtime, reverse=True)
            csv_path = csv_files[0]
            print(f"Usando CSV: {csv_path.name}")
        else:
            print(f"ERROR: No se encontraron archivos trades_*.csv")
            return None, None, None
    
    return str(latest_log), str(csv_path), timestamp


def main():
    # Directorio de logs (relativo al workspace)
    logs_dir = Path("../../NinjaTrader 8/PinkButterfly/logs")
    
    print("="*70)
    print("GENERADOR AUTOMÁTICO DE INFORMES - PinkButterfly")
    print("="*70)
    print()
    
    # Buscar archivos más recientes
    print("Buscando archivos más recientes...")
    log_path, csv_path, timestamp = find_latest_files(logs_dir)
    
    if not log_path or not csv_path:
        print("\nERROR: No se pudieron encontrar los archivos necesarios")
        sys.exit(1)
    
    print(f"[OK] LOG encontrado: {Path(log_path).name}")
    print(f"[OK] CSV encontrado: {Path(csv_path).name}")
    print(f"[OK] Timestamp: {timestamp}")
    print()
    
    # Preparar paths de salida (nombres fijos que usan los scripts)
    diagnostico_output_temp = "export/DIAGNOSTICO_LOGS.md"
    kpi_output_temp = "export/KPI_SUITE_COMPLETA.md"
    logica_output_temp = "export/ANALISIS_LOGICA_DE_OPERACIONES.md"
    
    # Paths finales con timestamp
    diagnostico_output_final = f"export/DIAGNOSTICO_LOGS_{timestamp}.md"
    kpi_output_final = f"export/KPI_SUITE_COMPLETA_{timestamp}.md"
    logica_output_final = f"export/ANALISIS_LOGICA_DE_OPERACIONES_{timestamp}.md"
    
    # Ejecutar analizador-diagnostico-logs.py
    print("="*70)
    print("EJECUTANDO: analizador-diagnostico-logs.py")
    print("="*70)
    cmd_diagnostico = f'python export/analizador-diagnostico-logs.py --log "{log_path}" --csv "{csv_path}" -o "{diagnostico_output_temp}"'
    print(f"Comando: {cmd_diagnostico}")
    print()
    
    result1 = os.system(cmd_diagnostico)
    
    if result1 == 0:
        print(f"\n[OK] Informe diagnostico generado: {diagnostico_output_temp}")
        # Hacer copia con timestamp
        import shutil
        shutil.copy2(diagnostico_output_temp, diagnostico_output_final)
        print(f"[OK] Copia con timestamp: {diagnostico_output_final}")
    else:
        print(f"\n[ERROR] ERROR al generar informe diagnostico (codigo: {result1})")
    
    print()
    
    # Ejecutar analizador-DFM.py
    print("="*70)
    print("EJECUTANDO: analizador-DFM.py")
    print("="*70)
    cmd_kpi = f'python export/analizador-DFM.py "{log_path}" "{csv_path}" -o "{kpi_output_temp}"'
    print(f"Comando: {cmd_kpi}")
    print()
    
    result2 = os.system(cmd_kpi)
    
    if result2 == 0:
        print(f"\n[OK] KPI Suite generada: {kpi_output_temp}")
        # Hacer copia con timestamp
        import shutil
        shutil.copy2(kpi_output_temp, kpi_output_final)
        print(f"[OK] Copia con timestamp: {kpi_output_final}")
    else:
        print(f"\n[ERROR] ERROR al generar KPI Suite (codigo: {result2})")
    
    print()
    
    # Ejecutar analizador-logica-operaciones.py
    print("="*70)
    print("EJECUTANDO: analizador-logica-operaciones.py")
    print("="*70)
    cmd_logica = (
        f'python export/analizador-logica-operaciones.py '
        f'--log "{log_path}" --csv "{csv_path}" -o "{logica_output_temp}"'
    )
    print(f"Comando: {cmd_logica}")
    print()
    
    result3 = os.system(cmd_logica)
    
    if result3 == 0:
        print(f"\n[OK] Análisis Lógica de Operaciones generado: {logica_output_temp}")
        # Hacer copia con timestamp
        import shutil
        shutil.copy2(logica_output_temp, logica_output_final)
        print(f"[OK] Copia con timestamp: {logica_output_final}")
    else:
        print(f"\n[ERROR] ERROR al generar Análisis Lógica (codigo: {result3})")
    
    print()
    print("="*70)
    print("RESUMEN")
    print("="*70)
    
    if result1 == 0 and result2 == 0 and result3 == 0:
        print("[OK] Todos los informes generados correctamente")
        print(f"\nArchivos generados:")
        print(f"  - {diagnostico_output_final}")
        print(f"  - {kpi_output_final}")
        print(f"  - {logica_output_final}")
        print(f"\nArchivos base (siempre actualizados):")
        print(f"  - {diagnostico_output_temp}")
        print(f"  - {kpi_output_temp}")
        print(f"  - {logica_output_temp}")
        print(f"\nBasados en:")
        print(f"  - LOG: {Path(log_path).name}")
        print(f"  - CSV: {Path(csv_path).name}")
    else:
        print("[ERROR] Hubo errores al generar los informes")
        errors = []
        if result1 != 0: errors.append("Diagnóstico")
        if result2 != 0: errors.append("KPI")
        if result3 != 0: errors.append("Lógica")
        print(f"Informes con errores: {', '.join(errors)}")
        sys.exit(1)


if __name__ == "__main__":
    main()

