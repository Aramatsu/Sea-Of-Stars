using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Vector2 startpos;
    public Transform ReferencePoint;
    public float parallexEffect;
    void Start()
    {
        startpos = transform.position;
    }
    void LateUpdate()
    {
        Vector2 dist = ReferencePoint.transform.position * parallexEffect;
        transform.position = startpos + dist;
    }


}
