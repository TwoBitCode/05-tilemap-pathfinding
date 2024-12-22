using System;
using System.Collections.Generic;

public class Dijkstra
{
    /**
     * A generic implementation of the Dijkstra algorithm for weighted graphs.
     * @param graph The graph implementing the IGraph interface.
     * @param startNode The starting node.
     * @param endNode The target node.
     * @param getEdgeWeight A function to get the weight of an edge between two nodes.
     * @param maxIterations The maximum number of iterations before giving up.
     * @return A list of nodes representing the shortest path, or an empty list if no path exists.
     */
    public static List<NodeType> GetPath<NodeType>(
        IGraph<NodeType> graph,
        NodeType startNode,
        NodeType endNode,
        Func<NodeType, NodeType, float> getEdgeWeight,
        int maxIterations = 1000
    ) where NodeType : notnull
    {
        var distances = new Dictionary<NodeType, float>();
        var previousNodes = new Dictionary<NodeType, NodeType>();
        var visited = new HashSet<NodeType>();
        var priorityQueue = new SortedSet<(float, NodeType)>(
           Comparer<(float, NodeType)>.Create((a, b) =>
               a.Item1 != b.Item1
                   ? a.Item1.CompareTo(b.Item1)
                   : a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode()))
       );


        // Initialize distances and priority queue
        distances[startNode] = 0;
        priorityQueue.Add((0, startNode));

        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            if (priorityQueue.Count == 0) break;

            var (currentDistance, currentNode) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);

            if (visited.Contains(currentNode)) continue;

            visited.Add(currentNode);

            if (currentNode.Equals(endNode)) break;

            foreach (var neighbor in graph.Neighbors(currentNode))
            {
                if (visited.Contains(neighbor)) continue;

                float edgeWeight = getEdgeWeight(currentNode, neighbor);
                if (edgeWeight == float.MaxValue) continue; // Ignore unreachable tiles

                float newDistance = currentDistance + edgeWeight;

                if (!distances.ContainsKey(neighbor) || newDistance < distances[neighbor])
                {
                    if (distances.ContainsKey(neighbor))
                    {
                        priorityQueue.Remove((distances[neighbor], neighbor));
                    }

                    distances[neighbor] = newDistance;
                    previousNodes[neighbor] = currentNode;
                    priorityQueue.Add((newDistance, neighbor));
                }
            }
        }

        // Reconstruct the shortest path
        var path = new List<NodeType>();
        if (previousNodes.ContainsKey(endNode) || startNode.Equals(endNode))
        {
            for (var current = endNode; current != null; current = previousNodes.GetValueOrDefault(current))
            {
                path.Add(current);
                if (current.Equals(startNode)) break;
            }
            path.Reverse();
        }

        return path;
    }
}