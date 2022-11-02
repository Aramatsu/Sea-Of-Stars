using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange_dwarf_script : MonoBehaviour
{
    //assign variable at start
    public Orange_dwarf planet = new Orange_dwarf();
    [SerializeField] public Enemy.Star star;
    private float shoot_timer = 1;


    private void Start()
    {
        Player_Controller.Onshot += planet.Shot;//add 
        shoot_timer = Time.realtimeSinceStartup + shoot_timer;
        planet.star = star;
        planet.MoveToPlanet(transform.position, planet.planet_pos, star.rb, star.speed);

    }

    // Update is called once per frame
    void Update()
    {
        if (shoot_timer < Time.realtimeSinceStartup)
        {
            planet.Shoot(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            shoot_timer = Time.realtimeSinceStartup + 0.5f;

            /**for (int i = 0; i < 10; i++)
            {
                planet.Shoot(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            }
            shoot_timer = Time.realtimeSinceStartup + 5;**/
        }
        
        planet.MoveToPlanet(transform.position, planet.planet_pos, star.rb, star.speed);
    }
}
