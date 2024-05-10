using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 10f;

    // Get rigidbody component and apply upward force instantly
    public void Start()
    {
        SoundManager.PlaySound(SoundManager.Sound.Air_Vent, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
