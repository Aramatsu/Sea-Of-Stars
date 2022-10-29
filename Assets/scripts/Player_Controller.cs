using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public float Cooldown;
        public string Name;
        public bool CanPierce;
        public bool Repeatable;
        public Weapon(float damage, string name, bool CanPierce, float cooldown, bool repeatable)//weapon constructor
        {
            this.Damage = damage;
            this.Name = name;
            this.CanPierce = CanPierce;
            this.Cooldown = cooldown;
            this.Repeatable = repeatable;
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
    [SerializeField] private GameObject bullet; //Bullet
    [SerializeField] private Slider Health_bar; //the health bar the health is connected to
    [SerializeField] private Slider Mana_bar; //the mana bar the mana is connected to
    [SerializeField] private GameObject dash;//The dash gameobject
    [SerializeField] private GameObject gfx;//The gfx of the player gameobject
    [SerializeField] private Camera_controller cam_script;

    private float dash_timer;//The timer for the dash
    private float super_timer;//The timer for the super attack
    private float shooting_timer; //the timer for the normal attack
    private Vector2 velocity = Vector2.zero;
    private Vector2 target_velocity;
    private float horimove;
    private float vertmove;
    private float health = 100;
    private float mana = 0;

    private Weapon Staff = new Weapon(10, "Staff", false, 1, false); //the staff weapon
    private Weapon Machinegun = new Weapon(0.5f, "Machinegun", false, 0.5f, true); //the Machinegun weapon
    private Weapon Sniper = new Weapon(20, "Sniper", true, 2, false); //the sniper weapon

    private Weapon Current_weapon;

    //awake function
    private void Awake()
    {
        player_transform = transform;
    }

    //start yay
    private void Start()
    {
        dash_timer = Time.realtimeSinceStartup + 0.3f;
        super_timer = Time.realtimeSinceStartup + 1f;
        super_timer = Time.realtimeSinceStartup + Current_weapon.Cooldown;
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

        //super attack
        if (Input.GetMouseButton(1))
        {
            if (mana >= 20)
            {
                if (super_timer <= Time.realtimeSinceStartup)
                {

                    super_timer = Time.realtimeSinceStartup + 1f;
                    UpdateMana(-20);
                    player_anim.SetTrigger("Shot_M2");
                    AnimatorClipInfo[] hi = player_anim.GetCurrentAnimatorClipInfo(0);//gets the  anim clip
                    Invoke("OnM2", hi.Length - 0.3f);
                }
            }

        }

        //
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switch (Current_weapon.Name)
            {
                case "Staff":
                    Current_weapon = Machinegun;
                    break;
                case "Machinegun":
                    Current_weapon = Sniper;
                    break;
                case "Sniper":
                    Current_weapon = Staff;
                    break;
                default:
                    Current_weapon = Staff;
                    break;
            }
            Debug.Log(Current_weapon.Name);
        }

        //explode all objects within explosion radius when tapping e 
        if (Input.GetKeyDown(KeyCode.E))
        {
            

            exploded = Physics2D.OverlapCircleAll(transform.position, explode_radius, explodedable);
            for (int i = 0; i < exploded.Length; i++)
            {
                exploded[i].attachedRigidbody.AddForce(Vector2.ClampMagnitude(V2_distance(exploded[i].transform.position, transform.position) * explosion_power, max_explosion_power));
                Debug.Log(Vector2.ClampMagnitude(V2_distance(exploded[i].transform.position, transform.position) * explosion_power, max_explosion_power));
            }


            
        }

        //Dash when tapping space
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rigid2d.velocity.x) > 1 && dash_timer <= Time.realtimeSinceStartup|| Mathf.Abs(rigid2d.velocity.y) > 1 && Input.GetKeyDown(KeyCode.Space) && dash_timer <= Time.realtimeSinceStartup)
        {
            Dash();
            dash_timer = Time.realtimeSinceStartup + 0.3f;
            rigid2d.AddForce(new Vector3(horimove, vertmove) * dash_speed);
        }

        //Shoot with mouse
        if (Input.GetMouseButtonDown(0) && shooting_timer <= Time.realtimeSinceStartup && !Current_weapon.Repeatable || Input.GetMouseButton(0) && Current_weapon.Repeatable)
        {
            
            if(Current_weapon.CanPierce == true)// if the current weapon has a can peirce on true...
            {
                //use raycastall instead of raycast 
                RaycastHit2D[] hit_info = Physics2D.RaycastAll(transform.position, GetMousePos() - rigid2d.position, Mathf.Infinity, explodedable);
                foreach (var hit in hit_info)
                {
                    //if hit info is not equal to null, call the onshot delegate and pass the hit objects transform and th weapon struct
                    if (hit)
                    {
                        Onshot?.Invoke(hit.transform, Current_weapon);
                        

                    }
                }
            } 
            else if (Current_weapon.CanPierce == false) //else if it is false...
            {
                //use raycast 
                RaycastHit2D hit_info = Physics2D.Raycast(transform.position, GetMousePos() - rigid2d.position, Mathf.Infinity, explodedable);

                //if hit info is not equal to null, call the onshot delegate and pass the hit objects transform and th weapon struct
                if (hit_info)
                {
                    Onshot?.Invoke(hit_info.transform, Current_weapon);
                }
            }

            StartCoroutine(cam_script.Camera_shake(0.1f, 0.05f));
        }

        //set animator parameters
        player_anim.SetFloat("speed", MathF.Abs(horimove) + Mathf.Abs(vertmove));

        //if the horizontal input is positive...
        if (horimove > 0)
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
        GameObject dash_clone = Instantiate(dash, new Vector2(1000, 1000), Quaternion.identity);
        dash_clone.GetComponent<Dash_script>().Dash(transform, rigid2d);
    }
    
    //the method that defines the death of the player
    public void Die()
    {
        SceneManager.LoadScene("Death_screen");
    }

    //When colliding with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check tag if its the red dwarf
        if (collision.gameObject.CompareTag("Red Dwarf"))
        {
            health -= 5;
            if (health > 0)
            {
                Health_bar.value = health;
                rigid2d.velocity = -1 * rigid2d.velocity;
            }
            else
            {
                Die();
            }
        }
        //if it is a mana crystal
        else if (collision.gameObject.CompareTag("Mana"))
        {
            if (mana <= 100)
            {
                UpdateMana(1);

            }
            collision.gameObject.SetActive(false);
        }
    }

    //
    private void UpdateMana(float amount)
    {
        mana += amount;
        Mana_bar.value = mana;
    }

    //
    private void OnM2()
    {
        //change this to object pooling later
        GameObject bullet_clone = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, 180 + Mathf.Rad2Deg * Mathf.Atan2(transform.position.y - GetMousePos().y, transform.position.x - GetMousePos().x))));
        rigid2d.AddForce(-GetMousePos().normalized * 20, ForceMode2D.Impulse);
        bullet_clone.SendMessage("SetDirection", new Vector2(transform.position.x, transform.position.y) - GetMousePos());
        StartCoroutine(cam_script.Camera_shake(0.3f, 0.1f));
    }

    //function to get the position of the mouse
    private Vector2 GetMousePos()
    {
        //find mouse position in relation to world point
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
}