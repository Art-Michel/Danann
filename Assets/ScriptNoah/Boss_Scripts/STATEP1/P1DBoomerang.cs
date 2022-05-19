using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1DBoomerang : Danu_State
{
    public P1DBoomerang() : base(StateNames.P1D_BOOMERANG) { }
    GameObject boomerangL;
    GameObject boomerangR;
    AttackData boomerangAttackData;
    Transform target;
    Transform preview;
    float speed;
    float MaxStraightTime;
    Transform curveMidL;
    Transform curveMidR;
    float MaxCurveTime;
    float straightTime;
    Vector3 curveStartL;
    Vector3 curveStartR;    
    Vector3 startL;
    Vector3 startR;
    Vector3 curveEnd;
    float curveTime = 0;
    bool startCurve;
    float waitTime;
    bool wait;
    float maxWaitTime = 1;
    float maxDistance;

    // Start is called before the first frame update
    public override void Begin()
    {
        boomerangAttackData = fsm.GetBoomerangAttackData();
        boomerangAttackData.LaunchAttack();
        target = fsm.agent.GetPlayer();
        fsm.transform.LookAt(target);

        preview = fsm.GetP1sD_Preview();
        boomerangL = fsm.GetP1BRL();
        boomerangR = fsm.GetP1BRR();
        curveMidL = fsm.GetP1BoomeRangcurveMidL();
        curveMidR = fsm.GetP1BoomeRangcurveMidR();

        speed = fsm.GetP1BoomeRangSpeed();
        maxDistance=fsm.GetP1BR_MaxDist();
        MaxCurveTime = fsm.GetP1BoomeRangMaxCurveTime();
        curveTime=0;
        MaxStraightTime = fsm.GetP1BoomeRangMaxStraightTime();
        straightTime=0;
        curveEnd = fsm.transform.position;
        curveStartL = boomerangL.transform.position + fsm.transform.forward * maxDistance ;
        curveStartR = boomerangR.transform.position + fsm.transform.forward * maxDistance;
        Vector3 straightEnd = (curveStartL + curveStartR) / 2;
        
        preview.position = fsm.transform.position + (straightEnd - fsm.transform.position) / 2;
        preview.localScale = new Vector3(5, 1, Vector3.Distance(fsm.transform.position, straightEnd));
        preview.LookAt(straightEnd);
        preview.gameObject.SetActive(true);
        
        maxWaitTime = fsm.GetP1BR_Startup();
        wait = true;
        startL=boomerangL.transform.position;
        startR=boomerangR.transform.position;        
        waitTime=0;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (wait)
        {
            waitTime += Time.deltaTime;
            if (waitTime >= maxWaitTime)
                wait = false;
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
        
        boomerangL.transform.position = Vector3.Lerp(startL, curveStartL, straightTime / MaxStraightTime);
        boomerangR.transform.position = Vector3.Lerp(startR, curveStartR, straightTime / MaxStraightTime);
        straightTime += Time.deltaTime;
        if (boomerangL.transform.position == curveStartL)
        {    
            preview.gameObject.SetActive(false);
            startCurve = true;
            UpdateCurve();
        }
    }
    void UpdateCurve()
    {
        curveTime += Time.deltaTime;
        if (curveTime > MaxCurveTime)
        {
            
            curveTime=0;
            straightTime=0;
            startCurve=false;
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
        boomerangL.transform.position = Curve(boomerangL);
        boomerangR.transform.position = Curve(boomerangR);
    }
    private Vector3 Curve(GameObject boom)
    {
        float t = curveTime / MaxCurveTime;
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 point;
        if (boom == boomerangL)
        {
            point = uu * curveStartL;
            point += 2 * u * t * curveMidL.position;
        }
        else
        {
            point = uu * curveStartR;
            point += 2 * u * t * curveMidR.position;
        }
        point += tt * curveEnd;
        return point;
    }
    public override void End()
    {
        boomerangL.SetActive(false);
        boomerangR.SetActive(false);
        boomerangR.transform.position=startR;
        boomerangL.transform.position=startL;
        base.End();
    }
}