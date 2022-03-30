using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public FSM fsm = null;

    public string name { get; private set; }

    public State( string name )
    {
        this.name = name;
    }

    public virtual void Begin()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void End()
    {

    }

}
