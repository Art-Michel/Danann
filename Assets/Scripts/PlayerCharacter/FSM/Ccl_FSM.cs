using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Ccl_FSM : MonoBehaviour
{
    private Dictionary<string, Ccl_State> states;

    public Ccl_State currentState { get; private set; }
    public Ccl_State previousState { get; private set; }

    PlayerActions _playerActions;
    PlayerFeedbacks _playerFeedbacks;

    public Transform Body;

    void Awake()
    {
        states = new Dictionary<string, Ccl_State>();
        _playerActions = GetComponent<PlayerActions>();
        _playerFeedbacks = GetComponent<PlayerFeedbacks>();
    }

    void Start()
    {
        AddState(new Ccl_StateLightAttacking());
        AddState(new Ccl_StateLightAttackStartup());
        AddState(new Ccl_StateLightAttackRecovery());
        AddState(new Ccl_StateIdle());
        AddState(new Ccl_StateAiming());
        AddState(new Ccl_StateThrowing());
        AddState(new Ccl_StateDashing());
        AddState(new Ccl_StateShielding());
        AddState(new Ccl_StateRecalling());
        AddState(new Ccl_StateDodging());
        AddState(new Ccl_StateTargetting());

        ChangeState(Ccl_StateNames.IDLE);
    }

    public void AddState(Ccl_State state)
    {
        state._fsm = this;
        state._actions = _playerActions;
        state._feedbacks = _playerFeedbacks;
        states[state.Name] = state;
    }

    public void ChangeState(string nextStateName)
    {
        Ccl_State nextState = null;
        states.TryGetValue(nextStateName, out nextState);

        if (nextState == null)
        {
            Debug.LogError(nextStateName + " doesn't exist");
            return;
        }

        if (nextState == currentState)
            return;

        if (currentState != null)
            currentState.Exit();

        previousState = currentState;
        currentState = nextState;
        currentState.Begin();

        //Debug.Log(currentState.Name + " started");
    }

    private void Update()
    {
        if (currentState != null)
            currentState.StateUpdate();
    }
}
