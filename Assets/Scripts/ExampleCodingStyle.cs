using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExampleCodingStyle : MonoBehaviour
{
    // All comments must start with a spaced capital
    // Ex:
    // You look pretty cool
    // This is a cool comment
    // Wow thats crazy

    // Almost all variables must Start with lowercase letter, and rather than a space just capitalize the next word
    // Ex:
    float score;
    bool canSwim;
    GameObject player;
    Vector3 playerPosition;

    // Don't make variables public if they don't have to be
    // If you want something to show in the inspector but doesn't have to be public, use
    [SerializeField] private float maxSpeed;
    // Not
    public float minSpeed;

    // A good rule of thumb is if its not being used/changed by another script, it probably shouldn't be public

    // Small tip: if you need something public but don't want it in the inspector, use
    [HideInInspector] public int health;

    // If you want to have a changeable setting for a script in the inspector, make sure it has the Header "Properties"
    // Ex:
    [Header("Properties")]
    [SerializeField] private bool maxHealth;

    // And if you want to have a spot in the inspector to put a reference to another object/script, make sure it has the Header "References"
    // Ex:
    [Header("References")]
    [SerializeField] private GameObject playerCamera;

    // If you make an enum the first letter must be a capital and each state name inside must also start with a capital
    // State names must use underscores to separate words in the name
    // Ex:
    enum MovementState
    {
        Walking,
        Crouch_Walking,
        Sprinting,
        Crouch_Sprinting
    }

    // For constants use all caps and underscores for separating words
    // Ex:
    float PLAYER_SPEED;

    // All Methods Should follow the same guide lines as Variables but the must start with a capital letter and each method needs to be spaced one line apart
    // Ex:
    void GetPosition() { }

    void CheckIfAlive() { }

    // Don't keep Start() or Update(), if they are not being used
    void Start()
    {
        
    }

    // That should be all for now, thank you for reading!
}
