using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public Grid grid;
    [SerializeField] Vector2 moveDirection;
    public Vector2Int goalLocation;
    float moveSpeed = 4f;
    float randomnessMove = 0.5f;
    public bool onUnwalkableSurface = false;
    Rigidbody rb;

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

        //If they're on unwalkable just get them out of there, no randomness
        // Move Direciton is set to nearest walkable cell direction (cellViz)
        if (onUnwalkableSurface)
        {
            addlDirection = moveDirection;
        }
        //check if at edge of map - if so, move inward
        if (grid != null && grid.GetCell((int)transform.position.x, (int)transform.position.z) == null)
        {
            addlDirection =
                GetWalkablePathDirection().normalized;
        }

        moveDirection += addlDirection;
    }
    void FixedUpdate()
    {
        //transform.position += new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed);
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed);
        if(rb.velocity.magnitude > 30)
        {
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public Vector2 GetWalkablePathDirection()
    {
        var mobPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        return (grid.GetNearestWalkableCell(null, mobPos).Value
                        - mobPos);
    }
}
