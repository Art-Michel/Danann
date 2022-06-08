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

    #region Init Targetting
    Spear_FSM _currentlyTargettedSpear;
    #endregion

    #region Init Aiming
    [SerializeField] Transform _body;
    Spear_FSM _currentlyHeldSpear;
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

        _inputs.Actions.ThrowL.started += _ => AimInput(_leftSpear);
        _inputs.Actions.ThrowR.started += _ => AimInput(_rightSpear);
        _inputs.Actions.ThrowL.canceled += _ => ThrowInput(_leftSpear);
        _inputs.Actions.ThrowR.canceled += _ => ThrowInput(_rightSpear);

        /*_inputs.Actions.DashL.started += _ => ParryInput(_leftSpear);
        _inputs.Actions.DashR.started += _ => ParryInput(_rightSpear);*/
    }

    #region Triggers Inputs
    private void AimInput(Spear_FSM spear)
    {
        if (_fsm.currentState.Name == Ccl_StateNames.IDLE && spear.currentState.Name == Spear_StateNames.ATTACHED)
            StartAiming(spear);
        else
        {
            bool canTarget = spear.currentState.Name != Spear_StateNames.ATTACHED;
            canTarget = canTarget && spear.currentState.Name != Spear_StateNames.AIMING;
            canTarget = canTarget && _fsm.currentState.Name != Ccl_StateNames.TARGETTING;
            if (canTarget)
                TargetSpear(spear);
        }
    }

    private void ThrowInput(Spear_FSM spear)
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
        _currentlyHeldSpear = spear;
        _fsm.ChangeState(Ccl_StateNames.AIMING);
        spear.ChangeState(Spear_StateNames.AIMING);
    }

    public void EnableCursor()
    {
        _cursor.transform.position = transform.position;
        _playerFeedbacks.SetCameraTargetWeight(4, 1);
        _cursor.SetActive(true);
        _playerFeedbacks.ChangeCursorColor(_currentlyHeldSpear.SpearAi.IsLeft);
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
            _currentlyHeldSpear.ChangeState(Spear_StateNames.ATTACHED);
            _currentlyHeldSpear = null;
        }
    }
    #endregion

    #region Throwing

    private void Throw()
    {
        _fsm.ChangeState(Ccl_StateNames.THROWING);
        _currentlyHeldSpear.ChangeState(Spear_StateNames.THROWN);
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

    public void StopTargettingSpear()
    {
        _currentlyTargettedSpear.SpearFeedbacks.UntargettedFeedbacks();
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
        StopTargettingSpear();
    }
    #endregion

    #region DodgeRoll
    private void DodgeInput()
    {
        if (_currentlyTargettedSpear != null)
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
            StopTargettingSpear();
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
    /*void ParryInput(Spear_FSM spear)
    {
        bool canDash = _fsm.currentState.Name == Ccl_StateNames.IDLE;
        canDash = canDash || _fsm.currentState.Name == Ccl_StateNames.DODGING;
        canDash = canDash || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY;

        if (canDash)
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
    }*/

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
            StopTargettingSpear();
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
            SpearDashedOn = _currentlyTargettedSpear;
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
        if (_comboWindow <= 0) _currentLightAttackIndex = 0;

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
