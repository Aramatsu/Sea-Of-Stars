using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Script : Enemy
{
    public Transform GfxTransform;
    private float _timer;
    private Vector2 velocity = Vector2.zero;
    [SerializeField] private float movement_damping;
    [SerializeField] private float DashSpeed;

    private void Awake()
    {
        _timer = Time.realtimeSinceStartup + 2;
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup >= _timer)
        {
            rb.AddForce((PlayerTransform.position - transform.position).normalized * DashSpeed, ForceMode2D.Impulse);
            _timer = Time.realtimeSinceStartup + 2;
        }
    }

    private void FixedUpdate()
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

        Vector2 target_velocity = (PlayerTransform.position - transform.position).normalized * speed;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref velocity, movement_damping);
    }


}
