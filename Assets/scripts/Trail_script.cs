using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail_script : MonoBehaviour
{
    [SerializeField]private TrailRenderer trailrenderer;


    //
    private void OnEnable()
    {
        trailrenderer.Clear();
    }
}
