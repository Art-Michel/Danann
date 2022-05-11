using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP1Shoot : GlobalStates
{
    public GP1Shoot() : base(StateNames.P1D_SPIN) { }
    Danu_State curr;
    P1CDash dash = new P1CDash();
    P1CMixDash mdash = new P1CMixDash();
    P1DShoot shoot = new P1DShoot();
    P1DBoomerang br = new P1DBoomerang();
    P1CTeleportation tp= new P1CTeleportation();
    // Start is called before the first frame update

    void Start()
    {
        dash.orig=mdash.orig=shoot.orig=br.orig=this;
        FlowControl();
        //playshoot
        //si parry
        //tp in
        //sinon 
        //boomerang
        //si dist>medium
        //shoot
        //sinon
        //Dash
    }
    void FlowControl()
    {
        if (nextWillEnd)
        {
            progression=0;
            oldProg=0;
            nextWillEnd=false;
            //fsm.returntoidle()
        }
        switch(progression)
        {
            case 0:
                curr.End();
                curr=shoot;
                curr.Begin();
                break;
            case 1:
                if (fsm.agent.wasParried)
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
                float dist=Vector3.Distance(fsm.agent.transform.position,fsm.agent.GetPlayer().position);
                if (dist>=fsm.agent.distLimit)
                {
                    curr.End();
                    curr=shoot;
                    curr.Begin();
                    nextWillEnd=true;
                }
                else
                {
                    if (fsm.agent.isRevengeHigh)
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
    }
    // Update is called once per frame
    public void GlobalUpdate()
    {
        curr.Update();
        if (oldProg<progression)
            FlowControl();
        oldProg=progression;
    }
}
