using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorMovement : MonoBehaviour
{
    PlayerInputMap _inputs;
    CharacterController _charaCon;
    Ccl_FSM _fsm;

    bool _isMoving;
    Vector3 _wantedDirection;
    [SerializeField] float _normalSpeed;
    [NonSerialized] public float MovementSpeed;
    float _easeInValue;
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
        _wantedDirection = Vector3.zero;
        _easeInValue = 0;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isMoving)
        {
            EaseInMovement();
            Move();
        }
        if (_rb.IsSleeping())
            _rb.WakeUp();
    }

    private void ReadMovementInputs()
    {
        _isMoving = true;
        _easeInValue = 0;
    }

    private void StopReadingMovementInputs()
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

    private void Move()
    {
        _wantedDirection = new Vector3(_inputs.Movement.CursorMove.ReadValue<Vector2>().x, 0, _inputs.Movement.CursorMove.ReadValue<Vector2>().y);
        _charaCon.Move(_wantedDirection * MovementSpeed * _easeInValue * Time.deltaTime);
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
