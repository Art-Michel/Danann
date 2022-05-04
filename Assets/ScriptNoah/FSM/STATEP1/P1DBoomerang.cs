using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1DBoomerang : Danu_State
{
    public P1DBoomerang() : base(StateNames.P1D_BOOMERANG) { }
    [SerializeField] private GameObject boomerangL;
    [SerializeField] private GameObject boomerangR;
    private Transform target;    
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
    // Start is called before the first frame update
    public override void Begin()
    {
        boomerangL.SetActive(true);
        boomerangR.SetActive(true);
        target=fsm.agent.GetPlayer();
        curveEnd=fsm.transform.position;
        fsm.transform.LookAt(target);
        curveStartL=boomerangL.transform.position+fsm.transform.forward*10;
        curveStartR=boomerangR.transform.position+fsm.transform.forward*10;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (startCurve)
        {
            UpdateCurve();
        }
        else
        {
            UpdateStraight();
        }
    }
    void UpdateStraight()
    {
        straightTime+=Time.deltaTime*speed;
        boomerangL.transform.position=Vector3.Lerp(boomerangL.transform.position,curveStartL,straightTime);
        boomerangR.transform.position=Vector3.Lerp(boomerangR.transform.position,curveStartR,straightTime);
        if (straightTime>=MaxStraightTime-0.18f)
        {
            startCurve=true;
            UpdateCurve();
            UpdateCurve();
        }
    }
     void UpdateCurve()
    {
            curveTime+=Time.deltaTime*speed;
            if (curveTime>1)
                return;
            boomerangL.transform.position=Curve(boomerangL);
            boomerangR.transform.position=Curve(boomerangR);
    }
    private Vector3 Curve(GameObject boom)
    {
        float u=1-curveTime ;
        float tt=curveTime*curveTime;
        float uu=u*u;
        Vector3 point;
        if (boom==boomerangL)
        {
            point=uu*curveStartL;
            point+=2*u*curveTime*curveMidL.position;
        }
        else
        {
            point=uu*curveStartR;
            point+=2*u*curveTime*curveMidR.position;
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