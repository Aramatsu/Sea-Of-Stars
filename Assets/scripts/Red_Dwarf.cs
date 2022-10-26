using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_Dwarf : MonoBehaviour
{
    //assign variable at start
    public Red_dwarf planet = new Red_dwarf();
    [SerializeField]public Enemy.Star star;
    

    private void Start()
    {
        Player_Controller.Onshot += planet.Shot;
        planet.star = star;
        planet.MoveToPlanet(transform.position, planet.planet_pos, star.rb, star.speed);

    }

    // Update is called once per frame
    void Update()
    {
        planet.MoveToPlanet(transform.position, planet.planet_pos, star.rb, star.speed);
    }

}

