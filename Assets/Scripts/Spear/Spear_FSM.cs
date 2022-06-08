using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_FSM : MonoBehaviour
{
    private Dictionary<string, Spear_State> states;
    public SpearAI SpearAi { get; private set; }
    public SpearFeedbacks SpearFeedbacks { get; private set; }

    public Spear_State currentState { get; private set; }
    public Spear_State previousState { get; private set; }


    void Awake()
    {
        states = new Dictionary<string, Spear_State>();
        SpearAi = GetComponent<SpearAI>();
        this.SpearFeedbacks = GetComponent<SpearFeedbacks>();
    }

    void Start()
    {
        AddState(new Spear_StateAttached());
        AddState(new Spear_StateThrown());
        AddState(new Spear_StateIdle());
        AddState(new Spear_StateAiming());
        AddState(new Spear_StateRecalled());
        AddState(new Spear_StateTriangling());
        AddState(new Spear_StateAttacking());

        ChangeState(Spear_StateNames.ATTACHED);
    }

    public void AddState(Spear_State state)
    {
        state._fsm = this;
        state._ai = SpearAi;
        state._feedbacks = this.SpearFeedbacks;
        states[state.Name] = state;
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

        //Debug.Log(currentState.Name + " started");
    }

    private void Update()
    {
        if (currentState != null)
            currentState.Update();
    }
}
