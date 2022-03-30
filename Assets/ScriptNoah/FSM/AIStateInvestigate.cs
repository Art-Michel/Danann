using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateInvestigate : State
{
    public AIStateInvestigate() : base( StateNames.P1C_BOOM )
    {

    }

	public override void Begin()
	{
        // feedback to show that we're alert
        this.fsm.agent.SetConeColour( Color.yellow );

        // store last position
        this.fsm.agent.targetLastPos = this.fsm.agent.target.position;

        // move to last position
        this.fsm.agent.navAgent.destination = this.fsm.agent.targetLastPos;
	}

	public override void Update()
	{
        // update the last position of our target
        this.fsm.agent.targetLastPos = this.fsm.agent.target.position;

        // if it's changed too much, update our destination
        if( Vector3.Distance( this.fsm.agent.targetLastPos, this.fsm.agent.navAgent.destination ) > 1.0f ) {
            this.fsm.agent.navAgent.destination = this.fsm.agent.targetLastPos;
        }
        
        // can we still see the player
        /*CanSeeStatus canSee = this.fsm.agent.CanSee(this.fsm.agent.target);
        if(canSee == CanSeeStatus.CAN_SEE) {
            this.fsm.ChangeState(StateNames.AI_ATTACK);
        } else if( canSee == CanSeeStatus.CANT_SEE ) {
            this.fsm.ChangeState(StateNames.AI_ON_LOSE_SIGHT);
        }*/
	}



    
}
