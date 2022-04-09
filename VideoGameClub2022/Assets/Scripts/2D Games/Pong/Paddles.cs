using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddles : MonoBehaviour
{
    [Header("Paddle objects")]
    [SerializeField] private GameObject PlayerPaddle;
    [SerializeField] private GameObject AIPaddle;

    [Space]

    [Header("Ball Object")]

    [Space]

    [Header("Settings")]
    [SerializeField] private float PaddleSpeed;

    private float userInput;
    private float aiInput;

    private bool active; //whether or not the players should be moving

    void Update()
    {
        if(active)
        {
            userInput = Input.GetAxis("Vertical");
        }
        else
        {
            aiInput = 0;
            userInput = 0;
        }
    }

    void FixedUpdate()
    {

    }

    public void setActive(bool active)
    {
        this.active = active;
    }
}
