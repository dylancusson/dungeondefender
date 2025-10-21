using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node
{
    public Vector2Int gridPosition;
    public bool isWalkable;
    public int gCost; // Cost from start node
    public int hCost; // Heuristic cost to end node
    public int fCost => gCost + hCost; // Total cost
    public Node parent; // Node we came from to reach this node

    public Node(bool walkable, Vector2Int pos)
    {
        isWalkable = walkable;
        gridPosition = pos;
    }
}
public class GridManager : MonoBehaviour
{
    public Tilemap walkableTilemap; // Assign in inspector
    public Tilemap wallTilemap;     // Assign in inspector
    public int width;
    public int height;
    private Node[,] grid;

    void Awake()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[width, height];
        BoundsInt bounds = walkableTilemap.cellBounds;

        bool[,] rawWalls = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Get Tilemap cell coordinates
                Vector3Int cellPos = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);

                // Check walls
                bool isWall = wallTilemap.HasTile(cellPos);

                // If tile is present and not a wall, it's walkable
                bool walkable = !isWall;

                // Grid of walkable nodes
                grid[x, y] = new Node(walkable, new Vector2Int(x, y));
                rawWalls[x, y] = isWall;
            }
        }
        int agentRadius = 2;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (rawWalls[x, y]) // If the current tile is a raw wall, we skip it
                    continue;

                // Check the surrounding area (the 'agentRadius' around the current node)
                for (int checkX = x - agentRadius; checkX <= x + agentRadius; checkX++)
                {
                    for (int checkY = y - agentRadius; checkY <= y + agentRadius; checkY++)
                    {
                        // Ensure the check indices are within the grid bounds
                        if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                        {
                            // If any surrounding tile is a raw obstacle, this node is too close to a wall
                            if (rawWalls[checkX, checkY])
                            {
                                grid[x, y].isWalkable = false; // Mark this node as UNWALKABLE
                                goto NextNode; // Jump out of both inner loops (optimization)
                            }
                        }
                    }
                }
            NextNode:; // Label for the goto
            }
        }
    }

    public Node GetNodeFromWorldPos(Vector3 worldPos)
    {
        // Convert from world pos to cell pos then get grid coordinates
        Vector3Int cellPos = walkableTilemap.WorldToCell(worldPos);
        int x = cellPos.x - walkableTilemap.cellBounds.xMin;
        int y = cellPos.y - walkableTilemap.cellBounds.yMin;

        // Make sure coordinates are within grid bounds
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return grid[x, y];
        }
        // If out of bounds, return null
        return null;
    }

    // Get neighboring nodes for pathfinding
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector2Int pos = node.gridPosition;

        // Check all 4 cardinal directions
        int[] checkX = { 0, 0, 1, -1 };
        int[] checkY = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int checkPosX = pos.x + checkX[i];
            int checkPosY = pos.y + checkY[i];
            if (checkPosX >= 0 && checkPosX < width && checkPosY >= 0 && checkPosY < height)
            {
                neighbors.Add(grid[checkPosX, checkPosY]);
            }
        }
        return neighbors;
    }
}
