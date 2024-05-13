using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Object;
using UnityEngine.UIElements;
using static GameAssets;
using static Unity.VisualScripting.Member;

public static class SoundManager
{
    // This enum is used to store all the different sounds that can be played.
    // In order to add a new sound, you need to add it to this enum.
    // The sound name here has to be the same as the sound clip name placed in the GameAssets object.
    public enum Sound
    {
        Pickup,
        Checkpoint,
        Player_Move,
        Moving_Platform,
        Player_Jump,
        Jump_Landing,
        Background_Music_1,
        Background_Music_2,
        Background_Music_3,
        Correct_Sound,
        Incorrect_Sound,
        Puzzle_Solved,
        Explosion,
        Air_Vent,
        Door_Open_1,
        Door_Open_2,
        Box_Collision,
        Key_Collision,
        Unlocking_Door,
        Nail_Gun,
        Glass_Shattering,
        Timer_Beep,
    }

    // This float is used to determine how frequently the player can play the playerMove sound.
    public static readonly float defaultPlayerMoveTimerMax = 0.5f;
    // The PlayerController script sets the playerMoveTimerMax to its value.
    public static float playerMoveTimerMax;

    // This dictionary is used to store the last time a sound was played.
    private static Dictionary<Sound, float> soundTimerDictionary;

    // This method is used to initialize the soundTimerDictionary.
    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.Player_Move] = 0;
    }

    // This method is used to play a sound.
    public static void PlaySound(Sound sound, Vector3? position = null, GameObject movingObject = null)
    {   
        // This checks if a sound can be played. (check below to see how it works)
        if (CanPlaySound(sound))
        {
            // This creates a new game object with an audio source and puts the correct audio clip in the audioSource.
            GameObject soundGameObject = new(sound + "_Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);

            // This gets the sound settings from the GameAssets object for the correct sound.
            SoundSettings settings = i.soundSettingsDictionary[sound];

            // This checks if the sound is moving.
            // If it is, it sets the audio source's parent to the moving object.
            // This is so the sound can move with the object.
            if (settings.isMoving)
            {
                audioSource.transform.parent = movingObject.transform;
            }

            // This checks if the sound is 3D or 2D.
            // It does this by checking if the position is null. (no position stated = 2D)
            if (position == null)
            {
                audioSource.spatialBlend = 0;
            // If the position is not null, it sets the audio source's position to the object's position.
            }
            else if (position != null)
            {
                audioSource.spatialBlend = 1;
                soundGameObject.transform.position = (Vector3)position;
            }

            // This sets the audio source's settings to the settings from the GameAssets object.
            // These are just very basic settings that do not need checks.
            audioSource.maxDistance = settings.maxDistance;
            audioSource.loop = settings.isLooped;
            audioSource.dopplerLevel = settings.dopplerLevel;
            audioSource.rolloffMode = settings.audioRolloffMode;

            // Once everything is in place, the sound is played.
            audioSource.Play();

            // This destroys the soundGameObject after the audio clip has finished playing.
            if (settings.destroyAfterFinished)
            {
                Destroy(soundGameObject, audioSource.clip.length);
            }
        }
    }

    // This method is used to prevent the same sound from playing too frequently, but mostly just for any walking sounds.
    // It can be very easily expanded to include other sounds, but for now it's just for walking.
    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            // Checks for the player walking sound.
            case Sound.Player_Move:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    // Checks if the last time the sound was played was more than the playerMoveTimerMax ago.
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true; 
                    }
                    else 
                    { 
                        return false;
                    }
                }
                else
                {
                    return true;
                }
        }
    }

    // This method is used to get the audio clip from the soundAudioClipArray in GameAssets.cs.
    private static AudioClip GetAudioClip(Sound sound)
    {
        // This loops through each soundAudioClip in the soundAudioClipArray and returns the audio clip if the sound matches.
        foreach (SoundAudioClip soundAudioClip in i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

}
