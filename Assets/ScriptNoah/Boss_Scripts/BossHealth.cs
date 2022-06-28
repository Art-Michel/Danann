using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
public class BossHealth : EntityHP
{
    private bool _isBlinking;
    const float _blinkingDuration = 0.3f;
    float _blinkingT = 0;
    [SerializeField]Image shieldBar;
    [SerializeField]Image shieldRemnants;
    [SerializeField]GameObject shieldGO;
    [SerializeField] PlayerFeedbacks _playerFeedbacks;
    DanuAI agent;
    [Required][SerializeField] GameObject _body;
    public void SetBody(GameObject nbody){_body=nbody;}
    private float oldValue;
    private float accel;
    private int shieldPoint;
    [SerializeField] private int maxShieldPoint;
    private float shieldRemnantTime;
    private bool activateShieldRemnant;
    private float oldShieldValue;

    void Awake()
    {
        agent = GetComponent<DanuAI>();
        //_maxHealthPoints = 500;
    }
    public void ActivateShield()
    {
        shieldPoint=maxShieldPoint;
        shieldGO.SetActive(true);
        agent.UpdateShield(shieldPoint);

    }
    override protected void DamageFeedback(string attackName, int plasmaRegainValue, float amount)
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

    public override bool TakeDamage(float amount, string attackName, int plasmaRegainValue, int revengeGain = 0, GameObject obj = null)
    {
        if (!activateRemnant)
            oldValue = (HealthPoints / _maxHealthPoints);
        accel = 0;        
        if (!activateShieldRemnant)
            oldShieldValue = (HealthPoints / _maxHealthPoints);
        
        float percent = (HealthPoints / _maxHealthPoints) * 100;
        bool cond =attackName == Ccl_Attacks.TRAVELINGSPEAR; 
        cond=cond || attackName == Ccl_Attacks.SPEARSWINGL;
        cond=cond|| attackName == Ccl_Attacks.SPEARSWINGR;

        if (agent.IsDM())
            return base.TakeDamage(0, attackName, plasmaRegainValue, revengeGain);
        if (agent.IsShielded() )
        {
            if ( !cond)
            {

            shieldPoint--;
            shieldBar.fillAmount=shieldPoint*0.25f;
            activateShieldRemnant=true;
            agent.UpdateShield(shieldPoint);
            accel=0;
            if (shieldPoint<=0)
            {
                DesactivateShield();
            }
            return base.TakeDamage(0, attackName, plasmaRegainValue, revengeGain);
            }
                        return base.TakeDamage(0, attackName, plasmaRegainValue, revengeGain);

        }
        if (((HealthPoints-amount)/_maxHealthPoints)*100<=5 && !agent.HasDM())
        {
            amount = (int)(HealthPoints - ((5f / 100f) * _maxHealthPoints)) + 1;
            agent.launchDM();
            return base.TakeDamage(amount, attackName, plasmaRegainValue, revengeGain);
        }
        if (percent < 70 && agent.GetPhase() == 1)
            agent.NextPhase();

        return base.TakeDamage(amount, attackName, plasmaRegainValue, revengeGain);
    }

    private void DesactivateShield()
    {
        shieldGO.SetActive(false);
        agent.UpdateShield(shieldPoint);

    }

    private void ResetBlinking()
    {
        _body.SetActive(true);
        _isBlinking = false;
    }

    void Update()
    {
        if (_isBlinking) HandlePostDamageBlinking();
        UpdateRemnant();
        UpdateShieldRemnant();
        accel = Mathf.Clamp(accel + Time.deltaTime, 0, 1);

    }
    private void UpdateRemnant() 
    {
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
    private void UpdateShieldRemnant() 
    {
        if (!activateShieldRemnant)
            return;
        shieldRemnantTime += Time.deltaTime * accel;
        float value = Mathf.InverseLerp(0, maxShieldPoint, shieldPoint);
        value = Mathf.Lerp(0, 1, value);
        shieldRemnants.fillAmount = Mathf.Lerp(oldShieldValue, value, shieldRemnantTime / maxRemnantTime);

        if (shieldRemnantTime >= maxRemnantTime)
        {
            shieldRemnantTime = 0;
            activateShieldRemnant = false;
        }
    }
    void FixedUpdate()
    {
        if (_isBlinking) _body.SetActive(!_body.activeSelf);
    }

    [Button]
    protected override void Die()
    {
        SoundManager.Instance.PlayBossDie();
        UiManager.Instance.PreWinScreen();
        base.Die();
        Time.timeScale = 0.2f;
        this.enabled=false;
    }
}
