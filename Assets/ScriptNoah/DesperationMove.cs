using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class DesperationMove : MonoBehaviour
{
    [Foldout("Phase 1 Shoot"), SerializeField] private int P1D_nbShot;
    public int GetP1d_nbShot() { return P1D_nbShot; }
    [Foldout("Phase 1 Shoot"), SerializeField] private float P1D_delay;
    public float GetP1d_delay() { return P1D_delay; }
    [Foldout("Phase 1 Shoot"), SerializeField] private float P1D_wait;
    public float GetP1d_wait() { return P1D_wait; }
    [Foldout("Phase 1 Shoot"), SerializeField] private float P1D_ProjLifeTime;
    public float GetP1d_ProjLifeTime() { return P1D_ProjLifeTime; }
    [Foldout("Phase 1 Shoot"), SerializeField] private float p1d_ShotSpeed;
    public float GetP1d_ShotSpeed() { return p1d_ShotSpeed; }
    [Foldout("Phase 1 Shoot"), SerializeField] private Pool pool;
     [Foldout("Phase 1 Dash"), SerializeField] Transform p1sDash_preview;
    
     [Foldout("Phase 2 TP"), SerializeField] GameObject p2TP_arrival;
    public GameObject GetP2TP_Arrival() { return p2TP_arrival; }
    [Foldout("Phase 2 TP"), SerializeField] GameObject p2TP_fakeArrival;
    public GameObject GetP2TP_FakeArrival() { return p2TP_fakeArrival; }
    [Foldout("Phase 2 TP"), SerializeField] GameObject p2TP_boomBox;
    public GameObject GetP2TP_Boombox() { return p2TP_boomBox; }
    [Foldout("Phase 2 TP"), SerializeField] GameObject p2TP_fakeBoomBox;
    public GameObject GetP2TP_FakeBoombox() { return p2TP_fakeBoomBox; }
    [Foldout("Phase 2 TP"), SerializeField] float p2TP_FadeTime;
    public float GetP2TP_Fadetime() { return p2TP_FadeTime; }
    [Foldout("Phase 2 TP"), SerializeField] float p2TP_Startup;
    public float GetP2TP_Startup() { return p2TP_Startup; }
    [Foldout("Phase 2 TP"), SerializeField] float p2TP_offsetValue;
    public float GetP2TP_Offset() { return p2TP_offsetValue; }
    [Foldout("Phase 2 TP"), SerializeField] float p2TP_Reco;
    public float GetP2TP_Recovery() { return p2TP_Reco; }
    [Foldout("Phase 2 TP"), SerializeField] float p2TP_Active;
    public float GetP2TP_Active() { return p2TP_Active; }
    [Foldout("Phase 2 TP"), SerializeField] float p2TP_farDist;
    public float GetP2TP_FarDist() { return p2TP_farDist; }
    public Transform GetP1sD_Preview() { return p1sDash_preview; }
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
    [Foldout("Rosace"), SerializeField] float arenaDist;
    public float GetArenaDist(){return arenaDist;}
    [Foldout("Phase 1 Spin")][SerializeField] private float P1maxWaitTime;
    public float GetMaxWaitTime() { return P1maxWaitTime; }
    [Foldout("Phase 1 Slam")][SerializeField] Vector3[] P1AttackFrames = new Vector3[3];
    public Vector3 GetAttackFrames(int index) { return P1AttackFrames[index]; }
    [Foldout("Phase 1 Slam")][SerializeField] private float P1SlamRecovery;
    public float GetP1SlamRecovery() { return P1SlamRecovery; }
    [Foldout("Phase 1 Slam")][SerializeField] Arr2D[] p1SlamHitBox;
    public Arr2D[] GetP1SlamHitBox() { return p1SlamHitBox; }
    public DanuAI agent;
    List<Dm_State> states=new List<Dm_State>();
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
    // Start is called before the first frame update
    void Start()
    {
        /*states.Add(new DM_Shoot());
        states.Add(new DM_DoubleTP());
        states.Add(new DM_Rosace());
        states.Add(new Dm_TP(false));
        //states.Add(new DM_Slam());
        states.Add(new Dm_TP(true));*/
        curr=new DM_Rosace();
        curr.fsm=this;
        curr.Begin(); 
    }

    // Update is called once per frame
    void Update()
    {
        curr.Update();
        //states[index].Update();
    }

    public void Next()
    {
        index++;
        if (index>=states.Count)
        {    
            agent.EndDM();
            enabled=false;
        }
    }
}
[System.Serializable]
public class Arr2D
{
    public GameObject[] arr=new GameObject[3] ;
    public AttackData[] ad=new AttackData[3];
}
