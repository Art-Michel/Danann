using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DM_Rosace : Dm_State
{
    [SerializeField] int nb;
    float arenaRadius;
    Vector3 arenaCenter;
    GameObject proj;
    Pool pool;
    float lifetime;
    GameObject[] spirales;
    int nbBullets;
    float maxDelay;
    private bool wait;
    private float waitTime;
    private float maxWaitTime;
    private Transform preview;
    float projSpeed;

    void Init()
    {
        stateName="Rosace";
        Debug.Log(stateName);
        arenaCenter=fsm.agent.GetArenaCenter();
        arenaRadius=fsm.GetArenaDist();
        pool=fsm.GetRosacePool();
        proj = fsm.GetStraightProj();
        nb=fsm.GetRosaceNumber();
        nbBullets=fsm.GetRosaceBulletNB();
        maxDelay=fsm.GetRosaceDelay();
        spirales=new GameObject[nb];
        maxWaitTime=fsm.GetMaxWaitTime();
        preview = fsm.GetP1sD_Preview();
        projSpeed=fsm.GetProjSpeed();
    }
    // Start is called before the first frame update
    public override void Begin()
    {
            Init();
        preview.localScale = new Vector3(1, 1, Vector3.Distance(fsm.transform.position, fsm.agent.GetArenaCenter()));
        preview.position = fsm.transform.position + (fsm.agent.GetArenaCenter() - fsm.transform.position) / 2;
        preview.LookAt(fsm.agent.GetArenaCenter());
        preview.gameObject.SetActive(true);
        lifetime=nbBullets*maxDelay+2;
        wait=true;
        fsm.agent.vfx[5].SetActive(true);
        waitTime=0;

       
    }

    // Update is called once per frame
    public override void Update() 
    {
        if (wait)
        {
            waitTime += Time.deltaTime;
            fsm.transform.position = Vector3.Lerp(fsm.transform.position, fsm.agent.GetArenaCenter(), waitTime / maxWaitTime);
            if (waitTime >= maxWaitTime)
            {
                wait = false;
                preview.gameObject.SetActive(false);
                StartSpawn();
            }
            else
                return;
        }
        lifetime-=Time.deltaTime;
        if  (lifetime<=0)
        {
        fsm.agent.vfx[5].SetActive(false);
           End();
           fsm.Next();
        }
    }

    private void StartSpawn()
    {
        int delta =360/nb;
        for (int i=0;i<nb;i++)
        {
            Vector3 pos = arenaCenter;
            float rad= delta*Mathf.Deg2Rad;
            Vector3 dest=new Vector3(Mathf.Cos(rad*i),0,Mathf.Sin(rad*i)).normalized;
            pos+=dest*arenaRadius;
            GameObject go= pool.SecondGet();
            go.transform.position=pos;
            go.transform.Rotate(0,rad*Mathf.Rad2Deg*i,0);
            Pool pooll= go.GetComponent<Pool>();
            go.SetActive(true);
            pooll.SetUp(null,null,proj);
            go.GetComponentInChildren<MovingSpirale>().SetBullets(nbBullets);
            go.GetComponentInChildren<MovingSpirale>().SetDelay(maxDelay);
            go.GetComponentInChildren<MovingSpirale>().SetProjSpeed(projSpeed);
            go.GetComponentInChildren<MovingSpirale>().SetOffset(true,rad*Mathf.Rad2Deg*i);
            spirales[i]=go;
        }
    }

    void End()
    {
        /*for (int i=0;i<nb;i++)
        {
            pool.SecondBack(spirales[i]);
            spirales[i].SetActive(false);
            spirales[i]=null;

        }*/
    }
}