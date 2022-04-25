using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Idle : State
{
    public P1Idle () : base(StateNames.P1IDLE) { }
    // Start is called before the first frame update
    public override void Begin()
    {
        idleTime=fsm.agent.GetWaitingTime();
    }

    // Update is called once per frame
    public override void Update()
    {
        idleTime-=Time.deltaTime;
        if (idleTime<=0f)
        {
            fsm.agent.NextPattern();
        }
    }
    public void SetIdleTime(float newTime)
    {
        idleTime=newTime;
    }
}
;