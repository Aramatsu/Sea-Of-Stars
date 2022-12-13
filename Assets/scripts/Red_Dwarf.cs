using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_Dwarf : Red_dwarf
{

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveToPlanet(transform.position, planet_pos, rb, speed);
    }

}

