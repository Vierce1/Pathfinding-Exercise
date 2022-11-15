using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class Grid : MonoBehaviour
{
    //X is number of columns. Y is number of rows.
    [SerializeField] int xCellCount = 100;
    [SerializeField] int zCellCount = 100;
    float heightAboveSeaLevel = 3;
    [SerializeField] GameObject cell;
    public List<Cell[]> cells { get; private set; }
    public List<Cell> cellList = new List<Cell>();
    float targetRecalcTimer = 4f;
    float targetRecalcTriggerTime = 1f;

    public Cell targetLocation;

    void Start()
    {
        cells = new List<Cell[]>();
        // Initialize the local cell holding List
        var cellInitialList = new List<List<Cell>>();

        // Create the grid
        for (int i = 0; i < xCellCount; i++)
        {
            cellInitialList.Add(new List<Cell>());
            for (int j = 0; j < zCellCount; j++)
            {
                GameObject cellGO = Instantiate(cell, new Vector3(i, heightAboveSeaLevel, j), Quaternion.identity);
                cellGO.transform.SetParent(gameObject.transform);
                var cellViz = cellGO.GetComponent<CellViz>();
                var cellScript = new Cell(this, new Vector2Int(i, j), cellViz);
                cellViz.cell = cellScript;
                cellInitialList[i].Add(cellScript);

                cellList.Add(cellScript);

                //TESTING
                //if (Random.Range(0, 3) == 2)
                //{
                //    ToggleWalkable(cellScript, false);
                //}
            }
        }

        // Initialize the global list
        for (int i = 0; i < xCellCount; i++)
        {
            cells.Add(new Cell[zCellCount]);
        }
        // Fill the global list
        for (int i = 0; i < cellInitialList.Count; i++)
        {
            cells[i] = cellInitialList[i].ToArray();
        }

        targetLocation = cellList[0];
        // Initial plot - Call it again as targets change or over time
        StartCoroutine(PlotPaths());
    }

    void FixedUpdate()
    {
        targetRecalcTimer += Time.deltaTime;
        // Update cell targets every x seconds
        if (targetRecalcTimer > targetRecalcTriggerTime)
        {
            targetRecalcTimer = 0; 
            ProcessCells();
        }

    }

    //Calculate path for every cell
    IEnumerator PlotPaths()
    {
        var sortedList = cellList.OrderBy(x => x.CompareTo(targetLocation)).ToList();
        PathPlotting plot = new PathPlotting();
        plot.CalculatePath(this, sortedList.First());
        yield return null;
    }
    void ProcessCells()
    {
        foreach(var cell in cellList)
        {
            cell.SelectMoveToTarget();
        }
    }

    public void PassPathToCell(Cell cell, Vector2Int direction)
    {
        cell.pathDirection = direction;
    }

    public List<Cell> GetNeighborCells(Vector2Int location)
    {
        List<Cell> neighbors = new List<Cell> { };
        int x = location.x;
        int z = location.y;

        //check Up
        if (z < zCellCount - 1)
        {
            int i = x;
            int j = z + 1;

            if (cells[i][j].isWalkable)
            {
                neighbors.Add(cells[i][j]);
            }
        }


        //check Top-Right
        if (z < zCellCount - 1 && x < xCellCount - 1)
        {
            int i = x + 1;
            int j = z + 1;
            if (cells[i][j].isWalkable)
            {
                neighbors.Add(cells[i][j]);
            }
        }

        //check right
        if (x < xCellCount - 1)
        {
            int i = x + 1;
            int j = z;
            if (cells[i][j].isWalkable)
            {
                neighbors.Add(cells[i][j]);
            }
        }

        ////check right-down
        if (x < xCellCount - 1 && z > 0)
        {
            int i = x + 1;
            int j = z - 1;
            if (cells[i][j].isWalkable)
            {
                neighbors.Add(cells[i][j]);
            }
        }

        //check down
        if (z > 0)
        {
            int i = x;
            int j = z - 1;
            if (cells[i][j].isWalkable)
            {
                neighbors.Add(cells[i][j]);
            }
        }

        //check down left
        if (x > 0 && z > 0)
        {
            int i = x - 1;
            int j = z - 1;
            if (cells[i][j].isWalkable)
            {
                neighbors.Add(cells[i][j]);
            }
        }

        //check left
        if (x > 0)
        {
            int i = x - 1;
            int j = z;
            if (cells[i][j].isWalkable)
            {
                neighbors.Add(cells[i][j]);
            }
        }

        //check left up
        if (x > 0 && z < zCellCount - 1)
        {
            int i = x - 1;
            int j = z + 1;
            if (cells[i][j].isWalkable)
            {
                neighbors.Add(cells[i][j]);
            }
        }

        return neighbors;
        //this returns all possible neighbors, except those that are marked unwalkable
    }

    public void ToggleWalkable(Cell cell, bool makeWalkable)
    {
        if (cell == null) { Debug.Log("Cell null"); return; }

        cell.isWalkable = makeWalkable;

        //change the visual color of the cell to show isWalkable state
        //comment this out for runtime
        cell.cellViz.SetColor(cell.isWalkable);
    }
    public Cell GetCell(int x, int z)
    {        
        if (x >= 0 && x < xCellCount && z >= 0 && z < zCellCount)
        {
            return cells[x][z];
        }
        return null;
    }
}
