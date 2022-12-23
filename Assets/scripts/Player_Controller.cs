using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Player_Controller : MonoBehaviour
{
    public delegate void ShootDelegate(Quaternion rotation, Vector2 direction, Weapon weapon, string[] tags, GameObject whoshot);//the delegate that cotains all the methods for when the player shoots
    public static ShootDelegate Onshot;

    //the delegate that cotains all the parameters for when the player uses their super
    public delegate void SuperDelegate(Transform transform);
    public static SuperDelegate OnSuper;




    [Serializable]
    public struct Weapon // the struct containing all the data of each weapon
    {
        public string Name;
        public float Damage;
        public float Speed;
        public float Cooldown;
        public float Accuracy;
        public float AudioVolume;
        public int ManaCost;
        public bool CanPierce;
        public bool Repeatable;

        public Weapon(string name, float damage, float speed, float cooldown, float Accuracy,int ManaCost, bool CanPierce, bool repeatable)//weapon constructor default
        {
            this.Damage = damage;
            this.Speed = speed;
            this.Name = name;
            this.CanPierce = CanPierce;
            this.Cooldown = cooldown;
            this.Repeatable = repeatable;
            this.Accuracy = Accuracy;
            this.ManaCost = ManaCost;
            AudioVolume = 1;
            
        }

        public Weapon(string name, float damage, float speed, float cooldown, float Accuracy, bool CanPierce, bool repeatable)//weapon constructor default
        {
            this.Damage = damage;
            this.Speed = speed;
            this.Name = name;
            this.CanPierce = CanPierce;
            this.Cooldown = cooldown;
            this.Repeatable = repeatable;
            this.Accuracy = Accuracy;
            this.ManaCost = 0;
            AudioVolume = 1;

        }

        public Weapon(string name, float damage, float speed, float cooldown, float Accuracy, int ManaCost, bool CanPierce, bool repeatable, float volume)//weapon constructor overload with control over volume
        {
            Damage = damage;
            Speed = speed;
            Name = name;
            this.CanPierce = CanPierce;
            Cooldown = cooldown;
            Repeatable = repeatable;
            this.Accuracy = Accuracy;
            this.ManaCost = ManaCost;
            AudioVolume = volume;
        }

        public Weapon(string name, float damage, float speed, float cooldown, float Accuracy, bool CanPierce, bool repeatable, float volume)//weapon constructor overload with control over volume
        {
            Damage = damage;
            Speed = speed;
            Name = name;
            this.CanPierce = CanPierce;
            Cooldown = cooldown;
            Repeatable = repeatable;
            this.Accuracy = Accuracy;
            this.ManaCost = 0;
            AudioVolume = volume;
        }

        public Weapon(string name)//weapon constructor overload name
        {
            Damage = 0;
            Speed = 0;
            this.ManaCost = 0;
            Name = name;
            this.CanPierce = false;
            Cooldown = 0;
            Repeatable = false;
            this.Accuracy = 0;
            AudioVolume = 1;
        }

        public Weapon(string name, float Cooldown)//weapon constructor overload name
        {
            Damage = 0;
            Speed = 0;
            this.ManaCost = 0;
            Name = name;
            this.CanPierce = false;
            this.Cooldown = Cooldown;
            Repeatable = false;
            this.Accuracy = 0;
            AudioVolume = 1;
        }
    }

    [Header("References")]
    [SerializeField] private Camera cam; //camera
    public Rigidbody2D rigid2d; //the rigidbody of this object, its public because other things need to access it
    [SerializeField] public static Transform player_transform; //putting the transform here because it saves al ittle on efficiency
    [SerializeField] private Animator player_anim; //the animator of the player
    [SerializeField] private Slider Health_bar; //the health bar the health is connected to
    [SerializeField] private Slider Mana_bar; //the mana bar the mana is connected to
    [SerializeField] private Camera_controller cam_script;

    [Space]
    [Header("Prefabs")]
    [SerializeField] private GameObject bullet; //camera
    [SerializeField] private GameObject dash;//The dash gameobject
    [SerializeField] private GameObject gfx;//The gfx of the player gameobject

    [Space]
    [Header("Values")]
    [SerializeField] private float speed; //speed
    [SerializeField] private float dash_speed; //dash speed 
    [SerializeField] private float movement_damping; //how much the movement should smooth out   

    private float dash_timer;//The timer for the dash
    private float super_timer;//The timer for the super attack
    private float shooting_timer; //the timer for the normal attack
    private Vector2 velocity = Vector2.zero;
    private Vector2 target_velocity;
    private float horimove;
    private float vertmove;
    private float health = 100;
    private float mana = 0;

    private Weapon Staff = new Weapon("Staff", 25, 50, 0.25f, 5, false, false, 0.5f); //the staff weapon
    private Weapon Machinegun = new Weapon("Machinegun", 10, 50, 0.125f, 10, false, true, 0.25f); //the Machinegun weapon
    private Weapon Sniper = new Weapon("Sniper", 40, 70, 0.5f, 0, true, false, 1); //the sniper weapon

    private Weapon Super_blast = new Weapon("Blast", 100, 50 , 0.5f, 0, true, false); //the Blast super weapon
    private Weapon Super_control = new Weapon("Control", 0.5f); //the Control super weapon

    private Weapon Current_weapon;
    private Weapon Current_super;

    private void Awake()
    {
        Current_weapon = Staff;
        OnSuper += Blast;
        player_transform = transform;
    }

    private void Start()
    {
        Current_super = Super_blast;
        
        dash_timer = Time.realtimeSinceStartup + 0.5f;
        super_timer = Time.realtimeSinceStartup + Current_super.Cooldown;
        shooting_timer = Time.realtimeSinceStartup + Current_weapon.Cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
        //update movement variables  
        horimove = Input.GetAxisRaw("Horizontal");
        vertmove = Input.GetAxisRaw("Vertical");

        //movement code
        target_velocity = new Vector3(horimove, vertmove) * speed;
        rigid2d.velocity = Vector2.SmoothDamp(rigid2d.velocity, target_velocity, ref velocity, movement_damping);






        if (!UI_Handler.IsPaused) // Only take input when is paused equal to false
        {
            //set animator parameters
            player_anim.SetFloat("speed", MathF.Abs(horimove) + Mathf.Abs(vertmove));

            //if the horizontal input is positive...
            if (horimove > 0)
            {
                //flip the player
                gfx.transform.rotation = Quaternion.Euler(new Vector2(0, 180));
            }
            else if (horimove < 0) //and if not...
            {
                //keep them at default
                gfx.transform.rotation = Quaternion.Euler(new Vector2(0, 0));

            }
          
            //switch weapons with tab
            if (Input.GetKeyDown(KeyCode.Tab) && !Input.GetKeyDown(KeyCode.LeftShift))
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

            //switch Supers with tab
            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                switch (Current_super.Name)
                {
                    case "Blast":
                        OnSuper -= Blast;
                        OnSuper += Control;
                        break;
                    case "Control":
                        OnSuper -= Control;
                        OnSuper += Blast;
                        break;
                    default:
                        Current_super = Super_blast;
                        break;
                }
                Debug.Log(Current_super.Name);
            }




            //Dash when tapping space
            if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rigid2d.velocity.x) > 1 && dash_timer <= Time.realtimeSinceStartup || Mathf.Abs(rigid2d.velocity.y) > 1 && Input.GetKeyDown(KeyCode.Space) && dash_timer <= Time.realtimeSinceStartup)
            {
                Dash();
                dash_timer = Time.realtimeSinceStartup + 0.3f;
                rigid2d.AddForce(new Vector3(horimove, vertmove) * dash_speed);
            }

            //Shoot with mouse
            if (Input.GetMouseButtonDown(0) && shooting_timer <= Time.realtimeSinceStartup && !Current_weapon.Repeatable || Input.GetMouseButton(0) && Current_weapon.Repeatable && shooting_timer <= Time.realtimeSinceStartup)
            {
                GameObject bullet_clone = Object_pool.Shared_instance.Create(Object_pool.Shared_instance.Pooled_bullets, transform.position, Quaternion.Euler(new Vector3(0, 0, 180 + Mathf.Rad2Deg * Mathf.Atan2(transform.position.y - GetMousePos().y, transform.position.x - GetMousePos().x))));//makes a bullet
                StartCoroutine(cam_script.Camera_shake(0.1f, 0.025f * Current_weapon.Damage)); // multiply the magnitude to damage to add more oomph to the more powerful weapons
                shooting_timer = Time.realtimeSinceStartup + Current_weapon.Cooldown;//reset the timer
                Onshot(Quaternion.Euler(new Vector3(0, 0, 180 + Mathf.Rad2Deg * Mathf.Atan2(transform.position.y - GetMousePos().y, transform.position.x - GetMousePos().x))), new Vector2(transform.position.x, transform.position.y) - GetMousePos(), Current_weapon, new string[] { "Orange Dwarf", "Red Dwarf", "Meteor", "Comet" }, gameObject);//send info to the newly made clone
                AudioManager.audioManager.PlaySound("PlayerShoot", Current_weapon.AudioVolume);
            }

            //super attack
            if (Input.GetMouseButton(1))
            {
                OnSuper(transform);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            UI_Handler.Pause();
        }


    }





    //when entering a trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if it is a mana crystal
        if (collision.gameObject.CompareTag("Mana"))
        {
            if (mana <= 100)
            {
                UpdateMana(1);

            }
            AudioManager.audioManager.PlaySound("PlayerPickup");
            collision.gameObject.SetActive(false);
        }
    }

    public void Blast(Transform transform)
    {
        if (mana >= 20 && super_timer <= Time.realtimeSinceStartup) 
        {
            super_timer = Time.realtimeSinceStartup + Super_blast.Cooldown;
            UpdateMana(-20);
            player_anim.SetTrigger("Shot_M2");
            AnimatorClipInfo[] hi = player_anim.GetCurrentAnimatorClipInfo(0);//gets the  anim clip
            Invoke("OnM2", hi.Length - 0.5f);
        }
    }

    public void Control(Transform transform)
    {

    }

    #region Methods
    public void Die()
    {
        UI_Handler.SharedInstance.PlaySceneTransition("SampleScene");
    }

    private void UpdateMana(float amount)
    {
        mana += amount;
        Mana_bar.value = mana;
    }

    public void damage(float damage)
    {
        AudioManager.audioManager.PlaySound("PlayerHurt");
        health -= damage;
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

    //Overload method to damage the player
    public void damage(float damage, Vector2 direction, float power)
    {
        rigid2d.AddForce(direction * power);
        health -= damage;
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

    //Called when using the special attack
    private void OnM2()
    {
        GameObject bullet_clone = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, 180 + Mathf.Rad2Deg * Mathf.Atan2(transform.position.y - GetMousePos().y, transform.position.x - GetMousePos().x))));
        AudioManager.audioManager.PlaySound("Explosion");
        rigid2d.AddForce(-GetMousePos().normalized * 50, ForceMode2D.Impulse);
        Onshot(Quaternion.Euler(new Vector3(0, 0, 180 + Mathf.Rad2Deg * Mathf.Atan2(transform.position.y - GetMousePos().y, transform.position.x - GetMousePos().x))), new Vector2(transform.position.x, transform.position.y) - GetMousePos(), Super_blast, new string[] { "Orange Dwarf", "Red Dwarf" }, gameObject);//send info to the newly made clone
        StartCoroutine(cam_script.Camera_shake(0.3f, 0.1f));
    }

    public void Dash()
    {
        GameObject dash_clone = Instantiate(dash, new Vector2(1000, 1000), Quaternion.identity);
        dash_clone.GetComponent<Dash_script>().Dash(transform, rigid2d);
    }

    private Vector2 GetMousePos()
    {
        //find mouse position in relation to world point
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
    #endregion
}


//math
public static class Vector_Math
{

    //function that converts an angle to a Vector
    public static Vector2 AngleToVec2(float rotation, float Magnitude)
    {
        rotation = Mathf.Deg2Rad * rotation;
        Vector2 ans = new Vector2(Mathf.Cos(rotation) * Magnitude, Mathf.Sin(rotation) * Magnitude);
        return ans;
    }
}