using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_FSM : MonoBehaviour
{
    private Dictionary<string, Ccl_State> states;

    public Ccl_State currentState { get; private set; }
    public Ccl_State previousState { get; private set; }

    void Awake()
    {
        states = new Dictionary<string, Ccl_State>();
    }

    void Start()
    {
    }

    public void AddState(Ccl_State state)
    {
        state.fsm = this;
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

        Debug.Log(currentState.Name + " started");
    }

    private void Update()
    {
        if (currentState != null)
            currentState.Update();
    }
}
