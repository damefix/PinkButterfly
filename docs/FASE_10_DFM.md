# Fase 10: Decision Fusion Model (DFM)

## 📋 Índice

1. [Visión General](#visión-general)
2. [Arquitectura](#arquitectura)
3. [Componentes del Pipeline](#componentes-del-pipeline)
4. [Modelos de Datos](#modelos-de-datos)
5. [Configuración](#configuración)
6. [Flujo de Ejecución](#flujo-de-ejecución)
7. [Scoring Multi-Factor](#scoring-multi-factor)
8. [Gestión de Riesgo](#gestión-de-riesgo)
9. [Explainability (XAI)](#explainability-xai)
10. [Ejemplos de Uso](#ejemplos-de-uso)
11. [Tests](#tests)

---

## 🎯 Visión General

El **Decision Fusion Model (DFM)** es el módulo de lógica operacional que traduce la información técnica del CoreBrain en decisiones de trading concretas. Genera señales BUY/SELL/WAIT con niveles precisos de Entry, StopLoss, TakeProfit y tamaño de posición calculado dinámicamente.

### Objetivos del DFM

1. **Fusionar información multi-timeframe** en zonas de confluencia (HeatZones)
2. **Evaluar contexto de mercado** (bias, claridad, volatilidad)
3. **Calcular riesgo profesional** (Entry estructural, SL/TP, position sizing)
4. **Generar decisiones explicables** (XAI - Explainable AI)
5. **Maximizar Risk:Reward** mediante entry estructural en límites de zonas

---

## 🏗️ Arquitectura

### Patrón: Chain of Responsibility

El DFM utiliza un pipeline de 6 componentes que se ejecutan secuencialmente, cada uno enriqueciendo el `DecisionSnapshot`:

```
┌─────────────────────────────────────────────────────────────────┐
│                      DecisionEngine                              │
│  (Orquestador - Validación Fail-Fast - Manejo de Errores)      │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│ 1. ContextManager                                                │
│    - Construye DecisionSummary (CurrentPrice, ATRByTF)          │
│    - Calcula GlobalBias y GlobalBiasStrength                    │
│    - Calcula MarketClarity (filtro alta jerarquía)             │
│    - Calcula MarketVolatilityNormalized (ATR vs ATR(200))      │
└──────────────────────┬──────────────────────────────────────────┘
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│ 2. StructureFusion                                               │
│    - Fusiona estructuras overlapping en HeatZones               │
│    - Usa Interval Tree para búsqueda O(log n + k)              │
│    - Calcula Score ponderado por TFWeight                       │
│    - Identifica estructura dominante                            │
└──────────────────────┬──────────────────────────────────────────┘
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│ 3. ProximityAnalyzer                                             │
│    - Calcula distancia jerárquica (0 si inside, else edge)     │
│    - Normaliza por ATR del timeframe dominante                  │
│    - Filtra zonas lejanas                                       │
│    - Ordena por proximidad                                      │
└──────────────────────┬──────────────────────────────────────────┘
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│ 4. RiskCalculator                                                │
│    - Entry estructural: zone.Low (BUY) / zone.High (SELL)      │
│    - StopLoss con buffer ATR                                    │
│    - TakeProfit basado en MinRiskRewardRatio                   │
│    - PositionSize dinámico (% riesgo + accountSize)            │
└──────────────────────┬──────────────────────────────────────────┘
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│ 5. DecisionFusionModel                                           │
│    - Scoring 7 factores ponderados (suma = 1.0)                │
│    - Normalización de confluence                                │
│    - Penalización por bias contrario (0.85)                    │
│    - Selección de BestZone                                      │
└──────────────────────┬──────────────────────────────────────────┘
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│ 6. OutputAdapter                                                 │
│    - Determina Action (BUY/SELL/WAIT)                          │
│    - Genera Rationale profesional                              │
│    - Incluye factor dominante                                   │
│    - Crea TradeDecision final                                   │
└─────────────────────────────────────────────────────────────────┘
```

### Principios de Diseño

- **Modularidad**: Cada componente es independiente y testeable
- **Inmutabilidad**: DecisionSnapshot se enriquece, no se muta
- **Fail-Fast**: Validación de configuración en constructor
- **Thread-Safety**: Componentes sin estado compartido
- **Explainability**: Cada decisión es completamente trazable

---

## 🔧 Componentes del Pipeline

### 1. ContextManager

**Responsabilidad**: Construir el contexto inicial del mercado

**Inputs**:
- `IBarDataProvider`: Datos de barras
- `CoreEngine`: Estructuras detectadas
- `currentBar`: Índice de barra actual

**Outputs** (en `DecisionSnapshot`):
- `DecisionSummary`:
  - `CurrentPrice`: Precio actual del timeframe primario
  - `ATRByTF`: ATR por cada timeframe configurado
  - `TotalStructures`: Número total de estructuras activas
  - `StructuresByType`: Conteo por tipo (FVG, OB, etc.)
  
- `GlobalBias`: "Bullish", "Bearish", "Neutral"
- `GlobalBiasStrength`: 0.0 - 1.0
- `MarketClarity`: 0.0 - 1.0 (basado en estructuras de alta jerarquía)
- `MarketVolatilityNormalized`: 0.0 - 1.0 (ATR actual vs ATR(200))

**Algoritmo GlobalBias**:
```csharp
// 1. Obtener CurrentMarketBias del CoreEngine
// 2. Obtener BOS/CHoCH recientes (últimos N)
// 3. Calcular peso por momentum y freshness
// 4. Determinar bias dominante
// 5. Calcular strength basado en consistencia
```

**Algoritmo MarketClarity**:
```csharp
// Solo cuenta estructuras de alta jerarquía:
var highHierarchy = { "FVG", "OB", "POI", "BOS", "CHoCH", "LG" };

int activeRelevant = structures.Count(s => highHierarchy.Contains(s.Type) && s.IsActive);
int recentRelevant = structures.Count(s => highHierarchy.Contains(s.Type) && 
                                           s.IsActive && 
                                           (currentBar - s.CreatedAtBarIndex) <= MaxAge);

double structuresFactor = Min(activeRelevant / MinStructures, 1.0);
double recentFactor = Min(recentRelevant / MinStructures, 1.0);

MarketClarity = (structuresFactor + recentFactor) / 2.0;
```

---

### 2. StructureFusion

**Responsabilidad**: Fusionar estructuras overlapping en HeatZones

**Inputs**:
- `DecisionSnapshot` con contexto
- `CoreEngine`: Para consultar estructuras

**Outputs** (en `DecisionSnapshot.HeatZones`):
- Lista de `HeatZone` con:
  - `Id`, `Direction`, `High`, `Low`
  - `Score`: Promedio ponderado por TFWeight
  - `ConfluenceCount`: Número de estructuras
  - `DominantType`, `TFDominante`, `DominantStructureId`
  - `SourceStructureIds`: IDs de estructuras fusionadas

**Algoritmo**:
```csharp
// 1. Obtener todas las estructuras activas de todos los TF
// 2. Filtrar por Score >= MinScore
// 3. Ordenar por Score descendente
// 4. Para cada estructura no asignada:
//    a. Calcular rango de búsqueda con tolerancia ATR
//    b. Usar CoreEngine.QueryOverlappingStructures() - O(log n + k)
//    c. Si hay >= MinConfluence estructuras cercanas:
//       - Crear HeatZone
//       - Calcular Score ponderado: Σ(score_i × tfWeight_i) / Σ(tfWeight_i)
//       - Calcular Direction: suma ponderada bullish vs bearish
//       - Identificar estructura dominante (mayor score)
```

**Eficiencia**:
- Usa Interval Tree del CoreEngine para búsqueda espacial
- Complejidad: O(n log n + k) donde k = estructuras por zona
- Evita comparaciones O(n²)

---

### 3. ProximityAnalyzer

**Responsabilidad**: Calcular proximidad de HeatZones al precio actual

**Inputs**:
- `DecisionSnapshot` con HeatZones

**Outputs** (en `HeatZone.Metadata`):
- `ProximityFactor`: 0.0 - 1.0 (1.0 = inside, decae con distancia)
- `distanceTicks`: Distancia en ticks

**Algoritmo de Distancia**:
```csharp
if (currentPrice >= zone.Low && currentPrice <= zone.High)
    distance = 0.0; // Inside zone
else
    distance = Min(Abs(currentPrice - zone.High), Abs(currentPrice - zone.Low));

// Normalizar por ATR del TF dominante
double atr = GetATR(zone.TFDominante, 14, currentBar);
double distanceATR = distance / atr;

// ProximityFactor decae exponencialmente
ProximityFactor = 1.0 / (1.0 + distanceATR);
```

**Filtrado**:
- Elimina zonas con `distanceATR > ProximityThresholdATR` (default: 3.0)
- Ordena zonas restantes por proximidad descendente

---

### 4. RiskCalculator

**Responsabilidad**: Calcular Entry, SL, TP y PositionSize

**Inputs**:
- `DecisionSnapshot` con HeatZones filtradas
- `accountSize`: Tamaño de cuenta en moneda base

**Outputs** (en `HeatZone.Metadata`):
- `Entry`: Precio de entrada estructural
- `StopLoss`: Precio de stop loss
- `TakeProfit`: Precio de take profit
- `PositionSizeContracts`: Número de contratos
- `ActualRR`: Risk:Reward ratio calculado
- `AccountRisk`: Riesgo en moneda base

**Algoritmo Entry Estructural**:
```csharp
// Para BUY: Entry en el borde INFERIOR de la zona (limit order)
if (zone.Direction == "Bullish")
    Entry = zone.Low;

// Para SELL: Entry en el borde SUPERIOR de la zona (limit order)
if (zone.Direction == "Bearish")
    Entry = zone.High;

// Objetivo: Maximizar Risk:Reward entrando en los extremos
```

**Algoritmo StopLoss**:
```csharp
double atr = GetATR(zone.TFDominante, 14, currentBar);
double buffer = SL_BufferATR * atr; // Default: 0.2 (20% del ATR)

if (zone.Direction == "Bullish")
    StopLoss = zone.Low - buffer;
else
    StopLoss = zone.High + buffer;
```

**Algoritmo TakeProfit**:
```csharp
double riskPerShare = Abs(Entry - StopLoss);
double rewardPerShare = riskPerShare * MinRiskRewardRatio; // Default: 1.5

if (zone.Direction == "Bullish")
    TakeProfit = Entry + rewardPerShare;
else
    TakeProfit = Entry - rewardPerShare;

ActualRR = rewardPerShare / riskPerShare;
```

**Algoritmo PositionSize**:
```csharp
// 1. Calcular riesgo en moneda base
double riskAmount = accountSize * (RiskPercentPerTrade / 100.0); // Default: 0.5%

// 2. Calcular riesgo por contrato
double riskPerContract = riskPerShare * PointValue; // PointValue = $50 para ES

// 3. Calcular número de contratos
double positionSize = riskAmount / riskPerContract;

// 4. Ajustar por TickSize y redondear
PositionSizeContracts = Max(1, Round(positionSize));

// 5. Validaciones
if (PointValue <= 0 || TickSize <= 0)
    throw InvalidOperationException("PointValue/TickSize inválidos");
```

---

### 5. DecisionFusionModel

**Responsabilidad**: Calcular confianza final mediante scoring multi-factor

**Inputs**:
- `DecisionSnapshot` con HeatZones + Risk calculado

**Outputs** (en `HeatZone.Metadata`):
- `ConfidenceBreakdown`: `DecisionScoreBreakdown` con desglose completo
- `FinalConfidence`: 0.0 - 1.0

**Fórmula de Scoring (7 factores)**:
```
RawConfidence = 
    Weight_CoreScore    (0.40) × zone.Score +
    Weight_Proximity    (0.18) × ProximityFactor +
    Weight_Confluence   (0.15) × ConfluenceFactor +
    Weight_Type         (0.10) × TypeMultiplier +
    Weight_Bias         (0.07) × BiasAlignment +
    Weight_Momentum     (0.05) × MomentumFactor +
    Weight_Volume       (0.05) × VolumeFactor

// Normalización de Confluence (evita saturación)
ConfluenceFactor = Min(1.0, zone.ConfluenceCount / MaxConfluenceReference)

// Penalización por bias contrario
if (zone.Direction != GlobalBias && GlobalBias != "Neutral")
    RawConfidence *= BiasOverrideConfidenceFactor; // Default: 0.85

FinalConfidence = Clamp(RawConfidence, 0.0, 1.0);
```

**Validación Fail-Fast**:
```csharp
// En constructor de DecisionEngine
double sumWeights = Weight_CoreScore + Weight_Proximity + ... + Weight_Volume;

if (Abs(sumWeights - 1.0) > 0.0001)
    throw InvalidOperationException($"Suma de pesos = {sumWeights}, debe ser 1.0");
```

**Selección de BestZone**:
```csharp
var bestZone = zones.OrderByDescending(z => z.Metadata["FinalConfidence"]).First();
snapshot.Metadata["BestZone"] = bestZone;
snapshot.Metadata["BestConfidence"] = bestZone.Metadata["FinalConfidence"];
```

---

### 6. OutputAdapter

**Responsabilidad**: Generar TradeDecision final con rationale

**Inputs**:
- `DecisionSnapshot` con BestZone seleccionada

**Outputs** (en `snapshot.Metadata["FinalDecision"]`):
- `TradeDecision` completo

**Algoritmo Action**:
```csharp
if (bestConfidence >= MinConfidenceForEntry) // Default: 0.65
{
    if (bestZone.Direction == "Bullish")
        Action = "BUY";
    else if (bestZone.Direction == "Bearish")
        Action = "SELL";
}
else
{
    Action = "WAIT";
}
```

**Generación de Rationale**:
```csharp
StringBuilder rationale = new StringBuilder();

rationale.AppendLine($"{Action} Limit @ {Entry:F2} (HeatZone {zone.Id}: {zone.Direction}, {zone.ConfluenceCount} structures, Score: {zone.Score:F2})");
rationale.AppendLine($"- Confidence: {confidence:F2} (Core: {breakdown.CoreScore:F2}, Proximity: {breakdown.Proximity:F2}, Confluence: {breakdown.Confluence:F2})");
rationale.AppendLine($"- SL: {StopLoss:F2}, TP: {TakeProfit:F2}, R:R = {ActualRR:F2}");
rationale.AppendLine($"- Position: {PositionSize} contracts (Risk: ${AccountRisk:F2})");
rationale.AppendLine($"- Global Bias: {GlobalBias} ({GlobalBiasStrength:F2})");
rationale.AppendLine($"- Dominant Factor: {DominantType} {TFDominante}H (Score: {dominantScore:F2})");

return rationale.ToString();
```

---

## 📦 Modelos de Datos

### HeatZone

```csharp
public class HeatZone
{
    public string Id { get; set; }                      // "HZ_001"
    public string Direction { get; set; }               // "Bullish", "Bearish", "Neutral"
    public double High { get; set; }                    // Precio máximo de la zona
    public double Low { get; set; }                     // Precio mínimo de la zona
    public double CenterPrice => (High + Low) / 2.0;   // Precio central calculado
    public double Score { get; set; }                   // Score agregado (ponderado por TF)
    public int ConfluenceCount { get; set; }            // Número de estructuras fusionadas
    public List<string> SourceStructureIds { get; set; } // IDs de estructuras origen
    public Dictionary<string, object> Metadata { get; set; } // Datos adicionales del pipeline
    
    // Estructura dominante
    public string DominantType { get; set; }            // "FVG", "OB", "POI"
    public int TFDominante { get; set; }                // 60, 240, etc.
    public string DominantStructureId { get; set; }     // ID de la estructura dominante
}
```

### TradeDecision

```csharp
public class TradeDecision
{
    public string Id { get; set; }                      // GUID único
    public string Action { get; set; }                  // "BUY", "SELL", "WAIT"
    public double Confidence { get; set; }              // 0.0 - 1.0
    public double Entry { get; set; }                   // Precio de entrada
    public double StopLoss { get; set; }                // Precio de stop loss
    public double TakeProfit { get; set; }              // Precio de take profit
    public double PositionSizeContracts { get; set; }   // Número de contratos
    public string Rationale { get; set; }               // Explicación textual
    public DecisionScoreBreakdown Explainability { get; set; } // Desglose XAI
    public List<string> SourceStructureIds { get; set; } // IDs de estructuras usadas
    public DateTime GeneratedAt { get; set; }           // Timestamp UTC
}
```

### DecisionScoreBreakdown

```csharp
public class DecisionScoreBreakdown
{
    public double CoreScoreContribution { get; set; }   // Contribución del score base
    public double ProximityContribution { get; set; }   // Contribución de proximidad
    public double ConfluenceContribution { get; set; }  // Contribución de confluencia
    public double TypeContribution { get; set; }        // Contribución del tipo
    public double BiasContribution { get; set; }        // Contribución del bias
    public double MomentumContribution { get; set; }    // Contribución del momentum
    public double VolumeContribution { get; set; }      // Contribución del volumen
    public double FinalConfidence { get; set; }         // Confianza final (0-1)
}
```

### DecisionSnapshot

```csharp
public class DecisionSnapshot
{
    public DateTime GeneratedAt { get; set; }
    public string Instrument { get; set; }
    public List<HeatZone> HeatZones { get; set; }
    public string GlobalBias { get; set; }              // "Bullish", "Bearish", "Neutral"
    public double GlobalBiasStrength { get; set; }      // 0.0 - 1.0
    public double MarketClarity { get; set; }           // 0.0 - 1.0
    public double MarketVolatilityNormalized { get; set; } // 0.0 - 1.0
    public DecisionSummary Summary { get; set; }
    public Dictionary<string, object> Metadata { get; set; } // Pipeline data
}
```

---

## ⚙️ Configuración

### Parámetros en EngineConfig.cs

```csharp
// ============================================================
// HEATZONE DETECTION
// ============================================================
public double HeatZone_OverlapToleranceATR { get; set; } = 0.5;
public int HeatZone_MinConfluence { get; set; } = 2;
public double HeatZone_MinScore { get; set; } = 0.3;

// ============================================================
// DECISION SCORING WEIGHTS (deben sumar 1.0)
// ============================================================
public double Weight_CoreScore { get; set; } = 0.40;
public double Weight_Proximity { get; set; } = 0.18;
public double Weight_Confluence { get; set; } = 0.15;
public double Weight_Type { get; set; } = 0.10;
public double Weight_Bias { get; set; } = 0.07;
public double Weight_Momentum { get; set; } = 0.05;
public double Weight_Volume { get; set; } = 0.05;

// ============================================================
// TYPE HIERARCHY (multiplicadores por tipo)
// ============================================================
public Dictionary<string, double> TypeMultipliers { get; set; } = new Dictionary<string, double>
{
    { "POI", 1.0 },
    { "OB", 0.9 },
    { "FVG", 0.85 },
    { "BOS", 0.8 },
    { "CHoCH", 0.75 },
    { "LG", 0.7 },
    { "Swing", 0.6 },
    { "LV", 0.5 },
    { "Double", 0.4 }
};

// ============================================================
// RISK MANAGEMENT
// ============================================================
public double RiskPercentPerTrade { get; set; } = 0.5;      // 0.5% de la cuenta
public double MinRiskRewardRatio { get; set; } = 1.5;       // Mínimo 1.5:1
public double SL_BufferATR { get; set; } = 0.2;             // 20% del ATR

// ============================================================
// DECISION THRESHOLDS
// ============================================================
public double MinConfidenceForEntry { get; set; } = 0.65;   // 65% mínimo para entrar
public double MinConfidenceForWait { get; set; } = 0.50;    // 50% para esperar
public double ProximityThresholdATR { get; set; } = 3.0;    // Máximo 3 ATR de distancia
public double BiasOverrideConfidenceFactor { get; set; } = 0.85; // Penalización contra-tendencia

// ============================================================
// MARKET CLARITY
// ============================================================
public int MarketClarity_MinStructures { get; set; } = 10;  // Mínimo para claridad alta
public int MarketClarity_MaxAge { get; set; } = 100;        // Edad máxima en barras

// ============================================================
// CONFLUENCE NORMALIZATION
// ============================================================
public int MaxConfluenceReference { get; set; } = 5;        // Saturación en 5 estructuras
```

---

## 🔄 Flujo de Ejecución

### Ejemplo Completo

```csharp
// 1. Inicializar
var config = EngineConfig.LoadDefaults();
var logger = new ConsoleLogger();
var barData = new NinjaBarDataProvider(this);
var coreEngine = new CoreEngine(barData, config, logger);
var decisionEngine = new DecisionEngine(config, logger);

// 2. En cada barra nueva
protected override void OnBarUpdate()
{
    if (CurrentBar < 200) return; // Esperar datos suficientes
    
    // 3. Generar decisión
    var decision = decisionEngine.GenerateDecision(
        barData: barData,
        coreEngine: coreEngine,
        currentBar: CurrentBar,
        accountSize: Account.Get(AccountItem.CashValue, Currency.UsDollar)
    );
    
    // 4. Evaluar decisión
    if (decision.Action == "BUY" && decision.Confidence >= 0.65)
    {
        // 5. Colocar orden limit
        EnterLongLimit(
            fromEntrySignal: 0,
            isLiveUntilCancelled: false,
            quantity: (int)decision.PositionSizeContracts,
            limitPrice: decision.Entry,
            signalName: "DFM_BUY"
        );
        
        // 6. Configurar SL/TP
        SetStopLoss("DFM_BUY", CalculationMode.Price, decision.StopLoss, false);
        SetProfitTarget("DFM_BUY", CalculationMode.Price, decision.TakeProfit);
        
        // 7. Log rationale
        Print(decision.Rationale);
        Print($"Explainability: Core={decision.Explainability.CoreScoreContribution:F3}, " +
              $"Proximity={decision.Explainability.ProximityContribution:F3}");
    }
    else if (decision.Action == "WAIT")
    {
        Print($"WAIT - Confidence: {decision.Confidence:F2} (min: 0.65)");
    }
}
```

---

## 📊 Scoring Multi-Factor

### Desglose de Contribuciones

| Factor | Peso | Descripción | Rango |
|--------|------|-------------|-------|
| **CoreScore** | 0.40 | Score base de la zona (ponderado por TF) | 0.0 - 1.0 |
| **Proximity** | 0.18 | Proximidad al precio actual (normalizada por ATR) | 0.0 - 1.0 |
| **Confluence** | 0.15 | Número de estructuras fusionadas (normalizado) | 0.0 - 1.0 |
| **Type** | 0.10 | Multiplicador por tipo de estructura dominante | 0.4 - 1.0 |
| **Bias** | 0.07 | Alineación con bias global | -1.0 - 1.0 |
| **Momentum** | 0.05 | Fuerza del movimiento reciente | 0.0 - 1.0 |
| **Volume** | 0.05 | Volumen relativo | 0.0 - 1.0 |

### Ejemplo de Cálculo

```
HeatZone: HZ_001
- CoreScore: 0.75
- ProximityFactor: 0.90 (precio dentro de la zona)
- ConfluenceCount: 7 estructuras
- DominantType: POI (multiplier = 1.0)
- GlobalBias: Bullish (alineado)
- Direction: Bullish

Cálculo:
CoreScoreContribution    = 0.75 × 0.40 = 0.300
ProximityContribution    = 0.90 × 0.18 = 0.162
ConfluenceContribution   = Min(7/5, 1.0) × 0.15 = 0.150
TypeContribution         = 1.0 × 0.10 = 0.100
BiasContribution         = 1.0 × 0.07 = 0.070  (alineado)
MomentumContribution     = 0.80 × 0.05 = 0.040
VolumeContribution       = 0.60 × 0.05 = 0.030

RawConfidence = 0.852

// Sin penalización (alineado con bias)
FinalConfidence = 0.852

// Si fuera contra bias:
// FinalConfidence = 0.852 × 0.85 = 0.724
```

---

## 🛡️ Gestión de Riesgo

### Entry Estructural

**Objetivo**: Maximizar Risk:Reward entrando en los extremos de las zonas

```
Zona Bullish:
High: 5010 ┌─────────────┐
           │             │
           │   HeatZone  │  ← Confluencia de FVG + OB
           │             │
Low: 5000  └─────────────┘
           ↑
           Entry BUY Limit @ 5000 (borde inferior)
           
SL: 4995 (5000 - 0.2×ATR)
TP: 5015 (5000 + 1.5×Risk)

Ventaja: Maximiza R:R al entrar en el mejor precio posible
```

### Position Sizing Dinámico

```
Cuenta: $100,000
RiskPercent: 0.5%
RiskAmount: $500

Entry: 5000
StopLoss: 4995
RiskPerShare: 5 puntos

PointValue: $50 (ES)
RiskPerContract: 5 × $50 = $250

PositionSize: $500 / $250 = 2 contratos

Riesgo Total: 2 × $250 = $500 (0.5% de la cuenta) ✓
```

### Validaciones

1. **MinRiskRewardRatio**: R:R >= 1.5:1
2. **PositionSize mínimo**: 1 contrato
3. **PointValue válido**: > 0
4. **TickSize válido**: > 0
5. **AccountSize válido**: > 0

---

## 🧠 Explainability (XAI)

### DecisionScoreBreakdown

Cada decisión incluye un desglose completo de cómo se calculó la confianza:

```csharp
decision.Explainability:
{
    CoreScoreContribution: 0.300,
    ProximityContribution: 0.162,
    ConfluenceContribution: 0.150,
    TypeContribution: 0.100,
    BiasContribution: 0.070,
    MomentumContribution: 0.040,
    VolumeContribution: 0.030,
    FinalConfidence: 0.852
}
```

### Rationale Profesional

```
BUY Limit @ 5000.00 (HeatZone HZ_001: Bullish, 7 structures, Score: 0.75)
- Confidence: 0.85 (Core: 0.30, Proximity: 0.16, Confluence: 0.15)
- SL: 4995.00, TP: 5015.00, R:R = 1.50
- Position: 2 contracts (Risk: $500.00)
- Global Bias: Bullish (0.80)
- Dominant Factor: POI 4H (Score: 0.82)
```

### Trazabilidad Completa

```csharp
// Estructuras origen
decision.SourceStructureIds:
[
    "FVG_60_001",
    "OB_240_005",
    "POI_240_002",
    "BOS_60_012",
    ...
]

// Metadata del pipeline
snapshot.Metadata:
{
    "BestZone": HeatZone { ... },
    "BestConfidence": 0.852,
    "BestBreakdown": DecisionScoreBreakdown { ... },
    "RejectedZones": 3,
    "ProcessingTimeMs": 12.5
}
```

---

## 💡 Ejemplos de Uso

### Ejemplo 1: Estrategia Básica

```csharp
public class DFMStrategy : Strategy
{
    private CoreEngine _coreEngine;
    private DecisionEngine _decisionEngine;
    
    protected override void OnStateChange()
    {
        if (State == State.Configure)
        {
            var config = EngineConfig.LoadDefaults();
            var logger = new NinjaLogger(this);
            var barData = new NinjaBarDataProvider(this);
            
            _coreEngine = new CoreEngine(barData, config, logger);
            _decisionEngine = new DecisionEngine(config, logger);
        }
    }
    
    protected override void OnBarUpdate()
    {
        if (CurrentBar < 200) return;
        
        var decision = _decisionEngine.GenerateDecision(
            barData: new NinjaBarDataProvider(this),
            coreEngine: _coreEngine,
            currentBar: CurrentBar,
            accountSize: Account.Get(AccountItem.CashValue, Currency.UsDollar)
        );
        
        if (decision.Action == "BUY" && decision.Confidence >= 0.70)
        {
            EnterLongLimit(0, false, (int)decision.PositionSizeContracts, 
                          decision.Entry, "DFM_BUY");
            SetStopLoss("DFM_BUY", CalculationMode.Price, decision.StopLoss, false);
            SetProfitTarget("DFM_BUY", CalculationMode.Price, decision.TakeProfit);
        }
    }
}
```

### Ejemplo 2: Filtrado Avanzado

```csharp
var decision = _decisionEngine.GenerateDecision(...);

// Filtros adicionales
bool passFilters = 
    decision.Confidence >= 0.75 &&                          // Alta confianza
    decision.Explainability.ProximityContribution >= 0.15 && // Cerca del precio
    snapshot.MarketClarity >= 0.60 &&                       // Mercado claro
    snapshot.GlobalBias == decision.Action.Replace("BUY", "Bullish")
                                          .Replace("SELL", "Bearish"); // Alineado

if (decision.Action != "WAIT" && passFilters)
{
    // Ejecutar trade
}
```

### Ejemplo 3: Logging Detallado

```csharp
Print($"=== DECISION REPORT ===");
Print($"Action: {decision.Action}");
Print($"Confidence: {decision.Confidence:F3}");
Print($"");
Print($"Entry: {decision.Entry:F2}");
Print($"StopLoss: {decision.StopLoss:F2}");
Print($"TakeProfit: {decision.TakeProfit:F2}");
Print($"R:R: {((decision.TakeProfit - decision.Entry) / (decision.Entry - decision.StopLoss)):F2}");
Print($"");
Print($"Position: {decision.PositionSizeContracts} contracts");
Print($"");
Print($"Explainability:");
Print($"  Core: {decision.Explainability.CoreScoreContribution:F3}");
Print($"  Proximity: {decision.Explainability.ProximityContribution:F3}");
Print($"  Confluence: {decision.Explainability.ConfluenceContribution:F3}");
Print($"  Type: {decision.Explainability.TypeContribution:F3}");
Print($"  Bias: {decision.Explainability.BiasContribution:F3}");
Print($"");
Print($"Rationale:");
Print(decision.Rationale);
```

---

## ✅ Tests

### Cobertura: 67 tests (100% pasando)

#### Modelos de Datos (15 tests)
- Creación y validación de HeatZone, TradeDecision, DecisionSnapshot, DecisionScoreBreakdown

#### ContextManager (8 tests)
- BuildSummary, GlobalBias, MarketClarity, Volatility

#### StructureFusion (5 tests)
- Fusión básica, agregación de scores, dirección, estructura dominante, eficiencia

#### ProximityAnalyzer (4 tests)
- Distancia inside/outside, filtrado, ordenamiento

#### RiskCalculator (7 tests)
- **CRÍTICO**: Entry estructural BUY/SELL
- SL con buffer, TP R:R, PositionSize
- **CRÍTICO**: Validación MinRR, manejo de cuenta pequeña

#### DecisionFusionModel (7 tests)
- Cálculo de confidence, normalización de confluence
- **CRÍTICO**: Penalización bias, selección de BestZone

#### OutputAdapter (9 tests)
- Generación de BUY/SELL/WAIT
- **CRÍTICO**: Formato de rationale, explainability completa

#### Integración (9 tests)
- Pipeline completo, orden de componentes, coherencia de decisión

#### Infraestructura (5 tests)
- Concurrencia, hash mismatch, logger levels

---

## 📚 Referencias

- **Archivo Principal**: `src/Decision/DecisionEngine.cs`
- **Componentes**: `src/Decision/ContextManager.cs`, `StructureFusion.cs`, etc.
- **Modelos**: `src/Decision/DecisionModels.cs`
- **Tests**: `src/Testing/DecisionEngineTests.cs`
- **Configuración**: `src/Core/EngineConfig.cs`

---

**Última actualización**: Fase 10 completada - 345/345 tests pasando (100%)

