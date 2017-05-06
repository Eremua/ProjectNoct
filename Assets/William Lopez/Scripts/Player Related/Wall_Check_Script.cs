using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Check_Script : MonoBehaviour {

    // Public
    public bool areYouALeftOrRightTrainer; // Decides which variable will be sent to PC Script
    public bool Left;
    public bool Right;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Climbable")
        {
            if (areYouALeftOrRightTrainer)
            {
                Left = true;
            }
            else if (!areYouALeftOrRightTrainer)
            {
                Right = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Climbable")
        {
            if (areYouALeftOrRightTrainer)
            {
                Left = true;
            }
            else if (!areYouALeftOrRightTrainer)
            {
                Right = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Climbable")
        {
            if (areYouALeftOrRightTrainer)
            {
                Left = false;
            }
            else if (!areYouALeftOrRightTrainer)
            {
                Right = false;
            }
        }
    }
}
