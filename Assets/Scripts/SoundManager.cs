using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ProjectManager<SoundManager>
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hit;

    private void PlaySound(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayHit()
    {
        PlaySound(hit, 1f);
    }
    

}
