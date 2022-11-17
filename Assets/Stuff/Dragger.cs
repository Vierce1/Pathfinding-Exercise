using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour
{
    bool beingDrug = false;
    float distFromCam;
    Vector3 camOffset;
    GameHandler gameHandler;
    Vector3 zDist;
    Transform parentTransform;

    void Update()
    {
        if (beingDrug)
        {
            AdjustZ();
            Drag();
        }
    }
    void AdjustZ()
    {
        zDist = Vector3.zero;
        if (distFromCam >= 3)
        {
            zDist = new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * 25);
            distFromCam += zDist.z;
        }
        else
        {
            zDist += new Vector3(0, 0, 3);
            distFromCam += zDist.z;
        }
    }
    public void Drag()
    {
        if (!Input.GetMouseButton(0))
        {
            return;
        }
        if (!beingDrug)
        {
            camOffset = parentTransform.position - Camera.main.transform.position;
            distFromCam = camOffset.magnitude;
            beingDrug = true;
            gameHandler.draggingRepulsor = true;
        }

        var mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distFromCam);
        var mousePos = Camera.main.ScreenPointToRay(mouse).GetPoint(distFromCam);
        parentTransform.position = mousePos;
        parentTransform.position += zDist;
        parentTransform.rotation = Camera.main.transform.rotation;
    }
    public void Drop()
    {
        beingDrug = false;
        gameHandler.draggingRepulsor = false;
    }


    void Start()
    {
        parentTransform = transform.parent;
        gameHandler = FindObjectOfType<GameHandler>();
    }
}
