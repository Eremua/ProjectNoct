/* 
Project Noct's code is availible to review aand gather ideas from however many not use to build a game from.

-William Lopez
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{

    // Public
    public float maxPlayerSpd; // How fast the player can go
    public float playerClimbSpeed; // How fast the player climbs
    public float playerCrouchSpeed; // How fast the player moves while crouched
    public float playerAccel; // Rate player speeds up
    public float playerDecel; // Rate player slows down
    public float playerJumpForce; // How powerful jump is from ground
    public float playerWallJumpForce; // How powerful jump is from wall
    public float lightDeathTimer; // Time till player dies from light exposure

    public GameObject crouchedPlayer;

    // Private
    private float curPlayerSpd; // Current speed of the player
    private float curClimbingSpeed; // Current climbing speed of the player
    private float storedMaxSpeed;
    private float timer;

    private bool onGround = true; // Checks if the player is on the ground
    private bool wallLeft = false; // Checks if the player is against the left side of  a climbable wall
    private bool wallRight = false; // Checks if the player is against the right side of a climbable wall
    private bool playerCrouched = false; // Checks if the player is currently crouched
    private bool hasJumpedFromGround = true; // Checks to prevent double / infinite jumping from ground
    private bool hasJumpedFromWall = true; // Checks to prevent double / infinite jumping on walls
    private bool allowClimbing = false; // Checks if the player can climb a wall
    private bool underObject = false; // Checks if the player is under an object while crouched
    private bool hidingAvailible = false; // Checks if the player is in a location that allows them to hide
    private bool playerHiding = false; // Checks if the player is currently hiding
    private bool caughtInLight = false; // Checks if the player is currently in a light source
    private bool respawnPlayer = false; // Checks if the player needs to be returned to a checkpoint

    // Component
    public Rigidbody rBody; // Allows access to the player's Rigidbody
    public Transform respawnPoint; // Sets location of where to bring player when respawning

    private void Start()
    {
        // Assigns the variable with the Rigidbody
        rBody = GetComponent<Rigidbody>();
        storedMaxSpeed = maxPlayerSpd;
    }

    private void FixedUpdate()
    {
        // Handles player jumping
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Key W or Space is being pressed.");

            // Checks for either jumping from the ground or a wall
            if (!hasJumpedFromGround)
            {
                hasJumpedFromGround = true;
                rBody.AddForce(new Vector3(0, playerJumpForce, 0), ForceMode.Impulse);
            }
            else if (!hasJumpedFromWall)
            {
                if (wallLeft)
                {
                    hasJumpedFromWall = true;
                    rBody.constraints = RigidbodyConstraints.FreezeRotation;
                    rBody.AddForce(new Vector3(playerWallJumpForce, playerJumpForce, 0), ForceMode.Impulse);
                }
                else if (wallRight)
                {
                    hasJumpedFromWall = true;
                    rBody.constraints = RigidbodyConstraints.FreezeRotation;
                    rBody.AddForce(new Vector3(-playerWallJumpForce, playerJumpForce, 0), ForceMode.Impulse);
                }
            }
        }
    }

    private void Update()
    {
        // Checks to see if the player is crouched and adjusts max speed accordingly
        if (playerCrouched)
        {
            maxPlayerSpd = playerCrouchSpeed; 
        }
        else
        {
            maxPlayerSpd = storedMaxSpeed;
        }

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

        // Handles player climbing
        if (allowClimbing)
        {
            // Locks player by the Y axis to allow for a grabbing effect
            rBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

            // Sets hasJumpedFromWall to false
            hasJumpedFromWall = false;

            // If player is climbing up
            if (Input.GetKey(KeyCode.R))
            {
                Debug.Log("Key R is being pressed.");
                curClimbingSpeed = curClimbingSpeed + playerClimbSpeed * Time.deltaTime;
            }

            // If player climbing down
            else if (Input.GetKey(KeyCode.F))
            {
                Debug.Log("Key F is being pressed.");
                curClimbingSpeed = curClimbingSpeed - playerClimbSpeed * Time.deltaTime;
            }
            else
            {
                curClimbingSpeed = 0;
            }

            newPosition.y = transform.position.y + curClimbingSpeed * Time.deltaTime;
            transform.position = newPosition;
        }

        // Handles player crouching
        if (Input.GetKey(KeyCode.S) && onGround && !playerHiding|| Input.GetKey(KeyCode.DownArrow) && onGround && !playerHiding)
        {
            // Turns off the mesh and colliders for the current game object and turns on a half sized block
            Debug.Log("Key S or Down Arrow is being pressed.");
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            crouchedPlayer.GetComponent<Renderer>().enabled = true;
            crouchedPlayer.GetComponent<BoxCollider>().enabled = true;
            playerCrouched = true;
        }
        else if (!underObject)
        {
            // Reverses the previous changes after the player is no longer in an area that they can only be crouched
            this.gameObject.GetComponent<Renderer>().enabled = true;
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
            crouchedPlayer.GetComponent<Renderer>().enabled = false;
            crouchedPlayer.GetComponent<BoxCollider>().enabled = false;
            playerCrouched = false;
        }

        // Handles player hiding spots 
        if (Input.GetKey(KeyCode.E) && hidingAvailible)
        {
            Debug.Log("Key E is being pressed.");
            playerHiding = true;
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }
        else if (playerHiding && !hidingAvailible)
        {
            Debug.Log("Key E is being released.");
            playerHiding = false;
            this.gameObject.GetComponent<Renderer>().enabled = true;
        }

        // Checks for environmental triggers for various jumping elements
        if (GameObject.Find("Ground_Check").GetComponent<Ground_Check_Script>().Ground)
        {
            hasJumpedFromGround = false;
        }
        else
        {
            hasJumpedFromGround = true;
        }

        if (GameObject.Find("LeftWall_Check").GetComponent<Wall_Check_Script>().Left)
        {
            wallLeft = true;
        }
        else
        {
            wallLeft = false;
        }

        if (GameObject.Find("RightWall_Check").GetComponent<Wall_Check_Script>().Right)
        {
            wallRight = true;
        }
        else
        {
            wallRight = false;
        }

        // Checks for ceiling to uncrouch player
        if (GameObject.Find("Ceiling_Check").GetComponent<Ceiling_Check_Script>().Ceiling)
        {
            underObject = true;
        }
        else
        {
            underObject = false;
        }

        if (caughtInLight)
        {
            timer = Time.deltaTime;
            if (timer <= lightDeathTimer)
            {
                //Code for particle effects and opacity changes
                return;
            }
            else if (timer >= lightDeathTimer)
            {
                //Code for death animation into spawn animation at the last checkpoint

            }
        }

        if (respawnPlayer)
        {
            respawnPlayer = false;
            curPlayerSpd = 0;
            rBody.velocity = Vector3.zero;
            rBody.angularVelocity = Vector3.zero;
            this.transform.position = respawnPoint.position;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Climbable")
        {
            allowClimbing = true;
        }
        else if (other.tag == "HidingSpot")
        {
            hidingAvailible = true;
        }
        else if (other.tag == "Light")
        {
            caughtInLight = true;
        }
        else if (other.tag == "Enemy")
        {
            return;
        }
        else if (other.tag == "Checkpoint")
        {
            respawnPoint.position = other.transform.position;
        }
        else if (other.tag == "Deathzone")
        {
            respawnPlayer = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Climbable")
        {
            allowClimbing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Climbable")
        {
            allowClimbing = false;
            curClimbingSpeed = 0;
            rBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else if (other.tag == "HidingSpot")
        {
            hidingAvailible = false;
        }
        else if (other.tag == "Light")
        {
            caughtInLight = false;
            timer = 0;
        }
    }
}
