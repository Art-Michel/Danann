using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1DShoot : State
{
    public P1DShoot() : base(StateNames.P1D_SHOOT) { }
    float timer;
    float delay=0.2f;
    int index;
    int nbShot=6;
    // Start is called before the first frame update
    public override void Begin()
    {
        index=0;
        timer=0;
    }

    // Update is called once per frame
    public override void Update()
    {
        timer+=Time.deltaTime;
        if (timer>delay){
            timer=0;
            if (index>nbShot)
            {
                fsm.agent.SetWaitingTime(2);
                fsm.agent.ToIdle();
            }
            else
            {
            fsm.agent.SpawnProjectile();
            index++;

            }
        }
    }
    public override void End()
    {
        
    }
}
