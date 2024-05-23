using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalPanel : Explodable
{
    [Header("References")]
    [SerializeField] private GameObject damagedPanel;
    [SerializeField] private OpenDoor secondDoor;
    [SerializeField] private Light secondDoorLight;
    [SerializeField] private LightFlicker lightFlicker;
    [SerializeField] private SequenceManager sequenceManager;
    [SerializeField] private AutoBarrelMaker autoBarrelMaker;

    public override void Explode()
    {
        // When exploded, change the panel model to broken, open the second door, flicker room lights, disable lights on floor buttons, and turn the light green on said door
        damagedPanel.transform.position = transform.position;
        damagedPanel.SetActive(true);
        secondDoor.Open();
        secondDoorLight.color = Color.green;
        lightFlicker.StartFlickering();
        sequenceManager.SetToDisabledMaterial();

        if (autoBarrelMaker != null)
        {
            autoBarrelMaker.enabled = false;
        }

        SoundManager.PlaySound(SoundManager.Sound.Breaker, transform.position);
        gameObject.SetActive(false);
    }
}
