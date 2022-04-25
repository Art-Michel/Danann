using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_State
{
    public Spear_FSM fsm = null;

    public string StateName { get; private set; }

    public Spear_State(string name)
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
