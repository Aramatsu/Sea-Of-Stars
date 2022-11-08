using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_Dwarf : Red_dwarf
{
    

    private void Start()
    {
        Player_Controller.Onshot += Shot;
        MoveToPlanet(transform.position, planet_pos, rb, speed);

    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlanet(transform.position, planet_pos, rb, speed);
    }

    //when entering a trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ontriggerenter(collision);
    }

}

