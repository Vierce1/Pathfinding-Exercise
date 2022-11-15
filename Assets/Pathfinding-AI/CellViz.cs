using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellViz : MonoBehaviour
{
    public Cell cell;
    [SerializeField] Vector2Int pathDirection;
    [SerializeField] Material nonWalkableMaterial;
    [SerializeField] Material walkableMaterial;

    void OnTriggerEnter(Collider other)
    {
        // On trigger enter, gameobject is passed the cell to move to
        var mob = other.gameObject.GetComponent<Mob>();
        mob.UpdateMoveDirection(cell.pathDirection);
    }
    public void SetColor(bool isWalkable)
    {
        GetComponent<MeshRenderer>().material = isWalkable ? walkableMaterial : nonWalkableMaterial;
    }
    private void Update()
    {
        pathDirection = cell.pathDirection;
    }
}
