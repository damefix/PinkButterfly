#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
ANALIZADOR COMPLETO - PinkButterfly CoreBrain
Fusiona anÃ¡lisis diagnÃ³stico de logs + KPI Suite + anÃ¡lisis avanzados
Genera un Ãºnico informe consolidado con todas las mÃ©tricas
"""

import argparse, csv, os, re, sys
from datetime import datetime
from collections import defaultdict, Counter

def to_float(num_str):
    if num_str is None or num_str == '' or num_str == '-' or num_str == 'N/A':
        return 0.0
    if isinstance(num_str, (int, float)):
        return float(num_str)
    s = str(num_str).strip().replace('\u202f', '').replace(' ', '').replace(',', '.')
    try:
        return float(s)
    except:
        return 0.0

def parse_log(log_path):
    """Parser consolidado de logs - extrae todas las mÃ©tricas diagnÃ³sticas"""
    re_diagnostic = r"DIAGN[Ã“O]STICO"
    
    # Regexes consolidados
    patterns = {
        'dfm_eval': re.compile(rf"\[(?:{re_diagnostic})\]\[DFM\].*Evaluadas:\s*Bull=(\d+)\s*Bear=(\d+)\s*\|\s*PassedThreshold=(\d+)"),
        'dfm_bins': re.compile(rf"\[(?:{re_diagnostic})\]\[DFM\]\s*ConfidenceBins:\s*(.*)"),
        'prox': re.compile(rf"\[(?:{re_diagnostic})\]\[Proximity\].*KeptAligned=(\d+)/(\d+),\s*KeptCounter=(\d+)/(\d+),\s*AvgProxAligned=([0-9\.,]+),\s*AvgProxCounter=([0-9\.,]+),\s*AvgDistATRAligned=([0-9\.,]+),\s*AvgDistATRCounter=([0-9\.,]+)"),
        'risk': re.compile(rf"\[(?:{re_diagnostic})\]\[Risk\]\s*Accepted=(\d+)\s*RejSL=(\d+)\s*RejTP=(\d+)\s*RejRR=(\d+)\s*RejEntry=(\d+)"),
        'risk_slaccepted': re.compile(rf"\[(?:{re_diagnostic})\]\[Risk\]\s*SLAccepted:\s*Zone=\S+\s*Dir=(\w+)\s*Entry=([0-9\.,]+)\s*SL=([0-9\.,]+)\s*TP=([0-9\.,]+)\s*SLDistATR=([0-9\.,]+).*?(?:RR=([0-9\.,]+))?(?:\s+Conf=([0-9\.,]+))?", re.IGNORECASE),
        'sl_candidate': re.compile(rf"\[(?:{re_diagnostic})\]\[Risk\]\s*SL_CANDIDATE:\s*Idx=(\d+)\s*Type=(\w+)\s*Score=([0-9\.,]+)\s*TF=(\d+)\s*DistATR=([0-9\.,]+)\s*Age=(\d+)\s*Price=([0-9\.,]+)\s*InBand=(True|False)"),
        'sl_selected': re.compile(rf"\[(?:{re_diagnostic})\]\[Risk\]\s*SL_SELECTED:\s*Zone=(\S+)\s*Type=(\w+)\s*Score=([0-9\.,]+)\s*TF=(-?\d+)\s*DistATR=([0-9\.,]+)\s*Age=(\d+)\s*Price=([0-9\.,]+)\s*Reason=(\S+)"),
        'tp_candidate': re.compile(rf"\[(?:{re_diagnostic})\]\[Risk\]\s*TP_CANDIDATE:\s*Idx=(\d+)\s*Priority=(\S+)\s*Type=(\w+)\s*Score=([0-9\.,]+)\s*TF=(\d+)\s*DistATR=([0-9\.,]+)\s*Age=(\d+)\s*Price=([0-9\.,]+)\s*RR=([0-9\.,]+)"),
        'tp_selected': re.compile(rf"\[(?:{re_diagnostic})\]\[Risk\]\s*TP_SELECTED:\s*Zone=(\S+)\s*Priority=(\S+)\s*Type=(\w+)\s*Score=([0-9\.,]+)\s*TF=(-?\d+)\s*DistATR=([0-9\.,]+)\s*Age=(\d+)\s*Price=([0-9\.,]+)\s*RR=([0-9\.,]+)\s*Reason=(\S+)"),
        # Nuevas métricas V6.1 (Sistema Inteligente)
        'volatility': re.compile(r"\[VOL\]\s*TF=(\d+)\s*Bar=(\d+)\s*currentATR=([0-9\.,]+)\s*avgATR=([0-9\.,]+)\s*volFactor=([0-9\.,]+)"),
        'freshness': re.compile(r"\[FRESH\]\s*TF=(\d+)\s*Age=(\d+)\s*Base=(\d+)\s*VolAdj=([0-9\.,]+)\s*Effective=(\d+)\s*Fresh=([0-9\.,]+)"),
        'scoring_dyn': re.compile(r"\[SCORING_DYN\]\s*TF=(\d+)\s*Prox=([0-9\.,]+)\s*ProxW=([0-9\.,]+)\s*FreshW=([0-9\.,]+)"),
        'purge_protect': re.compile(r"\[PURGE\]\[PROTECT\]\s*TF=(\d+)\s*Protected=(\d+)"),
    }
    
    stats = {
        'dfm': {'lines': 0, 'bull_evals': 0, 'bear_evals': 0, 'passed': 0, 'bins': [0]*10},
        'risk': {'accepted': 0, 'rej_sl': 0, 'rej_tp': 0, 'rej_rr': 0, 'rej_entry': 0, 'accepted_details': []},
        'sl_analysis': {'candidates': [], 'selected_list': []},
        'tp_analysis': {'candidates': [], 'selected_list': []},
        # Nuevas métricas V6.1
        'volatility': {'samples': [], 'avg_factor': 0.0},
        'freshness': {'samples': [], 'avg_volAdj': 0.0, 'avg_fresh': 0.0},
        'scoring_dyn': {'samples': [], 'avg_proxW': 0.0, 'avg_freshW': 0.0},
        'purge_protect': {'total_protected': 0, 'events': []},
    }
    
    try:
        with open(log_path, 'r', encoding='utf-8', errors='ignore') as f:
            for line in f:
                # DFM
                m = patterns['dfm_eval'].search(line)
                if m:
                    stats['dfm']['lines'] += 1
                    stats['dfm']['bull_evals'] += int(m.group(1))
                    stats['dfm']['bear_evals'] += int(m.group(2))
                    stats['dfm']['passed'] += int(m.group(3))
                    continue
                
                # Risk accepted
                m = patterns['risk_slaccepted'].search(line)
                if m:
                    d = m.group(1).upper()
                    e, s, t, slatr = to_float(m.group(2)), to_float(m.group(3)), to_float(m.group(4)), to_float(m.group(5))
                    conf = to_float(m.group(7)) if len(m.groups()) >= 7 else 0.0
                    stats['risk']['accepted_details'].append((d, e, s, t, slatr, conf))
                    continue
                
                # SL candidates/selected
                m = patterns['sl_candidate'].search(line)
                if m:
                    stats['sl_analysis']['candidates'].append({
                        'type': m.group(2), 'score': to_float(m.group(3)), 'tf': int(m.group(4)),
                        'dist_atr': to_float(m.group(5)), 'age': int(m.group(6)), 'in_band': (m.group(8).lower() == 'true')
                    })
                    continue
                
                m = patterns['sl_selected'].search(line)
                if m:
                    stats['sl_analysis']['selected_list'].append({
                        'type': m.group(2), 'score': to_float(m.group(3)), 'tf': int(m.group(4)),
                        'dist_atr': to_float(m.group(5)), 'age': int(m.group(6)), 'reason': m.group(8)
                    })
                    continue
                
                # TP candidates/selected
                m = patterns['tp_candidate'].search(line)
                if m:
                    stats['tp_analysis']['candidates'].append({
                        'priority': m.group(2), 'type': m.group(3), 'score': to_float(m.group(4)),
                        'tf': int(m.group(5)), 'dist_atr': to_float(m.group(6)), 'age': int(m.group(7)),
                        'rr': to_float(m.group(9))
                    })
                    continue
                
                m = patterns['tp_selected'].search(line)
                if m:
                    stats['tp_analysis']['selected_list'].append({
                        'priority': m.group(2), 'type': m.group(3), 'score': to_float(m.group(4)),
                        'tf': int(m.group(5)), 'dist_atr': to_float(m.group(6)), 'age': int(m.group(7)),
                        'rr': to_float(m.group(9)), 'reason': m.group(10)
                    })
                    continue
                
                # Nuevas métricas V6.1
                m = patterns['volatility'].search(line)
                if m:
                    stats['volatility']['samples'].append(to_float(m.group(5)))
                    continue
                
                m = patterns['freshness'].search(line)
                if m:
                    stats['freshness']['samples'].append({
                        'volAdj': to_float(m.group(4)), 'fresh': to_float(m.group(6))
                    })
                    continue
                
                m = patterns['scoring_dyn'].search(line)
                if m:
                    stats['scoring_dyn']['samples'].append({
                        'proxW': to_float(m.group(3)), 'freshW': to_float(m.group(4))
                    })
                    continue
                
                m = patterns['purge_protect'].search(line)
                if m:
                    protected = int(m.group(2))
                    stats['purge_protect']['total_protected'] += protected
                    stats['purge_protect']['events'].append({'tf': int(m.group(1)), 'count': protected})
                    continue
    except FileNotFoundError:
        print(f"ERROR: No se encontrÃ³ log: {log_path}", file=sys.stderr)
    
    # Calcular promedios de nuevas métricas V6.1
    if stats['volatility']['samples']:
        stats['volatility']['avg_factor'] = sum(stats['volatility']['samples']) / len(stats['volatility']['samples'])
    
    if stats['freshness']['samples']:
        stats['freshness']['avg_volAdj'] = sum(s['volAdj'] for s in stats['freshness']['samples']) / len(stats['freshness']['samples'])
        stats['freshness']['avg_fresh'] = sum(s['fresh'] for s in stats['freshness']['samples']) / len(stats['freshness']['samples'])
    
    if stats['scoring_dyn']['samples']:
        stats['scoring_dyn']['avg_proxW'] = sum(s['proxW'] for s in stats['scoring_dyn']['samples']) / len(stats['scoring_dyn']['samples'])
        stats['scoring_dyn']['avg_freshW'] = sum(s['freshW'] for s in stats['scoring_dyn']['samples']) / len(stats['scoring_dyn']['samples'])
    
    return stats

def parse_csv(csv_path):
    """Parser de CSV con matching tolerante"""
    out = {'rows': 0, 'executed': 0, 'cancelled': 0, 'expired': 0, 'buy': 0, 'sell': 0, 'trades': [], 'trade_book': []}
    if not csv_path or not os.path.exists(csv_path):
        return out
    
    trades_by_id = defaultdict(list)
    
    try:
        with open(csv_path, 'r', encoding='utf-8') as f:
            lines = f.readlines()
            for line_num, line in enumerate(lines[1:], start=2):
                if not line.strip():
                    continue
                parts = line.strip().split(',')
                if len(parts) < 10:
                    continue
                
                trade_id, timestamp, action, direction = parts[0], parts[1], parts[2], parts[3]
                entry = f"{parts[4]},{parts[5]}" if len(parts) > 5 else parts[4]
                
                row = {
                    'TradeID': trade_id, 'Timestamp': timestamp, 'Action': action, 'Direction': direction,
                    'Entry': entry, 'SL': '-', 'TP': '-', 'RR': '-', 'Status': '-', 'ExitReason': '-',
                    'ExitPrice': '-', 'PnLPoints': '-', 'PnLDollars': '-'
                }
                
                if action == 'REGISTERED':
                    row['SL'] = f"{parts[6]},{parts[7]}" if len(parts) > 7 and parts[6] != '-' else '-'
                    row['TP'] = f"{parts[8]},{parts[9]}" if len(parts) > 9 and parts[8] != '-' else '-'
                    row['RR'] = f"{parts[14]},{parts[15]}" if len(parts) > 15 and parts[14] != '-' else '-'
                    row['Status'] = parts[19] if len(parts) > 19 else '-'
                elif action == 'CLOSED':
                    row['SL'] = f"{parts[6]},{parts[7]}" if len(parts) > 7 and parts[6] != '-' else '-'
                    row['Status'] = parts[15] if len(parts) > 15 else '-'
                    row['ExitReason'] = parts[16] if len(parts) > 16 else '-'
                    row['ExitPrice'] = f"{parts[19]},{parts[20]}" if len(parts) > 20 and parts[19] != '-' else '-'
                    row['PnLPoints'] = f"{parts[21]},{parts[22]}" if len(parts) > 22 and parts[21] != '-' else '-'
                    row['PnLDollars'] = f"{parts[23]},{parts[24]}" if len(parts) > 24 and parts[23] != '-' else '-'
                
                if trade_id and trade_id != 'N/A':
                    trades_by_id[trade_id].append(row)
                
                # Acumular trades ejecutados para matching
                if action == 'CLOSED':
                    e, sl, tp, ex = to_float(entry), to_float(row['SL']), to_float(row['TP']), to_float(row['ExitPrice'])
                    out['trades'].append({'dir': direction, 'entry': e, 'sl': sl, 'tp': tp, 'exit': ex, 'status': row['ExitReason']})
                    out['executed'] += 1
                    if direction == 'BUY':
                        out['buy'] += 1
                    elif direction == 'SELL':
                        out['sell'] += 1
    except Exception as e:
        print(f"ERROR leyendo CSV: {e}", file=sys.stderr)
    
    # Construir trade_book para KPI Suite
    for trade_id, events in trades_by_id.items():
        registered = next((e for e in events if e['Action'] == 'REGISTERED'), None)
        if not registered:
            continue
        final = next((e for e in events if e['Action'] in ['CLOSED', 'CANCELLED', 'EXPIRED']), None)
        
        trade = {
            'trade_id': trade_id, 'direction': registered.get('Direction', 'N/A'),
            'entry': to_float(registered.get('Entry')), 'sl': to_float(registered.get('SL')),
            'tp': to_float(registered.get('TP')), 'rr': to_float(registered.get('RR')),
            'status': 'PENDING', 'exit_reason': 'N/A', 'exit_price': 0.0,
            'pnl_points': 0.0, 'pnl_dollars': 0.0
        }
        
        if final:
            trade['status'] = final.get('Status', 'N/A')
            trade['exit_reason'] = final.get('ExitReason', 'N/A')
            trade['exit_price'] = to_float(final.get('ExitPrice'))
            trade['pnl_points'] = to_float(final.get('PnLPoints'))
            trade['pnl_dollars'] = to_float(final.get('PnLDollars'))
        
        out['trade_book'].append(trade)
    
    out['rows'] = len(trades_by_id)
    return out

def render_markdown(log_path, csv_path, stats_log, stats_csv):
    """Generador de informe consolidado"""
    now = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    lines = []
    
    # HEADER
    lines.append(f"# ðŸ“Š ANÃLISIS COMPLETO - PinkButterfly CoreBrain")
    lines.append(f"**Informe consolidado: DiagnÃ³stico + KPI Suite + AnÃ¡lisis Avanzados**\n")
    lines.append(f"**Fecha:** {now}")
    lines.append(f"**Log:** `{log_path}`")
    lines.append(f"**CSV:** `{csv_path}`\n")
    lines.append("---\n")
    
    # RESUMEN EJECUTIVO
    trade_book = stats_csv.get('trade_book', [])
    closed = [t for t in trade_book if t['status'] in ['SL_HIT', 'TP_HIT', 'CLOSED']]
    tp_hits = [t for t in closed if t['exit_reason'] == 'TP']
    sl_hits = [t for t in closed if t['exit_reason'] == 'SL']
    cancelled = [t for t in trade_book if t['status'] == 'CANCELLED']
    expired = [t for t in trade_book if t['status'] == 'EXPIRED']
    
    wr = (len(tp_hits) / len(closed) * 100) if closed else 0
    gross_profit = sum(t['pnl_dollars'] for t in tp_hits)
    gross_loss = abs(sum(t['pnl_dollars'] for t in sl_hits))
    pf = (gross_profit / gross_loss) if gross_loss > 0 else 0
    
    lines.append("# ðŸŽ¯ RESUMEN EJECUTIVO\n")
    lines.append("## Operaciones\n")
    lines.append(f"| MÃ©trica | Valor |")
    lines.append(f"|---------|-------|")
    lines.append(f"| **Registradas** | {len(trade_book)} |")
    lines.append(f"| **Ejecutadas** | {len(closed)} |")
    lines.append(f"| **Canceladas** | {len(cancelled)} |")
    lines.append(f"| **Expiradas** | {len(expired)} |")
    lines.append(f"| **Win Rate** | {wr:.1f}% ({len(tp_hits)}/{len(closed)}) |")
    lines.append(f"| **Profit Factor** | {pf:.2f} |\n")
    
    # WR vs SLDistATR (SUB-BINS 0-10)
    def slatr_bin(v):
        if v < 2.0: return 0  # 0-2
        if v < 6.0: return 1  # 2-6
        if v < 10.0: return 2  # 6-10
        if v < 15.0: return 3  # 10-15
        return 4  # 15+
    
    wr_bins = [{'wins': 0, 'losses': 0} for _ in range(5)]
    idx_by_dir = {'BUY': [], 'SELL': []}
    for tr in stats_csv['trades']:
        if tr['dir'] in idx_by_dir:
            idx_by_dir[tr['dir']].append(tr)
    
    matched, unmatched = 0, 0
    for tup in stats_log['risk']['accepted_details']:
        d, e, s, t, slatr, conf = tup
        d_norm = 'BUY' if d.startswith('BULL') else ('SELL' if d.startswith('BEAR') else d)
        found = False
        for tr in idx_by_dir.get(d_norm, []):
            if abs(e - tr['entry']) <= 0.5 and (tr['sl'] is None or abs(s - tr['sl']) <= 0.5):
                b = slatr_bin(slatr)
                if 'TP' in tr['status']:
                    wr_bins[b]['wins'] += 1
                elif 'SL' in tr['status']:
                    wr_bins[b]['losses'] += 1
                matched += 1
                found = True
                break
        if not found:
            unmatched += 1
    
    lines.append("## WR vs SLDistATR (SUB-BINS)\n")
    lines.append(f"**Matched:** {matched} | **Unmatched:** {unmatched}\n")
    labels = ["0-2 ATR", "2-6 ATR", "6-10 ATR", "10-15 ATR", "15+ ATR"]
    for i, label in enumerate(labels):
        w, l = wr_bins[i]['wins'], wr_bins[i]['losses']
        tot = max(1, w + l)
        wrp = w / tot * 100
        lines.append(f"- **{label}**: Wins={w} Losses={l} **WR={wrp:.1f}%** (n={w+l})")
    lines.append("")
    
    # ANÃLISIS POST-MORTEM SL/TP
    sla = stats_log['sl_analysis']
    tpa = stats_log['tp_analysis']
    
    lines.append("## AnÃ¡lisis Post-Mortem: SL/TP\n")
    
    if sla['selected_list']:
        lines.append("### Stop Loss (SL)\n")
        scores = [s['score'] for s in sla['selected_list']]
        ages = [s['age'] for s in sla['selected_list']]
        dists = [s['dist_atr'] for s in sla['selected_list']]
        low_score = sum(1 for s in scores if s < 0.5)
        lines.append(f"- **Seleccionados:** {len(sla['selected_list'])}")
        lines.append(f"- **Score avg:** {sum(scores)/len(scores):.2f} | **Score < 0.5:** {low_score} ({low_score/len(scores)*100:.1f}%)")
        lines.append(f"- **Edad med/max:** {sorted(ages)[len(ages)//2]}/{max(ages)} barras")
        lines.append(f"- **DistATR avg:** {sum(dists)/len(dists):.1f}\n")
        
        # Sensibilidad MinSLScore (0.30, 0.35, 0.40, 0.45, 0.50)
        lines.append("#### Sensibilidad MinSLScore\n")
        for threshold in [0.30, 0.35, 0.40, 0.45, 0.50]:
            remaining = sum(1 for s in scores if s >= threshold)
            pct = remaining / len(scores) * 100
            lines.append(f"- **MinSLScore={threshold:.2f}**: {remaining}/{len(scores)} SL (keep {pct:.1f}%)")
        lines.append("")
    
    if tpa['selected_list']:
        lines.append("### Take Profit (TP)\n")
        scores = [s['score'] for s in tpa['selected_list']]
        fallback = sum(1 for s in tpa['selected_list'] if 'Fallback' in s['reason'] or 'NoStructural' in s['reason'])
        lines.append(f"- **Seleccionados:** {len(tpa['selected_list'])}")
        lines.append(f"- **Score avg:** {sum(scores)/len(scores):.2f}")
        lines.append(f"- **Fallback (sin estructura):** {fallback} ({fallback/len(tpa['selected_list'])*100:.1f}%)\n")
        
        # Fallback por TF
        lines.append("#### TP Fallback por TimeFrame\n")
        tf_counts = Counter(s['tf'] for s in tpa['selected_list'])
        tf_fallback = Counter(s['tf'] for s in tpa['selected_list'] if 'Fallback' in s['reason'] or 'NoStructural' in s['reason'])
        for tf in sorted(tf_counts.keys()):
            fb = tf_fallback.get(tf, 0)
            tot = tf_counts[tf]
            pct = fb / tot * 100
            lines.append(f"- **TF {tf}m**: {fb}/{tot} fallback ({pct:.1f}%)")
        lines.append("")
    
    # RECOMENDACIONES AUTOMÃTICAS
    lines.append("# ðŸŽ¯ RECOMENDACIONES AUTOMÃTICAS\n")
    
    if wr < 50:
        lines.append(f"- âš ï¸ **Win Rate bajo ({wr:.1f}% < 50%)**: Calibrar pesos DFM\n")
    if pf < 1.0:
        lines.append(f"- âš ï¸ **Profit Factor < 1.0 ({pf:.2f})**: Sistema perdedor. Aumentar `MinConfidenceForEntry`\n")
    
    # Bins 0-2 vs 2-6 vs 6-10
    wr_0_2 = wr_bins[0]['wins'] / max(1, wr_bins[0]['wins'] + wr_bins[0]['losses']) * 100
    wr_2_6 = wr_bins[1]['wins'] / max(1, wr_bins[1]['wins'] + wr_bins[1]['losses']) * 100
    wr_6_10 = wr_bins[2]['wins'] / max(1, wr_bins[2]['wins'] + wr_bins[2]['losses']) * 100
    wr_10_15 = wr_bins[3]['wins'] / max(1, wr_bins[3]['wins'] + wr_bins[3]['losses']) * 100
    
    if wr_0_2 < 45 and wr_bins[0]['wins'] + wr_bins[0]['losses'] > 5:
        lines.append(f"- âš ï¸ **SL 0-2 ATR tiene WR={wr_0_2:.1f}% (ruido de mercado)**: Considerar `MinSLDistanceATR=2.0`\n")
    if wr_10_15 < 40 and wr_bins[3]['wins'] + wr_bins[3]['losses'] > 5:
        lines.append(f"- âš ï¸ **SL 10-15 ATR tiene WR={wr_10_15:.1f}% (perdedor)**: Mantener `MaxSLDistanceATR=10.0`\n")
    
    if sla['selected_list']:
        low_score_pct = sum(1 for s in sla['selected_list'] if s['score'] < 0.5) / len(sla['selected_list'])
        if low_score_pct > 0.5:
            lines.append(f"- âš ï¸ **{low_score_pct*100:.0f}% de SL tienen score < 0.5**: Subir `MinSLScore` a 0.4-0.5\n")
    
    if tpa['selected_list']:
        fallback_pct = fallback / len(tpa['selected_list'])
        if fallback_pct > 0.4:
            lines.append(f"- âš ï¸ **{fallback_pct*100:.0f}% de TP son fallback**: Problema de calidad estructuras. Revisar detectores.\n")
    
    lines.append("\n---\n")
    lines.append(f"*Informe generado automÃ¡ticamente - {now}*")
    
    return '\n'.join(lines)

def main():
    parser = argparse.ArgumentParser(description='Analizador Completo PinkButterfly: Logs + CSV â†’ Informe consolidado')
    parser.add_argument('--log', required=True, help='Archivo log backtest_*.log')
    parser.add_argument('--csv', required=True, help='Archivo CSV trades_*.csv')
    parser.add_argument('-o', '--output', default='export/ANALISIS_COMPLETO.md', help='Archivo de salida .md')
    args = parser.parse_args()
    
    print("="*70)
    print("ANALIZADOR COMPLETO - PinkButterfly CoreBrain")
    print("="*70)
    print(f"\nProcesando LOG: {args.log}")
    stats_log = parse_log(args.log)
    print(f"OK LOG parseado")
    
    print(f"\nProcesando CSV: {args.csv}")
    stats_csv = parse_csv(args.csv)
    print(f"OK CSV parseado: {len(stats_csv['trade_book'])} trades")
    
    print(f"\nGenerando informe consolidado...")
    md = render_markdown(args.log, args.csv, stats_log, stats_csv)
    
    with open(args.output, 'w', encoding='utf-8') as f:
        f.write(md)
    
    print(f"\n[OK] Informe generado: {args.output}\n")
    print("="*70)

if __name__ == '__main__':
    main()
