using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public bool isHolding = true;
    Target target;

    private void Start()
    {
        target = GetComponentInParent<Target>();
    }
    private void FixedUpdate()
    {
        if (!isHolding)
        {
            transform.parent.rotation = Quaternion.identity;
            var rb = GetComponentInParent<Rigidbody>();            
            rb.constraints = RigidbodyConstraints.FreezeRotation;            
            Destroy(target.GetComponent<SpringJoint>());
            Destroy(gameObject);
        }
    }
}
