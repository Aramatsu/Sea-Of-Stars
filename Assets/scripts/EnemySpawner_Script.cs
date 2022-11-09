using UnityEngine;
using System;
using System.Collections;

public class EnemySpawner_Script : MonoBehaviour
{
    public delegate void Enemy_Delegate();//the delegate that 
    public static Enemy_Delegate Onkill;

    //Enemy types
    [SerializeField] private GameObject[] Enemys;

    [Serializable]
    public struct Wave_Enemy //A struct containing both the enemy and when it spawns
    {
        public GameObject Enemy_prefab;
        public float When_to_spawn;
    }

    [Serializable]
    public struct Wave //A struct containing both the enemy and when it spawns
    {
        public Wave_Enemy[] wave;
    }
   
    [SerializeField] private Wave[] Waves;
    

    //variables for enemy spawning
    [SerializeField] private Transform spawn_area;

    private int wave_number;
    private int Enemy_num; //number of enemys 
    

    //start function
    private void Start()
    {
        StartCoroutine(OnWave());
        Onkill += Enemy_died;
    }

    /**update function
    private void Update()
    {
        //once realtime is higher than spawn time plus the delay between each spawn...
        if(spawn_time + spawn_delay <= Time.realtimeSinceStartup)
        {
            //instatiate a red_dwarf
            Instantiate(Enemys[UnityEngine.Random.Range(0, Enemys.Length)],new Vector2(spawn_area.position.x + UnityEngine.Random.Range(-1 * (spawn_area.localScale.x / 2), spawn_area.localScale.x / 2), spawn_area.position.y), Quaternion.identity);
            //set spawntime to actual time once
            spawn_time = Time.realtimeSinceStartup;
        }
    }**/

    private void Enemy_died()
    {
        Enemy_num--;
        if (Enemy_num == 0)
        {
            Debug.Log("Wave over!!!");
            wave_number++;
            StopCoroutine(OnWave());
            Debug.Log("Next wave");
            StartCoroutine(OnWave());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(-1 * (spawn_area.localScale.x / 2), spawn_area.position.y),new Vector2(spawn_area.localScale.x / 2, spawn_area.position.y));
    }

    IEnumerator OnWave()
    {
        if (wave_number < Waves.Length)
        {
            for (int i = 0; i < Waves[wave_number].wave.Length; i++)
            {
                yield return new WaitForSeconds(Waves[wave_number].wave[i].When_to_spawn);
                Instantiate(Waves[wave_number].wave[i].Enemy_prefab, new Vector2(spawn_area.position.x + UnityEngine.Random.Range(-1 * (spawn_area.localScale.x / 2), spawn_area.localScale.x / 2), spawn_area.position.y), Quaternion.identity);
                Enemy_num++; //counts the amount of enemys currently present
            }
        }
        else
        {
            Debug.Log("No more waves bitch");
            while (wave_number >= Waves.Length)
            {
                yield return new WaitForSeconds(5);
                Instantiate(Enemys[UnityEngine.Random.Range(0, Enemys.Length)], new Vector2(spawn_area.position.x + UnityEngine.Random.Range(-1 * (spawn_area.localScale.x / 2), spawn_area.localScale.x / 2), spawn_area.position.y), Quaternion.identity);
            }

        }
    }
}


//The main class of all enemies
public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Player_Controller.Weapon current_player_weapon;
    public Rigidbody2D rb; // the rigidbody of the star
    public Animator anim;// the anim of the star

    [Space]
    [Header("Prefabs")]
    private GameObject current_mana; //variable used in the create method

    [Space]
    [Header("Values")]
    public Vector2 planet_pos = new Vector2(0, -225);
    public float health; //the enemies health
    public float speed; // their speed
    public float damage; // the amount of damage they deal to the player 
    public int Mana; // the amount of mana given when killed 

    //function used for when the star touches a trigger
    public void ontriggerenter(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                Damage(20);
                break;
                
        }
    }
    //function that hurts the star and does the coresponding other crap
    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die(Mana); // gio sucks
        }
        anim.SetTrigger("Hurt");
    }

    //the function that sets the weapon of the player
    public void Shot(Quaternion rotation, Vector2 direction, Player_Controller.Weapon weapon, string[] tags)
    {
        current_player_weapon = weapon;
    }

    // function that moves the star toward the planet
    public void MoveToPlanet(Vector2 r_pos, Vector2 p_pos, Rigidbody2D rb2D, float speed)
    {
        rb2D.AddForce((p_pos - r_pos) * speed, ForceMode2D.Force);
    }
    
    //the function that used to kill the star
    public void Die(int mana)
    {
        Player_Controller.Onshot -= Shot;
        EnemySpawner_Script.Onkill();
        for (int i = 0; i < mana; i++)
        {
            current_mana = Object_pool.Shared_instance.Create(Object_pool.Shared_instance.Pooled_Squares, transform.position + new Vector3(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2), 0), Quaternion.identity);
            if (current_mana)
            {
                current_mana.GetComponent<Object_script>().Explode(transform.position);
            }
        }
        Destroy(gameObject);
    }
   

}

//the class of red_dwarfs
public class Red_dwarf : Enemy
{
     
}

//the class of orange_dwarfs
public class Orange_dwarf : Enemy
{
    //variables
    Player_Controller.Weapon weapon = new Player_Controller.Weapon("Orange Dwarf", 25, 30, 5, 0, false, false);
    public float shoot_timer = 2;
    public float bullet_offset = 0;

    //method that creates bullets via the the object pool and then sets its direction
    public GameObject Shoot(Quaternion rotation, Vector2 direction)
    {
        GameObject bullet_clone = Object_pool.Shared_instance.Create(Object_pool.Shared_instance.Pooled_bullets, transform.position, rotation);
        Player_Controller.Onshot(rotation, direction, weapon, new string[] {"Player"});
        return bullet_clone;
    }
}