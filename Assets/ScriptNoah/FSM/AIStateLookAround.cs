using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateLookAround : State
{
    private Quaternion m_lookRotation = Quaternion.identity;
    public AIStateLookAround() : base( StateNames.P2C_SWEEP )
    {

    }

	public override void Begin(){
        // disable the nav mesh agent so we can manually change the rotation
        this.fsm.agent.navAgent.enabled = false;

        // start looking around
        this.fsm.StartCoroutine( this._changeLookDirection() );
	}
	public override void End(){
        this.fsm.StopAllCoroutines();

        // re-enable the nav mesh agent
        this.fsm.agent.navAgent.enabled = true;
	}
	public override void Update(){
        // lerp to our next rotation
        this.fsm.agent.transform.rotation = Quaternion.Lerp( this.fsm.transform.rotation, this.m_lookRotation, 0.1f );

        // can we see the player
       /* CanSeeStatus canSee = this.fsm.agent.CanSee(this.fsm.agent.target);
        if(canSee == CanSeeStatus.CAN_KINDA_SEE) {
            this.fsm.ChangeState(StateNames.AI_INVESTIGATE);
        } else if( canSee == CanSeeStatus.CAN_SEE ) {
            this.fsm.ChangeState(StateNames.AI_ATTACK);
        }*/
	}
    private IEnumerator _changeLookDirection()
    {
        for( int i = 0; i < 3; i++ ) {
            float angle = Random.Range( 0, 360.0f );
            this.m_lookRotation = Quaternion.AngleAxis( angle, Vector3.up );
            yield return new WaitForSeconds(1.0f);
        }
        // didn't see him; go back to patrol
        this.fsm.ChangeState( StateNames.P2D_WAVES );
    }

    
}
