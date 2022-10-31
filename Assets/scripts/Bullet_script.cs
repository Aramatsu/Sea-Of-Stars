using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_script : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    private bool? piercing_bullet;
    private Vector2 direction;

    //
    private void Awake()
    {
        Player_Controller.Onshot += SetValues;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = -direction;
    }

    private void OnEnable()
    {
        direction = new Vector2(0, 0);
    }

    public void SetValues(Vector2 direction, Player_Controller.Weapon weapon)
    {
        if (piercing_bullet.HasValue) 
        { 
            piercing_bullet = weapon.CanPierce; 
        }
        if (this.direction == new Vector2(0, 0) && gameObject.activeInHierarchy) 
        { 
            this.direction = direction.normalized * speed;  
        }
    }

    

    //delete the bullet when it touched the border
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            gameObject.SetActive(false);
        }
    }

    //
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (piercing_bullet.Value && collision.collider.CompareTag("Red Dwarf"))
        {
            gameObject.SetActive(false);
        }
    }
}
