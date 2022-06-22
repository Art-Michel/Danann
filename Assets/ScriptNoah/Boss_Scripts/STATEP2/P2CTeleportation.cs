using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class P2CTeleportation : Danu_State
{
    public P2CTeleportation() : base(StateNames.P2C_TELEPORTATION){}

    GameObject arrival, fakeArrival;
    GameObject boomBox,fakeBoomBox;
    AttackData boomBoxAttackData,fakeBoomBoxAttackData;
    Transform target;
    [SerializeField] P1CTeleportation.destPoints destination;
    [SerializeField] float MaxFadeTime;
    [SerializeField] float MaxSartup;
    [SerializeField] float offsetValue;
    [SerializeField] float maxReco;
    [SerializeField] float maxActive;
    [SerializeField] float farDist;
    float fadeTime;
    float startup;
    float active;
    [SerializeField]Vector3 arenaCenter;
    public enum destPoints
    {
        FAR,
        CLOSE
    }
    float reco;

    [SerializeField]float arenaRadius;
    CinemachineTargetGroup cam;
    private bool _lerpOut;
    private bool _lerpIn;
    float camWeight;
    public override void Init()
    {
        arrival=fsm.GetP1TP_Arrival();
        fakeArrival=fsm.GetP2TP_FakeArrival();
        boomBox=fsm.GetP2TP_Boombox();
        fakeBoomBox=fsm.GetP2TP_FakeBoombox();
        boomBoxAttackData=boomBox.GetComponent<AttackData>();
        fakeBoomBoxAttackData=fakeBoomBox.GetComponent<AttackData>();
        MaxFadeTime = fsm.GetP2TP_Fadetime();
        MaxSartup = fsm.GetP2TP_Startup();
        offsetValue = fsm.GetP2TP_Offset();
        maxReco = fsm.GetP2TP_Recovery();
        maxActive = fsm.GetP2TP_Active();
        farDist = fsm.GetP2TP_FarDist();
        target=fsm.agent.GetPlayer();
        arenaCenter = fsm.agent.GetArenaCenter();
        arenaRadius=fsm.agent.GetArenaRadius();
        camWeight=0.4f;
    }
    // Start is called before the first frame update
    public override void Begin() 
    {        
        if(!isInit)
            Init();
        destination=fsm.GetP1TP_Destination();
        arrival.SetActive(false);
        fakeArrival.SetActive(false);
        if (destination == P1CTeleportation.destPoints.FAR)
        {
            Debug.Log("far");
            float dist = farDist / Vector3.Distance(fsm.transform.position, target.position);
            Vector3 dir = fsm.transform.position - target.position;
            dir.Normalize();
            dir *= farDist;
            Vector2 rand = Random.insideUnitCircle;
            Vector3 offset = new Vector3(rand.x, 0, rand.y) * offsetValue*2;
            arrival.transform.position = fsm.transform.position + dir-offset;
            fakeArrival.transform.position = fsm.transform.position + dir+offset;
            if (Vector3.Distance(arrival.transform.position, arenaCenter) >= arenaRadius)
            {
                dir = fsm.transform.position - target.position;
                dir.Normalize();
                arrival.transform.position =arenaCenter+ arenaRadius * dir;
            }            
            if (Vector3.Distance(fakeArrival.transform.position, arenaCenter) >= arenaRadius)
            {
                dir = fsm.transform.position - target.position;
                dir.Normalize();
                fakeArrival.transform.position =arenaCenter+ arenaRadius * dir;
            }
        }
        else
        {
            Debug.Log("close");
            Vector2 rand = Random.insideUnitCircle;
            Vector3 offset = new Vector3(rand.x, 0, rand.y) * offsetValue;
            arrival.transform.position = target.position + offset;
            fakeArrival.transform.position = target.position - offset;
        }
        fsm.agent.vfx[1].SetActive(true);
        fsm.agent.vfx[2].SetActive(true);
        fsm.agent.vfx[3].SetActive(true);
        startup=0;
        fadeTime=0;
        active=0;
        reco=0;
    }

    // Update is called once per frame
    public override void Update() 
    {
        TP();
        LerpIn();
        LerpOut();
    }   
private void LerpIn()
    {
        if (!_lerpIn)
            return;
        float realtime=startup+fadeTime;
        float lerpValue=Mathf.Clamp(realtime,0,1);
        cam.m_Targets[cam.m_Targets.Length-1].weight=lerpValue;
        cam.m_Targets[cam.m_Targets.Length-2].weight=lerpValue;
        if (lerpValue>=camWeight)
        {
            _lerpIn=false;
        }


    }

    private void LerpOut()
    {
        if (!_lerpOut)
            return;
        float lerpValue=Mathf.Lerp(0,maxReco,(maxReco-reco)*camWeight);
        
        cam.m_Targets[cam.m_Targets.Length-1].weight=lerpValue;
        cam.m_Targets[cam.m_Targets.Length-2].weight=lerpValue;
        if (lerpValue==0)
        {
            _lerpOut=false;
        }
    }
    void TP()
    {
        if (startup <= MaxSartup)
        {
            startup += Time.deltaTime;
        }
        else if (fadeTime <= MaxFadeTime)
        {
            fsm.agent.HideMesh();
            arrival.SetActive(true);
            fakeArrival.SetActive(true);
            fadeTime += Time.deltaTime;
        }
        else if (active <= maxActive)
        {
            fsm.agent.ShowMesh();

            //boomBox.SetActive(true);
            boomBoxAttackData.LaunchAttack();
            fakeBoomBoxAttackData.LaunchAttack();
            fsm.transform.position = arrival.transform.position;
            boomBox.transform.position = arrival.transform.position;
            fakeBoomBox.transform.position = fakeArrival.transform.position;
            active += Time.deltaTime;
        }
        else if (reco <= maxReco)
        {
            boomBoxAttackData.StopAttack();
            fakeBoomBoxAttackData.StopAttack();
                    fsm.agent.vfx[1].SetActive(false);

            reco += Time.deltaTime;
        }
        else
        {
        fsm.agent.vfx[2].SetActive(false);
        fsm.agent.vfx[3].SetActive(false);
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
    }
    public override void End()
    {
        fsm.agent.vfx[2].SetActive(false);
        fsm.agent.vfx[3].SetActive(false);
        startup=0;
        fadeTime=0;
        active=0;
        reco=0;
        boomBoxAttackData.StopAttack();
        fakeBoomBoxAttackData.StopAttack();
        fsm.agent.vfx[1].SetActive(false);
        fsm.agent.ShowMesh();

    }

    
}