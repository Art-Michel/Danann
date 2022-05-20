using System;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ProjectManager<SoundManager>
{
    [NonSerialized] public AudioSource AudioSource;
    [SerializeField] AudioClip _bossPunched;
    [SerializeField] AudioClip _blockedHit;
    [SerializeField] AudioClip _bossPunchedH;
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

}