using System.Runtime.CompilerServices;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorMovement : MonoBehaviour
{
    PlayerInputMap _inputs;
    CharacterController _charaCon;
    Ccl_FSM _fsm;

    bool _isMoving;
    public Vector3 WantedDirection;
    [SerializeField] float _normalSpeed;
    const float _rightStickMultiplier = 4;
    [NonSerialized] public float MovementSpeed;
    public float EaseInValue;
    [SerializeField] float _easeInSpeed;
    Rigidbody _rb;

    private void Awake()
    {
        _fsm = GetComponent<Ccl_FSM>();
        _inputs = new PlayerInputMap();

        _inputs.Movement.CursorMove.started += _ => ReadMovementInputs();
        _inputs.Movement.CursorMove.canceled += _ => StopReadingMovementInputs();
    }

    private void Start()
    {
        _charaCon = GetComponent<CharacterController>();
        ResetMovementSpeed();
        _isMoving = false;
        WantedDirection = Vector3.zero;
        EaseInValue = 0;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isMoving)
            CalculateWantedDirection();
        EaseInMovement();
        Move();

        if (_rb.IsSleeping())
            _rb.WakeUp();
    }

    private void ReadMovementInputs()
    {
        _isMoving = true;
        EaseInValue = 0;
    }

    private void StopReadingMovementInputs()
    {
        _isMoving = false;
        WantedDirection = Vector3.zero;
    }

    public void ResetMovementSpeed()
    {
        MovementSpeed = _normalSpeed;
    }

    private void EaseInMovement()
    {
        if (EaseInValue < 1)
        {
            EaseInValue += Time.deltaTime * _easeInSpeed;
            EaseInValue = Mathf.Clamp(EaseInValue, 0, 1);
        }
    }

    void CalculateWantedDirection()
    {
        WantedDirection += new Vector3(_inputs.Movement.CursorMove.ReadValue<Vector2>().x, 0f, _inputs.Movement.CursorMove.ReadValue<Vector2>().y) * _rightStickMultiplier;
    }

    private void Move()
    {
        _charaCon.Move(WantedDirection * MovementSpeed * EaseInValue * Time.deltaTime);
        if (_isMoving) WantedDirection = Vector3.zero;
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
