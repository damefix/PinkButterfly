// ============================================================================
// IntervalTree.cs
// PinkButterfly CoreBrain - Interval Tree para consultas espaciales eficientes
// 
// Implementación de Interval Tree (Augmented Binary Search Tree)
// Permite consultas de overlap en tiempo O(log n + k) donde:
// - n = número total de intervalos
// - k = número de resultados que se superponen
//
// Usado para:
// - Detectar confluencias entre estructuras (POI)
// - Búsqueda rápida de estructuras en rango de precio
// - Merge de FVGs consecutivos
//
// Complejidad:
// - Insert: O(log n)
// - Remove: O(log n)
// - QueryOverlap: O(log n + k)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Interval Tree genérico para búsqueda eficiente de intervalos superpuestos
    /// Los intervalos se definen por [Low, High] donde Low <= High
    /// </summary>
    /// <typeparam name="T">Tipo de dato asociado a cada intervalo</typeparam>
    public class IntervalTree<T>
    {
        /// <summary>
        /// Nodo del árbol de intervalos
        /// Cada nodo almacena un intervalo y mantiene el valor máximo del subárbol (augmented data)
        /// </summary>
        private class IntervalNode
        {
            public double Low { get; set; }
            public double High { get; set; }
            public T Data { get; set; }
            public double MaxInSubtree { get; set; }  // Max High en este subárbol (augmented)
            public IntervalNode Left { get; set; }
            public IntervalNode Right { get; set; }
            public IntervalNode Parent { get; set; }

            public IntervalNode(double low, double high, T data)
            {
                Low = low;
                High = high;
                Data = data;
                MaxInSubtree = high;
            }
        }

        private IntervalNode _root;
        private int _count;
        private readonly object _lock = new object();

        /// <summary>
        /// Número de intervalos en el árbol
        /// </summary>
        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _count;
                }
            }
        }

        /// <summary>
        /// Inserta un nuevo intervalo en el árbol
        /// Complejidad: O(log n)
        /// </summary>
        /// <param name="low">Límite inferior del intervalo</param>
        /// <param name="high">Límite superior del intervalo</param>
        /// <param name="data">Dato asociado al intervalo</param>
        public void Insert(double low, double high, T data)
        {
            if (low > high)
                throw new ArgumentException($"Low ({low}) no puede ser mayor que High ({high})");

            lock (_lock)
            {
                var newNode = new IntervalNode(low, high, data);
                _root = InsertRecursive(_root, newNode);
                _count++;
            }
        }

        /// <summary>
        /// Inserción recursiva manteniendo propiedades BST y actualizando MaxInSubtree
        /// </summary>
        private IntervalNode InsertRecursive(IntervalNode node, IntervalNode newNode)
        {
            // Caso base: posición encontrada
            if (node == null)
                return newNode;

            // Decisión BST basada en Low (clave primaria)
            if (newNode.Low < node.Low)
            {
                node.Left = InsertRecursive(node.Left, newNode);
                if (node.Left != null)
                    node.Left.Parent = node;
            }
            else
            {
                node.Right = InsertRecursive(node.Right, newNode);
                if (node.Right != null)
                    node.Right.Parent = node;
            }

            // Actualizar MaxInSubtree (augmented data)
            UpdateMaxInSubtree(node);

            return node;
        }

        /// <summary>
        /// Actualiza el MaxInSubtree de un nodo basado en sus hijos
        /// </summary>
        private void UpdateMaxInSubtree(IntervalNode node)
        {
            if (node == null) return;

            double maxVal = node.High;

            if (node.Left != null)
                maxVal = Math.Max(maxVal, node.Left.MaxInSubtree);

            if (node.Right != null)
                maxVal = Math.Max(maxVal, node.Right.MaxInSubtree);

            node.MaxInSubtree = maxVal;
        }

        /// <summary>
        /// Elimina un intervalo del árbol
        /// Complejidad: O(log n)
        /// NOTA: Elimina la primera coincidencia encontrada
        /// </summary>
        /// <param name="low">Límite inferior del intervalo</param>
        /// <param name="high">Límite superior del intervalo</param>
        /// <returns>true si se eliminó, false si no se encontró</returns>
        public bool Remove(double low, double high)
        {
            lock (_lock)
            {
                var nodeToRemove = FindNodeRecursive(_root, low, high);
                if (nodeToRemove == null)
                    return false;

                _root = RemoveNode(_root, nodeToRemove);
                _count--;
                return true;
            }
        }

        /// <summary>
        /// Elimina un intervalo específico por su dato asociado
        /// Útil cuando hay múltiples intervalos con mismo low/high
        /// </summary>
        public bool RemoveByData(double low, double high, T data)
        {
            lock (_lock)
            {
                var nodeToRemove = FindNodeByDataRecursive(_root, low, high, data);
                if (nodeToRemove == null)
                    return false;

                _root = RemoveNode(_root, nodeToRemove);
                _count--;
                return true;
            }
        }

        /// <summary>
        /// Busca un nodo con el intervalo especificado
        /// </summary>
        private IntervalNode FindNodeRecursive(IntervalNode node, double low, double high)
        {
            if (node == null) return null;

            // Coincidencia exacta
            if (Math.Abs(node.Low - low) < 1e-10 && Math.Abs(node.High - high) < 1e-10)
                return node;

            // Búsqueda BST
            if (low < node.Low)
                return FindNodeRecursive(node.Left, low, high);
            else
                return FindNodeRecursive(node.Right, low, high);
        }

        /// <summary>
        /// Busca un nodo por intervalo Y dato asociado
        /// </summary>
        private IntervalNode FindNodeByDataRecursive(IntervalNode node, double low, double high, T data)
        {
            if (node == null) return null;

            // Coincidencia exacta con dato
            if (Math.Abs(node.Low - low) < 1e-10 && 
                Math.Abs(node.High - high) < 1e-10 && 
                EqualityComparer<T>.Default.Equals(node.Data, data))
                return node;

            // Buscar en ambos subárboles (podría haber duplicados)
            var leftResult = FindNodeByDataRecursive(node.Left, low, high, data);
            if (leftResult != null) return leftResult;

            return FindNodeByDataRecursive(node.Right, low, high, data);
        }

        /// <summary>
        /// Elimina un nodo específico del árbol
        /// </summary>
        private IntervalNode RemoveNode(IntervalNode root, IntervalNode nodeToRemove)
        {
            if (root == null) return null;

            // Búsqueda BST del nodo
            if (nodeToRemove.Low < root.Low)
            {
                root.Left = RemoveNode(root.Left, nodeToRemove);
            }
            else if (nodeToRemove.Low > root.Low)
            {
                root.Right = RemoveNode(root.Right, nodeToRemove);
            }
            else if (root == nodeToRemove)
            {
                // Nodo encontrado - casos de eliminación
                if (root.Left == null)
                    return root.Right;
                if (root.Right == null)
                    return root.Left;

                // Nodo con dos hijos: reemplazar con sucesor inorder (min del subárbol derecho)
                var successor = FindMin(root.Right);
                root.Low = successor.Low;
                root.High = successor.High;
                root.Data = successor.Data;
                root.Right = RemoveNode(root.Right, successor);
            }
            else
            {
                // Duplicados con mismo Low - buscar en subárbol derecho
                root.Right = RemoveNode(root.Right, nodeToRemove);
            }

            // Actualizar MaxInSubtree después de eliminación
            UpdateMaxInSubtree(root);

            return root;
        }

        /// <summary>
        /// Encuentra el nodo con el valor mínimo en el subárbol
        /// </summary>
        private IntervalNode FindMin(IntervalNode node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        /// <summary>
        /// Consulta todos los intervalos que se superponen con [queryLow, queryHigh]
        /// Complejidad: O(log n + k) donde k = número de resultados
        /// </summary>
        /// <param name="queryLow">Límite inferior del rango de consulta</param>
        /// <param name="queryHigh">Límite superior del rango de consulta</param>
        /// <returns>Enumeración de datos que se superponen</returns>
        public IEnumerable<T> QueryOverlap(double queryLow, double queryHigh)
        {
            if (queryLow > queryHigh)
                throw new ArgumentException($"QueryLow ({queryLow}) no puede ser mayor que QueryHigh ({queryHigh})");

            var results = new List<T>();

            lock (_lock)
            {
                QueryOverlapRecursive(_root, queryLow, queryHigh, results);
            }

            return results;
        }

        /// <summary>
        /// Búsqueda recursiva de overlaps
        /// Poda eficiente usando MaxInSubtree
        /// </summary>
        private void QueryOverlapRecursive(IntervalNode node, double queryLow, double queryHigh, List<T> results)
        {
            if (node == null) return;

            // Poda: si MaxInSubtree < queryLow, ningún intervalo en este subárbol se superpone
            if (node.MaxInSubtree < queryLow)
                return;

            // Buscar en subárbol izquierdo
            QueryOverlapRecursive(node.Left, queryLow, queryHigh, results);

            // Verificar si este nodo se superpone
            // Dos intervalos [a,b] y [c,d] se superponen si: a <= d AND c <= b
            if (node.Low <= queryHigh && queryLow <= node.High)
            {
                results.Add(node.Data);
            }

            // Buscar en subárbol derecho solo si es posible que haya overlaps
            // Si node.Low > queryHigh, todos los nodos a la derecha también lo serán (BST property)
            if (node.Low <= queryHigh)
            {
                QueryOverlapRecursive(node.Right, queryLow, queryHigh, results);
            }
        }

        /// <summary>
        /// Devuelve todos los intervalos en el árbol (inorder traversal)
        /// </summary>
        public IEnumerable<T> GetAll()
        {
            var results = new List<T>();

            lock (_lock)
            {
                InorderTraversal(_root, results);
            }

            return results;
        }

        /// <summary>
        /// Recorrido inorder para obtener todos los elementos
        /// </summary>
        private void InorderTraversal(IntervalNode node, List<T> results)
        {
            if (node == null) return;

            InorderTraversal(node.Left, results);
            results.Add(node.Data);
            InorderTraversal(node.Right, results);
        }

        /// <summary>
        /// Limpia el árbol completamente
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _root = null;
                _count = 0;
            }
        }

        /// <summary>
        /// Verifica si un punto específico está dentro de algún intervalo
        /// </summary>
        public IEnumerable<T> QueryPoint(double point)
        {
            return QueryOverlap(point, point);
        }

        /// <summary>
        /// Obtiene estadísticas del árbol (para debugging)
        /// </summary>
        public string GetStats()
        {
            lock (_lock)
            {
                int height = GetHeight(_root);
                int leafCount = CountLeaves(_root);
                
                return $"IntervalTree Stats: Count={_count}, Height={height}, Leaves={leafCount}, " +
                       $"Balance={(double)_count / Math.Max(height, 1):F2}";
            }
        }

        private int GetHeight(IntervalNode node)
        {
            if (node == null) return 0;
            return 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        private int CountLeaves(IntervalNode node)
        {
            if (node == null) return 0;
            if (node.Left == null && node.Right == null) return 1;
            return CountLeaves(node.Left) + CountLeaves(node.Right);
        }
    }
}

