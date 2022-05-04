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
    #endregion

    #region Light Attack
    int _currentLightAttackIndex = 0;
    float _comboWindow = 0f;
    float _comboMaxWindow = 0.5f;
    #endregion

    #region Attacks Data
    LightAttack1 _lightAttack1Data;
    #endregion

    private void Awake()
    {
        //Init
        _fsm = GetComponent<Ccl_FSM>();
        _inputs = new PlayerInputMap();

        //Attacks Data
        _lightAttack1Data = GetComponentInChildren<LightAttack1>();

        //Inputs
        _inputs.Actions.LightAttack.started += _ => LightAttackInput();
    }

    void Start()
    {
        //States
        this._fsm.AddState(new Ccl_StateLightAttacking());
    }

    private void LightAttackInput()
    {
        if (_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKING)
        {
            LightAttack();
        }
    }

    private void LightAttack()
    {
        Debug.Log("Launching Light Attack " + _currentLightAttackIndex);
        _comboWindow = _comboMaxWindow;
        if (_currentLightAttackIndex < 2) _currentLightAttackIndex++;
        else _currentLightAttackIndex = 0;
    }

    void Update()
    {
        _comboWindow -= Time.deltaTime;
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
