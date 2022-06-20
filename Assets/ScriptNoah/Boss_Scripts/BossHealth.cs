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
    DanuAI agent;
    [Required][SerializeField] GameObject _body;
    private float oldValue;
    private float accel;

    void Awake()
    {
        agent = GetComponent<DanuAI>();
        //_maxHealthPoints = 500;
    }

    override protected void DamageFeedback(string attackName, int plasmaRegainValue, int amount)
    {
        base.DamageFeedback(attackName,plasmaRegainValue,amount);
        SoundManager.Instance.PlayHitSound(attackName);
        _playerFeedbacks.StartShake(AttackShake.ShakeValue[attackName].x, AttackShake.ShakeValue[attackName].y);
        _playerFeedbacks.StartRumble(AttackShake.RumbleValue[attackName].x, AttackShake.RumbleValue[attackName].y, AttackShake.RumbleValue[attackName].z);
        _isBlinking = true;
        _blinkingT = _blinkingDuration;
        if (plasmaRegainValue > 0) _playerplasma.IncreasePlasma(plasmaRegainValue);
    }

    private void HandlePostDamageBlinking()
    {
        _blinkingT -= Time.unscaledDeltaTime;
        if (_blinkingT <= 0) ResetBlinking();
    }

    public override bool TakeDamage(int amount, string attackName, int plasmaRegainValue, int revengeGain = 0, GameObject obj = null)
    {
        float percent = (HealthPoints / _maxHealthPoints) * 100;
        Debug.Log(((HealthPoints-amount)/_maxHealthPoints)*100);
        if (((HealthPoints-amount)/_maxHealthPoints)*100<=5 && !agent.HasDM())
        {
            amount=(int)HealthPoints-(int)((5/100)*_maxHealthPoints);
                        agent.launchDM();
        }
        if (!activateRemnant)
            oldValue = (HealthPoints / _maxHealthPoints);
        accel = 0;
        if (percent < 50 && agent.GetPhase() == 1)
            agent.NextPhase();

        return base.TakeDamage(amount, attackName, plasmaRegainValue, revengeGain);
    }

    private void ResetBlinking()
    {
        _body.SetActive(true);
        _isBlinking = false;
    }

    void Update()
    {
        if (_isBlinking) HandlePostDamageBlinking();
        accel = Mathf.Clamp(accel + Time.deltaTime, 0, 1);
        if (!activateRemnant)
            return;
        remnantTime += Time.deltaTime * accel;
        float value = Mathf.InverseLerp(0, _maxHealthPoints, HealthPoints);
        value = Mathf.Lerp(0, 1, value);
        _healthBarRemnant.fillAmount = Mathf.Lerp(oldValue, value, remnantTime / maxRemnantTime);

        if (remnantTime >= maxRemnantTime)
        {
            remnantTime = 0;
            activateRemnant = false;
        }
    }

    void FixedUpdate()
    {
        if (_isBlinking) _body.SetActive(!_body.activeSelf);
    }
}
