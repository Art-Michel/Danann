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
    PlayerMovement _playerMovement;
    #endregion

    #region Light Attack
    int _currentLightAttackIndex = 0;
    float _comboWindow = 0f;
    float _comboMaxWindow = 0.5f;
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
        _playerMovement = GetComponent<PlayerMovement>();

        //Inputs
        _inputs.Actions.LightAttack.started += _ => LightAttackInput();
    }

    private void LightAttackInput()
    {
        if (_fsm.currentState.Name == Ccl_StateNames.IDLE /*|| _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKING*/)
        {
            LightAttack();
        }
    }

    private void LightAttack()
    {
        if (_currentLightAttackIndex < 2) _currentLightAttackIndex++;
        else _currentLightAttackIndex = 0;
        LaunchLightAttackAnimation();
        Debug.Log("Launching Light Attack" + _currentLightAttackIndex);
    }

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
        _playerMovement.MovementSpeed *= 0.75f;
        yield return new WaitForSeconds(0.3f);
        _playerMovement.ResetMovementSpeed();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
        _lightAttack2Data.TellHitboxToTellHurtboxesToResetIds();
        yield return null;
    }
    IEnumerator LightAttack1Animation()
    {
        _fsm.ChangeState(Ccl_StateNames.LIGHTATTACKING);
        _playerMovement.MovementSpeed *= 0.25f;
        yield return new WaitForSeconds(0.1f);
        _comboWindow = _comboMaxWindow;
        _lightAttack1Data.LaunchAttack();
        SoundManager.Instance.PlayPunch1();
        yield return new WaitForSeconds(0.4f);
        _playerMovement.ResetMovementSpeed();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
        _lightAttack1Data.TellHitboxToTellHurtboxesToResetIds();
        yield return null;
    }
    IEnumerator LightAttack2Animation()
    {
        _fsm.ChangeState(Ccl_StateNames.LIGHTATTACKING);
        _playerMovement.MovementSpeed = 0;
        yield return new WaitForSeconds(0.4f);
        _lightAttack2Data.LaunchAttack();
        SoundManager.Instance.PlayPunch2();
        yield return new WaitForSeconds(0.2f);
        _playerMovement.ResetMovementSpeed();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
        _lightAttack2Data.TellHitboxToTellHurtboxesToResetIds();
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
