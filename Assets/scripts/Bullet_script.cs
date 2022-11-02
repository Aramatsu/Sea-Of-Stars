using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_script : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [HideInInspector]
    public Player_Controller.Weapon current_weapon;
    public string[] Tags_affected;
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
    public void SetValues(Vector2 direction, Player_Controller.Weapon weapon, string[] tags)
    {
        current_weapon = weapon;
        this.direction = direction.normalized * weapon.Speed;
        this.Tags_affected = tags; //all the tags that will be affected by this bullet
        rb.position = rb.position - (direction * 0.25f);
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
                        collision.gameObject.GetComponent<Player_Controller>().damage(current_weapon.Damage);
                        gameObject.SetActive(false);
                        break;
                    case "Red Dwarf":
                        collision.gameObject.GetComponent<Red_Dwarf>().planet.Damage(current_weapon.Damage);
                        gameObject.SetActive(false);
                        break;
                    case "Orange Dwarf":
                        collision.gameObject.GetComponent<Orange_dwarf_script>().planet.Damage(current_weapon.Damage);
                        gameObject.SetActive(false);
                        break;

                }
            }
        }


        //delete the bullet when it touches the border
        if (collision.gameObject.CompareTag("Border"))
        {
            gameObject.SetActive(false);
        }
    }

    
}



