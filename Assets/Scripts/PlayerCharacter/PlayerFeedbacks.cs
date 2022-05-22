using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using NaughtyAttributes;
using Cinemachine;
using System;

public class PlayerFeedbacks : MonoBehaviour
{
    PlayerActions _playerActions;

    #region Health
    [SerializeField] AudioClip _playerHurt;
    #endregion

    #region Aiming
    [SerializeField] Volume _volume;
    [SerializeField] GameObject _cursor;
    SpriteRenderer _cursorSprite;

    Vignette _vignette;

    [SerializeField] AudioClip _zoomin;
    [SerializeField] AudioClip _zoomout;
    #endregion

    #region Throwing
    [SerializeField] AudioClip _throw;
    [SerializeField] AudioClip _recall;
    #endregion

    #region DodgeRoll
    [SerializeField] TrailRenderer _bodyTrailRenderer;
    [SerializeField] AudioClip _dodge;
    #endregion

    #region Light Attack
    [SerializeField] AudioClip _punch0;
    [SerializeField] AudioClip _punch1;
    [SerializeField] AudioClip _punch2;
    #endregion

    #region Audio
    [Required][SerializeField] AudioSource _audioSource;
    #endregion

    [Required][SerializeField] CinemachineTargetGroup _targetGroup;

    void Awake()
    {
        //aiming
        _cursorSprite = _cursor.GetComponent<SpriteRenderer>();
        _playerActions = GetComponent<PlayerActions>();
        if (_volume) _volume.profile.TryGet<Vignette>(out _vignette);
    }

    #region Aiming Feedbacks
    public void ZoomCamera()
    {
        _vignette.intensity.Override(0.6f);
    }

    public void UnzoomCamera()
    {
        _vignette.intensity.Override(0f);
    }

    public void ChangeCursorColor(bool isLeft)
    {
        if (isLeft)
            _cursorSprite.color = Color.cyan;
        else
            _cursorSprite.color = Color.yellow;
    }
    #endregion

    #region Dodgeroll Feedbacks
    public void SetTrailRenderer(bool enabled, bool emitting)
    {
        _bodyTrailRenderer.enabled = enabled;
        _bodyTrailRenderer.emitting = emitting;
    }
    #endregion

    #region SFX
    private void PlaySound(AudioClip clip, float volume)
    {
        _audioSource.PlayOneShot(clip, volume);
    }

    public void PlayPunchSfx()
    {
        switch (_playerActions._currentLightAttackIndex)
        {
            case 0:
                PlayPunch0();
                break;
            case 1:
                PlayPunch1();
                break;
            case 2:
                PlayPunch2();
                break;
        }
    }

    void PlayPunch0()
    {
        PlaySound(_punch0, 5f);
    }

    void PlayPunch1()
    {
        PlaySound(_punch1, 8f);
    }

    void PlayPunch2()
    {
        PlaySound(_punch2, 5f);
    }

    public void PlayZoomInSfx()
    {
        PlaySound(_zoomin, 5f);
    }

    public void PlayZoomOutSfx()
    {
        PlaySound(_zoomout, 5f);
    }

    public void PlayThrowSfx()
    {
        PlaySound(_throw, 0.8f);
    }

    public void PlayRecallSfx()
    {
        PlaySound(_recall, 2f);
    }

    public void PlayDodgeSfx()
    {
        PlaySound(_dodge, 2f);
    }

    public void PlayPlayerHurtSfx()
    {
        PlaySound(_playerHurt, 2f);
    }
    #endregion

    public void SetCameraTargetWeight(int target, int weight)
    {
        _targetGroup.m_Targets[target].weight = weight;
    }
}
