using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Cinemachine;

public class PlayerHP : EntityHP
{
    #region Variables for feedbacks: camera shake + time slowdown
    //cam shake
    [SerializeField] CinemachineVirtualCamera _vcam;
    CinemachineBasicMultiChannelPerlin _vcamPerlin;
    private const float _cameraShakeIntensity = 1f;

    //time slow
    const float _slowdownLength = 0.3f;
    float _slowdownT;
    private const float _timeScalePostHit = 0.05f;
    private bool _timeIsSlow;

    //blinking (same length as invul)
    private bool _isBlinking;
    [SerializeField] MeshRenderer _body;
    #endregion

    //invul
    const float _invulerabilityLength = 0.7f;
    float _invulerabilityT;

    Hurtbox _hurtbox;

    void Awake()
    {
        _maxHealthPoints = 100;
        _vcamPerlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _hurtbox = GetComponent<Hurtbox>();
    }

    override protected void DamageFeedback(string attackName= "")
    {
        SoundManager.Instance.PlayPlayerHurt();
        SlowDownTime();
        StartInvul();
    }

    void Update()
    {
        if (_isInvulnerable) HandlePostDamageInvul();
        if (_timeIsSlow) HandlePostDamageTimeSlow();
    }

    void FixedUpdate()
    {
        if(_isBlinking) _body.enabled = !_body.enabled;
    }

    #region slow down time after damage taken + camera shake
    void SlowDownTime()
    {
        Time.timeScale = _timeScalePostHit;
        _timeIsSlow = true;
        _slowdownT = _slowdownLength;

        // + camera shake
        _vcamPerlin.m_AmplitudeGain = _cameraShakeIntensity;
    }
    private void HandlePostDamageTimeSlow()
    {
        _slowdownT -= Time.unscaledDeltaTime;
        if (_slowdownT < 0) ResetSlowdown();
    }
    private void ResetSlowdown()
    {
        _vcamPerlin.m_AmplitudeGain = 0f;
        Time.timeScale = 1f;
        _timeIsSlow = false;
    }
    #endregion

    #region invulnerability time after damage taken + character's body is blinking
    void StartInvul()
    {
        _isInvulnerable = true;
        _invulerabilityT = _invulerabilityLength;
        _isBlinking = true;
    }
    private void HandlePostDamageInvul()
    {
        _invulerabilityT -= Time.unscaledDeltaTime;
        if (_invulerabilityT < 0) ResetInvulerability();
    }
    
    private void ResetInvulerability()
    {
        _isInvulnerable = false;
        _hurtbox.ForgetAllAttacks();
        ResetBlinking();
    }

    //post damage blinking while invul
    private void ResetBlinking()
    {
        _body.enabled = true;
        _isBlinking = false;
    }
    #endregion
}
