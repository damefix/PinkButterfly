// ============================================================================
// EventsTests.cs
// PinkButterfly CoreBrain - Tests del sistema de eventos
// 
// Valida que los eventos se disparen correctamente:
// - OnStructureAdded (cuando se añade una estructura)
// - OnStructureUpdated (cuando se actualiza una estructura)
// - OnStructureRemoved (cuando se elimina una estructura)
// ============================================================================

using System;
using System.Collections.Generic;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    public class EventsTests
    {
        private int _testsPassed = 0;
        private int _testsFailed = 0;
        private Action<string> _logger;

        public EventsTests(Action<string> logger = null)
        {
            _logger = logger ?? Console.WriteLine;
        }

        public void RunAllTests()
        {
            _logger("==============================================");
            _logger("EVENTS SYSTEM TESTS");
            _logger("==============================================");
            _logger("");

            // Tests de eventos (6 tests)
            Test_Event_OnStructureAdded();
            Test_Event_OnStructureUpdated();
            Test_Event_OnStructureRemoved();
            Test_Event_MultipleSubscribers();
            Test_Event_EventArgs_Validation();
            Test_Event_Unsubscribe();

            _logger("");
            _logger("==============================================");
            _logger($"RESULTADOS: {_testsPassed} passed, {_testsFailed} failed");
            _logger("==============================================");
        }

        private void Assert(bool condition, string testName, string message = "")
        {
            if (condition)
            {
                _testsPassed++;
                _logger($"✓ PASS: {testName}");
            }
            else
            {
                _testsFailed++;
                _logger($"✗ FAIL: {testName} - {message}");
            }
        }

        private void AddBar(MockBarDataProvider provider, int tfMinutes, int barIndex, double open, double high, double low, double close)
        {
            provider.AddBar(tfMinutes, new MockBar 
            { 
                Time = DateTime.UtcNow.AddMinutes(barIndex * tfMinutes), 
                Open = open, 
                High = high, 
                Low = low, 
                Close = close,
                Volume = 1000 
            });
        }

        private void Test_Event_OnStructureAdded()
        {
            var config = new EngineConfig
            {
                TimeframesToUse = new List<int> { 60 },
                EnableAutoPurge = false
            };

            var provider = new MockBarDataProvider();
            AddBar(provider, 60, 0, 5000, 5005, 4995, 5002);
            AddBar(provider, 60, 1, 5002, 5010, 5000, 5008);
            AddBar(provider, 60, 2, 5008, 5015, 5005, 5012);

            var logger = new TestLogger(_logger) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            // Suscribirse al evento
            bool eventFired = false;
            StructureAddedEventArgs capturedArgs = null;

            engine.OnStructureAdded += (sender, args) =>
            {
                eventFired = true;
                capturedArgs = args;
            };

            // Añadir una estructura manualmente
            var fvg = new FVGInfo
            {
                Id = "TEST_FVG_1",
                TF = 60,
                Low = 5000,
                High = 5010,
                Direction = "Bullish",
                StartTime = provider.GetBarTime(60, 0),
                EndTime = provider.GetBarTime(60, 0),
                CreatedAtBarIndex = 0,
                LastUpdatedBarIndex = 0,
                IsActive = true,
                Metadata = new StructureMetadata { CreatedByDetector = "TestDetector" }
            };

            engine.AddStructure(fvg);

            // Validar que el evento se disparó
            Assert(eventFired, "Event_OnStructureAdded_Fired", "Evento no se disparó");
            Assert(capturedArgs != null, "Event_OnStructureAdded_Args", "EventArgs es null");
            Assert(capturedArgs?.Structure.Id == "TEST_FVG_1", "Event_OnStructureAdded_StructureId", $"ID incorrecto: {capturedArgs?.Structure.Id}");
            Assert(capturedArgs?.TimeframeMinutes == 60, "Event_OnStructureAdded_TF", $"TF incorrecto: {capturedArgs?.TimeframeMinutes}");
            Assert(capturedArgs?.CreatedByDetector == "TestDetector", "Event_OnStructureAdded_Detector", $"Detector incorrecto: {capturedArgs?.CreatedByDetector}");
        }

        private void Test_Event_OnStructureUpdated()
        {
            var config = new EngineConfig
            {
                TimeframesToUse = new List<int> { 60 },
                EnableAutoPurge = false
            };

            var provider = new MockBarDataProvider();
            AddBar(provider, 60, 0, 5000, 5005, 4995, 5002);
            AddBar(provider, 60, 1, 5002, 5010, 5000, 5008);

            var logger = new TestLogger(_logger) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            // Añadir estructura
            var fvg = new FVGInfo
            {
                Id = "TEST_FVG_2",
                TF = 60,
                Low = 5000,
                High = 5010,
                Direction = "Bullish",
                StartTime = provider.GetBarTime(60, 0),
                EndTime = provider.GetBarTime(60, 0),
                CreatedAtBarIndex = 0,
                LastUpdatedBarIndex = 0,
                IsActive = true,
                Score = 0.5,
                Metadata = new StructureMetadata { CreatedByDetector = "TestDetector" }
            };

            engine.AddStructure(fvg);

            // Suscribirse al evento
            bool eventFired = false;
            StructureUpdatedEventArgs capturedArgs = null;

            engine.OnStructureUpdated += (sender, args) =>
            {
                eventFired = true;
                capturedArgs = args;
            };

            // Actualizar la estructura
            fvg.TouchCount_Body++;
            engine.UpdateStructure(fvg);

            // Validar que el evento se disparó
            Assert(eventFired, "Event_OnStructureUpdated_Fired", "Evento no se disparó");
            Assert(capturedArgs != null, "Event_OnStructureUpdated_Args", "EventArgs es null");
            Assert(capturedArgs?.Structure.Id == "TEST_FVG_2", "Event_OnStructureUpdated_StructureId", $"ID incorrecto: {capturedArgs?.Structure.Id}");
            Assert(capturedArgs?.UpdateType == "ScoreUpdated", "Event_OnStructureUpdated_Type", $"UpdateType incorrecto: {capturedArgs?.UpdateType}");
            Assert(capturedArgs?.PreviousScore != null, "Event_OnStructureUpdated_PrevScore", "PreviousScore es null");
            Assert(capturedArgs?.NewScore != null, "Event_OnStructureUpdated_NewScore", "NewScore es null");
        }

        private void Test_Event_OnStructureRemoved()
        {
            var config = new EngineConfig
            {
                TimeframesToUse = new List<int> { 60 },
                EnableAutoPurge = false
            };

            var provider = new MockBarDataProvider();
            AddBar(provider, 60, 0, 5000, 5005, 4995, 5002);

            var logger = new TestLogger(_logger) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            // Añadir estructura
            var fvg = new FVGInfo
            {
                Id = "TEST_FVG_3",
                TF = 60,
                Low = 5000,
                High = 5010,
                Direction = "Bullish",
                StartTime = provider.GetBarTime(60, 0),
                EndTime = provider.GetBarTime(60, 0),
                CreatedAtBarIndex = 0,
                LastUpdatedBarIndex = 0,
                IsActive = true,
                Score = 0.3,
                Metadata = new StructureMetadata { CreatedByDetector = "TestDetector" }
            };

            engine.AddStructure(fvg);

            // Suscribirse al evento
            bool eventFired = false;
            StructureRemovedEventArgs capturedArgs = null;

            engine.OnStructureRemoved += (sender, args) =>
            {
                eventFired = true;
                capturedArgs = args;
            };

            // Eliminar la estructura
            bool removed = engine.RemoveStructure("TEST_FVG_3");

            // Validar que el evento se disparó
            Assert(removed, "Event_OnStructureRemoved_Removed", "RemoveStructure retornó false");
            Assert(eventFired, "Event_OnStructureRemoved_Fired", "Evento no se disparó");
            Assert(capturedArgs != null, "Event_OnStructureRemoved_Args", "EventArgs es null");
            Assert(capturedArgs?.StructureId == "TEST_FVG_3", "Event_OnStructureRemoved_StructureId", $"ID incorrecto: {capturedArgs?.StructureId}");
            Assert(capturedArgs?.StructureType == "FVG", "Event_OnStructureRemoved_Type", $"Type incorrecto: {capturedArgs?.StructureType}");
            Assert(capturedArgs?.RemovalReason == "Manual", "Event_OnStructureRemoved_Reason", $"Reason incorrecto: {capturedArgs?.RemovalReason}");
            Assert(capturedArgs?.LastScore >= 0, "Event_OnStructureRemoved_Score", $"LastScore debe ser >= 0: {capturedArgs?.LastScore}");
        }

        private void Test_Event_MultipleSubscribers()
        {
            var config = new EngineConfig
            {
                TimeframesToUse = new List<int> { 60 },
                EnableAutoPurge = false
            };

            var provider = new MockBarDataProvider();
            AddBar(provider, 60, 0, 5000, 5005, 4995, 5002);

            var logger = new TestLogger(_logger) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            // Múltiples suscriptores
            int subscriber1Count = 0;
            int subscriber2Count = 0;
            int subscriber3Count = 0;

            engine.OnStructureAdded += (sender, args) => subscriber1Count++;
            engine.OnStructureAdded += (sender, args) => subscriber2Count++;
            engine.OnStructureAdded += (sender, args) => subscriber3Count++;

            // Añadir estructura
            var fvg = new FVGInfo
            {
                Id = "TEST_FVG_4",
                TF = 60,
                Low = 5000,
                High = 5010,
                Direction = "Bullish",
                StartTime = provider.GetBarTime(60, 0),
                EndTime = provider.GetBarTime(60, 0),
                CreatedAtBarIndex = 0,
                LastUpdatedBarIndex = 0,
                IsActive = true,
                Metadata = new StructureMetadata { CreatedByDetector = "TestDetector" }
            };

            engine.AddStructure(fvg);

            // Validar que todos los suscriptores recibieron el evento
            Assert(subscriber1Count == 1, "Event_MultipleSubscribers_Sub1", $"Subscriber 1 count: {subscriber1Count}");
            Assert(subscriber2Count == 1, "Event_MultipleSubscribers_Sub2", $"Subscriber 2 count: {subscriber2Count}");
            Assert(subscriber3Count == 1, "Event_MultipleSubscribers_Sub3", $"Subscriber 3 count: {subscriber3Count}");
        }

        private void Test_Event_EventArgs_Validation()
        {
            var config = new EngineConfig
            {
                TimeframesToUse = new List<int> { 60 },
                EnableAutoPurge = false
            };

            var provider = new MockBarDataProvider();
            AddBar(provider, 60, 0, 5000, 5005, 4995, 5002);

            var logger = new TestLogger(_logger) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            // Suscribirse y validar todos los campos de EventArgs
            StructureAddedEventArgs addedArgs = null;
            engine.OnStructureAdded += (sender, args) => addedArgs = args;

            var fvg = new FVGInfo
            {
                Id = "TEST_FVG_5",
                TF = 60,
                Low = 5000,
                High = 5010,
                Direction = "Bullish",
                StartTime = provider.GetBarTime(60, 0),
                EndTime = provider.GetBarTime(60, 0),
                CreatedAtBarIndex = 0,
                LastUpdatedBarIndex = 0,
                IsActive = true,
                Metadata = new StructureMetadata { CreatedByDetector = "FVGDetector" }
            };

            engine.AddStructure(fvg);

            // Validar todos los campos
            Assert(addedArgs != null, "Event_EventArgs_NotNull", "EventArgs es null");
            Assert(addedArgs?.Structure != null, "Event_EventArgs_Structure", "Structure es null");
            Assert(addedArgs?.TimeframeMinutes > 0, "Event_EventArgs_TF", $"TF inválido: {addedArgs?.TimeframeMinutes}");
            Assert(addedArgs?.BarIndex >= 0, "Event_EventArgs_BarIndex", $"BarIndex inválido: {addedArgs?.BarIndex}");
            Assert(addedArgs?.EventTimeUTC != default(DateTime), "Event_EventArgs_Time", "EventTimeUTC no inicializado");
            Assert(!string.IsNullOrEmpty(addedArgs?.CreatedByDetector), "Event_EventArgs_Detector", "CreatedByDetector vacío");
        }

        private void Test_Event_Unsubscribe()
        {
            var config = new EngineConfig
            {
                TimeframesToUse = new List<int> { 60 },
                EnableAutoPurge = false
            };

            var provider = new MockBarDataProvider();
            AddBar(provider, 60, 0, 5000, 5005, 4995, 5002);

            var logger = new TestLogger(_logger) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            // Suscribirse
            int eventCount = 0;
            EventHandler<StructureAddedEventArgs> handler = (sender, args) => eventCount++;

            engine.OnStructureAdded += handler;

            // Añadir estructura (debe disparar evento)
            var fvg1 = new FVGInfo
            {
                Id = "TEST_FVG_6",
                TF = 60,
                Low = 5000,
                High = 5010,
                Direction = "Bullish",
                StartTime = provider.GetBarTime(60, 0),
                EndTime = provider.GetBarTime(60, 0),
                CreatedAtBarIndex = 0,
                LastUpdatedBarIndex = 0,
                IsActive = true,
                Metadata = new StructureMetadata { CreatedByDetector = "TestDetector" }
            };

            engine.AddStructure(fvg1);

            Assert(eventCount == 1, "Event_Unsubscribe_Subscribed", $"Event count después de suscripción: {eventCount}");

            // Desuscribirse
            engine.OnStructureAdded -= handler;

            // Añadir otra estructura (NO debe disparar evento)
            var fvg2 = new FVGInfo
            {
                Id = "TEST_FVG_7",
                TF = 60,
                Low = 5010,
                High = 5020,
                Direction = "Bullish",
                StartTime = provider.GetBarTime(60, 0),
                EndTime = provider.GetBarTime(60, 0),
                CreatedAtBarIndex = 0,
                LastUpdatedBarIndex = 0,
                IsActive = true,
                Metadata = new StructureMetadata { CreatedByDetector = "TestDetector" }
            };

            engine.AddStructure(fvg2);

            Assert(eventCount == 1, "Event_Unsubscribe_Unsubscribed", $"Event count después de desuscripción: {eventCount} (esperado: 1)");
        }
    }
}
