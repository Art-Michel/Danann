using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using NaughtyAttributes;
using Cinemachine;
using System;
using UnityEngine.InputSystem;

public class PlayerFeedbacks : MonoBehaviour
{
    PlayerActions _playerActions;

    #region Health
    [SerializeField] AudioClip _playerHurt;

    #endregion

    #region Plasma
    [SerializeField] AudioClip _segmentFill;
    [SerializeField] GameObject _ShieldVfx;
    [SerializeField] AudioClip _plasmaGainSfx1;
    [SerializeField] AudioClip _plasmaGainSfx2;
    [SerializeField] AudioClip _plasmaGainSfx3;
    [SerializeField] AudioClip _plasmaGainSfx4;
    [SerializeField] AudioClip _plasmaGainSfx5;
    [SerializeField] AudioClip _plasmaGainSfx6;
    [SerializeField] AudioClip _plasmaGainSfx7;
    [SerializeField] AudioClip _plasmaGainSfx8;
    [SerializeField] AudioClip _plasmaGainSfx9;
    [SerializeField] AudioClip _plasmaGainSfx10;

    internal void StartShieldFeedback()
    {
        PlayShieldSfx();
        _ShieldVfx.SetActive(true);
    }

    internal void StopShieldFeedback()
    {
        _ShieldVfx.SetActive(false);
    }
    #endregion

    #region Aiming
    [SerializeField] Volume _volume;
    [SerializeField] GameObject _cursor;
    SpriteRenderer _cursorSprite;
    Color _orange = new Color(1, .8f, .2f);
    Color _blue = new Color(0, .5f, 1f);

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
    [SerializeField] AudioClip _dodgeRefilled;
    #endregion

    #region Light Attack
    [SerializeField] AudioClip _punch0;
    [SerializeField] AudioClip _punch1;
    [SerializeField] AudioClip _punch2;
    [SerializeField] ParticleSystem _rightPunchVfx1;
    [SerializeField] ParticleSystem _rightPunchVfx2;
    [SerializeField] ParticleSystem _leftPunchVfx1;
    [SerializeField] ParticleSystem _leftPunchVfx2;
    #endregion

    #region Dashing
    [SerializeField] AudioClip _dashShout;
    [SerializeField] GameObject _dashVfx;
    #endregion

    #region Shielding
    [SerializeField] AudioClip _shield;
    [SerializeField] AudioClip _shieldTrigger;
    #endregion

    #region Other
    [SerializeField] AudioClip _error;
    [SerializeField] Pooler _billboardsPool;
    #endregion

    #region Audio
    [Required][SerializeField] AudioSource _audioSource;
    #endregion

    #region Animations
    [Required][SerializeField] Animator _animator;
    #endregion

    [Required][SerializeField] CinemachineTargetGroup _targetGroup;

    void Awake()
    {
        //aiming
        _vcamPerlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cursorSprite = _cursor.GetComponent<SpriteRenderer>();
        _playerActions = GetComponent<PlayerActions>();
        if (_volume)
        {
            _volume.profile.TryGet<Vignette>(out _vignette);
        }
    }

