using System.Collections.Generic;
using System.Linq;

/**
 * A generic implementation of the Dijkstra algorithm for weighted graphs.
 * @author Vivian Umansky
 * @since 2023-12
 */
public class Dijkstra
{
    public static List<NodeType> GetPath<NodeType>(
        IGraph<NodeType> graph,
        NodeType startNode,
        NodeType endNode,
        System.Func<NodeType, NodeType, float> getEdgeWeight,
        int maxIterations = 1000
    ) where NodeType : notnull // Ensure NodeType is non-nullable
    {
        var distances = new Dictionary<NodeType, float>();
        var previousNodes = new Dictionary<NodeType, NodeType>();
        var visited = new HashSet<NodeType>();
        var priorityQueue = new SortedSet<(float, NodeType)>(Comparer<(float, NodeType)>.Create((a, b) =>
            a.Item1 != b.Item1 ? a.Item1.CompareTo(b.Item1) : Comparer<NodeType>.Default.Compare(a.Item2, b.Item2)));

        // Initialize distances and add the start node to the queue
        distances[startNode] = 0;
        priorityQueue.Add((0, startNode));

        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            if (priorityQueue.Count == 0)
                break;

            var (currentDistance, currentNode) = priorityQueue.First();
            priorityQueue.Remove(priorityQueue.First());

            if (visited.Contains(currentNode))
                continue;

            visited.Add(currentNode);

            if (currentNode.Equals(endNode))
                break;

            foreach (var neighbor in graph.Neighbors(currentNode))
            {
                if (visited.Contains(neighbor))
                    continue;

                float edgeWeight = getEdgeWeight(currentNode, neighbor);
                float newDistance = currentDistance + edgeWeight;

                if (!distances.ContainsKey(neighbor) || newDistance < distances[neighbor])
                {
                    if (distances.ContainsKey(neighbor))
                    {
                        priorityQueue.Remove((distances[neighbor], neighbor));
                    }
                    distances[neighbor] = newDistance;
                    previousNodes[neighbor] = currentNode!;
                    priorityQueue.Add((newDistance, neighbor));
                }
            }
        }

        // Construct the path
        var path = new List<NodeType>();
        if (previousNodes.ContainsKey(endNode) || startNode.Equals(endNode))
        {
            for (var current = endNode; current != null; current = previousNodes.ContainsKey(current) ? previousNodes[current] : default(NodeType))
            {
                path.Add(current);
                if (current.Equals(startNode))
                    break;
            }
            path.Reverse();
        }

        return path;
    }
}
