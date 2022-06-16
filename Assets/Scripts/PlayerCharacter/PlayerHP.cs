using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerHP : EntityHP
{
    #region Variables for feedbacks: + time slowdown


    //time slow
    const float _slowdownLength = 0.3f;
    float _slowdownT;
    private const float _timeScalePostHit = 0.05f;
    private bool _timeIsSlow;

    //blinking (same length as invul)
    private bool _isBlinking;
    [SerializeField] GameObject _body;
    #endregion

    //invul
    const float _invulerabilityLength = 0.7f;
    float _invulerabilityT;
    float _tookAHit;

    //Init
    PlayerFeedbacks _playerFeedbacks;
    PlayerPlasma _playerPlasma;
    Ccl_FSM _fsm;
    [SerializeField] DanuAI _danuAI;
    [SerializeField] BossHealth _bossHealth;

    Hurtbox _hurtbox;
    private float _regenT;
    private bool _canRegen;
    private float maxRegenT = 3f;

    void Awake()
    {
        _hurtbox = GetComponent<Hurtbox>();
        _playerFeedbacks = GetComponent<PlayerFeedbacks>();
        _playerPlasma = GetComponent<PlayerPlasma>();
        _fsm = GetComponent<Ccl_FSM>();
        _maxHealthPoints = 5;
    }

    override protected void DamageFeedback(string attackName = "", int plasmaRegainValue = 0)
    {
        _playerFeedbacks.PlayPlayerHurtSfx();
        SlowDownTime();
        StartInvul();
        _playerFeedbacks.StartShake(.3f, 1f);
        _playerFeedbacks.StartRumble(.3f, 0.6f, 0.9f);
        StartRegenCooldown();
    }

    private void StartRegenCooldown()
    {
        _regenT = maxRegenT;
        _canRegen = false;
    }

    void Update()
    {
        if (_invulerabilityT > 0) HandlePostDamageInvul();
        if (_timeIsSlow) HandlePostDamageTimeSlow();
        if (!_canRegen)
        {
            _regenT -= Time.deltaTime;
            if (_regenT <= 0) _canRegen = true;
        }
        else if (HealthPoints < _maxHealthPoints)
        {
            HealthPoints = Mathf.Clamp(HealthPoints + Time.deltaTime * 0.25f, 0, _maxHealthPoints);
            UpdateHealthBar();
        }
    }

    void FixedUpdate()
    {
        if (_isBlinking) _body.gameObject.SetActive(!_body.activeSelf);
    }

    protected override void Parry(GameObject obj, int plasmaRegainValue, string attackName)
    {
        _playerFeedbacks.PlayParryTriggerSfx();
        //Refund Parry cost
        _playerPlasma.IncreasePlasma(plasmaRegainValue);

        Ccl_StateParrying stateParrying = _fsm.currentState as Ccl_StateParrying;
        stateParrying.ParryT = 0f;

        bool attackIsMelee = Danu_Attacks.AttackIsMelee[attackName];
        _bossHealth.TakeDamage(plasmaRegainValue * 5, attackName, 0);
        if (attackIsMelee)
        {
            _danuAI.Stun(3);
            obj.transform.parent.gameObject.SetActive(false); // renvoyer le projo un jour
        }
        else
            obj.SetActive(false); // renvoyer le projo un jour
    }

    protected override void Die()
    {
        _playerFeedbacks.StopShake();
        _playerFeedbacks.StopRumble();
        base.Die();
    }

    #region slow down time after damage taken + camera shake
    void SlowDownTime()
    {
        Time.timeScale = _timeScalePostHit;
        _timeIsSlow = true;
        _slowdownT = _slowdownLength;
    }
    private void HandlePostDamageTimeSlow()
    {
        _slowdownT -= Time.unscaledDeltaTime;
        if (_slowdownT < 0) ResetSlowdown();
    }
    private void ResetSlowdown()
    {
        Time.timeScale = 1f;
        _timeIsSlow = false;
    }
    #endregion

    #region invulnerability time after damage taken + character's body is blinking
    void StartInvul()
    {
        IsInvulnerable = true;
        _invulerabilityT = _invulerabilityLength;
        _isBlinking = true;
    }

    private void HandlePostDamageInvul()
    {
        _invulerabilityT -= Time.unscaledDeltaTime;
        if (_invulerabilityT < 0) ResetInvulerability();
    }

    public void ResetInvulerability()
    {
        IsInvulnerable = false;
        IsParrying = false;
        _hurtbox.ForgetAllAttacks();
        ResetBlinking();
    }

    //post damage blinking while invul
    private void ResetBlinking()
    {
        _body.SetActive(true);
        _isBlinking = false;
    }
    #endregion
}