    void Start()
    {
        StopBlink();
    }
    #region Animations
    public void SetAnimationTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }
    #endregion

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
            _cursorSprite.color = _orange;
        else
            _cursorSprite.color = _blue;
    }
    #endregion

    #region Targetting feedbacks
    internal void TargetFeedbacks()
    {
        ZoomCamera();
    }

    internal void UntargetFeedbacks()
    {
        UnzoomCamera();
    }
    #endregion

    #region Light Attack Feedbacks
    public void EnablePunchVfx(int id, bool boolean)
    {
        if (id == 0 || id == 2)
        {
            _rightPunchVfx1.Clear();
            _rightPunchVfx1.Play();
            _rightPunchVfx2.Clear();
            _rightPunchVfx2.Play();
        }
        else
        {
            _leftPunchVfx1.Clear();
            _leftPunchVfx1.Play();
            _leftPunchVfx2.Clear();
            _leftPunchVfx2.Play();
        }
    }
    #endregion

    #region Dashing Feedbacks
    public void EnableDashVfx(bool boolean)
    {
        _dashVfx.SetActive(boolean);
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
        switch (_playerActions.CurrentLightAttackIndex)
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
        PlaySound(_punch0, 1f);
    }

    void PlayPunch1()
    {
        PlaySound(_punch1, 1f);
    }

    void PlayPunch2()
    {
        PlaySound(_punch2, .8f);
    }

    public void PlaySegmentFill()
    {
        PlaySound(_segmentFill, 1f);
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

    public void PlayShieldSfx()
    {
        PlaySound(_shield, 0.7f);
    }

    public void PlayShieldTriggerSfx()
    {
        PlaySound(_shieldTrigger, 1f);
    }

    public void PlayPlayerHurtSfx()
    {
        PlaySound(_playerHurt, 2f);
    }

    public void PlayDashingShoutSfx()
    {
        PlaySound(_dashShout, 1f);
    }

    public void PlayErrorSfx()
    {
        PlaySound(_error, 1f);
    }

    public void PlayDodgeReffiledSfx()
    {
        PlaySound(_dodgeRefilled, .1f);
    }

    public void PlayPlasmaGainedSfx(int i)
    {
        if (i <= 20)
            switch (i)
            {
                case 20:
                    PlaySound(_plasmaGainSfx10, .25f);
                    break;
                case 19:
                    PlaySound(_plasmaGainSfx10, .25f);
                    break;
                case 18:
                    PlaySound(_plasmaGainSfx9, .25f);
                    break;
                case 17:
                    PlaySound(_plasmaGainSfx9, .25f);
                    break;
                case 16:
                    PlaySound(_plasmaGainSfx8, .25f);
                    break;
                case 15:
                    PlaySound(_plasmaGainSfx8, .25f);
                    break;
                case 14:
                    PlaySound(_plasmaGainSfx7, .25f);
                    break;
                case 13:
                    PlaySound(_plasmaGainSfx7, .25f);
                    break;
                case 12:
                    PlaySound(_plasmaGainSfx6, .25f);
                    break;
                case 11:
                    PlaySound(_plasmaGainSfx6, .25f);
                    break;
                case 10:
                    PlaySound(_plasmaGainSfx5, .25f);
                    break;
                case 9:
                    PlaySound(_plasmaGainSfx5, .25f);
                    break;
                case 8:
                    PlaySound(_plasmaGainSfx4, .25f);
                    break;
                case 7:
                    PlaySound(_plasmaGainSfx4, .25f);
                    break;
                case 6:
                    PlaySound(_plasmaGainSfx3, .25f);
                    break;
                case 5:
                    PlaySound(_plasmaGainSfx3, .25f);
                    break;
                case 4:
                    PlaySound(_plasmaGainSfx2, .25f);
                    break;
                case 3:
                    PlaySound(_plasmaGainSfx2, .25f);
                    break;
                case 2:
                    PlaySound(_plasmaGainSfx1, .25f);
                    break;
                case 1:
                    PlaySound(_plasmaGainSfx1, .25f);
                    break;
                case 0:
                    PlaySound(_plasmaGainSfx1, .25f);
                    break;
            }
        else
            PlaySound(_plasmaGainSfx10, .25f);

    }
    #endregion

    #region Text
    public void NotEnoughPlasmaText()
    {
        Billboard billboard = _billboardsPool.Get() as Billboard;
        billboard.EnableAsInfo("Not enough\nPlasma!");
    }


    #endregion

    public void SetCameraTargetWeight(int target, int weight)
    {
        _targetGroup.m_Targets[target].weight = weight;
    }

    #region cam shake
    [SerializeField] CinemachineVirtualCamera _vcam;
    CinemachineBasicMultiChannelPerlin _vcamPerlin;
    float _shakeT;
    public void StartShake(float duration, float intensity)
    {
        if (duration > _shakeT)
            _shakeT = duration;
        if (intensity > _vcamPerlin.m_AmplitudeGain)
            _vcamPerlin.m_AmplitudeGain = intensity;
    }

    public void StopShake()
    {
        _vcamPerlin.m_AmplitudeGain = 0;
        _shakeT = -1;
    }
    #endregion

    #region rumble
    private float _rumbleT;
    private bool _isRumbling;
    public void StartRumble(float duration, float lowFrequencyIntensity, float highFrequencyIntensity)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(lowFrequencyIntensity, highFrequencyIntensity);
            _rumbleT = duration;
            _isRumbling = true;
        }
    }

    public void StopRumble()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0, 0);
            _vcamPerlin.m_AmplitudeGain = 0;
            _isRumbling = false;
        }
    }
    #endregion

    #region meshBlink
    float _blinkDuration = 0f;
    const float _blinkSpeed = 5f;
    const float _blinkStrength = 6;
    float _blinkT;
    [SerializeField] Material _meshMaterial;

    bool _isBlinking = false;

    [Button]
    public void StartBlink(float duration = .1f)
    {
        _isBlinking = true;
        _blinkT = 0;
        _blinkDuration = duration;
        //_meshMaterial.SetFloat("_LerpValue",20);
        UiManager.Instance.DodgeWhiteFrame.SetActive(true);
    }

    void Blink()
    {
        _blinkT += Time.deltaTime;
        _meshMaterial.SetFloat("_LerpValue", Mathf.Sin(_blinkT * _blinkSpeed) * _blinkStrength);
        if (_blinkT >= _blinkDuration)
            StopBlink();
    }

    private void StopBlink()
    {
        _meshMaterial.SetFloat("_LerpValue", -.1f);
        UiManager.Instance.DodgeWhiteFrame.SetActive(false);
        _isBlinking = false;
    }
    #endregion

    void Update()
    {
        if (_isBlinking)
        {
            Blink();
        }
        if (_vcamPerlin.m_AmplitudeGain > 0)
        {
            _shakeT -= Time.deltaTime;
            if (_shakeT < 0)
                StopShake();
        }
        if (_isRumbling)
        {
            _rumbleT -= Time.deltaTime;
            if (_rumbleT <= 0)
                StopRumble();
        }
    }


}
