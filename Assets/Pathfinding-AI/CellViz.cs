using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] int stepsToTarget;
    //public Target closestTarget;
    public List<Vector2> moveToCells;
    public bool inRadius;
    [SerializeField] RawImage arrowPic;
    void Start()
    {
        if (!Application.isEditor)
        {
            arrowPic.gameObject.SetActive(false);
        }
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
        //If target is marked as dont apply swarm in inspector, let the mob know
        // If there are no targets yet, they DO apply swarm
        mob.applySwarm = cell.targets.Count > 0 ? cell.targets[0].mobsApplySwarm : true;
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
        stepsToTarget = cell.stepsToTarget;
        pathDirection = cell.pathDirection;
        isWalkable = cell.isWalkable;
        var zeroDir = cell.pathDirection == Vector2Int.zero ? 0 : 90;
        arrowPic.transform.rotation = Quaternion.Euler(zeroDir, 0
            , GetArrowRotation((pathDirection.x, pathDirection.y)).z);

        closestTargetPosition = cell.closestTargetCell != null ?
            cell.closestTargetCell.Value : Vector2Int.zero;        
        targets = cell.targets;
    }

    Vector3 GetArrowRotation((int, int) dir) 
    {
        int angle = 0;
        switch (dir)
        {
            case (1, 1): angle = 310; break;
            case (1, 0): angle = 270; break;
            case (1, -1): angle = 215; break;
            case (0, -1): angle = 180; break;
            case (-1, -1): angle = 135; break;
            case (-1, 0): angle = 90; break;
            case (-1, 1): angle = 45; break;
        }
        return new Vector3(0, 0, angle);
    }
}


/* z axis
 * 1, 1 = -45 / 310
 * 1, 0 = -90 / 270
 * 1, -1 = -120 / 215
 * 0, -1 = -180 / 180
 * 
 * 
 * 
 * 
 * 
 */

