using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAssets : MonoBehaviour
{
    // This is a singleton class that holds all the game assets.
    private static GameAssets _i;

    // This is a property that returns the singleton instance of the GameAssets class.
    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    // On Awake it initializes the sound settings.
    public void Awake()
    {
        InitializeSoundSettings();
    }

    // This dictionary is used to store the sound settings for each sound.
    public Dictionary<SoundManager.Sound, SoundSettings> soundSettingsDictionary = new Dictionary<SoundManager.Sound, SoundSettings>();
    // This array is used to store all the sound audio clips.
    public SoundAudioClip[] soundAudioClipArray;

    // This is the class that holds the sound and the audio clip.
    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    // This is the class that holds the sound settings.
    [Serializable]
    public class SoundSettings
    {
        public float maxDistance = 100f;
        public float dopplerLevel = 0f;
        public AudioRolloffMode audioRolloffMode = AudioRolloffMode.Logarithmic;
        public bool isLooped = false;
        public bool isMoving = false;
        public bool destroyAfterFinished = false;
    }

    // This method is used to initialize the sound settings for each sound.
    // If nothing is stated it uses the default settings.
    private void InitializeSoundSettings()
    {
        soundSettingsDictionary.Add(SoundManager.Sound.Pickup, new SoundSettings { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true });
        soundSettingsDictionary.Add(SoundManager.Sound.Checkpoint, new SoundSettings { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true });
        soundSettingsDictionary.Add(SoundManager.Sound.Moving_Platform, new SoundSettings { maxDistance = 25f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = true, isMoving = true, destroyAfterFinished = false });
        soundSettingsDictionary.Add(SoundManager.Sound.Player_Jump, new SoundSettings { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true });
    }

}