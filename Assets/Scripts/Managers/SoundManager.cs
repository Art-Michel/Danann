using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ProjectManager<SoundManager>
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _playerHurt;
    [SerializeField] AudioClip _bossPunched;
    [SerializeField] AudioClip _blockedHit;
    [SerializeField] AudioClip _punch0;
    [SerializeField] AudioClip _punch1;
    [SerializeField] AudioClip _punch2;

    private void PlaySound(AudioClip clip, float volume)
    {
        _audioSource.PlayOneShot(clip, volume);
    }

    public void PlayPlayerHurt()
    {
        PlaySound(_playerHurt, 1f);
    }
    
    public void PlayBossPunched()
    {
        PlaySound(_bossPunched, 1f);
    }

    public void PlayBlockedHit()
    {
        PlaySound(_bossPunched, 1f);
    }

    public void PlayPunch0()
    {
        PlaySound(_punch0, 5f);
    }

    public void PlayPunch1()
    {
        PlaySound(_punch1, 8f);
    }

    public void PlayPunch2()
    {
        PlaySound(_punch2, 5f);
    }

    
}