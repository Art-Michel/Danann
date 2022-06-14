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
    PlayerPlasma _playerPlasma;
    [Required][SerializeField] Spear_FSM _leftSpear;
    [Required][SerializeField] Spear_FSM _rightSpear;

    #endregion

    //Feedbacks
    [Required] PlayerFeedbacks _playerFeedbacks;

    #region Init Light Attack
    [Foldout("LightAttackFrameData"), SerializeField] float[] _lightStartup;
    public float GetStartupTime() { return _lightStartup[CurrentLightAttackIndex]; }
    [Foldout("LightAttackFrameData"), SerializeField] float[] _lightActive;
    public float GetActiveTime() { return _lightActive[CurrentLightAttackIndex]; }
    [Foldout("LightAttackFrameData"), SerializeField] float[] _lightRecovery;
    public float GetRecoveryTime() { return _lightRecovery[CurrentLightAttackIndex]; }
    [Foldout("LightAttackFrameData"), SerializeField] float[] _moveSpeedWhileAttacking;
    public float GetAttackingMoveSpeed() { return _moveSpeedWhileAttacking[CurrentLightAttackIndex]; }


    public int CurrentLightAttackIndex { get; private set; } = 0;
    float _comboWindow = 0f;
    #endregion

    #region Init Targetting
    public Spear_FSM _currentlyTargettedSpear{get; private set;}
    #endregion

    #region Init Aiming
    [SerializeField] Transform _body;
    public Spear_FSM CurrentlyHeldSpear{get; private set;}
    [SerializeField] GameObject _cursor;
    #endregion

    #region Init Dodge Rolling
    public CharacterController Characon { get; private set; }
    public PlayerHP PlayerHP { get; private set; }
    bool _canDodge;
    float _dodgeCooldown = 0;
    const float _dodgeMaxCooldown = 0.25f;
    #endregion

    #region Init Attacks Data
    [Required][SerializeField] AttackData _lightAttack0Data;
    [Required][SerializeField] AttackData _lightAttack1Data;
    [Required][SerializeField] AttackData _lightAttack2Data;

    [Required][SerializeField] AttackData _DashAttackData;

    #endregion

    #region Init Dashing
    public Spear_FSM SpearDashedOn { get; private set; }
    #endregion

    #region Init Parrying
    public Spear_FSM SpearUsedToParry { get; private set; }
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
        this._playerPlasma = GetComponent<PlayerPlasma>();

        //Inputs
        _inputs.Actions.LightAttack.started += _ => LightAttackInput();

        _inputs.Actions.Dodge.started += _ => DodgeInput();

        _inputs.Actions.ThrowL.started += _ => PressTrigger(_leftSpear);
        _inputs.Actions.ThrowR.started += _ => PressTrigger(_rightSpear);
        _inputs.Actions.ThrowL.canceled += _ => ReleaseTrigger(_leftSpear);
        _inputs.Actions.ThrowR.canceled += _ => ReleaseTrigger(_rightSpear);

        _inputs.Actions.Parry.started += _ => ParryInput();

    }

    #region Triggers Inputs
    private void PressTrigger(Spear_FSM spear)
    {
        if (_fsm.currentState.Name == Ccl_StateNames.IDLE && spear.currentState.Name == Spear_StateNames.ATTACHED)
            StartAiming(spear);
        else
        {
            bool canBeTargetted = spear.currentState.Name == Spear_StateNames.IDLE;
            canBeTargetted = canBeTargetted || spear.currentState.Name == Spear_StateNames.ATTACKING;
            canBeTargetted = canBeTargetted || spear.currentState.Name == Spear_StateNames.THROWN;
            bool canTarget = _fsm.currentState.Name != Ccl_StateNames.TARGETTING;
            if (canTarget && canBeTargetted)
                TargetSpear(spear);
        }
    }

    private void ReleaseTrigger(Spear_FSM spear)
    {
        if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            Throw();
        else if (_fsm.currentState.Name == Ccl_StateNames.TARGETTING && _currentlyTargettedSpear == spear)
            Recall();
    }
    #endregion

    #region Aiming
    void StartAiming(Spear_FSM spear)
    {
        CurrentlyHeldSpear = spear;
        _fsm.ChangeState(Ccl_StateNames.AIMING);
        spear.ChangeState(Spear_StateNames.AIMING);
    }

    public void EnableCursor()
    {
        _cursor.transform.position = transform.position;
        _playerFeedbacks.SetCameraTargetWeight(4, 1);
        _cursor.SetActive(true);
        _playerFeedbacks.ChangeCursorColor(CurrentlyHeldSpear.SpearAi.IsLeft);
    }

    public void DisableCursor()
    {
        _cursor.SetActive(false);
    }

    internal void OrientateBody()
    {
        if (_cursor.transform.position != transform.position)
        {
            _body.transform.forward =
            new Vector3(_cursor.transform.position.x, transform.position.y, _cursor.transform.position.z) - transform.position;
        }
    }

    private void CancelAim()
    {
        {
            _fsm.ChangeState(Ccl_StateNames.IDLE);
            _playerFeedbacks.SetCameraTargetWeight(4, 0);
            CurrentlyHeldSpear.ChangeState(Spear_StateNames.ATTACHED);
            CurrentlyHeldSpear = null;
        }
    }
    #endregion

    #region Throwing

    private void Throw()
    {
        _fsm.ChangeState(Ccl_StateNames.THROWING);
        CurrentlyHeldSpear.ChangeState(Spear_StateNames.THROWN);
        CurrentlyHeldSpear = null;
    }
    #endregion

    #region Targetting
    void TargetSpear(Spear_FSM spear)
    {
        _currentlyTargettedSpear = spear;
        _fsm.ChangeState(Ccl_StateNames.TARGETTING);

        _playerFeedbacks.TargetFeedbacks();
        _currentlyTargettedSpear.SpearFeedbacks.TargettedFeedbacks();
    }

    public void StopTargettingSpear(bool shouldPlaySound)
    {
        _currentlyTargettedSpear.SpearFeedbacks.UntargettedFeedbacks(shouldPlaySound);
        _playerFeedbacks.UntargetFeedbacks();

        _fsm.ChangeState(Ccl_StateNames.IDLE);
        _currentlyTargettedSpear = null;
    }
    #endregion

    #region Recalling
    void Recall()
    {
        _fsm.ChangeState(Ccl_StateNames.RECALLING);
        _currentlyTargettedSpear.ChangeState(Spear_StateNames.RECALLED);
        StopTargettingSpear(false);
    }
    #endregion

    #region DodgeRoll`
    private void DodgeInput()
    {
        if (_fsm.currentState.Name == Ccl_StateNames.TARGETTING && _currentlyTargettedSpear != null)
            Dash(_currentlyTargettedSpear);

        else if ((_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKING || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKSTARTUP) && _dodgeCooldown <= 0)
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
        this._playerFeedbacks.SetTrailRenderer(true, false);
        _canDodge = false;
    }

    private void CanDodgeAgain()
    {
        this._playerFeedbacks.SetTrailRenderer(false, false);
        _canDodge = true;
    }
    #endregion

    #region Light Attack
    private void LightAttackInput()
    {
        if (_fsm.currentState.Name == Ccl_StateNames.TARGETTING)
            StopTargettingSpear(true);
        else if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            CancelAim();
        else if (_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY)
            LightAttack();
    }

    private void LightAttack()
    {
        PlayerMovement.OrientateBodyInstantlyTowardsStickDirection();
        _fsm.ChangeState(Ccl_StateNames.LIGHTATTACKSTARTUP);
    }

    public void IncreaseLightAttackIndex()
    {
        if (CurrentLightAttackIndex < 2)
            CurrentLightAttackIndex++;
        else
            CurrentLightAttackIndex = 0;
    }

    public void EnableLightAttackHitbox()
    {
        switch (CurrentLightAttackIndex)
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
        switch (CurrentLightAttackIndex)
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
        switch (CurrentLightAttackIndex)
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
    void ParryInput()
    {
        if (_fsm.currentState.Name == Ccl_StateNames.TARGETTING)
            StopTargettingSpear(true);
        else if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            CancelAim();
        else if (_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY
        || _fsm.currentState.Name == Ccl_StateNames.RECALLING || _fsm.currentState.Name == Ccl_StateNames.THROWING)
            Parry();
    }

    private void BufferParry()
    {

    }

    private void BufferDash()
    {

    }

    private void Dash(Spear_FSM spear)
    {
        if (_playerPlasma.VerifyPlasma(Ccl_Attacks.DASHONSPEAR))
        {
            _playerPlasma.SpendPlasma(Ccl_Attacks.DASHONSPEAR);
            StopTargettingSpear(false);
            SpearDashedOn = spear;
            _fsm.ChangeState(Ccl_StateNames.DASHING);
        }
    }
    public void EnableDashHitbox()
    {
        _DashAttackData.LaunchAttack();
    }
    public void DisableDashHitbox()
    {
        _DashAttackData.StopAttack();
    }

    private void Parry()
    {
        if (_playerPlasma.VerifyPlasma(Ccl_Attacks.PARRY))
        {
            _playerPlasma.SpendPlasma(Ccl_Attacks.PARRY);
            _fsm.ChangeState(Ccl_StateNames.PARRYING);
        }
    }

    #endregion

    void Update()
    {
        if (_fsm.currentState.Name != Ccl_StateNames.LIGHTATTACKING)
        {
            _comboWindow -= Time.deltaTime;
        }
        if (_comboWindow <= 0) CurrentLightAttackIndex = 0;

        if (!_canDodge)
        {
            _dodgeCooldown -= Time.deltaTime;
            if (_dodgeCooldown <= 0) CanDodgeAgain();
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
