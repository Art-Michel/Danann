using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateAttack : State
{
    public AIStateAttack() : base( StateNames.P2C_BOOM )
    {

    }

	public override void Begin()
	{
        // feedback to show that we're attacking
        this.fsm.agent.SetConeColour( Color.red );

        // stop moving
        this.fsm.agent.navAgent.isStopped = true;

        
	}

	public override void Update()
	{
        /*// Check if
        // 1. the target is dead (return to patrol)
        // 2. the target has escaped (investigate)
        Health h = this.fsm.agent.target.GetComponent<Health>();
        if( h.isDead ) {
            // 1
            this.fsm.ChangeState( StateNames.AI_PATROL );
        } else {
            // 2
            CanSeeStatus canSee = this.fsm.agent.CanSee(this.fsm.agent.target);
            if(canSee == CanSeeStatus.CAN_KINDA_SEE || canSee == CanSeeStatus.CANT_SEE) {
                this.fsm.ChangeState(StateNames.AI_INVESTIGATE);
            }
        }*/
	}

	public override void End()
	{
        this.fsm.agent.navAgent.isStopped = false;
       
	}

    
}
