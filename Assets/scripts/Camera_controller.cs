using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_controller : MonoBehaviour
{
    static public Camera_controller shared_instance;// the shared instance
    [SerializeField] Camera cam1;// this camera
    private Vector3 Original_position; //vector 2 used for the camera shake func
    private float elapsed_time; //float used for the camera shake func

    //bishees
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
            cam1.orthographicSize = 10;
        }
    }

    public IEnumerator Camera_shake(float duration, float magnitude)
    {
        //i
        Original_position = transform.position;
        elapsed_time = 0f;

        while (elapsed_time <= duration)
        {
            //wanna
            transform.position += new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude), Original_position.z);

            elapsed_time += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = Original_position;
        //die
    }
}