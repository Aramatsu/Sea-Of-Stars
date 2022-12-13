using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange_dwarf_script : Orange_dwarf
{
    private GameObject Current_bullet;

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
}
