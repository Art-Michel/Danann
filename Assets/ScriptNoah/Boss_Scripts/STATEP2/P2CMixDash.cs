using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2DMixDash : Danu_State
{
    public P2DMixDash() : base(StateNames.P2C_MIXDASH){}
    enum state
    {
        CHARGING,
        DASH,
        STRAFE,
        RETURNDASH,
        NONE
    }
    state actual;
    [SerializeField] Transform target;
    [SerializeField] GameObject preview;
    [SerializeField] float maxDashTime;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashMod;
    [SerializeField] float maxChargingTime;
    AttackData dashAttackData;
    float altDSpeed;
    int dashCount;
    int maxDashCount = 4;
    Vector3 dir;
    float dashTime;
    float chargingTime;
    bool isDashing;
    Vector3 maxArrival;
    bool getFar;
    private Vector3 startPos;
    Vector3 strafeDest;
    float afterStrafe;
    float mafs;
    // Start is called before the first frame update
    public override void Begin()
    {
        if (!isInit)
            Init();
        StartDash();
    }
    public override void Init()
    {
        base.Init();
        maxChargingTime = fsm.GetP2sD_ChargingTime();
        dashSpeed = fsm.GetP2sD_DashSpeed();
        maxDashTime = fsm.GetP2sD_MDashT();
        preview = fsm.GetP1sD_Preview();
        target = fsm.agent.GetPlayer();
        dashMod = fsm.GetP2MD_dMod();
        dashAttackData = fsm.GetP2DashAttackData();
        mafs = 0.8f;
    }
    void StartDash()
    {
        altDSpeed = dashSpeed;
        dashCount = maxDashCount;
        dashTime = 0;
        chargingTime = 0;
        isDashing = true;
        dashAttackData.LaunchAttack();
        SetTarget();
        actual = state.CHARGING;
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
                if (actual != state.CHARGING)
                {
                    preview.gameObject.SetActive(false);
                    fsm.agent.vfx[0].SetActive(true);
                }
                break;
            case state.DASH:
                Dash();
                if (actual != state.DASH)
                {
                    dashAttackData.StopAttack();
                    StartStrafe();
                    fsm.agent.vfx[0].SetActive(false);
                }
                break;
            case state.STRAFE:
                Strafe();
                if (actual != state.STRAFE)
                {
                    preview.gameObject.SetActive(false);
                    dashAttackData.LaunchAttack();
                    fsm.agent.vfx[0].SetActive(true);
                }
                break;
            case state.RETURNDASH:
                ReturnDash();
                break;

        }

    }

    private void StartStrafe()
    {
        Vector3 dir = target.position - fsm.transform.position;
        dir.Normalize();
        Vector3 left = Vector3.Cross(dir, Vector3.up).normalized;
        Vector3 offset = left * Random.Range(-15, 15);
        offset = new Vector3(offset.x, 0, offset.z);
        strafeDest = fsm.transform.position + offset;
    }

    private bool CanDash()
    {
        if (!isDashing || dashCount == 0)
            return false;
        return true;
    }
    private void ChargeDash()
    {
        if (chargingTime <= maxChargingTime)
        {
            preview.gameObject.SetActive(true);
            chargingTime += Time.deltaTime;
            //Vector3 arrival= transform.position+dir*dashSpeed*maxDashTime ;
            //arrival=new Vector3(arrival.x,3.72f,arrival.z);
            /*dir=(-fsm.transform.position+target.position).normalized;
            startPos=fsm.transform.position;
            maxArrival=fsm.transform.position+dir*dashSpeed*dashTime;
            preview.position=Vector3.Lerp(preview.position, startPos+(dir*dashSpeed*maxDashTime)/2,0.8f);
            preview.LookAt(target);
            preview.localScale=new Vector3(fsm.transform.localScale.x,fsm.transform.localScale.y,maxDashTime*dashSpeed);*/
            return;
        }
        actual++;
    }
    private void Dash()
    {

        if (dashTime <= maxDashTime)
        {
            dashTime += Time.deltaTime;
            fsm.transform.position += dir * dashSpeed * Time.deltaTime;
            return;
        }
        dashCount--;
        dashTime = 0;
        dir = (-fsm.transform.position + target.position).normalized;
        chargingTime = 0;
        if (getFar)
            altDSpeed *= dashMod;
        else
            altDSpeed /= dashMod;
        actual++;
    }
    private void Strafe()
    {
        if (chargingTime <= maxChargingTime)
        {
            chargingTime += Time.deltaTime;
            fsm.transform.position = Vector3.Lerp(fsm.transform.position, strafeDest, chargingTime / (maxChargingTime * 0.95f));
            if (chargingTime >= maxChargingTime * 0.95f)
            {
                SetTarget();
                preview.gameObject.SetActive(true);
            }
            dir=(-fsm.transform.position+target.position).normalized;
            startPos=fsm.transform.position;
            maxArrival=fsm.transform.position+dir*dashSpeed*dashTime;
            Vector3 straightTarget =new Vector3( target.position.x,fsm.transform.position.y,target.position.z);
            fsm.transform.LookAt(straightTarget);
            return;
        }
        if (afterStrafe <= mafs)
        {
            afterStrafe += Time.deltaTime;
            return;
        }

        actual++;
    }
    private void ReturnDash()
    {
        if (dashTime <= maxDashTime)
        {
            dashTime += Time.deltaTime;
            fsm.transform.position += dir * dashSpeed * Time.deltaTime;
            return;
        }
        dashCount--;
        dashTime = 0;
        dashAttackData.LaunchAttack();
        dir = (-fsm.transform.position + target.position).normalized;
        if (dashCount != 0)
        {                
            SetTarget();
            preview.gameObject.SetActive(true);
            dashTime = 0;
            chargingTime = 0;
            actual = state.CHARGING;
            return;
        }
        isDashing = false;
        Debug.Log("End");
        actual = state.CHARGING;
        dashAttackData.StopAttack();
        fsm.agent.vfx[0].SetActive(false);

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
    void SetTarget()
    {
        dir = (-fsm.transform.position + target.position).normalized;
        startPos = fsm.transform.position;
        maxArrival = fsm.transform.position + dir * dashSpeed * dashTime;
        Vector3 straightTarget =new Vector3( target.position.x,fsm.transform.position.y,target.position.z);
        fsm.transform.LookAt(straightTarget);
    }
    public override void End()
    {
        preview.gameObject.SetActive(false);
            fsm.agent.vfx[0].SetActive(false);
                dashAttackData.StopAttack();
    }
}
