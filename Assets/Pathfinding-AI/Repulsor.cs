using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulsor : MonoBehaviour
{
    // Stick this on any repulsor and adjust the strength in inspector
    [SerializeField] float repulseStrength = 1f;
    Grid grid;
    GameHandler gameHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Cell")
        {
            return;
        }
        var cell = other.gameObject.GetComponent<CellViz>();
        grid.ToggleWalkable(cell.cell, false);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Cell")
        {
            return;
        }
        var cell = other.gameObject.GetComponent<CellViz>();
        grid.ToggleWalkable(cell.cell, true);
    }

    void Update()
    {

    }

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        gameHandler = FindObjectOfType<GameHandler>();
    }
}
