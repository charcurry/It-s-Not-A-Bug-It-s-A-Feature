using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionC4 : MonoBehaviour
{
    [Header("Properties")]
    public float countdown;

    [Header("References")]
    [SerializeField] private TextMeshPro countdownText;
    [SerializeField] GameObject explosionEffect;

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        countdownText.text = Mathf.Max(0, countdown).ToString("F2");
        if(countdown <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        GameObject explosionInstance = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosionInstance, 1f);
        SoundManager.PlaySound(SoundManager.Sound.Explosion, transform.position);
        Destroy(gameObject);
        this.enabled = false;
    }
}
