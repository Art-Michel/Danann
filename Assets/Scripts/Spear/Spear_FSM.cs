using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_FSM : MonoBehaviour
{
    private Dictionary<string, Spear_State> states;

    public Spear_State currentState { get; private set; }
    public Spear_State previousState { get; private set; }

    public GameObject Cursor;
    public AttackData TravelingAttackData;
    public float TravelSpeed = 5;

    void Awake()
    {
        states = new Dictionary<string, Spear_State>();
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

        Debug.Log(currentState.Name + " started");
    }

    private void Update()
    {
        if (currentState != null)
            currentState.Update();
    }
}
