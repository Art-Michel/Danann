using System;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class PlayerMovement : MonoBehaviour
{
    PlayerInputMap _inputs;
    CharacterController _charaCon;
    Ccl_FSM _fsm;
    [SerializeField] CursorMovement _cursorMovement;

    bool _isMoving;
    public bool CanWalk;
    public Vector3 _wantedDirection;
    public float _normalSpeed;
    [NonSerialized] public float MovementSpeed;
    float _easeInValue;
    [SerializeField] float _easeInSpeed;
    Rigidbody _rb;

    [Required] public GameObject PlayerBody;
    private const int BodyRotatingSpeed = 50;

    private void Awake()
    {
        _fsm = GetComponent<Ccl_FSM>();
        _inputs = new PlayerInputMap();

        _inputs.Movement.Move.started += ReadMovementInputs;
        _inputs.Movement.Move.canceled += StopReadingMovementInputs;
    }

    private void Start()
    {
        _charaCon = GetComponent<CharacterController>();
        ResetMovementSpeed();
        _isMoving = false;
        _wantedDirection = Vector3.zero;
        _easeInValue = 0;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        bool canMove = _isMoving && _fsm.currentState.Name == Ccl_StateNames.IDLE;
        canMove = canMove || _fsm.currentState.Name == Ccl_StateNames.AIMING;
        canMove = canMove || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKING;
        canMove = canMove || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKSTARTUP;
        canMove = canMove || _fsm.currentState.Name == Ccl_StateNames.LIGHTATTACKRECOVERY;
        if (canMove)
        {
            EaseInMovement();
            Move();
        }

        if (_rb.IsSleeping())
            _rb.WakeUp();
    }

    private void ReadMovementInputs(InputAction.CallbackContext obj)
    {
        _isMoving = true;
        _easeInValue = 0;
    }

    private void StopReadingMovementInputs(InputAction.CallbackContext obj)
    {
        _isMoving = false;
        _wantedDirection = Vector3.zero;
    }

    public void ResetMovementSpeed()
    {
        MovementSpeed = _normalSpeed;
    }

    private void EaseInMovement()
    {
        if (_easeInValue < 1)
        {
            _easeInValue += Time.deltaTime * _easeInSpeed;
            _easeInValue = Mathf.Clamp(_easeInValue, 0, 1);
        }
    }

    public void Move()
    {
        _wantedDirection = new Vector3(_inputs.Movement.Move.ReadValue<Vector2>().x, 0, _inputs.Movement.Move.ReadValue<Vector2>().y);
        _charaCon.Move(_wantedDirection * MovementSpeed * _easeInValue * Time.deltaTime);
        if (_cursorMovement)
        {
            _cursorMovement.WantedDirection = _wantedDirection;
            _cursorMovement.EaseInValue = 1;
        }

        if (_fsm.currentState.Name == Ccl_StateNames.IDLE) RotateBody();
    }

    private void RotateBody()
    {
        Vector3 bodyRotationOffset = new Vector3(0.01f, 0, 0.01f);
        //Ridiculously small offset to prevent body from looking directly in a cardinal direction
        //as it is very slow to RLerp from one rotation to its exact opposite.

        PlayerBody.transform.forward = Vector3.Lerp(PlayerBody.transform.forward, _wantedDirection + bodyRotationOffset, BodyRotatingSpeed * Time.deltaTime);
    }

    public void ReorientateBodyImmediately()
    {
        if(_wantedDirection != Vector3.zero) PlayerBody.transform.forward = _wantedDirection;
    }

    public Vector3 GetOrientation()
    {
        Debug.Log(_wantedDirection);
        if(_wantedDirection == Vector3.zero) return PlayerBody.transform.forward;
        else return _wantedDirection;
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
