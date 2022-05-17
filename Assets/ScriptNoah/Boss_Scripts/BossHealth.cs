using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EntityHP
{
    private bool _isBlinking;
    const float _blinkingDuration = 0.3f;
    float _blinkingT = 0;

    [SerializeField] GameObject _body;

    void Awake()
    {
        _maxHealthPoints = 1000;
    }
    
    override protected void DamageFeedback(string attackName)
    {
        SoundManager.Instance.PlayHitSound(attackName);
        _isBlinking = true;
        _blinkingT = _blinkingDuration;
    }

    private void HandlePostDamageBlinking()
    {
        _blinkingT -= Time.unscaledDeltaTime;
        if(_blinkingT <= 0) ResetBlinking();
    }

    private void ResetBlinking()
    {
        _body.SetActive(true);
        _isBlinking = false;
    }

    void Update()
    {
        if(_isBlinking) HandlePostDamageBlinking();
    }

    void FixedUpdate()
    {
        if(_isBlinking) _body.SetActive(!_body.activeSelf);
    }
}
