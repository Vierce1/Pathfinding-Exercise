using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellViz : MonoBehaviour
{
    public Cell cell;
    public bool affectedByNonPlayerTarget = false;
    [SerializeField] Vector2Int pathDirection;
    [SerializeField] bool isWalkable;
    [SerializeField] Material nonWalkableMaterial;
    [SerializeField] Material walkableMaterial;
    [SerializeField] Vector2Int closestTargetPosition;
    [SerializeField] List<Target> targets;
    //public Target closestTarget;
    public List<Vector2> moveToCells;
    public bool inRadius;

    void Start()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Mob")
        {
            return;
        }

        var direction = cell.pathDirection;
        // On trigger enter, gameobject is passed the cell to move to
        var mob = other.gameObject.GetComponent<Mob>();

        //check if they are on an unwalkable surface. If so, just get them out
        if (!cell.isWalkable)
        {
            direction = cell.GetWalkablePathDirection();
            mob.onUnwalkableSurface = true;
        }
        else
        {
            mob.onUnwalkableSurface = false;
        }
        mob.UpdateMoveDirection(direction);
    }
    public void SetColor(bool isWalkable)
    {
        GetComponent<MeshRenderer>().material = isWalkable ? walkableMaterial : nonWalkableMaterial;
    }
    public void AddMoveToLocations()
    {
        cell.moveToCells.ForEach(x => moveToCells.Add(x.Value));
    }
    private void Update()
    {
        pathDirection = cell.pathDirection;
        isWalkable = cell.isWalkable;
        //closestTargetPosition = cell.closestTargetCell != null ?
        //    cell.closestTargetCell.Value : Vector2Int.zero;
        //targets = cell.targets;        
    }
}
