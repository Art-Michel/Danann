using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1CDash : Danu_State
{
    public P1CDash() : base(StateNames.P1C_DASH)
    {

    }
    [SerializeField] Transform target;
    [SerializeField] Transform preview;
    [SerializeField] float maxDashTime;
    [SerializeField] int dashCount;
    [SerializeField] int maxDashCount;
    [SerializeField] float dashSpeed;
    [SerializeField]float maxChargingTime;
    Vector3 dir;
    float dashTime;
    float chargingTime;
    bool isDashing;
    Vector3 maxArrival;
    private Vector3 startPos;
    // Start is called before the first frame update
    public override void Begin()
    {
        if (target==null)
            SetUp();
        StartDash();
    }

    private void SetUp()
    {
        maxChargingTime= fsm.GetP1sD_ChargingTime();
        dashSpeed= fsm.GetP1sD_DashSpeed();
        maxDashTime= fsm.GetP1sD_MDashT();
        maxDashCount= 1;
        preview= fsm.GetP1sD_Preview();
        target= fsm.agent.GetPlayer();
    }

    // Update is called once per frame
    public override void Update()
    {
        Dash();
    }
    private void Dash()
    {
        if (!isDashing)
        {
            return;
        }
        if (dashCount==0)
        {
            return;
        }
        if (chargingTime<=maxChargingTime)
        {
            preview.gameObject.SetActive(true);
            chargingTime+=Time.deltaTime;
            //Vector3 arrival= transform.position+dir*dashSpeed*maxDashTime ;
            //arrival=new Vector3(arrival.x,3.72f,arrival.z);
            preview.position=startPos+(dir*dashSpeed*maxDashTime)/2;
            preview.LookAt(target);
            preview.localScale=new Vector3(1,1,maxDashTime*dashSpeed);
            return;
        }
        dashTime+=Time.deltaTime;
        fsm.transform.position+=dir*dashSpeed*Time.deltaTime;
        if (dashTime>=maxDashTime)
        {
            preview.gameObject.SetActive(false);
            dashCount--;
            dashTime=0;
            dir=(-fsm.transform.position+target.position).normalized;
            fsm.agent.ToIdle();
        }
        
    }
    void StartDash()
    {
        dashCount=maxDashCount;
        dashTime=0;
        chargingTime=0;
        isDashing=true;
        dir=(-fsm.transform.position+target.position).normalized;
        startPos=fsm.transform.position;
        maxArrival=fsm.transform.position+dir*dashSpeed*dashTime;
    }
}
