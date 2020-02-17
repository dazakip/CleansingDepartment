using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAnimationScript : MonoBehaviour
{
    private CursorScript cursor;
    private AudioSource audioSource;

    public AudioClip Slap;
    public AudioClip Flick;

    public void Awake()
    {
        cursor = transform.parent.GetComponent<CursorScript>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SlapOn()
    {
        cursor.windUpSlap = !cursor.windUpSlap;
        audioSource.clip = Slap;
        audioSource.Play();
    }

    public void SlapOff()
    {
        cursor.windUpSlap = !cursor.windUpSlap;
    }

    public void FlickOn()
    {
        cursor.flick = !cursor.flick;
        audioSource.clip = Flick;
        audioSource.Play();
    }

    public void FlickOff()
    {
        cursor.flick = !cursor.flick;
    }
}
