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
    bool insideRadius = false;
    public int distToTarget;
    public int stepsToTarget = 999;

    public Cell(Grid gridMap, Vector2Int value, CellViz cellVisual) : base(value)
    {
        grid = gridMap;
        isWalkable = true;
        cellViz = cellVisual;
    }

    // Called on every path update
    public void SetClosestTarget()
    {
        if (!isWalkable)
        {
            pathDirection = Vector2Int.zero;
            return;
        }
        if (targets == null || targets.Count == 0)
        {
            // If there is no target, set to null/zero so mobs only move randomly
            // And don't continue the chain to save processing power
            closestTargetCell = null;
            pathDirection = Vector2Int.zero;
            return;
        }
        
        //clear out closest target to re-check
        closestTargetCell = null;
        
        //Get the closest target - order the list by closest one to this cell
        // May need to refactor this to be based on steps
        targets = targets.OrderBy(
                            targ => targ.CompareTo(this)).ToList();
        var closestTarget = targets.First();

        var cell = grid.GetTargetCell(closestTarget, false);
        closestTargetCell = cell;
        

        // now if this cell is inside the Target.targetRadius,
        // pick another random cell inside the radius as target cell
        if (CompareTo(cell) < closestTarget.targetRadius)
        {
            insideRadius = true;
            cellViz.inRadius = true;
            cell = grid.GetTargetCell(closestTarget, true);
        }
        else {
            insideRadius = false;
            cellViz.inRadius = false;
        }
        SelectMoveToTarget(cell);
    }

    public void SelectMoveToTarget(Cell cellGoal)
    {
        if (moveToCells.Count == 0)
        {
            return;
        }
        // Determine which cell in moveTo list is closest to target in total Steps
        // and select that one as the goal
        // MoveToCells is added to first by PathPlotting for target cell, then here for addl cells
        // Also order secondly by total distance to target - So a cell has to have both lower steps
        // AND be closer to the target than it's co-neighbors of this cell
        var sortedCells = moveToCells.OrderBy(x => x.stepsToTarget)
                    .OrderBy(x=> x.CompareTo(this.closestTargetCell)).ToList();
        stepsToTarget = sortedCells.First().stepsToTarget + 1;

        //Check if we are in the radius and just need to move randomly inside
        // Only closer cells are added to moveToCells, so a random one probably isn't contained
        if ( insideRadius)
        {
            try
            {
                var normalizedVector = new Vector2(cellGoal.Value.x - Value.x,
                                                cellGoal.Value.y - Value.y).normalized;
                pathDirection = new Vector2Int(
                            Mathf.RoundToInt(normalizedVector.x)
                            , Mathf.RoundToInt((int)normalizedVector.y));
            }
            catch (Exception e) { Debug.Log(e); }
        }
        else // Otherwise just move normally
        {
            pathDirection = sortedCells.First().Value - Value;
        }

        // Fill the visual stuff
        cellViz.moveToCells.Clear();
        cellViz.AddMoveToLocations();

        AddNeighborMoveTos();
        moveToCells.Clear();
    }
    void AddNeighborMoveTos()
    {
        // Now tell all neighbor cells that this is a possible cell they can move to
        var neighbors = GetNeighborCells(true);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.isWalkable)
            {
                neighbor.moveToCells.Add(this);
            }            
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
        // Use a step counter that incriments every step to determine distance from target
        foreach(var cell in neighs)
        {
            if(cell.closestTargetCell == this.closestTargetCell
                && ( cell.stepsToTarget > this.stepsToTarget))
            {
                moveFromCells.Add(cell);
            }
        }
        return moveFromCells;
    }
    public int CompareTo(Cell other)
    {
        if(other == null)
        {
            return 9999;
        }
        return Mathf.Abs(Value.x - other.Value.x) + Mathf.Abs(Value.y - other.Value.y);
    }
    public int CompareToVector2(Vector2Int vector)
    {
        return Mathf.Abs(Value.x - vector.x) + Mathf.Abs(Value.y - vector.y);
    }
    
    //for use when a unit ends up on a non-walkable cell. Get them off quickly
    public Vector2Int GetWalkablePathDirection()
    {
        return grid.GetNearestWalkableCell(this, Vector2Int.zero, false).Value - Value;
    }
}
