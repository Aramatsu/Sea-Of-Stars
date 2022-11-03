using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange_dwarf_script : MonoBehaviour
{
    //assign variable at start
    public Orange_dwarf planet = new Orange_dwarf();
    [SerializeField] public Enemy.Star star;
    private float shoot_timer = 5;

    private GameObject Current_bullet;


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

            for (int i = 0; i < 5; i++)
            {
                Current_bullet = planet.Shoot(Quaternion.Euler(0, 0, 180 + (72 * (i + 1))), AngleToVec2(72 * (i + 1), 1));
            }
            shoot_timer = Time.realtimeSinceStartup + 5;
        }
        
        planet.MoveToPlanet(transform.position, planet.planet_pos, star.rb, star.speed);
    }

    private Vector2 AngleToVec2(float rotation, float Magnitude)
    {
        rotation = Mathf.Deg2Rad * rotation;
        Vector2 ans = new Vector2(Mathf.Cos(rotation) * Magnitude, Mathf.Sin(rotation) * Magnitude);
        return ans;
    }
}
