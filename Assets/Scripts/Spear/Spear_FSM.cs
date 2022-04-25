using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_FSM : MonoBehaviour
{
    private Dictionary<string, Spear_State> states;

    public Spear_State currentState { get; private set; }
    public Spear_State previousState { get; private set; }

    void Awake()
    {
        states = new Dictionary<string, Spear_State>();
    }

    public void AddState(Spear_State state)
    {
        state.fsm = this;
        states[state.StateName] = state;
    }

    public void ChangeState(string nextStateName)
    {
        Spear_State nextState = null;
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

        Debug.Log(currentState.StateName + " started");
    }

    private void Update()
    {
        if (currentState != null)
            currentState.Update();
    }
}
