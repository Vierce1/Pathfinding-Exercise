using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public Grid grid;
    [SerializeField] Vector2 moveDirection;
    public Vector2Int goalLocation;
    float moveSpeed = 0.15f;
    float randomnessMove = 0.5f;
    public bool onUnwalkableSurface = false;

    // Called when Mob comes into contact with centerpoint of new cell    
    public void UpdateMoveDirection(Vector2Int newDir)
    {
        moveDirection = newDir;
        var currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        goalLocation = currentPos + newDir;
    }
    public void AddRandomnessToMovement()
    {
        var randomDirection = new Vector2(
                Random.Range(-randomnessMove, randomnessMove)
                , Random.Range(-randomnessMove, randomnessMove));

        //If they're out of bounds just get them out of there, no randomness
        if (onUnwalkableSurface)
        {
            randomDirection = moveDirection;
        }
        //check if at edge of map - if so, move inward
        if (grid != null &&
            grid.GetCell((int)transform.position.x, (int)transform.position.z) == null)
        {
            transform.position = Vector3.zero;
            randomDirection = 
                (grid.targetList[0].transform.position - transform.position).normalized;
        }        

        moveDirection += randomDirection;
    }
    void Update()
    {
        transform.position += new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed);
    }

    private void Start()
    {
        
    }    
}
