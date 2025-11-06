// ============================================================================
// IntervalTreeTests.cs
// PinkButterfly CoreBrain - Tests unitarios para IntervalTree
// 
// Valida funcionalidad básica del Interval Tree:
// - Insert
// - QueryOverlap
// - Remove
// - Performance
// ============================================================================

using System;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace PinkButterfly.Tests
{
    /// <summary>
    /// Tests del IntervalTree
    /// Para ejecutar: instanciar esta clase y llamar RunAllTests()
    /// </summary>
    public class IntervalTreeTests
    {
        private int _testsPassed = 0;
        private int _testsFailed = 0;
        private Action<string> _logger;

        public IntervalTreeTests(Action<string> logger = null)
        {
            _logger = logger ?? Console.WriteLine;
        }

        public void RunAllTests()
        {
            _logger("==============================================");
            _logger("INTERVAL TREE TESTS");
            _logger("==============================================");
            _logger("");

            Test_Insert_BasicFunctionality();
            Test_QueryOverlap_NoResults();
            Test_QueryOverlap_WithResults();
            Test_QueryOverlap_MultipleResults();
            Test_Remove_BasicFunctionality();
            Test_QueryPoint();
            Test_Performance_LargeDataset();

            _logger("");
            _logger("==============================================");
            _logger($"RESULTADOS: {_testsPassed} passed, {_testsFailed} failed");
            _logger("==============================================");
        }

        private void Assert(bool condition, string testName, string message)
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

        // ========================================================================
        // TESTS
        // ========================================================================

        private void Test_Insert_BasicFunctionality()
        {
            var tree = new IntervalTree<string>();
            tree.Insert(10, 20, "A");
            tree.Insert(15, 25, "B");
            tree.Insert(30, 40, "C");

            Assert(tree.Count == 3, "Insert_BasicFunctionality", 
                   $"Expected count=3, got {tree.Count}");
        }

        private void Test_QueryOverlap_NoResults()
        {
            var tree = new IntervalTree<string>();
            tree.Insert(10, 20, "A");
            tree.Insert(30, 40, "B");

            var results = tree.QueryOverlap(21, 29).ToList();

            Assert(results.Count == 0, "QueryOverlap_NoResults", 
                   $"Expected 0 results, got {results.Count}");
        }

        private void Test_QueryOverlap_WithResults()
        {
            var tree = new IntervalTree<string>();
            tree.Insert(10, 20, "A");
            tree.Insert(15, 25, "B");
            tree.Insert(30, 40, "C");

            // Query [12, 18] debe devolver "A" y "B"
            var results = tree.QueryOverlap(12, 18).ToList();

            Assert(results.Count == 2, "QueryOverlap_WithResults_Count", 
                   $"Expected 2 results, got {results.Count}");

            Assert(results.Contains("A") && results.Contains("B"), 
                   "QueryOverlap_WithResults_Content", 
                   "Expected A and B in results");
        }

        private void Test_QueryOverlap_MultipleResults()
        {
            var tree = new IntervalTree<string>();
            tree.Insert(5, 15, "A");
            tree.Insert(10, 20, "B");
            tree.Insert(12, 18, "C");
            tree.Insert(25, 35, "D");

            // Query [11, 19] debe devolver A, B, C
            var results = tree.QueryOverlap(11, 19).ToList();

            Assert(results.Count == 3, "QueryOverlap_MultipleResults", 
                   $"Expected 3 results, got {results.Count}");
        }

        private void Test_Remove_BasicFunctionality()
        {
            var tree = new IntervalTree<string>();
            tree.Insert(10, 20, "A");
            tree.Insert(15, 25, "B");
            tree.Insert(30, 40, "C");

            bool removed = tree.Remove(15, 25);

            Assert(removed, "Remove_ReturnValue", "Remove should return true");
            Assert(tree.Count == 2, "Remove_Count", $"Expected count=2, got {tree.Count}");

            var results = tree.QueryOverlap(15, 25).ToList();
            Assert(!results.Contains("B"), "Remove_NotInQuery", 
                   "Removed item should not appear in query");
        }

        private void Test_QueryPoint()
        {
            var tree = new IntervalTree<string>();
            tree.Insert(10, 20, "A");
            tree.Insert(15, 25, "B");
            tree.Insert(30, 40, "C");

            // Point 17 debe estar en A y B
            var results = tree.QueryPoint(17).ToList();

            Assert(results.Count == 2, "QueryPoint_Count", 
                   $"Expected 2 results for point 17, got {results.Count}");
        }

        private void Test_Performance_LargeDataset()
        {
            var tree = new IntervalTree<int>();
            int n = 1000;

            // Insert 1000 intervalos
            var startInsert = DateTime.Now;
            for (int i = 0; i < n; i++)
            {
                tree.Insert(i * 10, i * 10 + 20, i);
            }
            var insertTime = (DateTime.Now - startInsert).TotalMilliseconds;

            // Query que debería retornar múltiples resultados
            var startQuery = DateTime.Now;
            var results = tree.QueryOverlap(5000, 5100).ToList();
            var queryTime = (DateTime.Now - startQuery).TotalMilliseconds;

            _logger($"  Performance: Insert {n} items = {insertTime:F2}ms, " +
                    $"Query = {queryTime:F2}ms, Results = {results.Count}");

            Assert(insertTime < 100, "Performance_Insert", 
                   $"Insert too slow: {insertTime}ms (expected < 100ms)");

            Assert(queryTime < 10, "Performance_Query", 
                   $"Query too slow: {queryTime}ms (expected < 10ms)");
        }
    }
}

