using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling_Check_Script : MonoBehaviour {

    // Public
    public bool Ceiling;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ceiling")
        {
            Ceiling = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ceiling")
        {
            Ceiling = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ceiling")
        {
            Ceiling = false;
        }
    }
}
