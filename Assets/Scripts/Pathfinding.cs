using UnityEngine;
using System.Collections.Generic;
public class Pathfinding : MonoBehaviour
{
    public GridManager gridManager; // Assign in inspector

    public List<Node> FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
    {
        Node startNode = gridManager.GetNodeFromWorldPos(startWorldPos);
        Node targetNode = gridManager.GetNodeFromWorldPos(targetWorldPos);

        if (startNode == null || targetNode == null || !targetNode.isWalkable)
        {
            Debug.LogWarning("Invalid start or target node!");
            return null; // Invalid start or target
        }
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // Find node with lowest fCost
            Node currentNode = openSet[0];

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }
            // Add lowest cost node to closed set
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Path found
            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }
            // Check neighbors
            foreach (Node neighbor in gridManager.GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                {
                    continue; // Skip non-walkable or already evaluated nodes
                }
                int newMovementCostToNeighbor = currentNode.gCost + 1; // Add 1 since all moves cost the same
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null; // No path found
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    int GetDistance(Node a, Node b)
    {
        return Mathf.Abs(a.gridPosition.x - b.gridPosition.x) + Mathf.Abs(a.gridPosition.y - b.gridPosition.y);
    }
}
