using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class Grid : MonoBehaviour
{
    GameHandler gameHandler;
    //X is number of columns. Y is number of rows.
    public int xCellCount = 100;
    public int zCellCount = 100;
    float heightAboveSeaLevel = 3;
    [SerializeField] GameObject cell;
    public List<Target> targetList = new List<Target>();
    public List<Cell[]> cells { get; private set; }
    public List<Cell> cellList = new List<Cell>();
    float targetRecalcTimer = 1f;
    float targetRecalcTriggerTime = 0.15f;

    public void BuildGrid()
    {
        gameHandler = gameHandler == null ? FindObjectOfType<GameHandler>() : gameHandler;
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

        // Find all initial targets. Can add more later
        var targGOs = GameObject.FindGameObjectsWithTag("Target").ToList();
        targGOs.ForEach(x => targetList.Add(x.GetComponent<Target>()));

        // Initial plot - Call it again as targets change or over time
        PlotPaths();
    }
    public void DestroyGrid()
    {
        for(int i = cells.Count - 1; i >= 0; i--)
        {
            var cellArray = cells[i];
            for(int j = cellArray.Length - 1; j >= 0; j--)
            {
                var cell = cellArray[j];
                Destroy(cell.cellViz.gameObject);
            }    
        }
        cells.Clear();
    }
    void FixedUpdate()
    {
        targetRecalcTimer += Time.deltaTime;
        // Update cell targets every x seconds
        if (targetRecalcTimer > targetRecalcTriggerTime)
        {
            targetRecalcTimer = 0;
            PlotPaths();
            ProcessCells();
            gameHandler.UpdateMobMoveTargets();
        }

    }

    //Calculate path for every cell. Called every path update
    void PlotPaths()
    {
        if (gameHandler.beatLevel)
        {
            return;
        }
        // sort the list of cells to get the closest cells for each target
        // only get the central cell of the targets for now 
        foreach(var target in targetList)
        {
            var targetPos = GetTargetCell(target, false);
            var sortedList = cellList.OrderBy(x => x.CompareTo(targetPos)).ToList();
            PathPlotting plot = new PathPlotting();
            plot.CalculatePath(this, sortedList.First());
        }
    }
    void ProcessCells()
    {
        for (int i = 0; i < cellList.Count; i++)
        {
            // Only need to set the closest target every time we reprocess
            cellList[i].SetClosestTarget();
        }
    }

    public void PassPathToCell(Cell cell, Vector2Int direction)
    {
        cell.pathDirection = direction;
    }

    #region get neighbors
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
    #endregion

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
        try
        {
            if (x >= 0 && x < xCellCount && z >= 0 && z < zCellCount)
            {
                return cells[x][z];
            }
        }
        catch { return null; }
        return null;
    }
    public Cell GetTargetCell(Target target, bool randomInsideRadius)
    {
        try
        {
            var cell = GetCell(
                Mathf.RoundToInt(target.transform.position.x),
                Mathf.RoundToInt(target.transform.position.z));

            //if asking for a random in radius cell, roll it
            if (randomInsideRadius)
            {
                var dist = target.targetRadius;
                var targetPos = new Vector3Int(
                    Mathf.RoundToInt(target.transform.position.x), 0
                    , Mathf.RoundToInt(target.transform.position.z));

                if (cell.isWalkable)
                {
                    cell = GetCell(targetPos.x + Random.Range(-dist, dist + 1)
                        , targetPos.z + Random.Range(-dist, dist + 1));
                }
            }

            if (cell == null)
            {
                cell = GetCell(
                        Mathf.RoundToInt(target.transform.position.x),
                        Mathf.RoundToInt(target.transform.position.z));

            }
            return cell;
        }
        catch { }
        return null;
    }

    public Cell GetNearestWalkableCell(Cell fromCell, Vector2Int mobPos, bool isMob)
    {
        //If don't pass in a cell just iterate through all cells
        if(isMob)
        {
            var list = cellList;            
            return list.OrderBy(cell => cell.CompareToVector2(mobPos)).First();
        }

        var cells = new List<Cell>();
        //iterate through nearby cells
        for (int i = fromCell.Value.x - 50; i < fromCell.Value.x + 50; i++)
        {
            if(i < 0 || i >= xCellCount)
            {
                continue;
            }
            for (int j = fromCell.Value.y - 50; j < fromCell.Value.y + 50; j++)
            {
                if (j < 0 || j >= zCellCount)
                {
                    continue;
                }
                var cell = GetCell(i, j);
                if (cell.isWalkable)
                {
                    cells.Add(cell);
                }
            }
        }
        try
        {
            cells = cells.OrderBy(cell => cell.CompareTo(fromCell)).ToList();
            return cells.First();
        }
        catch { return null; }
    }
}
