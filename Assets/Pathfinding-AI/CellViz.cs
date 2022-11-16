using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellViz : MonoBehaviour
{
    public Cell cell;
    [SerializeField] Vector2Int pathDirection;
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
        mob.onUnwalkableSurface = false;

        //check if they are on an unwalkable surface. If so, just get them out
        if (!cell.isWalkable)
        {
            direction = cell.GetWalkablePathDirection();
            mob.onUnwalkableSurface = true;
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
        //pathDirection = cell.pathDirection;
        //closestTargetPosition = cell.closestTargetCell != null ?
        //    cell.closestTargetCell.Value : Vector2Int.zero;
        //targets = cell.targets;        
    }
}
