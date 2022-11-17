using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.15f;
    [SerializeField] float rotateSpeed = 1.5f;
    [SerializeField] float upDownSpeed = 0.4f;
    [SerializeField] float draggingSlowdown = 0.5f;
    GameHandler gameHandler;

    void Start()
    {
        gameHandler = FindObjectOfType<GameHandler>();
        
    }


    void FixedUpdate()
    {
        var dragging = gameHandler.draggingRepulsor;
        UpdateMovement(dragging);
        if (dragging)
        {            
            return;
        }

        if (Input.GetMouseButton(1))
        {
            UpdateRotation();
        }        
        
    }

    void UpdateMovement(bool dragging)
    {
        var move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            move += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            move += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            move += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            move += new Vector3(0, upDownSpeed, 0);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            move += new Vector3(0, -upDownSpeed, 0);
        }

        if (dragging)
        {
            move *= draggingSlowdown;
        }
        transform.Translate(move * moveSpeed);
    }
    void UpdateRotation()
    {        
        float yRot = Input.GetAxis("Mouse X");
        float xRot = Input.GetAxis("Mouse Y");
        var rotation = new Vector3(xRot, -yRot) * rotateSpeed;
        transform.eulerAngles = transform.eulerAngles - rotation;
    }
}
