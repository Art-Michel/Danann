using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
public class DesperationMove : MonoBehaviour
{
    #region Shoot
    [Foldout("Shoot"), SerializeField] private int P1D_nbShot;
    public int GetP1d_nbShot() { return P1D_nbShot; }
    [Foldout("Shoot"), SerializeField] private int shootAngle;
    public int GetShootAngle() { return shootAngle; }
    [Foldout("Shoot"), SerializeField] private float P1D_delay;
    public float GetP1d_delay() { return P1D_delay; }
    [Foldout("Shoot"), SerializeField] private float P1D_wait;
    public float GetP1d_wait() { return P1D_wait; }
    [Foldout("Shoot"), SerializeField] private float P1D_ProjLifeTime;
    public float GetP1d_ProjLifeTime() { return P1D_ProjLifeTime; }
    [Foldout("Shoot"), SerializeField] private float p1d_ShotSpeed;
    public float GetP1d_ShotSpeed() { return p1d_ShotSpeed; }

    [Foldout("Shoot"), SerializeField] private Pool pool;
    [Foldout("Preview"), SerializeField] Transform _preview;
    [Foldout("TP"), SerializeField] GameObject p2TP_arrival;
    public GameObject GetP2TP_Arrival() { return p2TP_arrival; }
    #endregion
    #region TP
    [Foldout("TP"), SerializeField] GameObject p2TP_fakeArrival;
    public GameObject GetP2TP_FakeArrival() { return p2TP_fakeArrival; }
    [Foldout("TP"), SerializeField] GameObject p2TP_boomBox;
    public GameObject GetP2TP_Boombox() { return p2TP_boomBox; }
    [Foldout("TP"), SerializeField] GameObject p2TP_fakeBoomBox;
    public GameObject GetP2TP_FakeBoombox() { return p2TP_fakeBoomBox; }
    [Foldout("TP"), SerializeField] float p2TP_FadeTime;
    public float GetP2TP_Fadetime() { return p2TP_FadeTime; }
    [Foldout("TP"), SerializeField] float p2TP_Startup;
    public float GetP2TP_Startup() { return p2TP_Startup; }
    [Foldout("TP"), SerializeField] float p2TP_offsetValue;
    public float GetP2TP_Offset() { return p2TP_offsetValue; }
    [Foldout("TP"), SerializeField] float p2TP_Reco;
    public float GetP2TP_Recovery() { return p2TP_Reco; }
    [Foldout("TP"), SerializeField] float p2TP_Active;
    public float GetP2TP_Active() { return p2TP_Active; }
    [Foldout("TP"), SerializeField] float p2TP_farDist;
    public float GetP2TP_FarDist() { return p2TP_farDist; }
    #endregion
    #region Rosace
    public Transform GetP1sD_Preview() { return _preview; }
    public Pool GetPool() { return pool; }
    [Foldout("Rosace"), SerializeField] Pool rosacePool;
    public Pool GetRosacePool(){return rosacePool;}
    [Foldout("Rosace"), SerializeField] GameObject straightProj;
    public GameObject GetStraightProj(){return straightProj;} 
    [Foldout("Rosace"), SerializeField] int rosaceNumber;
    public int GetRosaceNumber(){return rosaceNumber;}    
    [Foldout("Rosace"), SerializeField] int bulletNumber;
    public int GetRosaceBulletNB(){return bulletNumber;}
    [Foldout("Rosace"), SerializeField] float rosaceDelay;
    public float GetRosaceDelay(){return rosaceDelay;}
    [Foldout("Rosace"), SerializeField] float projSpeed;
    public float GetProjSpeed(){return projSpeed;}
    [Foldout("Rosace"), SerializeField] float arenaDist;
    public float GetArenaDist(){return arenaDist;}
    [Foldout("Rosace")][SerializeField] private float P1maxWaitTime;
    public float GetMaxWaitTime() { return P1maxWaitTime; }
    #endregion
    #region Slam
    [Foldout("Slam")][SerializeField] Vector3[] P1AttackFrames = new Vector3[3];
    public Vector3 GetAttackFrames(int index) { return P1AttackFrames[index]; }
    [Foldout("Slam")][SerializeField] private float P1SlamRecovery;
    public float GetP1SlamRecovery() { return P1SlamRecovery; }
    [Foldout("Slam")][SerializeField] Arr2D[] p1SlamHitBox;
    public Arr2D[] GetP1SlamHitBox() { return p1SlamHitBox; }

