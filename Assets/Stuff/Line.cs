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
            var targ = GetComponentInParent<Target>();
            transform.parent.position = targ.startingPos;
            targ.outOfPlay = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.velocity = Vector3.zero;
            Destroy(target.GetComponent<SpringJoint>());
            Destroy(gameObject);
        }
    }

    public void CutLine()
    {
        isHolding = false;
    }
}
