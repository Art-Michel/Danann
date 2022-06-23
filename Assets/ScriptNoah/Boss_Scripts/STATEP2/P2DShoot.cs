using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2DShoot : Danu_State
{
    public P2DShoot() : base(StateNames.P2D_SHOOT){}
    float timer;
    Transform preview;
    float delay = 0.2f;
    int index;
    int nbShot = 6;
    List<GameObject> proj;
    Pool pool;
    float maxLifeTime;
    float speed;
    private bool wait;
    private float waitTime;
    private float maxWaitTime=0.2f;
    Transform target;
    private int angle;

    // Start is called before the first frame update
    public override void Begin()
    {
        if (!isInit)
            Init();
        index = 0;
        timer = 0;
        waitTime=0;
        wait=true;
        Vector3 dir = (-fsm.transform.position + target.position).normalized;
        preview.gameObject.SetActive(true);

    }
    public override void Init()
    {
        pool = fsm.GetStraightPool();
        angle=fsm.GetProjAngle();
        maxLifeTime = fsm.GetP2d_ProjLifeTime();
        delay = fsm.GetP2d_delay();
        nbShot = fsm.GetP2d_nbShot();
        speed = fsm.GetP2d_ShotSpeed();
        fsm.transform.LookAt(fsm.agent.GetPlayer());
        preview=fsm.GetP2sD_Preview();
        target=fsm.agent.GetPlayer();
        base.Init();
    }
    // Update is called once per frame
    public override void Update()
    {
        if (wait)
        {
            waitTime+=Time.deltaTime;
            fsm.transform.LookAt(fsm.agent.GetPlayer());
            float rotateValue=fsm.transform.rotation.eulerAngles.y;
            fsm.transform.rotation=Quaternion.Euler(0,rotateValue,0);

            if (waitTime>=maxWaitTime)
            {
                preview.gameObject.SetActive(false);
                wait=false;
            }
            return;
        }
        timer += Time.deltaTime;
        if (timer > delay)
        {
            timer = 0;
            if (index > nbShot)
            {
                if (!fsm.agent.followsGlobal)
                {
                    fsm.agent.SetWaitingTime(2);
                    fsm.agent.ToIdle();
                }
                else
                {
                    orig.AddWaitTime(2);
                    orig.FlowControl();
                }
                return;
            }
            else
            {
                index++;
                if (index%2==0)
                {
                    for (int i=0;i<3;i++)
                    {
                        GameObject go = pool.Get();
                        go.GetComponent<Projectiles>().SetSpeed(speed);
                        go.transform.position = fsm.transform.position;
                        go.transform.LookAt(target);
                        go.SetActive(true);
                        go.GetComponent<AttackData>().LaunchAttack();
                        if (i==1)
                            go.transform.Rotate(0,angle,0);
                        else if (i==2)
                            go.transform.Rotate(0,-angle,0);
                        go.transform.position-=go.transform.forward*2;
                    }
                }
                else
                {
                    for (int i=0;i<2;i++)
                    {
                        GameObject go = pool.Get();
                        go.GetComponent<Projectiles>().SetSpeed(speed);
                        go.transform.position = fsm.transform.position;
                        go.SetActive(true);
                        go.transform.LookAt(target);
                        go.GetComponent<AttackData>().LaunchAttack();
                        if (i==0)
                            go.transform.Rotate(0,angle/2,0);
                        else
                            go.transform.Rotate(0,-angle/2,0);
                        go.transform.position-=go.transform.forward*2;
                    }
                }

            }
        }
    }
    public override void End()
    {

    }
}
