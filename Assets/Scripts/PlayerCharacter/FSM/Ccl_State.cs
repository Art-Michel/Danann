using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_State
{
    public Ccl_FSM fsm = null;

    public string StateName { get; private set; }

    public Ccl_State(string name)
    {
        this.StateName = name;
    }

    public virtual void Begin()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
        
    }
}
