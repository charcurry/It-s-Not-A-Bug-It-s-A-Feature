using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NarratorEvent : ScriptableObject
{
    public string NarratorEventID;
    // Class that represents a specific type of vehicle
    public List<AudioClip> audio_clips;

    public List<String> subtitles;

    public string TriggeredById;

    public float TriggeredDelay;

    public string DiscardById;

    public bool Interruptable = true;

    public bool IsRunning = false;

    public int current_audio_clip = 0;

    public void CleanUp()
    {
        IsRunning = false;
        current_audio_clip = 0;
    }
    public void InterruptEvent(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        IsRunning = false;
    }

    public void StartEvent(AudioSource audioSource)
    {
        if (audio_clips.Count > 0)
        {
            audioSource.clip = audio_clips[0];
            audioSource.Play();


            IsRunning = true;
            current_audio_clip = 0;
        }
    }

    public bool PlayNextClip(AudioSource audioSource)
    {
        current_audio_clip++;
        if (current_audio_clip < audio_clips.Count)
        {
            audioSource.clip = audio_clips[current_audio_clip];
            audioSource.Play();

            return true;
        }

        return false;
    }
}