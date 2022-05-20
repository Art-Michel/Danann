using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using NaughtyAttributes;

public class PlayerActions : MonoBehaviour
{
    #region Init
    PlayerInputMap _inputs;
    Ccl_FSM _fsm;
    public PlayerMovement PlayerMovement { get; private set; }
    [Required][SerializeField] Spear_FSM _leftSpear;

    [Required][SerializeField] Spear_FSM _rightSpear;

    #endregion

    //Feedbacks
    [Required] PlayerFeedbacks _playerFeedbacks;

    #region Light Attack
    int _currentLightAttackIndex = 0;
    float _comboWindow = 0f;
    float _comboMaxWindow = 0.5f;
    #endregion

    #region Aiming
    [SerializeField] Transform _body;
    Spear_FSM _currentlyHeldSpear;
    [SerializeField] GameObject _cursor;
    #endregion

    #region Dodge Rolling
    public CharacterController Characon { get; private set; }
    public PlayerHP PlayerHP { get; private set; }
    bool _canDodge;
    float _dodgeCooldown = 0;
    const float _dodgeMaxCooldown = 0.4f;
    #endregion

    #region Attacks Data
    [Required][SerializeField] AttackData _lightAttack0Data;
    [Required][SerializeField] AttackData _lightAttack1Data;
    [Required][SerializeField] AttackData _lightAttack2Data;
    #endregion

    private void Awake()
    {
        //Init
        _fsm = GetComponent<Ccl_FSM>();
        _inputs = new PlayerInputMap();
        this.PlayerMovement = GetComponent<PlayerMovement>();
        Characon = GetComponent<CharacterController>();
        this.PlayerHP = GetComponent<PlayerHP>();
        this._playerFeedbacks = GetComponent<PlayerFeedbacks>();

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
        bool canBeRecalled = spear.currentState.Name == Spear_StateNames.IDLE;
        canBeRecalled = canBeRecalled || spear.currentState.Name == Spear_StateNames.ATTACKING;
        canBeRecalled = canBeRecalled || spear.currentState.Name == Spear_StateNames.TRIANGLING;
        if (_fsm.currentState.Name == Ccl_StateNames.IDLE && spear.currentState.Name == Spear_StateNames.ATTACHED)
        {
            _currentlyHeldSpear = spear;
            _fsm.ChangeState(Ccl_StateNames.AIMING);
            spear.ChangeState(Spear_StateNames.AIMING);
        }
        else if (_fsm.currentState.Name == Ccl_StateNames.IDLE && canBeRecalled)
        {
            _fsm.ChangeState(Ccl_StateNames.RECALLING);
            spear.ChangeState(Spear_StateNames.RECALLED);
        }
    }

    public void EnableCursor()
    {
        _cursor.transform.position = transform.position;
        _cursor.SetActive(true);
        _playerFeedbacks.ChangeCursorColor(_currentlyHeldSpear.IsLeft);
    }

    public void DisableCursor()
    {
        _cursor.SetActive(false);
    }

    internal void OrientateBody()
    {
        _body.transform.forward =
        new Vector3(_cursor.transform.position.x, transform.position.y, _cursor.transform.position.z) - transform.position;
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
            _playerFeedbacks.SetCameraTargetWeight(4, 0);
        }
    }


    #endregion

    #region DodgeRoll
    private void DodgeInput()
    {
        if ((_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKING) && _dodgeCooldown <= 0)
            DodgeRoll();

        else if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            CancelAim();
    }

    private void DodgeRoll()
    {
        _fsm.ChangeState(Ccl_StateNames.DODGING);
    }

    public void StartDodgeCooldown()
    {
        _dodgeCooldown = _dodgeMaxCooldown;
        this._playerFeedbacks.SetTrailRenderer(false, true);
        _canDodge = false;
    }

    private void CanDashAgain()
    {
        this._playerFeedbacks.SetTrailRenderer(false, false);
        _canDodge = true;
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
        _playerFeedbacks.PlayPunch0();
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
        _playerFeedbacks.PlayPunch1();
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
        _playerFeedbacks.PlayPunch2();
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

        if (!_canDodge)
        {
            _dodgeCooldown -= Time.deltaTime;
            if (_dodgeCooldown <= 0) CanDashAgain();
        }
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
