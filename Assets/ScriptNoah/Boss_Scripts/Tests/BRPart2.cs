using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRPart2 : MonoBehaviour
{
    [SerializeField]GameObject boomerangL1, boomerangR1,boomerangL2, boomerangR2;
    [SerializeField]AttackData boomerangAttackData;
    [SerializeField]Transform target;
    [SerializeField]Transform preview;
    [SerializeField]float speed;
    [SerializeField]float MaxStraightTime;
    [SerializeField]Transform curveMidL1, curveMidR1,curveMidL2, curveMidR2;
    [SerializeField]float MaxCurveTime;
    [SerializeField]float straightTime;
    [SerializeField]Vector3 curveStartL1,curveStartR1,curveStartL2,curveStartR2;
    [SerializeField]Vector3 baseStartL1,baseStartR1,baseStartL2,baseStartR2;
    [SerializeField]Vector3 startL1,startR1,startL2,startR2;
   [SerializeField] Vector3 curveEnd;
    float curveTime = 0;
    bool startCurve;
    [SerializeField]float waitTime;
    bool wait=false;
    float maxWaitTime = 1;
    [SerializeField]float maxDistance;

    // Start is called before the first frame update
    private void Awake() {

        Init();
        curveTime=0;
        straightTime=0;
        transform.LookAt(target);
        boomerangR1.transform.localPosition=baseStartR1;
        boomerangL1.transform.localPosition=baseStartL1;        
        boomerangR2.transform.localPosition=baseStartR2;
        boomerangL2.transform.localPosition=baseStartL2;
        startL1=boomerangL1.transform.position;
        startR1=boomerangR1.transform.position;
        startL2=boomerangL2.transform.position;
        startR2=boomerangR2.transform.position;
        curveStartL1 = boomerangL1.transform.position + transform.forward * maxDistance ;
        curveStartR1 = boomerangR1.transform.position + transform.forward * maxDistance;
        curveStartL2 = boomerangL2.transform.position + transform.forward * maxDistance ;
        curveStartR2 = boomerangR2.transform.position + transform.forward * maxDistance;
        curveEnd = transform.position;
        Vector3 straightEnd = (curveStartL1 + curveStartR1+curveStartL2 + curveStartR2) / 4;
        preview.position = transform.position + (straightEnd - transform.position) / 2;
        preview.localScale = new Vector3(5, 1, Vector3.Distance(transform.position, straightEnd));
        preview.LookAt(straightEnd);
        preview.gameObject.SetActive(true);
        
        wait = true;
     
        waitTime=0;
    }
    void Init()
    {
        baseStartL1=boomerangL1.transform.localPosition;
        baseStartR1=boomerangR1.transform.localPosition;   
        baseStartL2=boomerangL2.transform.localPosition;
        baseStartR2=boomerangR2.transform.localPosition;   
    }
    // Update is called once per frame
    void Update()
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
        
        boomerangL1.transform.position = Vector3.Lerp(startL1, curveStartL1, straightTime / MaxStraightTime);
        boomerangR1.transform.position = Vector3.Lerp(startR1, curveStartR1, straightTime / MaxStraightTime);
        boomerangL2.transform.position = Vector3.Lerp(startL2, curveStartL2, straightTime / MaxStraightTime);
        boomerangR2.transform.position = Vector3.Lerp(startR2, curveStartR2, straightTime / MaxStraightTime);
        straightTime += Time.deltaTime;
        bool cond = boomerangL1.transform.position == curveStartL1;
        cond = cond||boomerangL2.transform.position == curveStartL2;
        cond = cond||boomerangR2.transform.position==curveStartR2;
        cond = cond||boomerangR1.transform.position==curveStartR1;
        if (cond)
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
            Debug.Log("over");
            enabled=false;

        }
        boomerangL1.transform.position = Curve(boomerangL1);
        boomerangR1.transform.position = Curve(boomerangR1);
        boomerangL2.transform.position = Curve(boomerangL2);
        boomerangR2.transform.position = Curve(boomerangR2);
    }
    private Vector3 Curve(GameObject boom)
    {
        float t = curveTime / MaxCurveTime;
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 point;
        if (boom == boomerangL1)
        {
            point = uu * curveStartL1;
            point += 2 * u * t * curveMidL1.position;
        }
        else if (boom == boomerangR1)
        {
            point = uu * curveStartR1;
            point += 2 * u * t * curveMidR1.position;
        }
        else if (boom == boomerangL2)
        {
            point = uu * curveStartL2;
            point += 2 * u * t * curveMidL2.position;
        }
        else
        {
            point = uu * curveStartR2;
            point += 2 * u * t * curveMidR2.position;
        }
        point += tt * curveEnd;
        return point;
    }
    void End()
    {
        /*boomerangL.SetActive(false);
        boomerangR.SetActive(false);
        boomerangR.transform.localPosition=baseStartR;
        boomerangL.transform.localPosition=baseStartL;
        base.End();*/
    }
}
