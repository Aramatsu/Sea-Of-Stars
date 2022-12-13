using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_controller : MonoBehaviour
{
    static public Camera_controller shared_instance;// the shared instance
    [SerializeField] Camera cam1;// this camera
    [SerializeField]private Transform Player_position; //vector 2 used for the camera shake func
    private float elapsed_time; //float used for the camera shake func

    //assign the shared instance to this
    private void Awake()
    {
        shared_instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        
        //if the player is pressing Q
        if (Input.GetKey(KeyCode.Q))
        {
            //zoom out
            cam1.orthographicSize = 30;
        }
        else
        {
            //keep the normal size
            cam1.orthographicSize = 15;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            StopAllCoroutines();
        }

        //
        transform.position = new Vector3(Mathf.Clamp(Player_position.position.x, -101.2f, 100), Mathf.Clamp(Player_position.position.y, -14.3f, 123.2f), -10);
    }

    public IEnumerator Camera_shake(float duration, float magnitude)
    {
        //i
        elapsed_time = 0f;

        while (elapsed_time <= duration)
        {
            //wanna
            transform.position = new Vector3(Mathf.Clamp(Player_position.position.x, -101.2f, 100), Mathf.Clamp(Player_position.position.y, -14.3f, 123.2f), 0) + new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude), -10);

            elapsed_time += Time.deltaTime;

            yield return null;
        }

        transform.position = new Vector3(Mathf.Clamp(Player_position.position.x, -101.2f, 100), Mathf.Clamp(Player_position.position.y, -14.3f, 123.2f), -10);
        //die
    }
}