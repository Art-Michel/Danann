using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccl_State
{
    public Ccl_FSM fsm = null;

    public string Name { get; private set; }

    public Ccl_State(string name)
    {
        this.Name = name;
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
