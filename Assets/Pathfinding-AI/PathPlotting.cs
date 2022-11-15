using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlotting
{
    List<Vector2Int> waypoints = new List<Vector2Int>();
    Grid grid;
    Cell cell;

    public void CalculatePath(Grid gridMap, Cell gridCell)
    {
        cell = gridCell;
        grid = gridMap;       

        // Get neighboring cells for the end target, and if they are walkable,
        // add this end target cell to their moveTo list
        var neighbors = cell.GetNeighborCells(false);

        foreach (var neighbor in neighbors)
        {
            neighbor.moveToCells.Add(cell);
            neighbor.SelectMoveToTarget();
        }
    }
    public void AddWayPoint(Vector2Int pt)
    {
        waypoints.Add(pt);
    }

}
