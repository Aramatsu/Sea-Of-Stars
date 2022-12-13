using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_script : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [HideInInspector]
    public Player_Controller.Weapon current_weapon;
    public GameObject whoshot;
    public string[] Tags_affected;
    public SpriteRenderer spriteRenderer;
    private Quaternion rotation;
    private Vector2 direction;

    //      
    private void onawake()
    {
        Player_Controller.Onshot += SetValues;// on awake add Setvalues to onshot
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = -direction;// constanlty move the bullet
    }

    //on enable re add setvalues to onshot
    private void OnEnable()
    {
        Player_Controller.Onshot += SetValues;
    }

    //sets variables
    public void SetValues(Quaternion rotation, Vector2 direction, Player_Controller.Weapon weapon, string[] tags, GameObject whoshot)
    {
        float rotationOffset = Random.Range(-weapon.Accuracy, weapon.Accuracy);
        this.whoshot = whoshot;
        current_weapon = weapon;
        this.direction = Vector_Math.AngleToVec2((Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + rotationOffset, 1) * weapon.Speed;
        transform.rotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z + rotationOffset);
        this.Tags_affected = tags; //all the tags that will be affected by this bullet

        //
        if (Tags_affected[0] == "Player") //If the enemy shot the bullet...
        {
            transform.localScale = new Vector3(1, 1, 1);
            rb.position = rb.position - (direction * 0.25f);

        }
        else if (Tags_affected[0] == "Orange Dwarf") //If the player shot the bullet...
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            rb.position = rb.position - (direction * 0.05f);
        }
        Player_Controller.Onshot -= SetValues;// remove setvalues to ensure you dont add more values on top of its given ones
    }

    //When the bullet collides with something...
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < Tags_affected.Length; i++)
        {
            if (collision.gameObject.CompareTag(Tags_affected[i]))//check if it matches with tags affected.
            {
                switch (collision.gameObject.tag)//check specifically which one
                {
                    case "Player":
                        Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();
                        player.damage(current_weapon.Damage, -direction, 100);
                        player.rigid2d.AddForce(rb.velocity * 10);
                        gameObject.SetActive(false);
                        break;
                    case "Red Dwarf":
                        collision.gameObject.GetComponent<Red_Dwarf>().Damage(current_weapon.Damage);
                        if (gameObject.CompareTag("Bullet2")) //If the bullet is the big bullet then it will delete itslef rather than disabling itself
                        {
                            Destroy(gameObject);
                        }
                        else
                        {
                            gameObject.SetActive(false);
                        }
                        break;
                    case "Orange Dwarf":
                        collision.gameObject.GetComponent<Orange_dwarf_script>().Damage(current_weapon.Damage);
                        if (gameObject.CompareTag("Bullet2")) //If the bullet is the big bullet then it will delete itslef rather than disabling itself
                        {
                            Destroy(gameObject);
                        }
                        else
                        {
                            gameObject.SetActive(false);
                        }
                        break;

                    
                }


            }
            

        }

                    //as long as it wasnt what shot it...
        if (collision.gameObject != whoshot)
        {
            //it should kill itself
            if (gameObject.CompareTag("Bullet2")) //If the bullet is the big bullet then it will delete itslef rather than disabling itself
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //delete the bullet when it touches the border
        if (collision.gameObject.CompareTag("Border"))
        {
            if (gameObject.CompareTag("Bullet2")) //If the bullet is the big bullet then it will delete itslef rather than disabling itself
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }

        }
    }

    
}



