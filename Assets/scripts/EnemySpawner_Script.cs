using UnityEngine;
using System;

public class EnemySpawner_Script : MonoBehaviour
{
    //Enemy types
    [SerializeField] private GameObject[] Enemys;

    

    //variables for enemy spawning
    [SerializeField] private Transform spawn_area;
    [SerializeField] private float spawn_delay;
    private float spawn_time;

    //start function
    private void Start()
    {
        //set spawntime to actual time once
        spawn_time = Time.realtimeSinceStartup;
    }

    //update function
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(-1 * (spawn_area.localScale.x / 2), spawn_area.position.y),new Vector2(spawn_area.localScale.x / 2, spawn_area.position.y));
    }

}


//The main class of all enemies
public class Enemy
{
    public Star star;

    public Vector2 planet_pos = new Vector2(0, -225);
    public Player_Controller.Weapon current_player_weapon;

    private GameObject current_mana; //variable used in the create method

    [Serializable]
    public struct Star // the struct containing all the values for each star
    {
        public GameObject gameObject;//the gameobject of the 
        public Animator anim;// the anim of the star
        public float health; //the enemies health
        public float speed; // their speed
        public float damage; // the amount of damage they deal to the player 
        public int Mana; // the amount of mana given when killed 
        public Rigidbody2D rb; // the rigidbody of the star



        //the contructor that allows me to assign all this shit in the inspector
        public Star(GameObject gameObject, Animator anim, float health, float speed, float defense, float damage, Rigidbody2D rb, int mana)
        {
            this.gameObject = gameObject;
            this.anim = anim;
            this.health = health;
            this.speed = speed;
            this.damage = damage;
            this.rb = rb;
            Mana = mana;

        }
    }
    //function that hurts the star and does the coresponding other crap
    public void Damage(float damage)
    {
        star.health -= damage;
        if (star.health <= 0)
        {
            Die(star.Mana); // gio sucks
        }
        star.anim.SetTrigger("Hurt");
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
        for (int i = 0; i < mana; i++)
        {
            current_mana = Object_pool.Shared_instance.Create(Object_pool.Shared_instance.Pooled_Squares, star.gameObject.transform.position + new Vector3(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2), 0), Quaternion.identity);
            if (current_mana)
            {
                current_mana.GetComponent<Object_script>().Explode(star.gameObject.transform.position);
            }
        }
        UnityEngine.Object.Destroy(star.gameObject);
    }
   

}

//the class of red_dwarfs
public class Red_dwarf : Enemy
{
     
}

//the class of orange_dwarfs
public class Orange_dwarf : Enemy
{
    //variable
    Player_Controller.Weapon weapon = new Player_Controller.Weapon(25, 30, "Orange Dwarf", false, 5, false);
    
    //method that creates bullets via the the object pool and then sets its direction
    public GameObject Shoot(Quaternion rotation, Vector2 direction)
    {
        GameObject bullet_clone = Object_pool.Shared_instance.Create(Object_pool.Shared_instance.Pooled_bullets, star.gameObject.transform.position, rotation);
        Player_Controller.Onshot(rotation, direction, weapon, new string[] {"Player"});
        return bullet_clone;
    }
}