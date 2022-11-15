using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // attraction force = how close the target keeps the units
    // when they arrive. Lower means more random movement
    public float attractionForce { get; private set; }

    [SerializeField] float targetColliderSize = 15f;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Cell")
        {
            return;
        }
        var cell = other.gameObject.GetComponent<CellViz>();
        cell.cell.targets.Add(this);
        cell.cell.SetClosestTarget();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Cell")
        {
            return;
        }
        var cell = other.gameObject.GetComponent<CellViz>();
        if (cell.cell.targets.Contains(this))
        { 
            cell.cell.targets.Remove(this);
        }

        cell.cell.SetClosestTarget();
    }
}
