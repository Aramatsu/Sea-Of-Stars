using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet_script : MonoBehaviour
{
    private float planet_health = 100;
    [SerializeField]private Image health_graph;
    [SerializeField]private Sprite health_1;
    [SerializeField]private Sprite health_2;
    [SerializeField]private Sprite health_3;
    [SerializeField]private Sprite health_4;

        
    

    //when a trigger enters the planet...
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "Red Dwarf":
                DamagePlanet(10);
                Player_Controller.Onshot -= collision.gameObject.GetComponent<Red_Dwarf>().planet.Shot;
                Destroy(collision.gameObject);
                Debug.Log("bishees worked");
                break;
        }
        
        //ewy gross code fix later
        if(planet_health > 75)
        {
            Change_health(health_1);
        }
        else if(planet_health > 50 && 75 > planet_health)
        {
            Change_health(health_2);
        }
        else if (planet_health > 25 && 50 > planet_health)
        {
            Change_health(health_3);
        }
        else if (planet_health > 0 && 25 > planet_health)
        {
            Change_health(health_4);
        }
    }

    private void DamagePlanet(float damage)
    {
        planet_health -= damage;
        Debug.Log("planet oofed");
    }

    //hallo
    private void Change_health(Sprite sprite)
    {
        health_graph.sprite = sprite;
        if (planet_health <= 0)
        {
            //JARED MAKE A DIEE FUNCTION REEEEEEEEEEEEEEEEEEEE
            Debug.Log("planet ded lol");
        }
    }
}
