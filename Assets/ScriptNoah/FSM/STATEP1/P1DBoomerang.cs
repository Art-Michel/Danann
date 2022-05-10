using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1DBoomerang : Danu_State
{
    public P1DBoomerang() : base(StateNames.P1D_BOOMERANG) { }
    [SerializeField] private GameObject boomerangL;
    [SerializeField] private GameObject boomerangR;
    private Transform target;    
    Transform preview;
    [SerializeField] private float speed;
    [SerializeField] private float MaxStraightTime;
    [SerializeField]Transform curveMidL;
    [SerializeField]Transform curveMidR;
    [SerializeField] private float MaxCurveTime;
    float straightTime;
    Vector3 curveStartL;
    Vector3 curveStartR;
    Vector3 curveEnd;
    float curveTime=0;
    bool startCurve;
    float waitTime;
    private bool wait;
    private float maxWaitTime=1;

    // Start is called before the first frame update
    public override void Begin()
    {
        preview=fsm.GetP1sD_Preview();
        boomerangL=fsm.GetP1BRL();
        if (fsm.GetP1BRL()==null)
            Debug.Log("c'pas normal");
        boomerangL.SetActive(true);
        boomerangR=fsm.GetP1BRR();
        boomerangR.SetActive(true);
        curveMidL=fsm.GetP1BoomeRangcurveMidL();
        curveMidR=fsm.GetP1BoomeRangcurveMidR();
        MaxCurveTime=fsm.GetP1BoomeRangMaxCurveTime();
        MaxStraightTime=fsm.GetP1BoomeRangMaxStraightTime();
        speed=fsm.GetP1BoomeRangSpeed();
        target=fsm.agent.GetPlayer();
        curveEnd=fsm.transform.position;
        fsm.transform.LookAt(target);
        curveStartL=boomerangL.transform.position+fsm.transform.forward*(Vector3.Distance(fsm.transform.position,target.position)/ (speed/1.25f))*speed;
        curveStartR=boomerangR.transform.position+fsm.transform.forward*(Vector3.Distance(fsm.transform.position,target.position)/ (speed/1.25f))*speed;
        Vector3 straightEnd=(curveStartL+curveStartR)/2;
        preview.position=fsm.transform.position+(straightEnd-fsm.transform.position)/2;
        preview.localScale=new Vector3(5,1,Vector3.Distance(fsm.transform.position,straightEnd));
        preview.LookAt(straightEnd);
        preview.gameObject.SetActive(true);
        wait=true;
        maxWaitTime=fsm.GetP1BR_Startup();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (wait)
        {
            waitTime+=Time.deltaTime;
            if (waitTime>=maxWaitTime)
                wait=false;
            return;
        }
        if (startCurve)
        {
            UpdateCurve();
        }
        if (!startCurve)
        {
            UpdateStraight();
        }
    }
    void UpdateStraight()
    {
        straightTime+=Time.deltaTime;
        boomerangL.transform.position=Vector3.Lerp(boomerangL.transform.position,curveStartL,straightTime/MaxStraightTime);
        boomerangR.transform.position=Vector3.Lerp(boomerangR.transform.position,curveStartR,straightTime/MaxStraightTime);
        Debug.Log("e");
        if(boomerangL.transform.position==curveStartL) 
            preview.gameObject.SetActive(false);
        if (straightTime>=MaxStraightTime-0.18f)
        {
            Debug.Log("e");
            startCurve=true;
            UpdateCurve();
            UpdateCurve();
        }
    }
     void UpdateCurve()
    {
            curveTime+=Time.deltaTime;
            Debug.Log(curveTime);
            if (curveTime>MaxCurveTime)
            {
                Debug.Log("eeee");
                fsm.agent.ToIdle();
                
            }
            boomerangL.transform.position=Curve(boomerangL);
            boomerangR.transform.position=Curve(boomerangR);
    }
    private Vector3 Curve(GameObject boom)
    {
        float t =curveTime/MaxCurveTime;
        float u=1-t ;
        float tt=t*t;
        float uu=u*u;
        Vector3 point;
        Debug.Log("eee");
        if (boom==boomerangL)
        {
            point=uu*curveStartL;
            Debug.Log(curveMidL);
            point+=2*u*t*curveMidL.position;
        }
        else
        {
            point=uu*curveStartR;
            point+=2*u*t*curveMidR.position;
        }
        point+=tt*curveEnd;
        return point;
    }
    public override void End()
    {
        boomerangL.SetActive(false);
        boomerangR.SetActive(false);
        base.End();
    }
}