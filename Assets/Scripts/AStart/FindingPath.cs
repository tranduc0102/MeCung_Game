using System.Collections.Generic;
using UnityEngine;

public class FindingPath : MonoBehaviour
{
    public static List<Tile> FindPath(Tile startTile, List<Tile> targetTiles)
    {
        List<Tile> openList = new List<Tile>();
        HashSet<Tile> closedList = new HashSet<Tile>();
        openList.Add(startTile);

        // Initialize gCost for the startTile
        startTile.gCost = 0;
        startTile.hCost = CalculateHeuristic(startTile, targetTiles);
        startTile.parent = null;

        while (openList.Count > 0)
        {
            Tile currentTile = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentTile.FCost ||
                    Mathf.Approximately(openList[i].FCost, currentTile.FCost) && openList[i].hCost < currentTile.hCost)
                {
                    currentTile = openList[i];
                }
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (targetTiles.Contains(currentTile))
            {
                return RetracePath(startTile, currentTile);
            }

            foreach (Tile neighbor in currentTile.Neighbors)
            {
                if (closedList.Contains(neighbor))
                {
                    continue;
                }

                float newMovementCostToNeighbor = currentTile.gCost + GetDistance(currentTile, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = CalculateHeuristic(neighbor, targetTiles);
                    neighbor.parent = currentTile;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        // No path found
        return null;
    }

    private static List<Tile> RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }
        path.Reverse();

        return path;
    }

    private static float GetDistance(Tile tileA, Tile tileB)
    {
        // Using Manhattan Distance for a grid-based movement
        return Mathf.Abs(tileA.transform.position.x - tileB.transform.position.x) +
               Mathf.Abs(tileA.transform.position.y - tileB.transform.position.y);
    }

    private static float CalculateHeuristic(Tile tile, List<Tile> targetTiles)
    {
        // Choose the minimum distance to any target tile as the heuristic
        float closestDistance = Mathf.Infinity;

        foreach (Tile targetTile in targetTiles)
        {
            float distance = GetDistance(tile, targetTile);
            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }

        return closestDistance;
    }
}
