using System;
using System.Collections.Generic;

namespace TestDijkstraNamespace
{
    public class SimpleGraph : IGraph<int>
    {
        private Dictionary<int, List<(int, float)>> adjacencyList = new Dictionary<int, List<(int, float)>>();

        // Add an edge to the graph
        public void AddEdge(int from, int to, float weight)
        {
            if (!adjacencyList.ContainsKey(from))
            {
                adjacencyList[from] = new List<(int, float)>();
            }
            adjacencyList[from].Add((to, weight));
        }

        // Return neighbors of a node
        public IEnumerable<int> Neighbors(int node)
        {
            if (adjacencyList.ContainsKey(node))
            {
                foreach (var neighbor in adjacencyList[node])
                {
                    yield return neighbor.Item1;
                }
            }
        }

        // Get the weight of an edge
        public float GetEdgeWeight(int from, int to)
        {
            if (adjacencyList.ContainsKey(from))
            {
                foreach (var (neighbor, weight) in adjacencyList[from])
                {
                    if (neighbor == to)
                    {
                        return weight;
                    }
                }
            }
            return float.MaxValue; // Return "infinite" weight if no edge exists
        }
    }

    public class TestDijkstra
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start Dijkstra Tests\n");

            // Create a simple graph
            var graph = new SimpleGraph();
            graph.AddEdge(1, 2, 1); // A -> B (weight 1)
            graph.AddEdge(1, 3, 4); // A -> C (weight 4)
            graph.AddEdge(2, 3, 2); // B -> C (weight 2)
            graph.AddEdge(2, 4, 6); // B -> D (weight 6)
            graph.AddEdge(3, 4, 3); // C -> D (weight 3)

            // Run tests
            TestShortestPath(graph);
            TestDisconnectedGraph();
            TestNegativeWeightGraph();
            TestSingleNodeGraph();
            TestNoPath();
            TestShortestPathAlternativeRoute();
            TestShortestPathCycle();

            Console.WriteLine("\nEnd Dijkstra Tests");
        }

        private static void TestShortestPath(SimpleGraph graph)
        {
            Console.WriteLine("Test 1: Shortest Path");
            var path = Dijkstra.GetPath(graph, 1, 4, graph.GetEdgeWeight);
            string expected = "1 -> 2 -> 3 -> 4";
            PrintResults(path, expected);
        }

        private static void TestShortestPathAlternativeRoute()
        {
            Console.WriteLine("Test 6: Shortest Path with Alternative Route");
            var graph = new SimpleGraph();
            graph.AddEdge(1, 2, 1);
            graph.AddEdge(1, 3, 5);
            graph.AddEdge(2, 3, 2);
            graph.AddEdge(3, 4, 1);
            graph.AddEdge(2, 4, 4);

            var path = Dijkstra.GetPath(graph, 1, 4, graph.GetEdgeWeight);
            string expected = "1 -> 2 -> 3 -> 4"; // Corrected expected path
            PrintResults(path, expected);
        }


        private static void TestShortestPathCycle()
        {
            Console.WriteLine("Test 7: Shortest Path with Cycle");
            var graph = new SimpleGraph();
            graph.AddEdge(1, 2, 1);
            graph.AddEdge(2, 3, 1);
            graph.AddEdge(3, 1, 2); // Cycle
            graph.AddEdge(3, 4, 1);

            var path = Dijkstra.GetPath(graph, 1, 4, graph.GetEdgeWeight);
            string expected = "1 -> 2 -> 3 -> 4"; // Shortest path avoids unnecessary cycles
            PrintResults(path, expected);
        }

        private static void TestDisconnectedGraph()
        {
            Console.WriteLine("Test 2: Disconnected Graph");
            var graph = new SimpleGraph();
            graph.AddEdge(1, 2, 5);
            graph.AddEdge(3, 4, 2);

            var path = Dijkstra.GetPath(graph, 1, 4, graph.GetEdgeWeight);
            string expected = ""; // No path exists
            PrintResults(path, expected);
        }

        private static void TestNegativeWeightGraph()
        {
            Console.WriteLine("Test 3: Graph with Negative Weights");
            var graph = new SimpleGraph();
            graph.AddEdge(1, 2, -5); // Dijkstra cannot handle negative weights properly
            graph.AddEdge(2, 3, 2);
            graph.AddEdge(1, 3, 10);

            var path = Dijkstra.GetPath(graph, 1, 3, graph.GetEdgeWeight);
            string expected = "1 -> 2 -> 3"; // In Dijkstra, behavior may be undefined with negative weights
            PrintResults(path, expected, warnNegativeWeights: true);
        }

        private static void TestSingleNodeGraph()
        {
            Console.WriteLine("Test 4: Single Node Graph");
            var graph = new SimpleGraph();
            graph.AddEdge(1, 1, 0); // A single node pointing to itself

            var path = Dijkstra.GetPath(graph, 1, 1, graph.GetEdgeWeight);
            string expected = "1";
            PrintResults(path, expected);
        }

        private static void TestNoPath()
        {
            Console.WriteLine("Test 5: No Path Between Nodes");
            var graph = new SimpleGraph();
            graph.AddEdge(1, 2, 3);

            var path = Dijkstra.GetPath(graph, 2, 1, graph.GetEdgeWeight); // No reverse edge
            string expected = ""; // No path exists
            PrintResults(path, expected);
        }

        private static void PrintResults(List<int> path, string expected, bool warnNegativeWeights = false)
        {
            string actual = string.Join(" -> ", path);
            Console.WriteLine("Expected: " + (string.IsNullOrEmpty(expected) ? "No path" : expected));
            Console.WriteLine("Actual:   " + (string.IsNullOrEmpty(actual) ? "No path" : actual));
            if (warnNegativeWeights)
            {
                Console.WriteLine("Warning: Dijkstra may behave unpredictably with negative weights.");
            }
            Console.WriteLine(path.SequenceEqual(ParsePath(expected)) ? "Test Passed" : "Test Failed");
            Console.WriteLine();
        }

        private static List<int> ParsePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return new List<int>();
            return new List<int>(Array.ConvertAll(path.Split(" -> "), int.Parse));
        }
    }
}
