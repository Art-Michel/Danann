using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP1Shoot : GlobalStates
{
    public GP1Shoot() : base(StateNames.P1GSHOOT) { }
    
    P1CDash dash = new P1CDash();
    P1CMixDash mdash = new P1CMixDash();
    P1DShoot shoot = new P1DShoot();
    P1DBoomerang br = new P1DBoomerang();
    P1CTeleportation tp= new P1CTeleportation();    

    public PhaseStats GetPhaseStats(){return stats;}
    List<Danu_State> old;
    // Start is called before the first frame update

    public override void Begin()
    {
        progression=0;
        nextWillEnd=false;
        fsm=gFSM.GetComponent<Danu_FSM>();
        fsm.AddState(dash);
        fsm.AddState(mdash);
        fsm.AddState(shoot);
        fsm.AddState(br);
        fsm.AddState(tp);
        dash.orig=mdash.orig=shoot.orig=br.orig=this;
        stats=gFSM.GetPhaseStats();
        FlowControl();
        
        //playshoot
        //si shield
        //tp in
        //sinon 
        //boomerang
        //si dist>medium
        //shoot
        //sinon
        //Dash
    }
    public override void FlowControl()
    {
        if (nextWillEnd)
        {
            progression=0;
            oldProg=0;
            nextWillEnd=false;
            gFSM.agent.SetWaitingTime(combinedWaitTime);
            combinedWaitTime=0;
            gFSM.agent.ToIdle();
        }
        switch(progression)
        {
            case 0:
                //curr.End();
                curr=shoot;
                curr.Begin();
                break;
            case 1:
                if (gFSM.agent.wasParried)
                {   
                    curr.End();
                    curr=tp;
                    curr.Begin();
                    nextWillEnd=true;
                }
                else
                    curr.End();
                    curr=br;
                    curr.Begin();
                break;
            case 2:
                float dist=Vector3.Distance(gFSM.agent.transform.position,gFSM.agent.GetPlayer().position);
                if (dist>=gFSM.agent.GetDistLimit())
                {
                    curr.End();
                    curr=shoot;
                    curr.Begin();
                    nextWillEnd=true;
                }
                else
                {
                    if (gFSM.agent.isRevengeHigh)
                    {
                        curr.End();
                        curr=mdash;
                        curr.Begin();
                    }
                    else
                    {
                        curr.End();
                        curr=dash;
                        curr.Begin();
                    }
                    nextWillEnd=true;
                }
            break;
        }
        progression++;
    }

    // Update is called once per frame
    public override void Update()
    {
        curr.Update();
    }
}
