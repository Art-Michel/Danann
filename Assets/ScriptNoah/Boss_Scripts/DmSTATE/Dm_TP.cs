using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Dm_TP : Dm_State
{
    public Dm_TP(bool last)  {isLast=last; }
    public bool isLast;
    GameObject arrival, fakeArrival;
    GameObject boomBox,fakeBoomBox;
    private GameObject departBoomBox;
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
    AttackData departAttackData;

    void Init()
    {
        stateName="TP";
        Debug.Log(stateName+" "+isLast);
        arrival=fsm.GetP2TP_Arrival();
        fakeArrival=fsm.GetP2TP_FakeArrival();
        boomBox=fsm.GetP2TP_Boombox();
        fakeBoomBox=fsm.GetP2TP_FakeBoombox();
        departBoomBox=fsm.GetDepartBoombox();
        departAttackData=departBoomBox.GetComponent<AttackData>();
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
        Init();
        SoundManager.Instance.PlayBossTpCharge();
        arrival.SetActive(false);
        fakeArrival.SetActive(false);
        Vector2 rand = Random.insideUnitCircle;
        Vector3 offset = new Vector3(rand.x, 0, rand.y) * offsetValue;
        arrival.transform.position = target.position + offset;
        if (!isLast)
            fakeArrival.transform.position = target.position - offset;
        
        fsm.agent.vfx[1].SetActive(true);
        fsm.agent.vfx[2].SetActive(true);
        if (!isLast)
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
            if (startup>=MaxSartup)
            {
                SoundManager.Instance.PlayBossTpIn();
                departAttackData.LaunchAttack();
            }

        }
        else if (fadeTime <= MaxFadeTime)
        {
            if (fadeTime>0.1f)
                departAttackData.StopAttack();

            arrival.SetActive(true);
            fsm.agent.HideMesh();
            if (!isLast)
            fakeArrival.SetActive(true);
            fadeTime += Time.deltaTime;
            if (fadeTime>=MaxFadeTime)
                SoundManager.Instance.PlayBossTpOut();
        }
        else if (active <= maxActive)
        {
            fsm.agent.ShowMesh();
            //boomBox.SetActive(true);
            boomBoxAttackData.LaunchAttack();
            if (!isLast)
                fakeBoomBoxAttackData.LaunchAttack();
            fsm.transform.position = arrival.transform.position;
            boomBox.transform.position = arrival.transform.position;
            fakeBoomBox.transform.position = fakeArrival.transform.position;
            active += Time.deltaTime;
            if (active<=maxActive)
                fsm.agent.m_animsP2.SetTrigger("TPOver");

        }
        else if (reco <= maxReco)
        {
            boomBoxAttackData.StopAttack();
            fakeBoomBoxAttackData.StopAttack();
                    fsm.agent.vfx[1].SetActive(false);
            if (isLast)
            reco += Time.deltaTime*2;
            else
            reco += Time.deltaTime;
        }
        else
        {
        fsm.agent.vfx[2].SetActive(false);
        fsm.agent.vfx[3].SetActive(false);
            fsm.Next();
        }
    }


    
}