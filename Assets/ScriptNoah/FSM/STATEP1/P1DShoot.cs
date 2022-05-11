using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1DShoot : Danu_State
{
    public P1DShoot() : base(StateNames.P1D_SHOOT) { }
    float timer;
    float delay=0.2f;
    int index;
    int nbShot=6;
    List<GameObject> proj;
    Pool pool;
    float maxLifeTime;
    float speed;
    // Start is called before the first frame update
    public override void Begin()
    {
                        if (fsm==null)
                    Debug.Log("é");
        pool=fsm.GetPool();
        maxLifeTime=fsm.GetP1d_ProjLifeTime();
        delay=fsm.GetP1d_delay();
        nbShot=fsm.GetP1d_nbShot();
        speed=fsm.GetP1d_ShotSpeed();
        fsm.transform.LookAt(fsm.agent.GetPlayer());
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
                if (orig==null)
                {
                fsm.agent.SetWaitingTime(2);
                fsm.agent.ToIdle();
                }
                else
                {
                    orig.AddWaitTime(2);
                    orig.FlowControl();
                }
            }
            else
            {

                GameObject go =pool.Get();
                go.GetComponent<Projectiles>().SetSpeed(speed);
                go.transform.position=fsm.transform.position;
                go.SetActive(true);
                index++;
            }
        }
    }
    public override void End()
    {
        
    }
}
