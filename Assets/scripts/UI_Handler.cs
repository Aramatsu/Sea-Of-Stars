using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    [SerializeField] private Text Timer;
    [SerializeField] private Text Wave_counter;

    public static float round_time;
    public static float current_wave;
    
    // Start is called before the first frame update
    void Start()
    {
        round_time = 0; //reset the timer when the round starts
    }

    // Update is called once per frame
    void Update()
    {
        round_time += Time.deltaTime;
        Timer.text = "Time: " + Mathf.RoundToInt(round_time);
        Wave_counter.text = "Wave: " + (current_wave + 1);
    }

    public static void Set_CurrentWave(int currentwave)
    {
        current_wave = currentwave;
    }
}
