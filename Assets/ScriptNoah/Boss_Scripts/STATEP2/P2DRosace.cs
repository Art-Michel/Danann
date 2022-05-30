using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2DRosace : Danu_State
{
    public P2DRosace() : base(StateNames.P2R_SPIRALE){}
    [SerializeField] int nb;
    float arenaRadius;
    Vector3 arenaCenter;
    GameObject proj;
    Pool pool;
    float lifetime;
    GameObject[] spirales;
    int nbBullets;
    float maxDelay;
    public override void Init()
    {
        base.Init();
        arenaCenter=fsm.agent.GetArenaCenter();
        arenaRadius=fsm.agent.GetArenaRadius();
        pool=fsm.GetRosacePool();
        proj = fsm.GetStraightProj();
        nb=fsm.GetP2RosaceNumber();
        nbBullets=fsm.GetP2RosaceBulletNB();
        maxDelay=fsm.GetP2RosaceDelay();
        spirales=new GameObject[nb];
    }
    // Start is called before the first frame update
    public override void Begin()
    {
        if (!isInit)
            Init();
        lifetime=nbBullets*maxDelay+5;

        int delta =360/nb;
        for (int i=0;i<nb;i++)
        {
            Vector3 pos = arenaCenter;
            float rad= delta*Mathf.Deg2Rad;
            Vector3 dest=new Vector3(Mathf.Cos(rad*i),0,Mathf.Sin(rad*i)).normalized;
            pos+=dest*arenaRadius;
            GameObject go= pool.SecondGet();
            go.transform.position=pos;
            Pool pooll= go.GetComponent<Pool>();
            go.SetActive(true);
            pooll.SetUp(null,null,proj);
            go.GetComponentInChildren<MovingSpirale>().SetBullets(nbBullets);
            go.GetComponentInChildren<MovingSpirale>().SetDelay(maxDelay);
            spirales[i]=go;
        }
    }

    // Update is called once per frame
    public override void Update() 
    {
        lifetime-=Time.deltaTime;
        if  (lifetime<=0)
        {
             if (orig == null)
            {
                fsm.agent.ToIdle();
            }
            else
            {
                orig.AddWaitTime(2);
                orig.FlowControl();
            }
        }
    }
    public override void End()
    {
        for (int i=0;i<nb;i++)
        {
            pool.SecondBack(spirales[i]);
            spirales[i].SetActive(false);
            spirales[i]=null;

        }
    }

}
