using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalPanel : Explodable
{
    [Header("References")]
    [SerializeField] private GameObject damagedPanel;
    [SerializeField] private OpenDoor secondDoor;
    [SerializeField] private Light secondDoorLight;

    public override void Explode()
    {
        // When exploded, change the panel model to broken, open the second door, and turn the light green on said door
        damagedPanel.transform.position = transform.position;
        damagedPanel.SetActive(true);
        secondDoor.Open();
        secondDoorLight.color = Color.green;
        SoundManager.PlaySound(SoundManager.Sound.Breaker, transform.position);

        gameObject.SetActive(false);
    }
}
