using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour 
{
    //X is number of columns. Y is number of rows.
    [SerializeField] int xCellCount = 100;
    [SerializeField] int zCellCount = 100;
    float heightAboveSeaLevel = 3;
    [SerializeField] GameObject cell;
    List<Cell[]> cells = new List<Cell[]>();

    //pathfinding
    float pathfindRecalcTimer = 0f;

    void Start()
    {
        // Initialize the local cell holding List
        var cellList = new List<List<Cell>>();

        // Create the grid
        for(int i = 0; i < xCellCount; i++)
        {
            cellList.Add(new List<Cell>());
            for(int j = 0; j < zCellCount; j++)
            {
                GameObject cellGO = Instantiate(cell, new Vector3(i, heightAboveSeaLevel, j), Quaternion.identity);
                cellGO.transform.SetParent(gameObject.transform);
                var cellScript = cellGO.GetComponent<Cell>();
                cellList[i].Add(cellScript);
            }
        }

        // Initialize the global list
        for (int i = 0; i < xCellCount; i++)
        {
            cells.Add(new Cell[zCellCount]);
        }
        // Fill the global list
        for(int i = 0; i < cellList.Count; i++)
        {
            cells[i] = cellList[i].ToArray();
        }
    }

    void FixedUpdate()
    {
        pathfindRecalcTimer += Time.deltaTime;
        if(pathfindRecalcTimer > 1f)
        {
            pathfindRecalcTimer = 0f;
            CalculatePath();
        }
    }

    void CalculatePath()
    {
        foreach(var list in cells)
        {
            foreach(var cell in list)
            {
                // Randomness to test
                var dir = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));                
                while(dir == Vector2Int.zero)
                {
                    dir = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
                }
                PassPathToCell(cell, dir);
            }
        }
    }

    void PassPathToCell(Cell cell, Vector2Int direction)
    {
        cell.pathDirection = direction;
    }
}
