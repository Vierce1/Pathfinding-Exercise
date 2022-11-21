using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulsor : MonoBehaviour
{
    // Stick this on any repulsor and adjust the strength in inspector
    [SerializeField] float repulseStrength = 1f;
    Grid grid;
    GameHandler gameHandler;
    [SerializeField] float maxLifeTime = -1f;
    float burnoutTimer = 0f;
    List<CellViz> cellsTouching = new List<CellViz>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Cell")
        {
            return;
        }
        var cell = other.gameObject.GetComponent<CellViz>();
        cell.cell.stepsToTarget = 999;
        cellsTouching.Add(cell);
        grid.ToggleWalkable(cell.cell, false);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Cell")
        {
            return;
        }
        var cell = other.gameObject.GetComponent<CellViz>();
        cellsTouching.Remove(cell);
        grid.ToggleWalkable(cell.cell, true);
    }

    void FixedUpdate()
    {
        if(maxLifeTime > -1 && gameHandler.levelPlaying)
        {
            burnoutTimer += Time.deltaTime;
            if(burnoutTimer > maxLifeTime)
            {
                Destroy(GetComponentInChildren<Light>());                
                cellsTouching.ForEach(cell => grid.ToggleWalkable(cell.cell, true));
                cellsTouching.ForEach(cell => cell.cell.stepsToTarget = 999);
                cellsTouching.Clear();                
                GetComponent<SphereCollider>().enabled = false;
            }
        }
    }

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        gameHandler = FindObjectOfType<GameHandler>();
    }
}
