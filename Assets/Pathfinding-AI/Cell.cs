using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;
using System;

public class Cell : Node<Vector2Int>, IComparable<Cell>
{
    // Grid passes pathDirection which denotes which cell a unit on this cell should move to
    // e.g., (0, 1) means move to the cell directly above (positive Z value)
    public Vector2Int pathDirection;
    public bool isWalkable { get; set; }
    Grid grid;
    public CellViz cellViz;

    public List<Cell> moveToCells = new List<Cell>();
    public List<Target> targets = new List<Target>();
    public Cell closestTargetCell { get; private set; }

    public Cell(Grid gridMap, Vector2Int value, CellViz cellVisual) : base(value)
    {
        grid = gridMap;
        isWalkable = true;
        cellViz = cellVisual;
    }

    // Called by Grid on every path update
    public void SelectMoveToTarget()
    {
        if (moveToCells.Count == 0)
        {
            return;
        }
        // Determine which cell in moveTo list is closest to target and select that one as the goal
        moveToCells = moveToCells.OrderBy(x => x.CompareTo(closestTargetCell)).ToList();
        pathDirection = moveToCells.First().Value - Value;
        AddNeighborMoveTos();

        //cellViz.moveToCells.Clear();
        //cellViz.AddMoveToLocations();

        moveToCells.Clear();
    }
    void AddNeighborMoveTos()
    {
        // Now tell all neighbor cells to move to this cell
        var neighbors = GetNeighborCells(true);
        foreach (var neighbor in neighbors)
        {
            neighbor.moveToCells.Add(this);
        }
    }
    public List<Cell> GetNeighborCells(bool onlyFartherCells)
    {
        var neighs = grid.GetNeighborCells(Value);
        if (!onlyFartherCells)
        {
            return neighs;
        }

        var moveFromCells = new List<Cell>();
        // Check each neighbor cell. Only add ones that are 
        // Farther away from the target than this one
        foreach(var cell in neighs)
        {
            if(cell.CompareTo(closestTargetCell) > this.CompareTo(closestTargetCell) 
                //&& cell.closestTargetCell == this.closestTargetCell
                )
            {
                moveFromCells.Add(cell);
            }
        }
        return moveFromCells;
    }


    public void SetClosestTarget()
    {
        if(targets == null || targets.Count == 0)
        {
            //If target leaves area, set to null/zero so mobs only move randomly
            closestTargetCell = null;
            pathDirection = Vector2Int.zero;
            return;
        }
        var closestTarget = targets.OrderBy(
            targ => targ.transform.position - cellViz.transform.position).First();
        closestTargetCell = grid.GetTargetCell(closestTarget);
    }

    public int CompareTo(Cell other)
    {
        if(other == null)
        {
            return 9999;
        }
        return Mathf.Abs(Value.x - other.Value.x) + Mathf.Abs(Value.y - other.Value.y);
    }
}
