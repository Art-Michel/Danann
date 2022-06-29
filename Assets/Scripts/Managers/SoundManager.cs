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
    [SerializeField] AudioClip _bossZapped;
    [SerializeField] AudioClip _counterAttack;
    [SerializeField] AudioClip _menuOk;
    [SerializeField] AudioClip _menuCancel;
    [SerializeField] AudioClip _menuOpen;

    [SerializeField] AudioClip _menuClose;
    [SerializeField] AudioClip _menuMove;
    [SerializeField] AudioClip _playerDeath;
    [SerializeField] AudioClip _triangleCharge;
    [SerializeField] AudioClip _triangleActivation;
    [SerializeField] AudioClip _triangleCancel;
    [SerializeField] AudioClip _triangleExplosion;
    [SerializeField] AudioClip _triangleBreak;
    [SerializeField] AudioClip _ultReady;

    Dictionary<string, AudioClip> _attackSoundEffect;
    #region Boss
    [SerializeField, Foldout("BossSFX")] AudioClip _bossTPIn;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossTPCharge;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossTPOut;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossShoot;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossStartRise;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossDeath;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossEndRise;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossDash;

    [SerializeField, Foldout("BossSFX")] AudioClip _bossBoomerangCharge;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossBoomerangFly1;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossBoomerangFly2;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossAllOut;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossLaser;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossMeteorCharge;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossMeteorBoom;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossDM;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossShiedlSmallBreak;
    [SerializeField, Foldout("BossSFX")] AudioClip _bossShieldBreak;
    public void PlayBossLaser()
    {
        PlaySound(_bossLaser,1f);
    }
    public void PlayBossSmallBreak()
    {
        PlaySound(_bossShiedlSmallBreak,1f);
    }    public void PlayBossBreak()
    {
        PlaySound(_bossShieldBreak,0.7f);
    }
    public void PlayDMTransition()
    {
        PlaySound(_bossDM,1f);
    }
    public void PlayBossMeteorCharge()
    {
        PlaySound(_bossMeteorCharge, 0.2f);
    }
    public void PlayBossMeteorBoom()
    {
        PlaySound(_bossMeteorBoom, 0.2f);
    }
    public void PlayBossBoomerangBack()
    {
        PlaySound(_bossBoomerangFly2, 1f);
    }
    public void PlayBossBoomerangCharge()
    {
        PlaySound(_bossBoomerangCharge, 1f);
    }
    public void PlayBossTpCharge()
    {
        PlaySound(_bossTPCharge, 1.8f);
    }
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
        PlaySound(_bossShoot, .2f);
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
    public void PlayBossBoomerangGo()
    {
        PlaySound(_bossBoomerangFly1, 1f);
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
        {Ccl_Attacks.TRIANGLETICK, _bossZapped},
        {Ccl_Attacks.TRIANGLEBOOM, _bossPunchedH},
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
        PlaySound(_bossDeath, 2f);
    }

    public void PlayBossPunchedH()
    {
        PlaySound(_bossPunchedH, 5f);
    }

    public void PlayBossHeavySlashed()
    {
        PlaySound(_bossSlashedH, 5f);
    }

    public void PlayBossZapped()
    {
        PlaySound(_bossZapped, 5f);
    }

    public void PlayBlockedHit()
    {
        PlaySound(_blockedHit, 1f);
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

    public void PlayDeathSound()
    {
        PlaySound(_playerDeath, 1f);
    }

    public void PlayTriangleCharge()
    {
        this.AudioSource.Play();
    }

    public void PlayTriangleActivation()
    {
        PlaySound(_triangleActivation, 1f);
    }

    internal void PlayTriangleCancel()
    {
        PlaySound(_triangleCancel, .2f);
    }

    internal void PlayTriangleExplosion()
    {
        PlaySound(_triangleExplosion, 1f);
    }

    internal void PlayTriangleBreak()
    {
        PlaySound(_triangleBreak, 2f);
    }

    internal void PlayUltReady()
    {
        PlaySound(_ultReady, 0.5f);
    }
}