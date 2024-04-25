using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    GameObject pCamera;
    Rigidbody rb;
    Camera pCameraComponent;
    PlayerTrigger groundTrigger;

    /*enum SoundState {walking, running, sliding, sneaking}
    SoundState soundState;*/

    float accelerationBaseValue;
    float maxSpeedBaseValue;
    float moveLeftRight;
    float moveForwardBackward;
    float jumpTimeStamp;
    //float audioTimeDelay;

    bool isGrounded;
    bool upPressed;
    bool downPressed;
    bool rightPressed;
    bool leftPressed;
    bool sprintPressed;
    bool jumpPressed;
    bool interactPressed;
    bool crouchPressed;
    bool disableRegularForce;
    //bool overideNextSound;

    double speedXZ;

    //RaycastHit hit;

    //System.Random RNG = new System.Random();

    [Header("Properties")]
    public float acceleration;
    public float maxSpeed;
    public float jumpStrength;
    public float sprintMultiplier;
    public float crouchMultiplier;
    public float extraGravity;

    /*[Header("Sounds")]
    public AudioClip step1;
    public AudioClip step2;
    public AudioClip step3;
    public AudioClip step4;
    public AudioClip sliding;*/



    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true;

        rb = GetComponent<Rigidbody>();
        pCamera = transform.GetChild(0).gameObject;
        pCameraComponent = pCamera.GetComponent<Camera>();
        groundTrigger = transform.GetChild(1).GetComponent<PlayerTrigger>();

        accelerationBaseValue = acceleration;
        maxSpeedBaseValue = maxSpeed;

        //overideNextSound = false;

        upPressed = false;
        downPressed = false;
        rightPressed = false;
        leftPressed = false;
        sprintPressed = false;
        jumpPressed = false;
        interactPressed = false;
        crouchPressed = false;

        jumpTimeStamp = -0.2f;
    }


    private void Update()
    {
        // Grabs player input and stores it
        if (Input.GetKey(KeyCode.W))
            upPressed = true;

        if (Input.GetKey(KeyCode.S))
            downPressed = true;

        if (Input.GetKey(KeyCode.D))
            rightPressed = true;

        if (Input.GetKey(KeyCode.A))
            leftPressed = true;

        if (Input.GetKey(KeyCode.LeftShift))
            sprintPressed = true;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;

        if (Input.GetKeyDown(KeyCode.E))
            interactPressed = true;

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
            crouchPressed = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (jumpTimeStamp + 0.2f < Time.time)
            isGrounded = groundTrigger.isObjectHere;

        speedXZ = Math.Sqrt(Math.Pow(rb.velocity.x, 2) + Math.Pow(rb.velocity.z, 2));

        disableRegularForce = false;
        
        //set acceleration and max speed back to normal after sprint
        acceleration = accelerationBaseValue;
        maxSpeed = maxSpeedBaseValue;

        //soundState = SoundState.walking;

        // Adds a constant downward force on the player
        rb.AddRelativeForce(new Vector3(0, -extraGravity * Time.deltaTime, 0));

        // If sprint is input, up both accelertion and speed
        if (sprintPressed && !crouchPressed && (upPressed || leftPressed || rightPressed))
        {
            sprintPressed = false;
            //soundState = SoundState.running;
            maxSpeed = maxSpeed * sprintMultiplier;
            acceleration = acceleration * sprintMultiplier;
        }

        // If crouch is input, lower the camera and shorten the collider of the player
        if (isGrounded && crouchPressed && !jumpPressed)
        {
            crouchPressed = false;

            // Reduces player and camera height
            pCamera.transform.position = new Vector3(pCamera.transform.position.x, Mathf.Lerp(pCamera.transform.position.y, transform.position.y + 0.2f, Time.deltaTime * 10), pCamera.transform.position.z);
            transform.GetComponent<CapsuleCollider>().height = 1;
            transform.GetComponent<CapsuleCollider>().center = new Vector3(0, -0.5f, 0);
            maxSpeed = maxSpeed * crouchMultiplier;
            acceleration = acceleration * crouchMultiplier;

        }
        else
        {
            // Sets player and camera height back to the default
            pCamera.transform.position = new Vector3(pCamera.transform.position.x, Mathf.Lerp(pCamera.transform.position.y, transform.position.y + 0.6f, Time.deltaTime * 10), pCamera.transform.position.z);
            transform.GetComponent<CapsuleCollider>().height = 2;
            transform.GetComponent<CapsuleCollider>().center = Vector3.zero;
        }

        // Bug Fix
        if (sprintPressed && crouchPressed)
        {
            sprintPressed = false;
            crouchPressed = false;
        }

        //SoundManager();

        // Resets force adding variables to zero for the next add force
        moveForwardBackward = 0;
        moveLeftRight = 0;

        // Turns WASD Input into the add force variables 
        if (upPressed)
        {
            moveForwardBackward += 1;
            upPressed = false;
        }
        if (downPressed)
        {
            moveForwardBackward -= 1;
            downPressed = false;
        }
        if (rightPressed)
        {
            moveLeftRight += 1;
            rightPressed = false;
        }
        if (leftPressed)
        {
            moveLeftRight -= 1;
            leftPressed = false;
        }

        if (!disableRegularForce)
        {
            // Uses the add force variables to add force as well as capping the speed
            if (speedXZ < maxSpeed)
                rb.AddRelativeForce(new Vector3(moveLeftRight, 0, moveForwardBackward).normalized * Time.deltaTime * 1000 * acceleration);

            // Increases deceleration to prevent sliding
            rb.velocity = new Vector3 (rb.velocity.x * 0.9f, rb.velocity.y, rb.velocity.z * 0.9f);
        }

        // If the player is on the ground and pressed jump then add force to the y for a jump
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(new Vector3(0.0f, jumpStrength, 0.0f), ForceMode.Impulse);
            jumpTimeStamp = Time.time;
        }

        jumpPressed = false;

        // Changes fov based on speed
        pCameraComponent.fieldOfView = Mathf.Lerp(pCameraComponent.fieldOfView, 60 + (15 * Mathf.Clamp01(((float)speedXZ - 2) / ((maxSpeedBaseValue * sprintMultiplier * 1.2f) - 2))), Time.deltaTime * 2);
    

    }

    // Takes a Y rotation and sets the player's camera to that direction
    public void RecenterCamera(float yRotation = 0)
    {
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        pCamera.GetComponent<CameraController>().xRotation = 0;
    }

    // Gives player postition but helps account for crouching
    public Vector3 GetPosition()
    {
        if (isGrounded && crouchPressed && !sprintPressed)
            return (transform.position + new Vector3(0, -0.5f, 0));
        else
            return (transform.position + new Vector3(0, 0.5f, 0));
    }

    /*public void SoundManager()
    {
        audioTimeDelay = 0;

        switch (soundState)
        {
            case SoundState.walking:

                if ((upPressed || downPressed || rightPressed || leftPressed) && isGrounded)
                {
                    audioTimeDelay = 0.4f;
                    GetComponent<AudioSource>().volume = 0.42f;
                    PlaySound(FootSteps());
                }

                break;

            case SoundState.running:

                if ((upPressed || downPressed || rightPressed || leftPressed) && isGrounded)
                {
                    audioTimeDelay = 0.2f;
                    GetComponent<AudioSource>().volume = 0.5f;
                    PlaySound(FootSteps());
                }

                break;

            case SoundState.sneaking:

                if ((upPressed || downPressed || rightPressed || leftPressed) && isGrounded)
                {
                    audioTimeDelay = 0.7f;
                    GetComponent<AudioSource>().volume = 0.26f;
                    PlaySound(FootSteps());
                }

                break;

            case SoundState.sliding:

                GetComponent<AudioSource>().volume = 0.42f;
                if (GetComponent<AudioSource>().clip != sliding)
                    PlaySound(sliding, true, true);
                break;
        }


        void PlaySound(AudioClip audioToPlay, bool overideCurrentSound = false, bool overideNextSound = false)
        {
            if (overideNextSound)
                this.overideNextSound = true;

            if (overideCurrentSound)
            {
                GetComponent<AudioSource>().clip = audioToPlay;
                GetComponent<AudioSource>().Play();
            }
            else
            {
                if ((!GetComponent<AudioSource>().isPlaying && timeStampTwo + audioTimeDelay < Time.time) || this.overideNextSound)
                {
                    timeStampTwo = Time.time;
                    this.overideNextSound = false;
                    GetComponent<AudioSource>().clip = audioToPlay;
                    GetComponent<AudioSource>().Play();
                }
            }
        }


        AudioClip FootSteps()
        {
            AudioClip stepSoundToPlay = step1;

            switch (RNG.Next(1, 5))
            {
                case 1:
                    stepSoundToPlay = step1;
                    break;

                case 2:
                    stepSoundToPlay = step2;
                    break;

                case 3:
                    stepSoundToPlay = step3;
                    break;

                case 4:
                    stepSoundToPlay = step4;
                    break;
            }

            if (stepSoundToPlay == GetComponent<AudioSource>().clip)
            {
                stepSoundToPlay = step1;
                if (stepSoundToPlay == GetComponent<AudioSource>().clip)
                    stepSoundToPlay = step2;
            }

            return stepSoundToPlay;
        }
    }*/
}
