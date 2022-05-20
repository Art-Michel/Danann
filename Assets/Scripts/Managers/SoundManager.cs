using System;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ProjectManager<SoundManager>
{
    [NonSerialized] public AudioSource AudioSource;
    [SerializeField] AudioClip _playerHurt;
    [SerializeField] AudioClip _bossPunched;
    [SerializeField] AudioClip _blockedHit;
    [SerializeField] AudioClip _punch0;
    [SerializeField] AudioClip _punch1;
    [SerializeField] AudioClip _punch2;
    [SerializeField] AudioClip _bossPunchedH;
    [SerializeField] AudioClip _dash;
    [SerializeField] AudioClip _zoomin;
    [SerializeField] AudioClip _zoomout;
    [SerializeField] AudioClip _throw;
    [SerializeField] AudioClip _recall;
    Dictionary<string, AudioClip> _attackSoundEffect;

    override protected void Awake()
    {
        base.Awake();
        AudioSource = GetComponent<AudioSource>();
        _attackSoundEffect = new Dictionary<string, AudioClip>
        {
        {Ccl_Attacks.LIGHTATTACK1, _bossPunched},
        {Ccl_Attacks.LIGHTATTACK2, _bossPunched},
        {Ccl_Attacks.LIGHTATTACK3, _bossPunchedH},
        {Ccl_Attacks.TRAVELINGSPEAR, _bossPunched}
        };
    }

    public void PlayHitSound(string attackName)
    {
        PlaySound(_attackSoundEffect[attackName], 1);
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        AudioSource.PlayOneShot(clip, volume);
    }

    public void PlayPlayerHurt()
    {
        PlaySound(_playerHurt, 1f);
    }

    public void PlayDash()
    {
        PlaySound(_dash, 1f);
    }

    public void PlayBossPunched()
    {
        PlaySound(_bossPunched, 1f);
    }

    public void PlayBossPunchedH()
    {
        PlaySound(_bossPunchedH, 1f);
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

    public void PlayZoomIn()
    {
        PlaySound(_zoomin, 5f);
    }

    public void PlayZoomOut()
    {
        PlaySound(_zoomout, 5f);
    }

    public void PlayThrow()
    {
        PlaySound(_throw, 0.8f);
    }

    public void PlayRecall()
    {
        PlaySound(_recall, 2f);
    }
}