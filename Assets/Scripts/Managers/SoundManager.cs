﻿using System;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SoundManager : ProjectManager<SoundManager>
{
    [NonSerialized] public AudioSource AudioSource;
    [SerializeField] AudioClip _bossPunched;
    [SerializeField] AudioClip _blockedHit;
    [SerializeField] AudioClip _bossPunchedH;
    [SerializeField] AudioClip  _bossSlashedH;
    [SerializeField] AudioClip _bossSwinged;
    Dictionary<string, AudioClip> _attackSoundEffect;
    #region Boss
    [SerializeField,Foldout("BossSFX")] AudioClip _bossTPIn;
    [SerializeField,Foldout("BossSFX")] AudioClip _bossTPOut;
    [SerializeField,Foldout("BossSFX")] AudioClip _bossShoot;
    [SerializeField,Foldout("BossSFX")] AudioClip _bossStartRise;
    [SerializeField,Foldout("BossSFX")] AudioClip _bossEndRise;
    [SerializeField,Foldout("BossSFX")] AudioClip _bossDash;
    [SerializeField,Foldout("BossSFX")] AudioClip _bossBoomerang;
    [SerializeField,Foldout("BossSFX")] AudioClip _bossAllOut;
    public void PlayBossTpIn()
    {
        PlaySound(_bossTPIn, 1f);
    }    public void PlayBossTpOut()
    {
        PlaySound(_bossTPOut, 1f);
    }    public void PlayBossShoot()
    {
        PlaySound(_bossShoot, 1f);
    }    
    public void PlayBossRiseStart()
    {
        PlaySound(_bossStartRise, 1f);
    }
    public void PlayBossRiseEnd()
    {
        PlaySound(_bossEndRise, 1f);
    }
    public void PlayBossDash()
    {
        PlaySound(_bossDash, 1f);
    }
    public void PlayBossBoomerang()
    {
        PlaySound(_bossBoomerang, 1f);
    }
    public void PlayBossAllOut()
    {
        PlaySound(_bossAllOut, 1f);
    }

    #endregion

    override protected void Awake()
    {
        base.Awake();
        AudioSource = GetComponent<AudioSource>();
        _attackSoundEffect = new Dictionary<string, AudioClip>
        {
        {Ccl_Attacks.LIGHTATTACK1, _bossPunched},
        {Ccl_Attacks.LIGHTATTACK2, _bossPunched},
        {Ccl_Attacks.LIGHTATTACK3, _bossPunchedH},
        {Ccl_Attacks.DASHONSPEAR, _bossPunchedH},
        {Ccl_Attacks.SPEARSWINGL, _bossSwinged},
        {Ccl_Attacks.SPEARSWINGR, _bossSwinged},
        {Ccl_Attacks.TRAVELINGSPEAR, _bossSlashedH}
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
        PlaySound(_bossPunched, 3f);
    }

    public void PlayBossPunchedH()
    {
        PlaySound(_bossPunchedH, 5f);
    }

    public void PlayBossHeavySlashed()
    {
        PlaySound(_bossSlashedH, 5f);
    }    

    public void PlayBlockedHit()
    {
        PlaySound(_blockedHit, .5f);
    }

}