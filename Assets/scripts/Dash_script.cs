using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class Dash_script : MonoBehaviour
{
    [SerializeField] private Animator anim; //the animation of this object
    [SerializeField] private Rigidbody2D rigid2d; //the animation of this object



    public void Dash(Transform player_transform, Rigidbody2D player_rb)
    {
        rigid2d.rotation = Mathf.Rad2Deg * Mathf.Atan2(player_rb.velocity.y, player_rb.velocity.x);//rotates the dash toward the player
        transform.position = new Vector2(player_transform.position.x, player_transform.position.y) + player_rb.velocity / 2;//assigns the position of the dash while adding the offset
        anim.SetTrigger("IsDash");//trigger the dash anim
        AnimatorClipInfo[] hi = anim.GetCurrentAnimatorClipInfo(0);//gets the  anim clip
        Invoke("Delay", hi.Length);                                                          
    }



    //the delay used for deleting the object
    private void Delay()
    {
        Destroy(gameObject); // change to object ooling later
    }


}
    

