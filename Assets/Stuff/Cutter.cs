using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        try
        {
            other.GetComponent<Line>().CutLine();
        }
        catch { }
    }

    void Update()
    {
        if (transform.position.y < 2.5f)
        {
            transform.position = new Vector3(transform.position.x,
                2.665f, transform.position.z);
        }
    }
}
