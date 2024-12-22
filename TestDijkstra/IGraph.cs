using System.Collections.Generic;

/**
 * Interface representing a graph with nodes of type NodeType.
 * Used for generic graph traversal algorithms like BFS and Dijkstra.
 */
public interface IGraph<NodeType>
{
    // Returns the neighbors of a given node
    IEnumerable<NodeType> Neighbors(NodeType node);
}
