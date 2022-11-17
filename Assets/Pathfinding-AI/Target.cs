using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Target : MonoBehaviour, IComparable<Cell>
{
    // targetsize is the radius around central cell that cells
    // will mark as a target. They will pick a random one each frame
    // if they are inside the radius
    public int targetRadius = 2;
    public bool isClickable = false;
    [SerializeField] bool isPlayer = false;

    //[SerializeField] float targetColliderSize = 15f;

    private void Start()
    {
        if (GetComponent<Hero>() == null && !isClickable)
        {
            GetComponentInChildren<Dragger>().enabled = false;
            GetComponentInChildren<EventTrigger>().enabled = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Cell")
        {
            return;
        }
        var cell = other.gameObject.GetComponent<CellViz>();
        if(isPlayer && cell.affectedByNonPlayerTarget)
        {
            return;
        }
        cell.cell.targets.Add(this);
        if (!isPlayer)
        {
            cell.affectedByNonPlayerTarget = true;
        }
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

    public int CompareTo(Cell other)
    {
        if(other == null)
        {
            return 9999;
        }
        return 
            Mathf.Abs(Mathf.RoundToInt(transform.position.x) - other.Value.x
            + Mathf.Abs(Mathf.RoundToInt(transform.position.z) - other.Value.y));
    }
}
