using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Check_Script : MonoBehaviour {

    //public
    public bool Ground = false; // Checks for player on ground

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Ground = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
        {
            Ground = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            Ground = false;
        }
    }
}
