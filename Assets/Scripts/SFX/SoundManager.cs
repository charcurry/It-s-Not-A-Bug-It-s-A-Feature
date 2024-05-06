using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        PlayerJump,
        PlayerDeath
    }

    public static void PlaySound(string soundName, GameObject gameObject)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/" + soundName));
        soundGameObject.transform.position = gameObject.transform.position;
        Object.Destroy(soundGameObject, audioSource.clip.length);
    }
}
