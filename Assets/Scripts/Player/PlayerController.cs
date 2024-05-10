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

    // References
    private GameObject playerCamera;
    private Rigidbody rb;
    private Camera playerCameraComponent;
    private PlayerTrigger groundTrigger;
    private PlayerTrigger headTrigger;
    private Rigidbody heldObject;
    private GameObject heldObjectPoint;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private Vector3 movementVector;

    private float accelerationBaseValue;
    private float maxSpeedBaseValue;
    private float jumpTimeStamp;
    private float heldObjectDistanceCurrent;

    private bool isGrounded;
    private bool wasGroundedLastFrame;
    private bool isUnderObject;
    private bool isCrouching;
    private bool isSprinting;
    private bool isMoving;
    private bool isHoldingObject;
    private bool willJump;

    private bool upPressed;
    private bool downPressed;
    private bool rightPressed;
    private bool leftPressed;
    private bool sprintPressed;
    private bool jumpPressed;
    private bool interactPressed;
    private bool crouchPressed;

    private double speedXZ;

    private RaycastHit hit;

    // Debug
    private bool doesUXVariablesExist = true;

    [Header("Movement Properties")]
    [SerializeField] private float acceleration = 90;
    [SerializeField] private float maxSpeed = 120;
    [SerializeField] private float decelerationMultiplier = 0.8f;
    [SerializeField] private float jumpStrength = 23;
    [SerializeField] public float gravityMultiplier = 3;
    [SerializeField] private float sprintMultiplier = 1.7f;
    [SerializeField] private float crouchMultiplier = 0.6f;

    [Header("Size Properties")]
    [SerializeField] private float playerHeight = 2;
    [SerializeField] private float cameraHeight = 1.6f;
    [SerializeField] private float playerCrouchHeight = 1.3f;
    [SerializeField] private float cameraCrouchHeight = 1;
    [SerializeField] private float headTriggerOffset = -0.055f;

    [Header("Interaction Properties")]
    [SerializeField] private float interactDistance = 3;
    [SerializeField] private float heldObjectDistanceDefault = 3;
    [SerializeField] private float heldObjectDistanceMin = 1;
    [SerializeField] private float heldObjectDistanceMax = 5;
    [SerializeField] private float scollSensitivity = 0.5f;
    [SerializeField] private float heldObjectDampenFactor = 0.8f;
    [SerializeField] private float heldObjectPull = 45;

    [Header("Miscellaneous Properties")]
    [SerializeField] private float dynamicFOVRateOfChange = 10;
    [SerializeField] private bool renderPlayerMesh = true;
    [SerializeField] private bool renderHeldObjectPoint = false;

    [HideInInspector] public bool canControl = true;

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
        isMoving = false;
        isCrouching = false;
        isSprinting = false;
        isHoldingObject = false;
        willJump = false;

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.drag = 2f;  
        rb.angularDrag = 2f;
        rb.mass = 2f;

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

        ManageInputs(true);

        heldObject = null;

        jumpTimeStamp = -0.2f;

        if (!renderPlayerMesh)
            transform.GetComponent<MeshRenderer>().forceRenderingOff = true;

        if (!renderHeldObjectPoint)
            heldObjectPoint.transform.GetComponent<MeshRenderer>().forceRenderingOff = true;
    }


    private void Update()
    {
        ManageInputs();

        // Sets the position of the heldObjectPoint
        heldObjectPoint.transform.position = playerCamera.transform.position + (playerCamera.transform.forward * heldObjectDistanceCurrent);

        if (doesUXVariablesExist)
            playerCamera.GetComponent<CameraController>().mouseSensitivity = uxVariables.flMouseSensitivity;

        // Calls on GroundTrigger to find out whether or not the player is grounded (:
        if (jumpTimeStamp + 0.2f < Time.time)
            isGrounded = groundTrigger.isObjectHere;

        // Calls on HeadTrigger to find out whether or not the player is under an object
        isUnderObject = headTrigger.isObjectHere;
        
        ObjectInteraction();

        ChangeMovementState();

        SetMovementVector();

        PlaySounds();

        speedXZ = Math.Sqrt(Math.Pow(rb.velocity.x, 2) + Math.Pow(rb.velocity.z, 2));

        // Changes fov based on speed
        if (doesUXVariablesExist && uxVariables.bDynamicFov || !doesUXVariablesExist)
            playerCameraComponent.fieldOfView = Mathf.Lerp(playerCameraComponent.fieldOfView, 60 + (15 * Mathf.Clamp01(((float)speedXZ - 2) / ((maxSpeedBaseValue * sprintMultiplier * 1.2f) - 2))), Time.deltaTime * dynamicFOVRateOfChange);

        ManageInputs(true);

        wasGroundedLastFrame = isGrounded;
    }

    void FixedUpdate()
    {
        // Adds additional gravity on the player
        rb.AddRelativeForce(Physics.gravity * (gravityMultiplier - 1), ForceMode.Acceleration);

        // If an object has been interacted with and has the pickupable bool on, the object will be sucked towards a point in front of the player
        if (isHoldingObject)
        {
            float objectPointDistance = Vector3.Distance(heldObjectPoint.transform.position, heldObject.transform.position);
            heldObject.velocity *= heldObjectDampenFactor * Mathf.Clamp(objectPointDistance * 5, 0.5f, 1);
            heldObject.angularVelocity *= 0.9f;
            // Direction * distance^2 * pull
            heldObject.AddForce((heldObjectPoint.transform.position - heldObject.transform.position).normalized * heldObjectPull * Mathf.Pow(objectPointDistance, 2));
        }

        // If the player is on the ground and pressed jump then add force to the y for a jump
        if (willJump)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            willJump = false;
        }

        // Uses the add force variables to add force as well as capping the speed
        if (speedXZ < maxSpeed)
            rb.AddRelativeForce(movementVector);

        // Increases deceleration to prevent sliding
        rb.velocity = new Vector3(rb.velocity.x * decelerationMultiplier, rb.velocity.y, rb.velocity.z * decelerationMultiplier);
    }

    private void ObjectInteraction()
    {
        if (doesUXVariablesExist)
            uxVariables.bIsInteracting = false;

        if (!isHoldingObject)
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
                            heldObject.GetComponent<Interactable>().isPickedUp = true;
                            heldObject.useGravity = false;
                            isHoldingObject = true;
                        }
                    }
                }
            }
        }
        else if (interactPressed)
            isHoldingObject = false;
        
        // If nothing is being held/the player drops the object, decouple the heldObject from the heldObjectPoint
        if (heldObject == null || !isHoldingObject || !heldObject.GetComponent<Interactable>().pickupable)
        {
            if (heldObject != null)
            {
                heldObject.useGravity = true;
                heldObject.GetComponent<Interactable>().isPickedUp = false;
            }

            isHoldingObject = false;
            heldObjectDistanceCurrent = heldObjectDistanceDefault;
            heldObject = null;
        }
    }

    private void ChangeMovementState()
    {
        // Set acceleration and max speed back to normal after sprint and crouch
        acceleration = accelerationBaseValue;
        maxSpeed = maxSpeedBaseValue;

        // If sprint is input, up both accelertion and speed
        if (sprintPressed && !isCrouching && (upPressed || leftPressed || rightPressed))
        {
            isSprinting = true;
            maxSpeed = maxSpeed * sprintMultiplier;
            acceleration = acceleration * sprintMultiplier;
        }
        else
            isSprinting = false;

        // If crouch is input, lower the camera and shorten the collider of the player
        if (isGrounded && crouchPressed && !jumpPressed || isUnderObject && isCrouching && !isSprinting)
        {
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

        // If the player is on the ground and pressed jump then add force to the y for a jump
        if (jumpPressed && isGrounded && !isCrouching)
        {
            willJump = true;
            jumpTimeStamp = Time.time;
            isGrounded = false;
        }
    }

    private void SetMovementVector()
    {
        // Resets force adding variables to zero for the next add force
        float moveForwardBackward = 0;
        float moveLeftRight = 0;

        // Turns WASD input into the add force variables 
        if (upPressed)
            moveForwardBackward += 1;

        if (downPressed)
            moveForwardBackward -= 1;

        if (rightPressed)
            moveLeftRight += 1;

        if (leftPressed)
            moveLeftRight -= 1;

        if (moveForwardBackward != 0 || moveLeftRight != 0)
            isMoving = true;
        else
            isMoving = false;

         movementVector = new Vector3(moveLeftRight, 0, moveForwardBackward).normalized * acceleration;
    }

    private void PlaySounds()
    {
        // If the player is on the ground and moving, play the walking sound.
        if (isGrounded && isMoving)
        {
            // If the player is not sprinting, the playerMoveTimerMax is set back to the default value
            SoundManager.playerMoveTimerMax = SoundManager.defaultPlayerMoveTimerMax;

            // If the player is crouching, the playerMoveTimerMax is increased to make the walking sound play less frequently. (less steps = slower walking sound)
            if (isCrouching)
                SoundManager.playerMoveTimerMax = SoundManager.defaultPlayerMoveTimerMax / 0.6f;

            // If the player is sprinting, the playerMoveTimerMax is halved to make the walking sound play more frequently. (more steps = faster walking sound)
            if (isSprinting)
                SoundManager.playerMoveTimerMax = SoundManager.defaultPlayerMoveTimerMax / 1.7f;

            SoundManager.PlaySound(SoundManager.Sound.Player_Move, transform.position);
        }

        if (!wasGroundedLastFrame && isGrounded)
            SoundManager.PlaySound(SoundManager.Sound.Jump_Landing, transform.position);

        if (jumpPressed && (jumpTimeStamp == Time.time) && !isCrouching)
            SoundManager.PlaySound(SoundManager.Sound.Player_Jump, transform.position);
    }

    private void ManageInputs(bool resetInputs = false)
    {
        if (!resetInputs)
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
        }
        else
        {
            upPressed = false;
            downPressed = false;
            rightPressed = false;
            leftPressed = false;
            sprintPressed = false;
            jumpPressed = false;
            interactPressed = false;
            crouchPressed = false;
        }
    }

    public void Kill()
    {
        // Makes the player drop what their holding, if anything
        if (heldObject != null)
        {
            heldObject.useGravity = true;
            heldObject.GetComponent<Interactable>().isPickedUp = false;
            isHoldingObject = false;
            heldObjectDistanceCurrent = heldObjectDistanceDefault;
            heldObject = null;
        }

        // Player position gets put back to their spawn point
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        playerCamera.GetComponent<CameraController>().vecRelativeRotation.x = 0;
    }

    // Lets other scripts change the players spawn point
    public void SetRespawn(Vector3 position, float facingDirection = 0)
    {
        spawnPosition = position;
        spawnRotation = Quaternion.Euler(0, facingDirection, 0);
    }
}
