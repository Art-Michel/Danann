using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    #region Init
    PlayerInputMap _inputs;
    Ccl_FSM _fsm;
    public PlayerMovement PlayerMovement { get; private set; }
    [SerializeField] Spear_FSM _leftSpear;
    [SerializeField] Spear_FSM _rightSpear;
    #endregion

    #region Light Attack
    int _currentLightAttackIndex = 0;
    float _comboWindow = 0f;
    float _comboMaxWindow = 0.5f;
    #endregion

    #region Aiming
    Spear_FSM _currentlyHeldSpear;
    #endregion

    #region Dodge Rolling
    public CharacterController Characon { get; private set; }
    //public Hurtbox Hurtbox { get; private set; }
    public PlayerHP PlayerHP {get; private set;}
    public TrailRenderer BodyTrailRenderer;
    #endregion

    #region Attacks Data
    [SerializeField] AttackData _lightAttack0Data;
    [SerializeField] AttackData _lightAttack1Data;
    [SerializeField] AttackData _lightAttack2Data;
    #endregion

    private void Awake()
    {
        //Init
        _fsm = GetComponent<Ccl_FSM>();
        _inputs = new PlayerInputMap();
        this.PlayerMovement = GetComponent<PlayerMovement>();
        Characon = GetComponent<CharacterController>();
        this.PlayerHP = GetComponent<PlayerHP>();
        //Hurtbox = GetComponent<Hurtbox>();

        //Inputs
        _inputs.Actions.LightAttack.started += _ => LightAttackInput();
        _inputs.Actions.Dodge.started += _ => DodgeInput();
        _inputs.Actions.ThrowL.started += _ => AimInput(_leftSpear);
        _inputs.Actions.ThrowR.started += _ => AimInput(_rightSpear);
        _inputs.Actions.ThrowL.canceled += _ => ThrowInput();
        _inputs.Actions.ThrowR.canceled += _ => ThrowInput();
    }

    #region Aiming and Throwing
    private void AimInput(Spear_FSM spear)
    {
        if (_fsm.currentState.Name == Ccl_StateNames.IDLE && spear.currentState.Name == Spear_StateNames.ATTACHED)
        {
            _fsm.ChangeState(Ccl_StateNames.AIMING);
            spear.ChangeState(Spear_StateNames.AIMING);
            _currentlyHeldSpear = spear;
        }
        else if (_fsm.currentState.Name == Ccl_StateNames.IDLE && (spear.currentState.Name == Spear_StateNames.IDLE || spear.currentState.Name == Spear_StateNames.ATTACKING || spear.currentState.Name == Spear_StateNames.TRIANGLING))
        {
            spear.ChangeState(Spear_StateNames.RECALLED);
        }
    }

    private void ThrowInput()
    {
        if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            Throw();
    }

    private void Throw()
    {
        _fsm.ChangeState(Ccl_StateNames.THROWING);
        _currentlyHeldSpear.ChangeState(Spear_StateNames.THROWN);
    }

    private void CancelAim()
    {
        {
            _fsm.ChangeState(Ccl_StateNames.IDLE);
            _currentlyHeldSpear.ChangeState(Spear_StateNames.ATTACHED);
            _currentlyHeldSpear = null;
            _fsm.TargetGroup.m_Targets[4].weight = 0;
        }
    }
    #endregion

    #region DodgeRoll
    private void DodgeInput()
    {
        if (_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKING)
            DodgeRoll();

        else if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            CancelAim();
    }

    private void DodgeRoll()
    {
        _fsm.ChangeState(Ccl_StateNames.DODGING);
    }
    #endregion

    #region Light Attack
    private void LightAttackInput()
    {
        if (_fsm.currentState.Name == Ccl_StateNames.IDLE /*|| _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKING*/)
            LightAttack();
        else if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            CancelAim();
    }

    private void LightAttack()
    {
        LaunchLightAttackAnimation();
        if (_currentLightAttackIndex < 2) _currentLightAttackIndex++;
        else _currentLightAttackIndex = 0;
        //Debug.Log("Launching Light Attack" + _currentLightAttackIndex);
    }
    #endregion

    #region Placeholder coroutines instead of animation keys
    void LaunchLightAttackAnimation()
    {
        switch (_currentLightAttackIndex)
        {
            case 0:
                StartCoroutine("LightAttack0Animation");
                break;
            case 1:
                StartCoroutine("LightAttack1Animation");
                break;
            case 2:
                StartCoroutine("LightAttack2Animation");
                break;
        }
    }
    IEnumerator LightAttack0Animation()
    {
        _fsm.ChangeState(Ccl_StateNames.LIGHTATTACKING);
        _comboWindow = _comboMaxWindow;
        _lightAttack0Data.LaunchAttack();
        SoundManager.Instance.PlayPunch0();
        PlayerMovement.MovementSpeed *= 0.75f;
        yield return new WaitForSeconds(0.3f);

        _lightAttack0Data.StopAttack();
        PlayerMovement.ResetMovementSpeed();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
        yield return null;
    }
    IEnumerator LightAttack1Animation()
    {
        _fsm.ChangeState(Ccl_StateNames.LIGHTATTACKING);
        PlayerMovement.MovementSpeed *= 0.25f;
        yield return new WaitForSeconds(0.1f);

        _comboWindow = _comboMaxWindow;
        _lightAttack1Data.LaunchAttack();
        SoundManager.Instance.PlayPunch1();
        yield return new WaitForSeconds(0.4f);

        _lightAttack1Data.StopAttack();
        PlayerMovement.ResetMovementSpeed();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
        yield return null;
    }
    IEnumerator LightAttack2Animation()
    {
        _fsm.ChangeState(Ccl_StateNames.LIGHTATTACKING);
        PlayerMovement.MovementSpeed = 0;
        yield return new WaitForSeconds(0.4f);

        _comboWindow = _comboMaxWindow;
        _lightAttack2Data.LaunchAttack();
        SoundManager.Instance.PlayPunch2();
        yield return new WaitForSeconds(0.2f);

        _lightAttack2Data.StopAttack();
        PlayerMovement.ResetMovementSpeed();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
        yield return null;
    }
    #endregion

    void Update()
    {
        if (_fsm.currentState.Name != Ccl_StateNames.LIGHTATTACKING)
        {
            _comboWindow -= Time.deltaTime;
        }
        if (_comboWindow <= 0) _currentLightAttackIndex = 0;
    }

    #region disable inputs on Player disable to avoid weird inputs
    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
    #endregion
}
