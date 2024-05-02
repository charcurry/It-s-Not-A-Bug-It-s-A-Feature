using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExampleCodingStyle : MonoBehaviour
{
    // All comments must start with a spaced capital
    // Ex:
    // You look pretty cool
    // I am lost please help
    // Wow thats crazy

    // Almost all variables must Start with lowercase letter, and rather than a space just capitalize the next word
    // Ex:
    float score;
    bool canSwim;
    GameObject player;
    Vector3 playerPosition;

    // Don't make variables public if they don't have to be
    // Ex:

    // If you want something to show in the inspector but doesn't have to be public, use
    [SerializeField] private float maxSpeed;
    // Not
    public float minSpeed;

    // A good rule of thumb if its not being used/changed by another script, it probOnly use public if its being used or changed by another script

    // Small tip: if you need something public but don't want it in the inspector, use
    [HideInInspector] public int health;


    // Start and Update don't need their comments
    void Start()
    {
        
    }

}
