using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStates {

    public Danu_GlobalFSM gFSM = null;
    public Danu_FSM fsm=null;
    public int progression;
    public int oldProg;
    public bool nextWillEnd;
    public float idleTime;
    public float combinedWaitTime;
    public PhaseStats stats;
    public Danu_State curr;
    public string name { get; private set; }

    public GlobalStates( string name )
    {
        this.name = name;
        
    }

    public virtual void Begin()
    {

    }

    public virtual void Update()
    {
        
    }
    public virtual void FlowControl()
    {

    }
    public void AddWaitTime(float plusTime)
    {
        combinedWaitTime+=plusTime;
    }
    public virtual void End()
    {

    }

}