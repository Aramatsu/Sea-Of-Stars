using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_script : MonoBehaviour
{
    private Transform player_transform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;

    //start func woohoo
    private void Start()
    {
        player_transform = Player_Controller.player_transform;
    }

    //
    private void Update()
    {
        rb.velocity = ((player_transform.position - transform.position) * speed);
    }



}
