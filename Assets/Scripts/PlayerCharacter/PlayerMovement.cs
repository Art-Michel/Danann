using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInputMap _inputs;
    CharacterController _charaCon;
    Ccl_FSM _fsm;

    bool _isMoving;
    Vector3 _wantedDirection;
    [SerializeField] float _speed;
    float _easeInValue;
    [SerializeField] float _easeInSpeed;
    Rigidbody _rb;

    [SerializeField] GameObject _playerBody;
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
        this._fsm.AddState(new Ccl_StateIdle());
        this._fsm.AddState(new Ccl_StateAiming());
        
        _charaCon = GetComponent<CharacterController>();
        _isMoving = false;
        _wantedDirection = Vector3.zero;
        _easeInValue = 0;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isMoving && (_fsm.currentState.Name == Ccl_StateNames.IDLE || _fsm.currentState.Name == Ccl_StateNames.AIMING))
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
        _wantedDirection = new Vector3(_inputs.Movement.Move.ReadValue<Vector2>().x, 0, _inputs.Movement.Move.ReadValue<Vector2>().y);
        _charaCon.Move(_wantedDirection * _speed * _easeInValue * Time.deltaTime);

        if (_fsm.currentState.Name == Ccl_StateNames.IDLE) RotateBody();
    }

    private void RotateBody()
    {
        Vector3 bodyRotationOffset = new Vector3(0.01f, 0, 0.01f);
        //Ridiculously small offset to prevent body from looking directly in a cardinal direction
        //as it takes a long time to go the opposite way afterwards

        _playerBody.transform.forward = Vector3.Lerp(_playerBody.transform.forward, _wantedDirection + bodyRotationOffset, BodyRotatingSpeed * Time.deltaTime);
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
