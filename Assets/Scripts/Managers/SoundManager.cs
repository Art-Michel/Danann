using System;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SoundManager : LocalManager<SoundManager>
{
    [NonSerialized] public AudioSource AudioSource;
    [SerializeField] AudioClip _bossPunched;
    [SerializeField] AudioClip _blockedHit;
    [SerializeField] AudioClip _bossPunchedH;
    [SerializeField] AudioClip _bossSlashedH;
    [SerializeField] AudioClip _bossSwinged;
    [SerializeField] AudioClip _counterAttack;
    [SerializeField] AudioClip _menuOk;
    [SerializeField] AudioClip _menuCancel;
    [SerializeField] AudioClip _menuOpen;
    [SerializeField] AudioClip _menuClose;
    [SerializeField] AudioClip _menuMove;
    Dictionary<string, AudioClip> _attackSoundEffect;
    #region Boss
    [SerializeField, Foldout("BossSFX")] AudioClip _bossTPIn;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossTPOut;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossShoot;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossStartRise;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossDeath;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossEndRise;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossDash;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossBoomerang;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossAllOut;

    public void PlayBossTpIn()
    {
        PlaySound(_bossTPIn, 1f);
    }
    public void PlayBossTpOut()
    {
        PlaySound(_bossTPOut, 1f);
    }
    public void PlayBossShoot()
    {
        PlaySound(_bossShoot, .3f);
    }
    public void PlayBossRiseStart()
    {
        PlaySound(_bossStartRise, .3f);
    }
    public void PlayBossRiseEnd()
    {
        PlaySound(_bossEndRise, .3f);
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
    public void PlayCounterAttack()
    {
        PlaySound(_counterAttack, 0.4f);
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
        {Ccl_Attacks.TRAVELINGSPEAR, _bossSlashedH},
        {Danu_Attacks.DASH, _counterAttack},
        {Danu_Attacks.DASH2, _counterAttack},
        {Danu_Attacks.PROJECTILE, _counterAttack},
        {Danu_Attacks.SLAM1, _counterAttack},
        {Danu_Attacks.SLAM2, _counterAttack},
        {Danu_Attacks.SLAM3, _counterAttack},
        {Danu_Attacks.TP, _counterAttack}
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

    internal void PlayBossDie()
    {
        PlaySound(_bossDeath, 1f);
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
        PlaySound(_blockedHit, .2f);
    }

    public void PlayMenuOk()
    {
        PlaySound(_menuOk, 1f);
    }

    public void PlayMenuCancel()
    {
        PlaySound(_menuCancel, 1f);
    }

    public void PlayMenuOpen()
    {
        PlaySound(_menuOpen, 1f);
    }

    public void PlayMenuClose()
    {
        PlaySound(_menuClose, 1f);
    }

    public void PlayMenuMove()
    {
        PlaySound(_menuMove, 1f);
    }
}