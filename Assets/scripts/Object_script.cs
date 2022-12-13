using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_script : MonoBehaviour
{
    private Transform player_transform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float movement_damping; //how much the movement should smooth out   


    private Vector2 velocity = Vector2.zero;
    private Vector2 target_velocity;

    //start func woohoo
    private void Start()
    {
        player_transform = Player_Controller.player_transform;
    }

    //
    private void Update()
    {
        target_velocity = (player_transform.position - transform.position) * (speed * Mathf.Clamp(1 / (player_transform.position - transform.position).magnitude, 0.1f, Mathf.Infinity));
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref velocity, movement_damping);
    }

    //method for adding a explosive force
    public void Explode(Vector2 star_pos)
    {
        rb.AddForce((star_pos - new Vector2(transform.position.x, transform.position.y)).normalized * 0.25f);
    }

}
