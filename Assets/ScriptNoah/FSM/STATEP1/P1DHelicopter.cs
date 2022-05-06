using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1DHelicopter : Danu_State
{
    public P1DHelicopter() : base(StateNames.P1D_SPIN) { }
    private GameObject globalGO; 
    private Transform dSphereN;
    private Transform  dSphereSW;
    private Transform  dSphereSE;
    private float dist; 
    private float rotationSpeed;
    private bool turningRight;
    private float maxWaitTime;
    private float lifetime;
    Vector3 n;
    Vector3 w;
    Vector3 e;
    List<GameObject> nblades=new List<GameObject>();
    List<GameObject> wblades=new List<GameObject>();
    List<GameObject> eblades=new List<GameObject>();
    private bool wait;
    private float waitTime;
    bool isSetUp;
    // Start is called before the first frame update
    public override void Begin()
    {
        //setup des variables
        dist=fsm.GetP1Sp_Dist();
        globalGO=fsm.GetP1GlobalGO();
        Transform[] nweTrans=fsm.GetP1NWEMax();
        dSphereN=nweTrans[0];
        dSphereSE=nweTrans[1];
        dSphereSW=nweTrans[2];
        rotationSpeed=fsm.GetP1RotationSpeed();
        turningRight=fsm.GetP1TurningRight();
        maxWaitTime=fsm.GetP1MaxWaitTime();
        lifetime=fsm.GetP1SpinLifeTime();
  
        //repositionnement du boss et des helices
        //fsm.transform.position=fsm.agent.GetArenaCenter();
        /*n=dSphereN.position;
        w=dSphereSW.position;
        e=dSphereSE.position;*/
        
        //activation des helices, ou instantiation si c'est la premiere fois
        globalGO.SetActive(true);
        float delta = 1/dist;
        if (isSetUp)
        {
            wait=true;
            waitTime=0;
            for (int i=0;i<nblades.Count;i++)
            {
                nblades[i].SetActive(true);
                wblades[i].SetActive(true);
                eblades[i].SetActive(true);
            }
        }
        else
        {
            /*Vector3 newW=(dSphereSW.position-fsm.transform.position);
            Vector3 newE=(dSphereSE.position-fsm.transform.position);
            dSphereN.position=newn*fsm.agent.GetArenaRadius();
            dSphereSW.position=newW*fsm.agent.GetArenaRadius();
            dSphereSE.position=newE*fsm.agent.GetArenaRadius();*/
            n=dSphereN.position;
            w=dSphereSW.position;
            e=dSphereSE.position;
            for (int i =0;i<dist;i++)
            {
                Vector3 nPos;
                Vector3 wPos;
                Vector3 ePos;
                nPos=Vector3.Lerp(fsm.transform.position,dSphereN.position,delta*i);
                wPos=Vector3.Lerp(fsm.transform.position,dSphereSW.position,delta*i);
                ePos=Vector3.Lerp(fsm.transform.position,dSphereSE.position,delta*i);
                nblades.Add(fsm.InstantiateStaticProjectile(nPos));
                wblades.Add(fsm.InstantiateStaticProjectile(wPos));
                eblades.Add(fsm.InstantiateStaticProjectile(ePos));
                Debug.Log("e");
            }
            isSetUp=true;
        }
        wait=true;
        
    }

    // Update is called once per frame
    public override void Update()
    {
        if (wait)
        {
            waitTime+=Time.deltaTime;
            fsm.transform.position=Vector3.Lerp(fsm.transform.position,fsm.agent.GetArenaCenter(),waitTime/maxWaitTime);
            if (waitTime>=maxWaitTime)
            {
                wait=false;
            }
            else
                return;
        }
        lifetime-=Time.deltaTime;
        if(lifetime<=0)
            fsm.agent.ToIdle();
        Rotate();
    }
    private void Rotate()
    {
        if (turningRight)
            globalGO.transform.Rotate(new Vector3(0,rotationSpeed*Time.deltaTime,0));
        else
            globalGO.transform.Rotate(new Vector3(0,-rotationSpeed*Time.deltaTime,0));    
    }
    public override void End()
    {
        globalGO.SetActive(false);
        lifetime=fsm.GetP1SpinLifeTime();
        /*dSphereN.localPosition=n;
        dSphereSW.localPosition=w;
        dSphereSE.localPosition=e;*/
        globalGO.transform.rotation=Quaternion.identity;
        float delta=1/dist;
        for (int i=0;i<nblades.Count;i++)
        {
            nblades[i].SetActive(false);
            //nblades[i].transform.position=Vector3.Lerp(fsm.transform.position,n,delta*i);
            wblades[i].SetActive(false);
            //wblades[i].transform.position=Vector3.Lerp(fsm.transform.position,w,delta*i);
            eblades[i].SetActive(false);
            //eblades[i].transform.position=Vector3.Lerp(fsm.transform.position,e,delta*i);
        }
    }
}