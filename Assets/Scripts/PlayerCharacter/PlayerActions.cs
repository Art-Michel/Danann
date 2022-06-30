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
    public PlayerPlasma _playerPlasma { get; private set; }
    [Required] public Spear_FSM _leftSpear;
    [Required] public Spear_FSM _rightSpear;

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
    public Spear_FSM _currentlyTargettedSpear { get; private set; }
    #endregion

    #region Init Aiming
    [SerializeField] Transform _body;
    public Spear_FSM CurrentlyHeldSpear { get; private set; }
    [SerializeField] GameObject _cursor;
    #endregion

    #region Init Dodge Rolling
    public CharacterController Characon { get; private set; }
    public PlayerHP PlayerHP { get; private set; }
    bool _canDodge;
    float _dodgeCooldown = 0;
    float _dodgeBigCooldown = 0;
    const float _dodgeMaxCooldown = 1.5f;
    const float _dodgeResetCooldown = 1f;
    const float _dodgeMinCooldown = 0.12f;
    int _dodgesLeft;
    float _timeSinceLastDodged;
    private bool _isPressingDodge;
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

    #region Init Shielding
    public Spear_FSM SpearUsedToShield { get; private set; }
    Hurtbox _hurtbox;
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
        this._hurtbox = GetComponent<Hurtbox>();

        //Inputs
        _inputs.Actions.LightAttack.started += _ => LightAttackInput();

        _inputs.Actions.Dodge.started += _ => DodgeInput();
        _inputs.Actions.Dodge.canceled += _ => DodgeInputReleased();

        _inputs.Actions.ThrowL.started += _ => PressTrigger(_leftSpear);
        _inputs.Actions.ThrowR.started += _ => PressTrigger(_rightSpear);
        _inputs.Actions.ThrowL.canceled += _ => ReleaseTrigger(_leftSpear);
        _inputs.Actions.ThrowR.canceled += _ => ReleaseTrigger(_rightSpear);

        _inputs.Actions.Shield.started += _ => ShieldInput();
    }

    void Start()
    {
        SetDodgesLeft(3);
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
            //canBeTargetted = canBeTargetted || spear.currentState.Name == Spear_StateNames.THROWN;
            bool canTarget = _fsm.currentState.Name == Ccl_StateNames.IDLE;
            canTarget = canTarget || _fsm.currentState.Name == Ccl_StateNames.DODGING;
            canTarget = canTarget || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY;
            canTarget = canTarget || _fsm.currentState.Name == Ccl_StateNames.RECALLING;
            canTarget = canTarget || _fsm.currentState.Name == Ccl_StateNames.THROWING;
            if (canTarget && canBeTargetted)
                TargetSpear(spear);
            else if (_fsm.currentState.Name == Ccl_StateNames.TARGETTING && canBeTargetted)
            {
                Super(spear);
            }
            else if (canTarget && spear.currentState.Name == Spear_StateNames.THROWN)
            {
                Spear_StateThrown state = spear.currentState as Spear_StateThrown;
                state.BufferTarget();
            }
        }
    }

    private void ReleaseTrigger(Spear_FSM spear)
    {
        if (_fsm.currentState.Name == Ccl_StateNames.TRIANGLING)
        {
            Ccl_StateTriangling state = _fsm.currentState as Ccl_StateTriangling;
            state.StopAttack();
            _fsm.ChangeState(Ccl_StateNames.IDLE);
            spear.ChangeState(Spear_StateNames.RECALLED);
            if (spear == _rightSpear)
                TargetSpear(_leftSpear);
            else
                TargetSpear(_rightSpear);
        }
        else if (spear.currentState.Name == Spear_StateNames.THROWN)
        {
            Recall(spear);
        }
        else
        {
            if (_fsm.currentState.Name == Ccl_StateNames.AIMING && CurrentlyHeldSpear == spear)
                Throw();
            else if (_fsm.currentState.Name == Ccl_StateNames.TARGETTING && _currentlyTargettedSpear == spear)
                Recall(spear);
        }
    }
    #endregion

    #region Aiming
    void StartAiming(Spear_FSM spear)
    {
        CurrentlyHeldSpear = spear;
        _fsm.ChangeState(Ccl_StateNames.AIMING);
        spear.ChangeState(Spear_StateNames.AIMING);
        spear.SpearFeedbacks.AimedFeedbacks();
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
            _playerFeedbacks.SetAnimationTrigger("Idle");
            _playerFeedbacks.SetCameraTargetWeight(4, 0);
            CurrentlyHeldSpear.ChangeState(Spear_StateNames.ATTACHED);
            CurrentlyHeldSpear.SpearFeedbacks.UnaimedFeedbacks();
            CurrentlyHeldSpear = null;
            _isPressingDodge = false;
        }
    }
    #endregion

    #region Throwing

    private void Throw()
    {
        _fsm.ChangeState(Ccl_StateNames.THROWING);
        CurrentlyHeldSpear.ChangeState(Spear_StateNames.THROWN);
        CurrentlyHeldSpear.SpearFeedbacks.UnaimedFeedbacks();
        CurrentlyHeldSpear = null;
    }
    #endregion

    #region Targetting
    public void TargetSpear(Spear_FSM spear)
    {
        _currentlyTargettedSpear = spear;
        _fsm.ChangeState(Ccl_StateNames.TARGETTING);

        _playerFeedbacks.TargetFeedbacks();
        _currentlyTargettedSpear.SpearFeedbacks.TargettedFeedbacks();
    }

    public void StopTargettingSpear(bool shouldPlaySound, Spear_FSM spear)
    {
        spear.SpearFeedbacks.UntargettedFeedbacks(shouldPlaySound);
        _playerFeedbacks.UntargetFeedbacks();

        _fsm.ChangeState(Ccl_StateNames.IDLE);
        _currentlyTargettedSpear = null;
    }
    #endregion

    #region Super
    void Super(Spear_FSM spear)
    {
        if (_playerPlasma.VerifyPlasma(Ccl_Attacks.TRIANGLEBOOM))
        {
            _currentlyTargettedSpear.ChangeState(Spear_StateNames.TRIANGLING);
            spear.ChangeState(Spear_StateNames.TRIANGLING);
            StopTargettingSpear(false, spear);
            _rightSpear.SpearFeedbacks.TargettedFeedbacks();
            _leftSpear.SpearFeedbacks.TargettedFeedbacks();
            _playerFeedbacks.TargetFeedbacks();
            _fsm.ChangeState(Ccl_StateNames.TRIANGLING);
        }
        else
        {
            _playerFeedbacks.NotEnoughPlasmaText();
            _playerFeedbacks.PlayErrorSfx();
        }
    }

    void CancelSuper()
    {
        Ccl_StateTriangling state = _fsm.currentState as Ccl_StateTriangling;
        state.StopAttack();
        _fsm.ChangeState(Ccl_StateNames.IDLE);
        StopTargettingSpear(false, _rightSpear);
        StopTargettingSpear(false, _leftSpear);
        _leftSpear.SpearFeedbacks.UntargettedFeedbacks(false);
        _rightSpear.SpearFeedbacks.UntargettedFeedbacks(false);
    }

    public void RecallSpears()
    {
        StopTargettingSpear(_leftSpear, _leftSpear);
        StopTargettingSpear(_rightSpear, _rightSpear);
        _rightSpear.ChangeState(Spear_StateNames.RECALLED);
        _leftSpear.ChangeState(Spear_StateNames.RECALLED);
    }

    public void ResetSpears()
    {
        _rightSpear.ChangeState(Spear_StateNames.IDLE);
        _leftSpear.ChangeState(Spear_StateNames.IDLE);
    }

    public void SlowDownDuringTriangling(float f)
    {
        this.PlayerMovement.MovementSpeed = this.PlayerMovement._normalSpeed * f;
    }
    #endregion

    #region Recalling
    void Recall(Spear_FSM spear)
    {
        Spear_FSM mario = spear;
        StopTargettingSpear(false, spear);
        mario.ChangeState(Spear_StateNames.RECALLED);
        _fsm.ChangeState(Ccl_StateNames.RECALLING);
    }
    #endregion

    #region DodgeRoll
    private void DodgeInput()
    {
        _isPressingDodge = true;
        
        if (_fsm.currentState.Name == Ccl_StateNames.TARGETTING && _currentlyTargettedSpear != null)
            Dash(_currentlyTargettedSpear);

        else if ((_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKING || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKSTARTUP))
            {
                if(_dodgesLeft <= 0 || !_canDodge)
                    _playerFeedbacks.PlayErrorSfx();
                TryToDodge();
            }

        else if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            CancelAim();

        else if (_fsm.currentState.Name == Ccl_StateNames.TRIANGLING)
            CancelSuper();
    }

    void DodgeInputReleased()
    {
        _isPressingDodge = false;
    }

    void TryToDodge()
    {
        bool canActuallyDodge = _fsm.currentState.Name == Ccl_StateNames.IDLE;
        canActuallyDodge = canActuallyDodge || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY;
        canActuallyDodge = canActuallyDodge || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKING;
        canActuallyDodge = canActuallyDodge || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKSTARTUP;
        canActuallyDodge = canActuallyDodge && _dodgesLeft > 0;
        canActuallyDodge = canActuallyDodge && _canDodge;
        if (canActuallyDodge) DodgeRoll();
    }

    private void DodgeRoll()
    {
        if (_dodgeCooldown <= 0)
        {
            if (_fsm.currentState.Name != Ccl_StateNames.LIGHTATTACKING)
                PlayerMovement.OrientateBodyInstantlyTowardsStickDirection();
            _fsm.ChangeState(Ccl_StateNames.DODGING);
        }
    }

    public void StartDodgeCooldown()
    {
        if (_dodgesLeft > 1)
        {
            SetDodgesLeft(_dodgesLeft - 1);
            _dodgeCooldown = _dodgeMinCooldown;
        }

        else
        {
            SetDodgesLeft(0);
            UiManager.Instance.SetDodgeTransparency(true);
        }

        _dodgeBigCooldown = _dodgeMaxCooldown;
        _canDodge = false;
    }

    private void CanDodgeAgain()
    {
        _canDodge = true;
    }

    void SetDodgesLeft(int i)
    {
        _dodgesLeft = i;
        UiManager.Instance.SetText(UiManager.Instance.DodgesCount, i.ToString());
    }
    #endregion

    #region Light Attack
    private void LightAttackInput()
    {
        if (_fsm.currentState.Name == Ccl_StateNames.TRIANGLING)
            CancelSuper();
        else if (_fsm.currentState.Name == Ccl_StateNames.TARGETTING)
            StopTargettingSpear(true, _currentlyTargettedSpear);
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

    #region Dash / Shield
    void ShieldInput()
    {
        if (_fsm.currentState.Name == Ccl_StateNames.TARGETTING)
            StopTargettingSpear(true, _currentlyTargettedSpear);
        else if (_fsm.currentState.Name == Ccl_StateNames.AIMING)
            CancelAim();
        else if (_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY
        || _fsm.currentState.Name == Ccl_StateNames.RECALLING || _fsm.currentState.Name == Ccl_StateNames.THROWING)
            Shield();
        else if (_fsm.currentState.Name == Ccl_StateNames.TRIANGLING)
            CancelSuper();
    }

    public void EnlargenHurtbox()
    {
        _hurtbox.SetRadius(3f);
    }

    public void ResetHurtboxSize()
    {
        _hurtbox.SetRadius(0.3f);
    }

    private void Dash(Spear_FSM spear)
    {
        if (_playerPlasma.VerifyPlasma(Ccl_Attacks.DASHONSPEAR))
        {
            _playerPlasma.SpendPlasma(Ccl_Attacks.DASHONSPEAR);
            StopTargettingSpear(false, _currentlyTargettedSpear);
            SpearDashedOn = spear;
            _fsm.ChangeState(Ccl_StateNames.DASHING);
            _isPressingDodge = false;
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

    private void Shield()
    {
        if (_playerPlasma.VerifyPlasma(Ccl_Attacks.SHIELD))
        {
            _playerPlasma.SpendPlasma(Ccl_Attacks.SHIELD);
            _fsm.ChangeState(Ccl_StateNames.SHIELDING);
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

        if (_isPressingDodge) TryToDodge();

        if (!_canDodge)
        {
            _dodgeCooldown -= Time.deltaTime;
            if (_dodgeCooldown <= 0) CanDodgeAgain();
        }

        if (_dodgesLeft < 3)
        {
            _dodgeBigCooldown -= Time.deltaTime;
            UiManager.Instance.FillDodge(Mathf.InverseLerp(0, _dodgeMaxCooldown, _dodgeBigCooldown));
            if (_dodgeBigCooldown <= 0)
            {
                SetDodgesLeft(3);
                _playerFeedbacks.StartBlink();
                _playerFeedbacks.PlayDodgeReffiledSfx();
                UiManager.Instance.SetDodgeTransparency(false);
            }
        }
    }

    #region disable inputs on Player disable to avoid weird inputs
    private void OnEnable()
    {
        _inputs.Enable();
        if(Gamepad.current !=null)Gamepad.current.SetMotorSpeeds(0,0);
    }

    private void OnDisable()
    {
        _inputs.Disable();
        if(Gamepad.current !=null)Gamepad.current.SetMotorSpeeds(0,0);
    }
    #endregion
}
