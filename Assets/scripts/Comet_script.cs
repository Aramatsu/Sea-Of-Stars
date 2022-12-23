using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet_script : Enemy
{
    [SerializeField] private Transform GfxTransform;

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveToPlayer();
    }

    public void MoveToPlayer()
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, Mathf.Atan2((PlayerTransform.position - transform.position).y, (PlayerTransform.position - transform.position).x) * Mathf.Rad2Deg));

        if (rotation.eulerAngles.z > 90 && rotation.eulerAngles.z < 270)
        {
            rotation = Quaternion.Euler(new Vector3(180, 0, -rotation.eulerAngles.z));
        }
        GfxTransform.rotation = rotation;

        rb.AddForce((PlayerTransform.position - transform.position).normalized * speed);
    }
}
