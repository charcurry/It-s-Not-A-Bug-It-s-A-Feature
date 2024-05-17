using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    // Lets you either set the respawn values manually or with a game object
    [Header("Respawn Point Set: Manual")]
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private float respawnFacingDirection;

    [Header("Respawn Point Set: GameObject")]
    [SerializeField] public Transform respawnPoint;

    [Header("Properties")]
    [SerializeField] private bool DestroyTriggerOnActivation = true;

    // If the player touches a trigger with this script, their respawn point gets changed to a new position
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (respawnPoint == null)
                other.GetComponent<PlayerController>().SetRespawn(respawnPosition, respawnFacingDirection);
            else
                other.GetComponent<PlayerController>().SetRespawn(respawnPoint.position, respawnPoint.rotation.eulerAngles.y);

            if (DestroyTriggerOnActivation)
                Destroy(gameObject);
        }
    }
}
