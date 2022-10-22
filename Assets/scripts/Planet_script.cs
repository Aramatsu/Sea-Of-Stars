using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_script : MonoBehaviour
{
    private float planet_health = 100;

    //when a trigger enters the planet...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Red Dwarf":
                DamagePlanet(10);
                break;
        }
    }

    private void DamagePlanet(float damage)
    {
        planet_health -= damage;
        Debug.Log("planet oofed");
        if (planet_health <= 0)
        {
            //JARED MAKE A DIEE FUNCTION REEEEEEEEEEEEEEEEEEEE
            Debug.Log("planet ded lol");
        }
    }
}
