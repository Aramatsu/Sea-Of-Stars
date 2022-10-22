using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player_Controller : MonoBehaviour
{
    public delegate void Mydelegate(Transform transform, Weapon weapon);//the delegate that cotains all the methods for when the player shoots
    public static Mydelegate Onshot; 

    [Serializable]
    public struct Weapon // the struct containing all the data of each weapon
    {
        public float Damage;
        public string Name;
        public bool CanPierce;
        public Weapon(float damage, string name, bool CanPierce)//weapon contructor
        {
            this.Damage = damage;
            this.Name = name;
            this.CanPierce = CanPierce;
        }
    }

    [SerializeField] private float speed; //speed
    [SerializeField] private Camera cam; //camera
    [SerializeField] private float dash_speed; //dash speed
    [SerializeField] private Rigidbody2D rigid2d; //the rigidbady of this object
    [SerializeField] public static Transform player_transform; //putting the transform here because it saves al ittle on efficiency
    [SerializeField] private float movement_damping; //how much the movement should smooth out    
    [SerializeField] private Collider2D[] exploded; //objects that have been exploded
    [SerializeField] private LayerMask explodedable; //objects that should be exploded
    [SerializeField] private float explode_radius; //radius of the explosion
    [SerializeField] private float explosion_power; //power of explosion
    [SerializeField] private Animator player_anim; //the animator of the player
    [SerializeField] private float max_explosion_power; //the max amount of force you can apply on an object
    [SerializeField] private GameObject test_cubes; //temp cubes
    [SerializeField] private Slider Health_bar; //the health bar the health is connected to
    [SerializeField] private Slider Mana_bar; //the mana bar the mana is connected to
    [SerializeField] private Weapon Staff;//the staff weapon
    [SerializeField] private GameObject dash;//The dash gameobject
    [SerializeField] private GameObject gfx;//The gfx of the player gameobject
    [SerializeField] private float camerashake_length;//the length of the camera shake
    [SerializeField] private float camerashake_power;//how powerful the camera dhake is

    private float dash_timer;//The timer for the dash
    private Vector2 velocity = Vector2.zero;
    private Vector2 mousepos;
    private Vector2 target_velocity;
    private float horimove;
    private float vertmove;
    private float health = 100;
    private float mana = 0;

    //awake function
    private void Awake()
    {
        player_transform = transform;
    }

    //start yay
    private void Start()
    {
        dash_timer = Time.realtimeSinceStartup + 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        
        //update movement variables variables 
        horimove = Input.GetAxisRaw("Horizontal");
        vertmove = Input.GetAxisRaw("Vertical");


        

        //movement code
        target_velocity = new Vector3(horimove, vertmove) * speed;
        rigid2d.velocity = Vector2.SmoothDamp(rigid2d.velocity, target_velocity, ref velocity, movement_damping);

        

        //create new objects when pressing the right mouse button
        if (Input.GetMouseButton(1))
        {
            GameObject square = Object_pool.Shared_instance.Create(Object_pool.Shared_instance.Pooled_Squares, mousepos);

        }

        //explode all objects within explosion radius when tapping e 
        if (Input.GetKeyDown(KeyCode.E))
        {
            

            exploded = Physics2D.OverlapCircleAll(transform.position, explode_radius, explodedable);
            for (int i = 0; i < exploded.Length; i++)
            {
                exploded[i].attachedRigidbody.AddForce(Vector2.ClampMagnitude(V2_distance(exploded[i].transform.position, transform.position) * explosion_power, max_explosion_power)); 
            }


            
        }

        //Dash when tapping space
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rigid2d.velocity.x) > 1 && dash_timer < Time.realtimeSinceStartup|| Mathf.Abs(rigid2d.velocity.y) > 1 && Input.GetKeyDown(KeyCode.Space) && dash_timer < Time.realtimeSinceStartup)
        {
            Dash();
            dash_timer = Time.realtimeSinceStartup + 0.3f;
            rigid2d.AddForce(new Vector3(horimove, vertmove) * dash_speed);
        }


        //find mouse position in relation to world point
        mousepos = cam.ScreenToWorldPoint(Input.mousePosition);

        //Shoot with mouse
        if (Input.GetMouseButtonDown(0))
        {
            if(Staff.CanPierce == true)// if the current weapon has a can peirce on true...
            {
                //use raycastall instead of raycast 
                RaycastHit2D[] hit_info = Physics2D.RaycastAll(transform.position, mousepos - rigid2d.position, Mathf.Infinity, explodedable);
                foreach (var hit in hit_info)
                {
                    //if hit info is not equal to null, call the onshot delegate and pass the hit objects transform and th weapon struct
                    if (hit)
                    {
                        Onshot?.Invoke(hit.transform, Staff);

                    }
                }
            } 
            else if (Staff.CanPierce == false) //else if it is false...
            {
                //use raycast 
                RaycastHit2D hit_info = Physics2D.Raycast(transform.position, mousepos - rigid2d.position, Mathf.Infinity, explodedable);

                //if hit info is not equal to null, call the onshot delegate and pass the hit objects transform and th weapon struct
                if (hit_info)
                {
                    Onshot?.Invoke(hit_info.transform, Staff);
                }
            }

            // regardless, make the camera shake
            Camera_controller.shared_instance.StartCoroutine(Camera_controller.shared_instance.Camera_shake(camerashake_length, camerashake_power));
        }

        //set animator parameters
        player_anim.SetFloat("speed", MathF.Abs(horimove) + Mathf.Abs(vertmove));

        //if the horizontal input is positive...
        if(horimove > 0)
        {
            //flip the player
            gfx.transform.rotation = Quaternion.Euler(new Vector2(0, 180));
        }
        else if(horimove < 0) //and if not...
        {
            //keep them at default
            gfx.transform.rotation = Quaternion.Euler(new Vector2(0, 0));

        }

        
    }


    //function to find the distance of two vectors and return a vector
    private Vector2 V2_distance(Vector2 a, Vector2 b)
    {
        Vector2 final = (a - b);
        return final;
    }

    //function to handle dashing
    public void Dash()
    {
        GameObject dash_clone = Instantiate(dash, Vector2.zero, Quaternion.identity);
        dash_clone.GetComponent<Dash_script>().Dash(transform, rigid2d);
    }

    //When colliding with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check tag if its the red dwarf
        if (collision.gameObject.CompareTag("Red Dwarf"))
        {
            health -= 5;
            Health_bar.value = health;
            rigid2d.velocity = -1 * rigid2d.velocity; 
        }
        //and it is a mana crystal
        else if (collision.gameObject.CompareTag("Mana"))
        {
            if (mana <= 100)
            {
                mana += 1;
                Mana_bar.value = mana;

            }
            else
            {
                Debug.Log("Reached max mana");
            }
            collision.gameObject.SetActive(false);
        }
    }

}