    #endregion
    [SerializeField] GameObject meteorShower;
    public DanuAI agent;
    [SerializeField] Volume postProcess;
    [SerializeField]List<Dm_State> states=new List<Dm_State>();
    enum State
    {
        SHOOT,
        DOUBLETP,
        ROSACE,
        TP,
        SLAM,
        LASTTP,
        LASER
    }
    int index;
    Dm_State curr;
    [SerializeField] GameObject laserGO;
    [SerializeField] Vector3 endPos;
    private float lerpValue;
    private Vector3 startPos;
    [SerializeField] float maxTime;

    bool over;
    private float ppTime;
    private bool goUp;
    [SerializeField] float maxWaitingTime;
    float waitingTime;

    // Start is called before the first frame update
    void Start()
    {
        agent.vfx[6].SetActive(true);
        states.Add(new DM_DoubleTP());
        states.Add(new DM_DoubleTP());
        states.Add(new DM_Rosace());
        states.Add(new Dm_TP(false));
        states.Add(new DM_Slam());
        states.Add(new Dm_TP(true));
        states.Add(new Dm_State());
        Debug.Log(states.Count+"eeeee"+states[index]);
        curr=new DM_Shoot();
        curr.fsm=this;
        curr.Begin();
        goUp=true;
        waitingTime=0;
        agent.m_anims.SetTrigger("DM");
        agent.m_anims.SetFloat("Pattern",-2);

        laserGO.GetComponentInChildren<Laser>().SetDM(this);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePP();
        if (waitingTime<maxWaitingTime)
        {
            waitingTime+=Time.deltaTime;
            return;
        }
        if (!over)
        curr.Update();
        else
        Move();

        //states[index].Update();
    }

    public void End()
    {
            agent.vfx[4].SetActive(false);
            agent.vfx[6].SetActive(false);
            agent.EndDM();
            enabled=false;
        endPos=agent.GetArenaCenter();
        transform.position=agent.GetArenaCenter();
    }

    private void UpdatePP()
    {
        if (goUp)
        {
            ppTime=Mathf.Clamp(ppTime+Time.deltaTime,0,1);
        }
        else
        {
            ppTime=Mathf.Clamp(ppTime-Time.deltaTime,0,1);
        }
        postProcess.weight=ppTime;
    }

    private void Move()
    {
        lerpValue+=Time.deltaTime;
        transform.position=Vector3.Lerp(startPos, endPos,lerpValue/maxTime);
        if (transform.position==endPos)
        {
            agent.m_anims.SetTrigger("LaserReady");
            laserGO.SetActive(true);
            goUp=false;
        }
    }

    public void Next()
    {
        index++;
        switch(index)
        {
            case 1:
                curr=new DM_DoubleTP();
                meteorShower.SetActive(true);

                break;
            case 2:
            curr=new DM_Rosace();
                break;
            case 3:
            curr=new Dm_TP(false);
                break;
            case 4:
            curr=new DM_Slam();
                break;
            case 5:
            curr=new Dm_TP(true);
                break;
            case 6:
            curr=new Dm_State();
                break;
            case 7:
            curr=states[index];
                break;

        }
        curr.fsm=this;
        curr.Begin();
        Debug.Log(index);

        if (index==states.Count-1)
        {
            postProcess.weight=0;
            over=true;
            Debug.Log(over);
            transform.LookAt(endPos);
            _preview.gameObject.SetActive(true);
            startPos=transform.position;
        }
        if (index>=states.Count)
        {    

        }
    }
}
[System.Serializable]
public class Arr2D
{
    public GameObject[] arr=new GameObject[3] ;
    public AttackData[] ad=new AttackData[3];
    public GameObject[] vfx=new GameObject[3];

}
