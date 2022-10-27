using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_script : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        rb.velocity = -direction * speed;
    }

    //
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }

    //delete the bullet when it touched the border
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
}
