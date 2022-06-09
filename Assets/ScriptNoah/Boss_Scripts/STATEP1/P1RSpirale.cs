using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1RSpirale : Danu_State
{
    public P1RSpirale() : base(StateNames.P1R_SPIRALE) { }
    // Start is called before the first frame update
    [SerializeField] int nb;
    float arenaRadius;
    Vector3 arenaCenter;
    GameObject proj;
    Pool pool;
    float lifetime;
    GameObject[] spirales;
    int nbBullets;
    float maxDelay;
    bool wait;
    private float waitTime;
    private float maxWaitTime;
    private Transform preview;
    public override void Init()
    {
        base.Init();
        arenaCenter=fsm.agent.GetArenaCenter();
        arenaRadius=fsm.agent.GetArenaRadius()+1.2f;
        pool=fsm.GetRosacePool();
        proj = fsm.GetStraightProj();
        nb=fsm.GetRosaceNumber();
        nbBullets=fsm.GetRosaceBulletNB();
        maxDelay=fsm.GetRosaceDelay();
        maxWaitTime=fsm.GetP1MaxWaitTime();
        spirales=new GameObject[nb];
        preview = fsm.GetP1sD_Preview();

    }
    // Start is called before the first frame update
    public override void Begin()
    {
        if (!isInit)
            Init();
        lifetime=nbBullets*maxDelay+5;
        waitTime=0;
        preview.localScale = new Vector3(1, 1, Vector3.Distance(fsm.transform.position, fsm.agent.GetArenaCenter()));
        preview.position = fsm.transform.position + (fsm.agent.GetArenaCenter() - fsm.transform.position) / 2;
        preview.LookAt(fsm.agent.GetArenaCenter());
        preview.gameObject.SetActive(true);
        wait=true;

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
                Start();
            }
            else
                return;
        }
        lifetime-=Time.deltaTime;
        if  (lifetime<=0)
        {
            Debug.Log("ee");
             if (orig == null)
            {
                Debug.Log("stop");
                fsm.agent.ToIdle();
            }
            else
            {
                orig.AddWaitTime(2);
                orig.FlowControl();
            }
        }
    }
    void Start()
    {

        int delta =360/nb;
        for (int i=0;i<nb;i++)
        {
            Vector3 pos = arenaCenter;
            float rad= delta*Mathf.Deg2Rad;
            Vector3 dest=new Vector3(Mathf.Cos(rad*i),0,Mathf.Sin(rad*i)).normalized;
            pos+=dest*arenaRadius;
            GameObject go= pool.Get();
            go.transform.position=pos;
            Pool pooll= go.GetComponent<Pool>();
            go.SetActive(true);
            pooll.SetUp(null,null,proj);
            go.GetComponentInChildren<MovingSpirale>().SetBullets(nbBullets);
            go.GetComponentInChildren<MovingSpirale>().SetDelay(maxDelay);
            spirales[i]=go;
        }
    }
    public override void End()
    {
        for (int i=0;i<nb;i++)
        {
            pool.Back(spirales[i]);
            fsm.agent.m_anims.SetTrigger("RosaceOver");
            spirales[i].SetActive(false);
            spirales[i]=null;

        }
    }

}
