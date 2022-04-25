using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateOnLoseSight : State
{
    public AIStateOnLoseSight() : base( StateNames.P1C_BOOM )
    {

    }

	public override void Begin()
	{
        //this.fsm.agent.navAgent.destination = this.fsm.agent.targetLastPos;
	}

	public override void Update()
	{
        // check if we've arrived at our destination, then look around
       // Vector3 toDestination = this.fsm.agent.navAgent.destination - this.fsm.agent.transform.position;
        //toDestination.y = 0.0f; 
        //if( toDestination.magnitude < 0.5f ) {
        //    this.fsm.ChangeState( StateNames.P1C_SWEEP );
        //}

        /*// can we see the player
        CanSeeStatus canSee = this.fsm.agent.CanSee(this.fsm.agent.target);
        if(canSee == CanSeeStatus.CAN_KINDA_SEE) {
            this.fsm.ChangeState(StateNames.AI_INVESTIGATE);
        } else if( canSee == CanSeeStatus.CAN_SEE ) {
          *  this.fsm.ChangeState(StateNames.AI_ATTACK);
        }*/
	}
    
}
