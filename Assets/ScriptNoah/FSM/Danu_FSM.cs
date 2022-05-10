using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class Danu_FSM : MonoBehaviour
{
    private Dictionary<string, Danu_State> m_states = new Dictionary<string, Danu_State>();

    [HideInInspector] public DanuAI agent = null;
    #region Phase 1
        #region Shoot
        [Foldout("Phase 1 Shoot"),SerializeField] private int P1D_nbShot;
        public int GetP1d_nbShot(){return P1D_nbShot;}
        [Foldout("Phase 1 Shoot"),SerializeField] private float P1D_delay;
        public float GetP1d_delay(){return P1D_delay;}
        [Foldout("Phase 1 Shoot"),SerializeField] private float P1D_wait;
        public float GetP1d_wait(){return P1D_wait;}
        [Foldout("Phase 1 Shoot"),SerializeField] private float P1D_ProjLifeTime;
        public float GetP1d_ProjLifeTime(){return P1D_ProjLifeTime;}
        [Foldout("Phase 1 Shoot"),SerializeField] private float p1d_ShotSpeed;
        public float GetP1d_ShotSpeed(){return p1d_ShotSpeed;}
        [Foldout("Phase 1 Shoot"),SerializeField] private Pool pool;
        public Pool GetPool(){return pool;}
        #endregion
        #region Slam
        [Foldout("Phase 1 Slam")][SerializeField]GameObject p1SlamHitBox;
        public GameObject Getp1SlamHitBox(){return p1SlamHitBox;}
        [Foldout("Phase 1 Slam")] [SerializeField] Vector3[] P1AttackFrames=new Vector3[3];
        public Vector3 GetAttackFrames(int index){return P1AttackFrames[index];}
        [Foldout("Phase 1 Slam")][SerializeField] private float P1SlamRecovery;
        public float GetP1SlamRecovery(){return P1SlamRecovery;}
        [Foldout("Phase 1 Slam")][SerializeField] private Vector3[] P1slamScales=new Vector3[3];
        public Vector3 GetP1SlamScale(int index) {return P1slamScales[index];}
        [Foldout("Phase 1 Slam")][SerializeField] private float[] p1S_radius=new float[3];
        public float[] GetP1S_Radius(){return p1S_radius;}

        [Foldout("Phase 1 Slam")][SerializeField] private int[] p1S_Damage;
        public int[] GetP1S_Damage(){return p1S_Damage;}
        [Foldout("Phase 1 Slam")][SerializeField]private float p1Sl_MaxMoveTime;
        public float GetP1Sl_MaxMoveTime(){return p1Sl_MaxMoveTime;}

        #endregion
        #region Spin
        [Foldout("Phase 1 Spin")][SerializeField] private GameObject staticProj;
        public GameObject GetStaticProj(){  return staticProj;  }
        [Foldout("Phase 1 Spin")][SerializeField] private GameObject P1globalGO;
        public GameObject GetP1GlobalGO(){return P1globalGO;}
        [Foldout("Phase 1 Spin")][SerializeField] private Transform[] P1nweMax=new Transform[3];
        public Transform[] GetP1NWEMax(){return P1nweMax;}
        [Foldout("Phase 1 Spin")][SerializeField] private float p1sp_dist;
        public float GetP1Sp_Dist(){return p1sp_dist;}
        [Foldout("Phase 1 Spin")][SerializeField] private float P1rotationSpeed;
        public float GetP1RotationSpeed(){return P1rotationSpeed;}
        [Foldout("Phase 1 Spin")][SerializeField] private bool P1turningRight;
        public bool GetP1TurningRight(){return P1turningRight;}
        [Foldout("Phase 1 Spin")][SerializeField] private float P1maxWaitTime;
        public float GetP1MaxWaitTime(){return P1maxWaitTime;}
        [Foldout("Phase 1 Spin")][SerializeField] private float P1SpinLifeTime;
        public float GetP1SpinLifeTime(){return P1SpinLifeTime;}
        [Foldout("Phase 1 Spin")][SerializeField] private Transform[] bladesPreview;
        public Transform[] GetBladesPreview(){return bladesPreview;}
        #endregion
        #region Boomerang
        [Foldout("Phase 1 Boomerang"),SerializeField] private GameObject P1BoomerangboomerangL;
        public GameObject GetP1BRL(){return P1BoomerangboomerangL;}
        [Foldout("Phase 1 Boomerang")][SerializeField] private GameObject P1BoomerangboomerangR;
        public GameObject GetP1BRR(){return P1BoomerangboomerangR;}

        [Foldout("Phase 1 Boomerang")][SerializeField] private float P1BoomeRangSpeed;
        public float GetP1BoomeRangSpeed(){return P1BoomeRangSpeed;}
        [Foldout("Phase 1 Boomerang")][SerializeField] private float P1BoomeRangMaxStraightTime;
        public float GetP1BoomeRangMaxStraightTime(){return P1BoomeRangMaxStraightTime;}
        [Foldout("Phase 1 Boomerang")][SerializeField]Transform P1BoomeRangcurveMidL;
        public Transform GetP1BoomeRangcurveMidL(){return P1BoomeRangcurveMidL;}
        [Foldout("Phase 1 Boomerang")][SerializeField]Transform P1BoomeRangcurveMidR;
        public Transform GetP1BoomeRangcurveMidR(){return P1BoomeRangcurveMidR;}
        [Foldout("Phase 1 Boomerang")][SerializeField] private float P1BoomeRangMaxCurveTime;
        public float GetP1BoomeRangMaxCurveTime(){return P1BoomeRangMaxCurveTime;}
        [Foldout("Phase 1 Boomerang")][SerializeField] private float p1Br_startup;
        public float GetP1BR_Startup(){return p1Br_startup;}
        #endregion
        #region Dash
        [Foldout("Phase 1 Dash"),SerializeField] private Transform p1sDash_preview;
        public Transform GetP1sD_Preview(){return p1sDash_preview;}
        [Foldout("Phase 1 Dash"),SerializeField] private float p1sDash_MaxDashTime;
        public float GetP1sD_MDashT(){return p1sDash_MaxDashTime;}
        [Foldout("Phase 1 Dash"),SerializeField] private int p1sDash_MaxDashCount;
        public int GetP1sD_MDCount(){return p1sDash_MaxDashCount;}
        [Foldout("Phase 1 Dash"),SerializeField] private float p1sDash_DashSpeed;
        public float GetP1sD_DashSpeed(){return p1sDash_DashSpeed;}
        [Foldout("Phase 1 Dash"),SerializeField] private float p1sDash_MaxChargingTime;
        public float GetP1sD_ChargingTime(){return p1sDash_MaxChargingTime;}
        [Foldout("Phase 1 Dash"),SerializeField] private float p1Dash_dashModifier;
        public float GetPMD_dMod(){return p1Dash_dashModifier;}

        #endregion
    
        #region TelePortation
        [Foldout("Phase 1 TP"),SerializeField]GameObject p1TP_arrival;
        public GameObject GetP1TP_Arrival(){return p1TP_arrival;}
        [Foldout("Phase 1 TP"),SerializeField]GameObject p1TP_boomBox;
        public GameObject GetP1TP_Boombox(){return p1TP_boomBox;}
        [Foldout("Phase 1 TP"),SerializeField]P1CTeleportation.destPoints p1TP_destination;
        public P1CTeleportation.destPoints GetP1TP_Destination(){return p1TP_destination;}
        [Foldout("Phase 1 TP"),SerializeField]float p1TP_FadeTime;
        public float GetP1TP_Fadetime(){return p1TP_FadeTime;}
        [Foldout("Phase 1 TP"),SerializeField]float p1TP_Startup;
        public float GetP1TP_Startup(){return p1TP_Startup;}
        [Foldout("Phase 1 TP"),SerializeField]float p1TP_offsetValue;
        public float GetP1TP_Offset(){return p1TP_offsetValue;}
        [Foldout("Phase 1 TP"),SerializeField]float p1TP_Reco;
        public float GetP1TP_Recovery(){return p1TP_Reco;}
        [Foldout("Phase 1 TP"),SerializeField]float p1TP_Active;
        public float GetP1TP_Active(){return p1TP_Active;}
        [Foldout("Phase 1 TP"),SerializeField] float p1TP_farDist;
        public float GetP1TP_FarDist(){return p1TP_farDist;}
        #endregion
    private List<GameObject> baseProjectiles = new List<GameObject>();
    public void AddProjectile()
    {
        GameObject go = Instantiate(agent.GetProjectile(),transform.position,transform.rotation);
        go.GetComponent<Projectiles>().SetTarget(agent.GetPlayer());
    }
    public GameObject GetProjectile()
    {
        return baseProjectiles[baseProjectiles.Count-1];
    }
    public int GetProjectileCount()
    {
        return baseProjectiles.Count;
    }
    #endregion
    int indexx;
    
    public Danu_State curr { get; private set; }
    public Danu_State prev { get; private set; }
    [Button]
    public void QuickDebug()
    {
       
    }
    public void AddState( Danu_State state )
    {
        state.fsm = this;
        this.m_states[state.name] = state;
    }
    public void RemoveState(Danu_State state)
    {
        m_states.Remove(state.name);
    }

    public void ChangeState( string nextStateName, float idleTime=0f )
    {
        Danu_State state = null;
        this.m_states.TryGetValue( nextStateName, out state );
        if( state == null )
        {
            Debug.LogError( $"[FSM] We don't have a state with the name {nextStateName}" );
            return;
        }

        if( state == this.curr )
            return; // already in this state

        if( this.curr != null ) {
            this.curr.End();
        }
        this.prev = curr;
        this.curr = state;
        this.curr.Begin();
        Debug.Log( $"[FSM] Started state {this.curr.name}" );
    }

    void Update()
    {
        if( this.curr != null ) 
        {
            this.curr.Update();
        }
    }
    
    public GameObject InstantiateStaticProjectile(Vector3 pos)
    {
        return Instantiate(staticProj,pos,Quaternion.identity,P1globalGO.transform);
    }
}
