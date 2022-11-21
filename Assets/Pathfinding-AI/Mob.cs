using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public Grid grid;
    [SerializeField] Vector2 moveDirection;
    public float swarmPower = 0.12f;
    public bool appliedSwarmCount = false;
    public Vector2Int goalLocation;
    float moveSpeed = 4f;
    float randomnessMove = 0.5f;
    public bool onUnwalkableSurface = false;
    public bool outOfBounds = false;
    Rigidbody rb;
    [SerializeField] bool debugMe = false;
    public bool applySwarm = true;

    // Called when Mob comes into contact with centerpoint of new cell    
    public void UpdateMoveDirection(Vector2Int newDir)
    {
        moveDirection = newDir;
        var currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        goalLocation = currentPos + newDir;
    }
    // Add movement based on factors (random 'scurrying', out of bounds, etc)
    // Called every path update
    public void AdditionalMovementAdder()
    {
        var addlDirection = new Vector2(
                Random.Range(-randomnessMove, randomnessMove)
                , Random.Range(-randomnessMove, randomnessMove));

        //check if at edge of map - if so, move inward
        var mobPos = new Vector2Int(Mathf.RoundToInt(transform.position.x)
            , Mathf.RoundToInt(transform.position.z));
        var onCell = grid.GetCell(mobPos.x, mobPos.y);
        if (grid != null &&  onCell == null)
        {
            outOfBounds = true;
            addlDirection =
                GetWalkablePathDirection(mobPos).normalized;
            //if (debugMe)
            //{
            //    Debug.Log(addlDirection);
            //}
            
            moveDirection = addlDirection;
            return;
        }
        outOfBounds = false;

        // If they're on unwalkable just get them out of there, no randomness
        // Move Direciton is set to nearest walkable cell direction (cellViz)
        if (onUnwalkableSurface)
        {
            //addlDirection = moveDirection;
            onUnwalkableSurface = false;
            return;
        }

        moveDirection += addlDirection;
    }
    void FixedUpdate()
    {
        var dir = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed);
        rb.velocity = dir;
        //dir = new Vector3(dir.x + 90, dir.y, dir.z);
         transform.LookAt(-dir + transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (appliedSwarmCount || other.isTrigger || !applySwarm)
        {
            return;
        }
        var hero = other.GetComponentInParent<Hero>();
        if (hero == null)
        {
            return;
        }
        hero.swarmCounter += swarmPower;
        appliedSwarmCount = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        var hero = other.GetComponentInParent<Hero>();
        if (hero == null)
        {
            return;
        }
        StartCoroutine(SwarmCooldown(hero));
    }
    IEnumerator SwarmCooldown(Hero hero)
    {
        yield return new WaitForSeconds(2f);
        hero.swarmCounter -= swarmPower;
        appliedSwarmCount = false;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public Vector2 GetWalkablePathDirection(Vector2Int mobPos)
    {
        var closestCell = grid.GetNearestWalkableCell(null, mobPos, true);
        var direction = closestCell.Value - mobPos;
        if (debugMe)
        {
            Debug.Log("closest cell = " + closestCell.Value);
        }
        return direction;
    }
}
