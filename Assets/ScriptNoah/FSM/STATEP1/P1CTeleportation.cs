using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1CTeleportation : Danu_State
{
    public P1CTeleportation() : base(StateNames.P1C_TELEPORTATION) { }
    [SerializeField]GameObject arrival;
    [SerializeField]GameObject boomBox;
    [SerializeField]Transform target;
    [SerializeField]destPoints destination;
    [SerializeField]float MaxFadeTime;
    [SerializeField]float MaxSartup;
    [SerializeField]float offsetValue;
    [SerializeField]float maxReco;
    [SerializeField]float maxActive;
    [SerializeField] float farDist;
    float fadeTime;
    float startup;
    float active;
    bool isSetUp;
    Vector3 arenaCenter;
    public enum destPoints
    {
        FAR,
        CLOSE
    }
    float reco;
    // Start is called before the first frame update
    public override void Begin()
    {
        if(!isSetUp)
        {
            arrival=fsm.GetP1TP_Arrival();
            boomBox=fsm.GetP1TP_Boombox();
            destination = fsm.GetP1TP_Destination();
            MaxFadeTime=fsm.GetP1TP_Fadetime();
            MaxSartup=fsm.GetP1TP_Startup();
            offsetValue=fsm.GetP1TP_Offset();
            maxReco=fsm.GetP1TP_Recovery();
            maxActive=fsm.GetP1TP_Active();
            farDist=fsm.GetP1TP_FarDist();
            target=fsm.agent.GetPlayer();
            arenaCenter=fsm.agent.GetArenaCenter();
            isSetUp=true;
        }
        destination=fsm.GetP1TP_Destination();
        arrival.SetActive(false);
        if (destination==destPoints.FAR)
        {
            float dist=farDist/Vector3.Distance(fsm.transform.position,target.position);
            Vector3 dir=fsm.transform.position-target.position;
            dir.Normalize();
            dir*=dist;
            arrival.transform.position=fsm.transform.position+dir;
            if (Vector3.Distance(arrival.transform.position, arenaCenter)>=fsm.agent.GetArenaRadius())
            {
                dir=fsm.transform.position-target.position;
                dir.Normalize();
                arrival.transform.position=fsm.agent.GetArenaRadius()*dir;
            }
        }
        else
        {
            Vector2 rand=Random.insideUnitCircle;
            Vector3 offset=new Vector3(rand.x,0,rand.y)*offsetValue;
            arrival.transform.position=target.position+offset;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        TP();
    }
    void TP()
    {
        if (startup<=MaxSartup)
        {
            startup+=Time.deltaTime;
        }
        else if (fadeTime<=MaxFadeTime)
        {
            arrival.SetActive(true);
            fadeTime+=Time.deltaTime;
        }
        else if (active<=maxActive)
        {
            boomBox.SetActive(true);
            fsm.transform.position=arrival.transform.position;
            active+=Time.deltaTime;
        }
        else if (reco<=maxReco)
        {
            boomBox.SetActive(false);
            arrival.SetActive(false);
            reco+=Time.deltaTime;
        }
        else
        {
            if (orig==null)
            {
                fsm.agent.ToIdle();
            }             
            else
            {
                orig.AddWaitTime(2);
                orig.FlowControl();
            }
        }
    }
}