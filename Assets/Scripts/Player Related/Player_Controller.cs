using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{

    // Public
    public float maxPlayerSpd; // How fast the player can go
    public float playerAccel; // Rate player speeds up
    public float playerDecel; // Rate player slows down
    public float playerJumpForce; // How powerful jump is

    // Private
    private float curPlayerSpd; // Current speed of the player
    private bool onGround = true; // Checks for if on ground


    // Component
    public Rigidbody rBody;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Handles player jumping
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Key W or Space is being pressed.");
            rBody.AddForce(new Vector3(0, playerJumpForce, 0), ForceMode.Impulse);
        }
    }

    private void Update()
    {
        // Handles player left and right movement
        if (Input.GetKey(KeyCode.A) && curPlayerSpd > -maxPlayerSpd || Input.GetKey(KeyCode.LeftArrow) && curPlayerSpd > -maxPlayerSpd)
        {
            Debug.Log("Key A or Left Arrow is being pressed.");
            curPlayerSpd = curPlayerSpd - playerAccel * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) && curPlayerSpd < maxPlayerSpd || Input.GetKey(KeyCode.RightArrow) && curPlayerSpd < maxPlayerSpd)
        {
            Debug.Log("Key D or Right Arrow is being pressed.");
            curPlayerSpd = curPlayerSpd + playerAccel * Time.deltaTime;
        }
        else
        {
            if (curPlayerSpd > playerDecel * Time.deltaTime)
            {
                curPlayerSpd = curPlayerSpd - playerDecel * Time.deltaTime;
            }
            else if (curPlayerSpd < -playerDecel * Time.deltaTime)
            {
                curPlayerSpd = curPlayerSpd + playerDecel * Time.deltaTime;
            }
            else
            {
                curPlayerSpd = 0;
            }
        }

        Vector3 newPosition = transform.position;
        newPosition.x = transform.position.x + curPlayerSpd * Time.deltaTime;
        transform.position = newPosition;

        // Handles player crouching
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Key S or Down Arrow is being pressed.");
        }

        // Handles player climbing up
        if (Input.GetKey(KeyCode.R))
        {
            Debug.Log("Key R is being pressed.");
        }

        // Handles player climbing down
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("Key F is being pressed.");
        }

        // Handles player hiding spots 
        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("Key E is being pressed.");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Climbable")
        {
            return;
        }
        else if (other.tag == "HidingSpot")
        {
            return;
        }
        else if (other.tag == "Checkpoint")
        {
            return;
        }
        else if (other.tag == "Light")
        {
            return;
        }
        else if (other.tag == "Enemy")
        {
            return;
        }
    }
}
