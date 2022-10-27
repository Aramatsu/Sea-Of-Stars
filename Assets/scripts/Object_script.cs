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
        target_velocity = -(transform.position - player_transform.position) * speed;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref velocity, movement_damping);
    }



}
