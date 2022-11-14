using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Grid passes pathDirection which denotes which cell a unit on this cell should move to
    // e.g., (0, 1) means move to the cell directly above (positive Z value)
    public Vector2Int pathDirection;


    void OnTriggerEnter(Collider other)
    {
        // On trigger enter, gameobject is passed the cell to move to
        var mob = other.gameObject.GetComponent<Mob>();
        mob.UpdateMoveDirection(pathDirection);
    }
}
