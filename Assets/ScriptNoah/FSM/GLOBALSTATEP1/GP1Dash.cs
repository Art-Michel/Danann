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
        //mdash
        //si parry
        //tp in
        //sinon 
        //boomerang
        //si dist>medium
        //shoot
        //sinon
        //Dash
    }
    // Update is called once per frame
    public override void Update()
    {
        
    }
    public override void FlowControl()
    {
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
