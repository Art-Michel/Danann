using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DM_Shoot : Dm_State
{
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
        Init();

        index = 0;
        timer = 0;
        waitTime=0;
        wait=true;
        Vector3 dir = (-fsm.transform.position + target.position).normalized;
        preview.position = fsm.transform.position + (dir * 10)/2;
        Vector3 straightTarget =new Vector3( target.position.x,fsm.transform.position.y,target.position.z);
        preview.LookAt(straightTarget);
        preview.localScale = new Vector3(fsm.transform.localScale.x, fsm.transform.localScale.y, 10);
        preview.gameObject.SetActive(true);

    }
    void Init()
    {        
        stateName="Shoot";
        Debug.Log(stateName);
        pool = fsm.GetPool();
        angle=fsm.GetShootAngle();
        maxLifeTime = fsm.GetP1d_ProjLifeTime();
        delay = fsm.GetP1d_delay();
        nbShot = fsm.GetP1d_nbShot();
        speed = fsm.GetP1d_ShotSpeed();
        target=fsm.agent.GetPlayer();
        Vector3 straightTarget =new Vector3( target.position.x,fsm.transform.position.y,target.position.z);
        fsm.transform.LookAt(straightTarget);
        preview=fsm.GetP1sD_Preview();
    }
    // Update is called once per frame
    public override void Update()
    {
        if (wait)
        {
            waitTime+=Time.deltaTime;

            Vector3 straightTarget =new Vector3( target.position.x,fsm.transform.position.y,target.position.z);
            fsm.transform.LookAt(straightTarget);
            float rotateValue=fsm.transform.rotation.eulerAngles.y;
            fsm.transform.rotation=Quaternion.Euler(0,rotateValue,0);
            Vector3 dir = (-fsm.transform.position + target.position).normalized;
            preview.position = fsm.transform.position + (dir * 10)/2;
            preview.LookAt(straightTarget);
            preview.localScale = new Vector3(fsm.transform.localScale.x, fsm.transform.localScale.y, 10);
            if (waitTime>=maxWaitTime)
            {
                preview.gameObject.SetActive(false);
                wait=false;
            }
            return;
        }
        timer += Time.deltaTime;
        Vector3 straightTargets =new Vector3( target.position.x,fsm.transform.position.y,target.position.z);
            fsm.transform.LookAt(straightTargets);
            float rotateValues=fsm.transform.rotation.eulerAngles.y;
            fsm.transform.rotation=Quaternion.Euler(0,rotateValues,0);
        if (timer > delay)
        {
            timer = 0;
            if (index > nbShot)
            {
                fsm.Next();
                return;
            }
            else
            {
                for (int i=0;i<3;i++)
                {
                index++;
                SoundManager.Instance.PlayBossShoot();
                GameObject go = pool.Get();
                go.GetComponent<Projectiles>().SetSpeed(speed);
                go.transform.position = fsm.transform.position;
                go.transform.LookAt(target);
                if (i==1)
                    go.transform.Rotate(0,angle,0);
                else if (i==2)
                    go.transform.Rotate(0,-angle,0);
                go.SetActive(true);

                go.GetComponent<AttackData>().LaunchAttack();
                }

            }
        }
    }
    public void End()
    {

    }
}