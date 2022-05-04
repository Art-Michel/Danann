using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Idle : Danu_State
{
    public P1Idle () : base(StateNames.P1IDLE) { }
    private Vector3 IdleMovement;
    // Start is called before the first frame update
    public override void Begin()
    {
        idleTime=fsm.agent.GetWaitingTime();
        Vector2 nextRelativePosition=Random.insideUnitCircle*fsm.agent.GetMovementRange();
        Vector3 nextTargetPos=new Vector3(fsm.transform.position.x+nextRelativePosition.x,fsm.transform.position.y,fsm.transform.position.z+nextRelativePosition.y);
        IdleMovement=nextTargetPos;
    }

    // Update is called once per frame
    public override void Update()
    {
        fsm.transform.LookAt(fsm.agent.GetPlayer());
        idleTime=Mathf.Clamp(idleTime-Time.deltaTime,0,10);
        if (!fsm.agent.GetIsPushed())
        fsm.transform.position=Vector3.Lerp(IdleMovement,fsm.transform.position,idleTime);
        if (idleTime==0f)
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