using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Shared_UXVariables uxVariables;

    private GameObject playerCamera;
    private Rigidbody rb;
    private Camera playerCameraComponent;
    private PlayerTrigger groundTrigger;
    private PlayerTrigger headTrigger;
    private Rigidbody heldObject;
    private GameObject heldObjectPoint;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private float accelerationBaseValue;
    private float maxSpeedBaseValue;
    private float moveLeftRight;
    private float moveForwardBackward;
    private float jumpTimeStamp;
    private float heldObjectDistanceCurrent;

    private bool isGrounded;
    private bool isUnderObject;
    private bool isCrouching;
    private bool upPressed;
    private bool downPressed;
    private bool rightPressed;
    private bool leftPressed;
    private bool sprintPressed;
    private bool jumpPressed;
    private bool interactPressed;
    private bool crouchPressed;
    private bool holdingObject;
    private bool wasGroundedLastFrame;

    private double speedXZ;

    private RaycastHit hit;

    // Debug
    private bool doesUXVariablesExist = true;

    [Header("Movement Properties")]
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float decelerationMultiplier;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float extraGravity;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float crouchMultiplier;

    [Header("Size Properties")]
    [SerializeField] private float playerHeight;
    [SerializeField] private float cameraHeight;
    [SerializeField] private float playerCrouchHeight;
    [SerializeField] private float cameraCrouchHeight;
    [SerializeField] private float headTriggerOffset;

    [Header("Interaction Properties")]
    [SerializeField] private float interactDistance;
    [SerializeField] private float heldObjectDistanceDefault;
    [SerializeField] private float heldObjectDistanceMin;
    [SerializeField] private float heldObjectDistanceMax;
    [SerializeField] private float scollSensitivity;
    [SerializeField] private float heldObjectDampenFactor;
    [SerializeField] private float heldObjectPull;

    [Header("Miscellaneous Properties")]
    [SerializeField] private float dynamicFOVRateOfChange;
    [SerializeField] private bool renderPlayerMesh;
    [SerializeField] private bool renderHeldObjectPoint;

    [HideInInspector] public bool canControl = true;
    [HideInInspector] public bool disableRegularForce = false;

    // On Awake it initializes the sound settings.
    // THIS DOES NOT HAVE TO BE IN THIS FILE, IT CAN BE IN ANYTHING THAT EXISTS IN EVERY SCENE
    public void Awake()
    {
        GameAssets.i.InitializeSoundSettings();
        SoundManager.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("UX_Main") != null)
            uxVariables = GameObject.Find("UX_Main").GetComponent<Shared_UXVariables>();
        else
            doesUXVariablesExist = false;

        isGrounded = true;
        wasGroundedLastFrame = true;
        isUnderObject = false;
        isCrouching = false;

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        playerCamera = transform.GetChild(0).gameObject;
        playerCameraComponent = playerCamera.GetComponent<Camera>();
        groundTrigger = transform.GetChild(1).GetComponent<PlayerTrigger>();
        headTrigger = transform.GetChild(2).GetComponent<PlayerTrigger>();
        heldObjectPoint = transform.GetChild(3).gameObject;

        if (doesUXVariablesExist)
            playerCamera.GetComponent<CameraController>().mouseSensitivity = uxVariables.flMouseSensitivity;
        else
            playerCamera.GetComponent<CameraController>().mouseSensitivity = 1;

        headTrigger.transform.localPosition = new Vector3(0, (playerHeight - 1) + headTriggerOffset, 0);

        accelerationBaseValue = acceleration;
        maxSpeedBaseValue = maxSpeed;
        heldObjectDistanceCurrent = heldObjectDistanceDefault;

        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, (transform.position.y - 1) + cameraHeight, playerCamera.transform.position.z);
        transform.GetComponent<CapsuleCollider>().height = playerHeight;
        transform.GetComponent<CapsuleCollider>().center = new Vector3(0, (playerHeight / 2) - 1, 0);

        upPressed = false;
        downPressed = false;
        rightPressed = false;
        leftPressed = false;
        sprintPressed = false;
        jumpPressed = false;
        interactPressed = false;
        crouchPressed = false;
        holdingObject = false;

        jumpTimeStamp = -0.2f;

        if (!renderPlayerMesh)
            transform.GetComponent<MeshRenderer>().forceRenderingOff = true;

        if (!renderHeldObjectPoint)
            heldObjectPoint.transform.GetComponent<MeshRenderer>().forceRenderingOff = true;
    }


    private void Update()
    {
        // Grabs player input and stores it
        if (canControl)
        {
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

            if (Input.GetKeyDown(KeyCode.Mouse0))
                interactPressed = true;

            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
                crouchPressed = true;

            // Takes scroll wheel input and uses it to move held objects closer or further depending on the direction you scroll the wheel
            heldObjectDistanceCurrent = Mathf.Clamp(heldObjectDistanceCurrent + Input.mouseScrollDelta.y * scollSensitivity, heldObjectDistanceMin, heldObjectDistanceMax);
        }

        heldObjectPoint.transform.position = playerCamera.transform.position + (playerCamera.transform.forward * heldObjectDistanceCurrent);

        if (doesUXVariablesExist)
            playerCamera.GetComponent<CameraController>().mouseSensitivity = uxVariables.flMouseSensitivity;

        // If the player is on the ground and moving, play the walking sound.
        if (isGrounded && (upPressed || downPressed || rightPressed || leftPressed))
        {
            SoundManager.PlaySound(SoundManager.Sound.Player_Move, transform.position);
        }
    }

    void FixedUpdate()
    {
        // Calls on GroundTrigger to find out whether or not the player is grounded (:
        if (jumpTimeStamp + 0.2f < Time.time)
            isGrounded = groundTrigger.isObjectHere;

        // Calls on HeadTrigger to find out whether or not the player is under an object
        isUnderObject = headTrigger.isObjectHere;

        // Adds a constant downward force on the player
        rb.AddRelativeForce(new Vector3(0, -extraGravity * Time.deltaTime, 0));

        if (doesUXVariablesExist)
            uxVariables.bIsInteracting = false;

        if (!holdingObject)
        {
            // Shoots a raycast out in the direction the player is looking
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactDistance, ~(1 << 6)))
            {
                // Checks if the raycast hits an object with the Interactable parent script
                if (hit.collider.GetComponent<Interactable>())
                {
                    if (doesUXVariablesExist)
                        uxVariables.bIsInteracting = true;

                    if (interactPressed)
                    {
                        hit.collider.GetComponent<Interactable>().interaction();

                        if (hit.collider.GetComponent<Interactable>().pickupable)
                        {
                            heldObject = hit.collider.GetComponent<Rigidbody>();
                            holdingObject = true;
                        }
                    }
                }
            }
        }
        else
        {
            if (interactPressed)
                holdingObject = false;
        }

        // If an object has been interacted with and has the pickupable bool on, the object will be sucked towards a point in front of the player
        if (holdingObject && heldObject != null && heldObject.GetComponent<Interactable>().pickupable)
        {
            heldObject.GetComponent<Interactable>().isPickedUp = true;
            heldObject.useGravity = false;
            float objectPointDistance = Vector3.Distance(heldObjectPoint.transform.position, heldObject.transform.position);
            heldObject.velocity *= heldObjectDampenFactor * Mathf.Clamp(objectPointDistance * 5, 0.5f, 1);
            heldObject.angularVelocity *= 0.9f;
            // Direction * deltatime * distance^2 * pull
            heldObject.AddForce((heldObjectPoint.transform.position - heldObject.transform.position).normalized * Time.deltaTime * 1000 * heldObjectPull * Mathf.Pow(objectPointDistance, 2));
        }
        else
        {
            // If nothing is being held/the player drops the object, decouple the heldObject from the heldPoint
            if (heldObject != null)
            {
                heldObject.useGravity = true;
                heldObject.GetComponent<Interactable>().isPickedUp = false;
            }

            holdingObject = false;
            heldObjectDistanceCurrent = heldObjectDistanceDefault;
            heldObject = null;
        }

        interactPressed = false;

        // Set acceleration and max speed back to normal after sprint and crouch
        acceleration = accelerationBaseValue;
        maxSpeed = maxSpeedBaseValue;

        // If sprint is input, up both accelertion and speed
        if (sprintPressed && !isCrouching && (upPressed || leftPressed || rightPressed))
        {
            sprintPressed = false;
            maxSpeed = maxSpeed * sprintMultiplier;
            acceleration = acceleration * sprintMultiplier;
            // If the player is sprinting, the playerMoveTimerMax is halved to make the walking sound play more frequently. (more steps = faster walking sound)
            SoundManager.playerMoveTimerMax = SoundManager.defaultPlayerMoveTimerMax / 2f;
        }
        else if (isCrouching && (upPressed || leftPressed || rightPressed || downPressed))
        {
            // If the player is crouching, the playerMoveTimerMax is increased to make the walking sound play less frequently. (less steps = slower walking sound)
            SoundManager.playerMoveTimerMax = SoundManager.defaultPlayerMoveTimerMax / 0.6f;
        }
        else
        {
            // If the player is not sprinting, the playerMoveTimerMax is set back to the default value.
            SoundManager.playerMoveTimerMax = SoundManager.defaultPlayerMoveTimerMax;
        }


        // If crouch is input, lower the camera and shorten the collider of the player
        if (isGrounded && crouchPressed && !jumpPressed || isUnderObject && isCrouching)
        {
            crouchPressed = false;
            isCrouching = true;
            // Reduces player height, camera height, and reduces speed
            playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, Mathf.Lerp(playerCamera.transform.position.y, (transform.position.y - 1) + cameraCrouchHeight, Time.deltaTime * 10), playerCamera.transform.position.z);
            transform.GetComponent<CapsuleCollider>().height = playerCrouchHeight;
            transform.GetComponent<CapsuleCollider>().center = new Vector3(0, (playerCrouchHeight / 2) - 1, 0);
            maxSpeed = maxSpeed * crouchMultiplier;
            acceleration = acceleration * crouchMultiplier;
        }
        else
        {
            isCrouching = false;
            // Sets player and camera height back to the default
            playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, Mathf.Lerp(playerCamera.transform.position.y, (transform.position.y - 1) + cameraHeight, Time.deltaTime * 10), playerCamera.transform.position.z);
            transform.GetComponent<CapsuleCollider>().height = playerHeight;
            transform.GetComponent<CapsuleCollider>().center = new Vector3(0, (playerHeight / 2) - 1, 0);
        }

        // Bug Fix
        if (sprintPressed && crouchPressed)
        {
            sprintPressed = false;
            crouchPressed = false;
        }

        // Resets force adding variables to zero for the next add force
        moveForwardBackward = 0;
        moveLeftRight = 0;

        // Turns WASD input into the add force variables 
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

        speedXZ = Math.Sqrt(Math.Pow(rb.velocity.x, 2) + Math.Pow(rb.velocity.z, 2));

        if (!disableRegularForce)
        {
            // Uses the add force variables to add force as well as capping the speed
            if (speedXZ < maxSpeed)
                rb.AddRelativeForce(new Vector3(moveLeftRight, 0, moveForwardBackward).normalized * Time.deltaTime * 1000 * acceleration);

            // Increases deceleration to prevent sliding
            rb.velocity = new Vector3 (rb.velocity.x * decelerationMultiplier, rb.velocity.y, rb.velocity.z * decelerationMultiplier);
        }

        // If the player is on the ground and pressed jump then add force to the y for a jump
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(new Vector3(0.0f, jumpStrength, 0.0f), ForceMode.Impulse);
            SoundManager.PlaySound(SoundManager.Sound.Player_Jump, transform.position);
            jumpTimeStamp = Time.time;
            isGrounded = false;
        }

        jumpPressed = false;

        // Changes fov based on speed
        if (doesUXVariablesExist)
        {
            if (uxVariables.bDynamicFov)
                playerCameraComponent.fieldOfView = Mathf.Lerp(playerCameraComponent.fieldOfView, 60 + (15 * Mathf.Clamp01(((float)speedXZ - 2) / ((maxSpeedBaseValue * sprintMultiplier * 1.2f) - 2))), Time.deltaTime * dynamicFOVRateOfChange);
        }
        else
            playerCameraComponent.fieldOfView = Mathf.Lerp(playerCameraComponent.fieldOfView, 60 + (15 * Mathf.Clamp01(((float)speedXZ - 2) / ((maxSpeedBaseValue * sprintMultiplier * 1.2f) - 2))), Time.deltaTime * dynamicFOVRateOfChange);

        if (!wasGroundedLastFrame && isGrounded)
        {

            SoundManager.PlaySound(SoundManager.Sound.Jump_Landing, transform.position);
        }

        wasGroundedLastFrame = isGrounded;

    }

    public void Kill()
    {
        // Makes the player drop what their holding, if anything
        if (heldObject != null)
        {
            heldObject.useGravity = true;
            heldObject.GetComponent<Interactable>().isPickedUp = false;
            holdingObject = false;
            heldObjectDistanceCurrent = heldObjectDistanceDefault;
            heldObject = null;
        }

        // Player position gets put back to their spawn point
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        playerCamera.GetComponent<CameraController>().xRotation = 0;
    }

    // Lets other scripts change the players spawn point
    public void SetRespawn(Vector3 position, float facingDirection = 0)
    {
        spawnPosition = position;
        spawnRotation = Quaternion.Euler(0, facingDirection, 0);
    }
}
