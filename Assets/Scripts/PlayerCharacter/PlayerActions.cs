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
    [Foldout("LightAttackFrameData"), SerializeField] float[] _lightStartup;
    public float GetStartupTime() { return _lightStartup[_currentLightAttackIndex]; }
    [Foldout("LightAttackFrameData"), SerializeField] float[] _lightActive;
    public float GetActiveTime() { return _lightActive[_currentLightAttackIndex]; }
    [Foldout("LightAttackFrameData"), SerializeField] float[] _lightRecovery;
    public float GetRecoveryTime() { return _lightRecovery[_currentLightAttackIndex]; }
    [Foldout("LightAttackFrameData"), SerializeField] float[] _moveSpeedWhileAttacking;
    public float GetAttackingMoveSpeed() { return _moveSpeedWhileAttacking[_currentLightAttackIndex]; }


    public int _currentLightAttackIndex { get; private set; } = 0;
    float _comboWindow = 0f;
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

    [Required][SerializeField] AttackData _DashAttackData;

    #endregion

    #region Dashing
    public Spear_FSM spearDashedOn {get; private set;}
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

        _inputs.Actions.DashL.started += _ => ParryInput(_leftSpear);
        _inputs.Actions.DashR.started += _ => ParryInput(_rightSpear);
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
        if ((_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKING || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKSTARTUP) && _dodgeCooldown <= 0)
            DodgeRoll();

        else if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            CancelAim();
    }

    private void DodgeRoll()
    {
        if (_fsm.currentState.Name != Ccl_StateNames.LIGHTATTACKING) PlayerMovement.OrientateBodyInstantlyTowardsStickDirection();
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
        if (_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY)
            LightAttack();
        else if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            CancelAim();
    }

    private void LightAttack()
    {
        PlayerMovement.OrientateBodyInstantlyTowardsStickDirection();
        _fsm.ChangeState(Ccl_StateNames.LIGHTATTACKSTARTUP);
    }

    public void IncreaseLightAttackIndex()
    {
        if (_currentLightAttackIndex < 2)
            _currentLightAttackIndex++;
        else
            _currentLightAttackIndex = 0;
    }

    public void EnableLightAttackHitbox()
    {
        switch (_currentLightAttackIndex)
        {
            case 0:
                _lightAttack0Data.LaunchAttack();
                break;
            case 1:
                _lightAttack1Data.LaunchAttack();
                break;
            case 2:
                _lightAttack2Data.LaunchAttack();
                break;
        }
    }

    public void SlowDownDuringAttack()
    {
        switch (_currentLightAttackIndex)
        {
            case 0:
                this.PlayerMovement.MovementSpeed = this.PlayerMovement._normalSpeed * _moveSpeedWhileAttacking[0];
                break;
            case 1:
                this.PlayerMovement.MovementSpeed = this.PlayerMovement._normalSpeed * _moveSpeedWhileAttacking[1];
                break;
            case 2:
                this.PlayerMovement.MovementSpeed = this.PlayerMovement._normalSpeed * _moveSpeedWhileAttacking[2];
                break;
        }
    }

    public void ResetMovementSpeed()
    {
        this.PlayerMovement.MovementSpeed = this.PlayerMovement._normalSpeed;
    }

    public void DisableLightAttackHitbox()
    {
        switch (_currentLightAttackIndex)
        {
            case 0:
                _lightAttack0Data.StopAttack();
                break;
            case 1:
                _lightAttack1Data.StopAttack();
                break;
            case 2:
                _lightAttack2Data.StopAttack();
                break;
        }
    }

    public void ResetComboWindow()
    {
        _comboWindow = GetRecoveryTime() + 0.4f;
    }
    #endregion

    #region Dash / Parry
    void ParryInput(Spear_FSM spear)
    {
        switch (spear.currentState.Name)
        {
            case Spear_StateNames.ATTACHED:
            Parry(spear);
            break;
            case Spear_StateNames.AIMING:
            Parry(spear);
            break;

            case Spear_StateNames.IDLE:
            Dash(spear);
            break;
            case Spear_StateNames.ATTACKING:
            Dash(spear);
            break;

            case Spear_StateNames.THROWN:
            BufferDash(spear);
            break;
            case Spear_StateNames.RECALLED:
            BufferParry(spear);
            break;
        }
    }

    private void BufferParry(Spear_FSM spear)
    {
        
    }

    private void BufferDash(Spear_FSM spear)
    {

    }

    private void Dash(Spear_FSM spear)
    {
        spearDashedOn = spear;
        _fsm.ChangeState(Ccl_StateNames.DASHING);
    }
    public void EnableDashHitbox()
    {
        _DashAttackData.LaunchAttack();
    }
    public void DisableDashHitbox()
    {
        _DashAttackData.StopAttack();
    }

    private void Parry(Spear_FSM spear)
    {

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
