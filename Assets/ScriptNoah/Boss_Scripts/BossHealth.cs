using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BossHealth : EntityHP
{
    private bool _isBlinking;
    const float _blinkingDuration = 0.3f;
    float _blinkingT = 0;

    [SerializeField] PlayerFeedbacks _playerFeedbacks;

    [Required][SerializeField] GameObject _body;

    void Awake()
    {
        _maxHealthPoints = 500;
    }

    override protected void DamageFeedback(string attackName)
    {
        SoundManager.Instance.PlayHitSound(attackName);
        _playerFeedbacks.StartShake(AttackShake.ShakeValue[attackName].x, AttackShake.ShakeValue[attackName].y);
        _isBlinking = true;
        _blinkingT = _blinkingDuration;
    }

    private void HandlePostDamageBlinking()
    {
        _blinkingT -= Time.unscaledDeltaTime;
        if (_blinkingT <= 0) ResetBlinking();
    }
    public override bool TakeDamage(int amount, string attackName, int plasmaRegainValue, int revengeGain = 0){
        base.TakeDamage(amount,attackName,plasmaRegainValue,revengeGain);
        return false;
    }
    private void ResetBlinking()
    {
        _body.SetActive(true);
        _isBlinking = false;
    }

    void Update()
    {
        if (_isBlinking) HandlePostDamageBlinking();
    }

    void FixedUpdate()
    {
        if (_isBlinking) _body.SetActive(!_body.activeSelf);
    }
}
