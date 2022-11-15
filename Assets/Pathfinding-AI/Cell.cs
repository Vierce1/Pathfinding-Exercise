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
    public bool pathfindRunning = false;

    public List<Cell> moveToCells = new List<Cell>();

    public Cell(Grid gridMap, Vector2Int value, CellViz cellVisual) : base(value)
    {
        grid = gridMap;
        isWalkable = true;
        cellViz = cellVisual;
    }

    //public override List<Node<Vector2Int>> GetNeighbors()
    //{
    //    return grid.GetNeighborCells(Value);
    //}

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
            if(cell.CompareTo(grid.targetLocation) >
                CompareTo(grid.targetLocation) )
            {
                moveFromCells.Add(cell);
            }
        }
        return moveFromCells;
    }

    public void SelectMoveToTarget()
    {
        if(moveToCells.Count == 0)
        {
            return;
        }
        // Determine which cell in moveTo list is closest to target and select that one as the goal
        moveToCells = moveToCells.OrderBy(x => x.CompareTo(grid.targetLocation)).ToList();
        pathDirection = moveToCells.First().Value - Value;
        AddNeighborMoveTos();
        moveToCells.Clear();
    }
    void AddNeighborMoveTos()
    {
        // Now tell all neighbor cells to move to this cell
        var neighbors = GetNeighborCells(true);
        foreach (var neighbor in neighbors)
        {
            //Already check for walkable in grid.GetNeighborCells
            //if (neighbor.isWalkable)
            //{
                neighbor.moveToCells.Add(this);
                //neighbor.SelectMoveToTarget();
            //}
        }
    }

    public int CompareTo(Cell other)
    {
        return Mathf.Abs(Value.x - other.Value.x) + Mathf.Abs(Value.y - other.Value.y);
    }
}
