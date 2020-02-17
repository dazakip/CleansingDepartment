using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltAudio : MonoBehaviour
{
    public AudioSource beltBellyAudioSource;
    public AudioSource buttonAudioSource;
    public AudioSource drumKickAudioSource;

    public AudioClip redButtonAudio;
    public AudioClip greenButtonAudio;

    public void ToggleConveyorBelt(bool toggle)
    {
        if (toggle)
        {
            buttonAudioSource.clip = greenButtonAudio;
            buttonAudioSource.Play();
            beltBellyAudioSource.Play();
            drumKickAudioSource.Play();
        }
        else
        {
            buttonAudioSource.clip = redButtonAudio;
            buttonAudioSource.Play();
            beltBellyAudioSource.Pause();
            drumKickAudioSource.Pause();
        }
    }
}
