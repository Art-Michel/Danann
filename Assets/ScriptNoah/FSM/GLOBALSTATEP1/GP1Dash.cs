using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP1Dash : GlobalStates
{
    public GP1Dash(): base(StateNames.P1GDASH) { }
    P1CDash dash = new P1CDash();
    P1CMixDash mdash = new P1CMixDash();
    P1DShoot shoot = new P1DShoot();
    P1CSlam slam = new P1CSlam();
    P1CTeleportation tp= new P1CTeleportation();
    bool isMix;
    bool[] test;
    Danu_State last;
    public override void Begin()
    {
        progression=0;
        nextWillEnd=false;
        fsm=gFSM.GetComponent<Danu_FSM>();
        fsm.AddState(dash);
        fsm.AddState(mdash);
        fsm.AddState(shoot);
        fsm.AddState(slam);
        fsm.AddState(tp);
        dash.orig=mdash.orig=shoot.orig=slam.orig=this;
        fsm=new Danu_FSM();
        stats=gFSM.GetPhaseStats();
        FlowControl();
        
                //si revenge
//          oui               non    
        //mdash                 dash
        //si parry              si parry
            //tp in                 si dist>limit
            //si parry                  in
                //tp out                si parry
            //sinon                         out
                //shoot                 sinon 
        //sinon                             shoot
            //slam              si dist<limit
            //si parry              slam
            //in                si parry
            //sinon                 out
            //out               sinon in
                     
    }
    // Update is called once per frame
    public override void Update()
    {
        curr.Update();
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
            Debug.Log("eeee");
            gFSM.agent.ToIdle();
            return;
        }
        if (isMix)
            MixFlow();
        else
            BaseFlow();
    }
    void BaseFlow()
    {
        /**if (!test[0])
        {
            if (test[1])
                tpout;
            shoot;
            return;
        }
        if (test[1])
        slam;
        if (!test[2])
            tp;
        tp;*/
    }
    void MixFlow()
    {
        /*if (dash)
        {
            if (parry)
            slam
            else if (dist>0.8f)
            {
                tp;
            }
            slam
        }
        if (mdash)
        if (slam)
        {
            
        }
        if (tpin)
        if (tpout)
        if (shoot)*/
        switch (progression)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
