using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange_dwarf_script : Enemy
{
    private GameObject Current_bullet;

    //variables
    Player_Controller.Weapon weapon = new Player_Controller.Weapon("Orange Dwarf", 25, 30, 5, 0, false, false);
    public float shoot_timer = 2;
    public float bullet_offset = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shoot_timer < Time.realtimeSinceStartup)
        {
            if (bullet_offset == 36)
            {
                for (int i = 0; i < 5; i++)
                {
                    Current_bullet = Shoot(Quaternion.Euler(0, 0,bullet_offset + 180 + (72 * (i + 1))), Vector_Math.AngleToVec2(36 + (72 * (i + 1)), 1));
                }
                bullet_offset = 0;
            }
            else if (bullet_offset == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Current_bullet = Shoot(Quaternion.Euler(0, 0, bullet_offset + 180 + (72 * (i + 1))), Vector_Math.AngleToVec2( 72 * (i + 1), 1));
                }
                bullet_offset = 36;
            }
            shoot_timer = Time.realtimeSinceStartup + 2;
        }
        
        MoveToPlanet(transform.position, planet_pos, rb, speed);
    }

    //method that creates bullets via the the object pool and then sets its direction
    public GameObject Shoot(Quaternion rotation, Vector2 direction)
    {
        GameObject bullet_clone = Object_pool.Shared_instance.Create(Object_pool.Shared_instance.Pooled_bullets, transform.position, rotation);
        Player_Controller.Onshot(rotation, direction, weapon, new string[] { "Player" }, gameObject);
        return bullet_clone;
    }
}
