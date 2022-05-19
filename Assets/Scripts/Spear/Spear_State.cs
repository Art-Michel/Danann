using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_State
{
    public Spear_FSM _fsm = null;
    public SpearAI _ai = null;

    public string Name { get; private set; }

    public Spear_State(string name)
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
