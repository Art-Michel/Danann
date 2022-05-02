using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private GameObject boomerangL;
    [SerializeField] private GameObject boomerangR;
    [SerializeField] private GameObject target;    
    [SerializeField] private float speed;
    [SerializeField] private float MaxStraightTime;
    float straightTime;
    Vector3 curveStartL;
    Vector3 curveStartR;
    [SerializeField]Transform curveMidL;
    [SerializeField]Transform curveMidR;
    Vector3 curveEnd;
    [SerializeField] private float MaxCurveTime;
    float curveTime=0;
    bool startCurve;
    // Start is called before the first frame update
    void Start()
    {   
        curveEnd=transform.position;
        transform.LookAt(target.transform);
        curveStartL=boomerangL.transform.position+transform.forward*10;
        curveStartR=boomerangR.transform.position+transform.forward*10;
    }

    // Update is called once per frame
    void Update()
    {
        if (startCurve)
        {
            UpdateCurve();
        }
        else
        {
            UpdateStraight();
        }
        /*Vector3 bL = boomerangL.transform.position;
        Vector3 bR = boomerangR.transform.position;
        boomerangL.transform.position= new Vector3(bL.x,transform.position.y,bL.z);
        boomerangR.transform.position= new Vector3(bR.x,transform.position.y,bR.z);
        *///boomerang.transform.Rotate(Vector3.up,rotationSpeed*Time.deltaTime);
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

}
