using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_State
{
    public Ccl_FSM _fsm = null;
    public PlayerActions _pa = null;

    public string Name { get; private set; }

    public Ccl_State(string name)
    {
        this.Name = name;
    }

    public virtual void Begin()
    {

    }

    public virtual void StateUpdate()
    {

    }

    public virtual void Exit()
    {
        
    }
}
