using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1CMixDash : Danu_State
{
    public P1CMixDash() : base(StateNames.P1C_MIXDASH)
    {

    }
    enum state
    {
        CHARGING,
        DASH,
        STRAFE,
        RETURNDASH
    }
    state actual;
    [SerializeField] Transform target;
    [SerializeField] Transform preview;
    [SerializeField] float maxDashTime;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashMod;
    [SerializeField]float maxChargingTime;
    float altDSpeed;
    int dashCount;
    int maxDashCount=2;
    Vector3 dir;
    float dashTime;
    float chargingTime;
    bool isDashing;
    Vector3 maxArrival;
    bool getFar;
    bool isSetup;
    private Vector3 startPos;
    Vector3 strafeDest;
    // Start is called before the first frame update
    public override void Begin()
    {
        if (!isSetup)
            SetUp();
        StartDash();
    }

    private void SetUp()
    {
        isSetup=true;
        maxChargingTime= fsm.GetP1sD_ChargingTime();
        dashSpeed= fsm.GetP1sD_DashSpeed();
        maxDashTime= fsm.GetP1sD_MDashT();
        preview= fsm.GetP1sD_Preview();
        target= fsm.agent.GetPlayer();
        dashMod=fsm.GetPMD_dMod();
    }
    void StartDash()
    {        
        altDSpeed=dashSpeed;
        dashCount=maxDashCount;
        dashTime=0;
        chargingTime=0;
        isDashing=true;
        dir=(-fsm.transform.position+target.position).normalized;
        startPos=fsm.transform.position;
        maxArrival=fsm.transform.position+dir*dashSpeed*dashTime;
        actual=state.CHARGING;
    }
    // Update is called once per frame
    public override void Update()
    {
        if (!CanDash())
            return;
        switch (actual)
        {
            case state.CHARGING:
                ChargeDash();
                if (actual!=state.CHARGING)
                    preview.gameObject.SetActive(false);
                break;
            case state.DASH:
                Dash();
                if (actual!=state.DASH)
                    StartStrafe();
                break;
            case state.STRAFE:
                Strafe();
                if (actual!=state.STRAFE)
                    preview.gameObject.SetActive(false);
                break;
            case state.RETURNDASH:
                ReturnDash();
                break;
                
        }

    }

    private void StartStrafe()
    {
        Vector3 dir = target.position-fsm.transform.position ;
        dir.Normalize();
        Vector3 left = Vector3.Cross(dir, Vector3.up).normalized;
        Vector3 offset =left*Random.Range(-15,15);
        offset= new Vector3(offset.x,0,offset.z);
        strafeDest=fsm.transform.position+offset;
    }

    private bool CanDash()
    {
        if (!isDashing || dashCount==0)
            return false;
        return true;
    }
    private void ChargeDash()
    {
        if (chargingTime<=maxChargingTime)
        {
            preview.gameObject.SetActive(true);
            chargingTime+=Time.deltaTime;
            //Vector3 arrival= transform.position+dir*dashSpeed*maxDashTime ;
            //arrival=new Vector3(arrival.x,3.72f,arrival.z);
            dir=(-fsm.transform.position+target.position).normalized;
            startPos=fsm.transform.position;
            maxArrival=fsm.transform.position+dir*dashSpeed*dashTime;
            preview.position=Vector3.Lerp(preview.position, startPos+(dir*dashSpeed*maxDashTime)/2,0.8f);
            preview.LookAt(target);
            preview.localScale=new Vector3(1,1,maxDashTime*dashSpeed);
            return;
        }
        actual++;
    }
    private void Dash()
    {
        
        if (dashTime<=maxDashTime)
        {
            dashTime+=Time.deltaTime;
            fsm.transform.position+=dir*dashSpeed*Time.deltaTime;
            return;
        }
        dashCount--;
        dashTime=0;
        dir=(-fsm.transform.position+target.position).normalized;
        chargingTime=0;
        if (getFar)
            altDSpeed*=dashMod;
        else
            altDSpeed/=dashMod;
        actual++;
    }
    private void Strafe()
    {
        if (chargingTime<=maxChargingTime)
        {
            preview.gameObject.SetActive(true);
            chargingTime+=Time.deltaTime;
            dir=(-fsm.transform.position+target.position).normalized;
            fsm.transform.position=Vector3.Lerp(fsm.transform.position,strafeDest,chargingTime/maxChargingTime);
            startPos=fsm.transform.position;
            maxArrival=fsm.transform.position+dir*dashSpeed*dashTime;
            preview.position=Vector3.Lerp(preview.position, startPos+(dir*dashSpeed*maxDashTime)/2,1);
            preview.LookAt(target);
            preview.localScale=new Vector3(1,1,maxDashTime*dashSpeed);
            return;
        }
        actual++;
    }
    private void ReturnDash()
    {
        if (dashTime<=maxDashTime)
        {
            dashTime+=Time.deltaTime;
            fsm.transform.position+=dir*dashSpeed*Time.deltaTime;
            return;
        }
        dashCount--;
        dashTime=0;
        dir=(-fsm.transform.position+target.position).normalized;
        if (dashCount==0)
        {
            isDashing=false;
            Debug.Log("End");
            actual=state.CHARGING;
            fsm.ChangeState(StateNames.P1IDLE);
        }
        actual=state.CHARGING;

    }

}
