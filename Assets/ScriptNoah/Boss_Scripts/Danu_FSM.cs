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
    public Pool GetPool() { return pool; }
    #endregion
        #region Slam
    [Foldout("Phase 1 Slam")][SerializeField] AttackData P1Slam1AttackData;
    public AttackData GetP1Slam1AttackData() { return P1Slam1AttackData; }
    [Foldout("Phase 1 Slam")][SerializeField] AttackData P1Slam2AttackData;
    public AttackData GetP1Slam2AttackData() { return P1Slam2AttackData; }
    [Foldout("Phase 1 Slam")][SerializeField] AttackData P1Slam3AttackData;
    public AttackData GetP1Slam3AttackData() { return P1Slam3AttackData; }

    [Foldout("Phase 1 Slam")][SerializeField] GameObject[] p1SlamHitBox;
    public GameObject[] GetP1SlamHitBox() { return p1SlamHitBox; }
    [Foldout("Phase 1 Slam")][SerializeField] Vector3[] P1AttackFrames = new Vector3[3];
    public Vector3 GetAttackFrames(int index) { return P1AttackFrames[index]; }
    [Foldout("Phase 1 Slam")][SerializeField] private float P1SlamRecovery;
    public float GetP1SlamRecovery() { return P1SlamRecovery; }
    /*[Foldout("Phase 1 Slam")][SerializeField] private Vector3[] P1slamScales = new Vector3[3];
    public Vector3 GetP1SlamScale(int index) { return P1slamScales[index]; }*/
    /*[Foldout("Phase 1 Slam")][SerializeField] private float[] p1S_radius = new float[3];
    public float[] GetP1S_Radius() { return p1S_radius; }*/

    /*[Foldout("Phase 1 Slam")][SerializeField] private int[] p1S_Damage;
    public int[] GetP1S_Damage() { return p1S_Damage; }*/
    [Foldout("Phase 1 Slam")][SerializeField] private float p1Sl_MaxMoveTime;
    public float GetP1Sl_MaxMoveTime() { return p1Sl_MaxMoveTime; }

    #endregion
        #region Spin
    [Foldout("Phase 1 Spin")][SerializeField] private GameObject staticProj;
    public GameObject GetStaticProj() { return staticProj; }
    [Foldout("Phase 1 Spin")][SerializeField] private GameObject P1globalGO;
    public GameObject GetP1GlobalGO() { return P1globalGO; }
    [Foldout("Phase 1 Spin")][SerializeField] private Transform[] P1nweMax = new Transform[3];
    public Transform[] GetP1NWEMax() { return P1nweMax; }
    [Foldout("Phase 1 Spin")][SerializeField] private float p1sp_dist;
    public float GetP1Sp_Dist() { return p1sp_dist; }
    [Foldout("Phase 1 Spin")][SerializeField] private float P1rotationSpeed;
    public float GetP1RotationSpeed() { return P1rotationSpeed; }
    [Foldout("Phase 1 Spin")][SerializeField] private bool P1turningRight;
    public bool GetP1TurningRight() { return P1turningRight; }
    [Foldout("Phase 1 Spin")][SerializeField] private float P1maxWaitTime;
    public float GetP1MaxWaitTime() { return P1maxWaitTime; }
    [Foldout("Phase 1 Spin")][SerializeField] private float P1SpinLifeTime;
    public float GetP1SpinLifeTime() { return P1SpinLifeTime; }
    [Foldout("Phase 1 Spin")][SerializeField] private Transform[] bladesPreview;
    public Transform[] GetBladesPreview() { return bladesPreview; }
    #endregion
        #region Boomerang
    [Foldout("Phase 1 Boomerang"), SerializeField] private GameObject P1BoomerangboomerangL;
    public GameObject GetP1BRL() { return P1BoomerangboomerangL; }
    [Foldout("Phase 1 Boomerang")][SerializeField] private GameObject P1BoomerangboomerangR;
    public GameObject GetP1BRR() { return P1BoomerangboomerangR; }
    [Foldout("Phase 1 Boomerang")][SerializeField] AttackData boomerangAttackData;
    public AttackData GetBoomerangAttackData() { return boomerangAttackData; }

    [Foldout("Phase 1 Boomerang")][SerializeField] float P1BoomeRangSpeed;
    public float GetP1BoomeRangSpeed() { return P1BoomeRangSpeed; }
    [Foldout("Phase 1 Boomerang")][SerializeField] float p1br_MaxDist;
    public float GetP1BR_MaxDist() { return p1br_MaxDist; }
    [Foldout("Phase 1 Boomerang")][SerializeField] float P1BoomeRangMaxStraightTime;
    public float GetP1BoomeRangMaxStraightTime() { return P1BoomeRangMaxStraightTime; }
    [Foldout("Phase 1 Boomerang")][SerializeField] Transform P1BoomeRangcurveMidL;
    public Transform GetP1BoomeRangcurveMidL() { return P1BoomeRangcurveMidL; }
    [Foldout("Phase 1 Boomerang")][SerializeField] Transform P1BoomeRangcurveMidR;
    public Transform GetP1BoomeRangcurveMidR() { return P1BoomeRangcurveMidR; }
    [Foldout("Phase 1 Boomerang")][SerializeField] float P1BoomeRangMaxCurveTime;
    public float GetP1BoomeRangMaxCurveTime() { return P1BoomeRangMaxCurveTime; }
    [Foldout("Phase 1 Boomerang")][SerializeField] float p1Br_startup;
    public float GetP1BR_Startup() { return p1Br_startup; }
    #endregion
        #region Dash
    [Foldout("Phase 1 Dash"), SerializeField] Transform p1sDash_preview;
    public Transform GetP1sD_Preview() { return p1sDash_preview; }
    [Foldout("Phase 1 Dash"), SerializeField] float p1sDash_MaxDashTime;
    public float GetP1sD_MDashT() { return p1sDash_MaxDashTime; }
    [Foldout("Phase 1 Dash"), SerializeField] int p1sDash_MaxDashCount;
    public int GetP1sD_MDCount() { return p1sDash_MaxDashCount; }
    [Foldout("Phase 1 Dash"), SerializeField] float p1sDash_DashSpeed;
    public float GetP1sD_DashSpeed() { return p1sDash_DashSpeed; }
    [Foldout("Phase 1 Dash"), SerializeField] float p1sDash_MaxChargingTime;
    public float GetP1sD_ChargingTime() { return p1sDash_MaxChargingTime; }
    [Foldout("Phase 1 Dash"), SerializeField] float p1Dash_dashModifier;
    public float GetPMD_dMod() { return p1Dash_dashModifier; }
    [Foldout("Phase 1 Dash"), SerializeField] AttackData p1Dash_AttackData;
    public AttackData GetP1DashAttackData() { return p1Dash_AttackData; }
    #endregion
        #region TelePortation
    [Foldout("Phase 1 TP"), SerializeField] GameObject p1TP_arrival;
    public GameObject GetP1TP_Arrival() { return p1TP_arrival; }
    [Foldout("Phase 1 TP"), SerializeField] GameObject p1TP_boomBox;
    public GameObject GetP1TP_Boombox() { return p1TP_boomBox; }
    [Foldout("Phase 1 TP"), SerializeField] P1CTeleportation.destPoints p1TP_destination;
    public P1CTeleportation.destPoints GetP1TP_Destination() { return p1TP_destination; }
    public void SetTPDest(P1CTeleportation.destPoints newDest) { p1TP_destination = newDest; }
    [Foldout("Phase 1 TP"), SerializeField] float p1TP_FadeTime;
    public float GetP1TP_Fadetime() { return p1TP_FadeTime; }
    [Foldout("Phase 1 TP"), SerializeField] float p1TP_Startup;
    public float GetP1TP_Startup() { return p1TP_Startup; }
    [Foldout("Phase 1 TP"), SerializeField] float p1TP_offsetValue;
    public float GetP1TP_Offset() { return p1TP_offsetValue; }
    [Foldout("Phase 1 TP"), SerializeField] float p1TP_Reco;
    public float GetP1TP_Recovery() { return p1TP_Reco; }
    [Foldout("Phase 1 TP"), SerializeField] float p1TP_Active;
    public float GetP1TP_Active() { return p1TP_Active; }
    [Foldout("Phase 1 TP"), SerializeField] float p1TP_farDist;
    public float GetP1TP_FarDist() { return p1TP_farDist; }
    #endregion    
    #endregion
    #region Phase 2
        #region Shoot
    [Foldout("Phase 2 Shoot"), SerializeField] private int P2D_nbShot;
    public int GetP2d_nbShot() { return P2D_nbShot; }
    [Foldout("Phase 2 Shoot"), SerializeField] private float P2D_delay;
    public float GetP2d_delay() { return P2D_delay; }
    [Foldout("Phase 2 Shoot"), SerializeField] private float P2D_wait;
    public float GetP2d_wait() { return P2D_wait; }
    [Foldout("Phase 2 Shoot"), SerializeField] private float P2D_ProjLifeTime;
    public float GetP2d_ProjLifeTime() { return P2D_ProjLifeTime; }
    [Foldout("Phase 2 Shoot"), SerializeField] private float p2d_ShotSpeed;
    public float GetP2d_ShotSpeed() { return p2d_ShotSpeed; }
    #endregion
        #region Slam
    [Foldout("Phase 2 Slam")][SerializeField] AttackData P2Slam1AttackData;
    public AttackData GetP2Slam1AttackData() { return P2Slam1AttackData; }
    [Foldout("Phase 2 Slam")][SerializeField] AttackData P2Slam2AttackData;
    public AttackData GetP2Slam2AttackData() { return P2Slam2AttackData; }
    [Foldout("Phase 2 Slam")][SerializeField] AttackData P2Slam3AttackData;
    public AttackData GetP2Slam3AttackData() { return P2Slam3AttackData; }

    [Foldout("Phase 2 Slam")][SerializeField] GameObject[] p2SlamHitBox;
    public GameObject[] GetP2SlamHitBox() { return p2SlamHitBox; }
    [Foldout("Phase 2 Slam")][SerializeField] Vector3[] P2AttackFrames = new Vector3[3];
    public Vector3 GetP2AttackFrames(int index) { return P2AttackFrames[index]; }
    [Foldout("Phase 2 Slam")][SerializeField] private float P2SlamRecovery;
    public float GetP2SlamRecovery() { return P2SlamRecovery; }
    [Foldout("Phase 2 Slam")][SerializeField] private float p2Sl_MaxMoveTime;
    public float GetP2Sl_MaxMoveTime() { return p2Sl_MaxMoveTime; }

    #endregion
        #region Spin
    [Foldout("Phase 2 Spin")][SerializeField] private GameObject P2globalGO;
    public GameObject GetP2GlobalGO() { return P2globalGO; }
    [Foldout("Phase 2 Spin")][SerializeField] private Transform[] P2nwesMax = new Transform[4];
    public Transform[] GetP2NWESMax() { return P2nwesMax; }
    [Foldout("Phase 2 Spin")][SerializeField] private float p2sp_dist;
    public float GetP2Sp_Dist() { return p2sp_dist; }
    [Foldout("Phase 2 Spin")][SerializeField] private float P2rotationSpeed;
    public float GetP2RotationSpeed() { return P2rotationSpeed; }
    [Foldout("Phase 2 Spin")][SerializeField] private bool P2turningRight;
    public bool GetP2TurningRight() { return P2turningRight; }
    [Foldout("Phase 2 Spin")][SerializeField] private float P2maxWaitTime;
    public float GetP2MaxWaitTime() { return P2maxWaitTime; }
    [Foldout("Phase 2 Spin")][SerializeField] private float P2SpinLifeTime;
    public float GetP2SpinLifeTime() { return P2SpinLifeTime; }
    [Foldout("Phase 2 Spin")][SerializeField] private Transform[] p2bladesPreview;
    public Transform[] Getp2BladesPreview() { return p2bladesPreview; }
    #endregion
        #region Boomerang
    [Foldout("Phase 2 Boomerang"), SerializeField] private GameObject P2BoomerangboomerangL1;
    [Foldout("Phase 2 Boomerang"), SerializeField] private GameObject P2BoomerangboomerangL2;
    public GameObject GetP2BRL1() { return P2BoomerangboomerangL1; }
    public GameObject GetP2BRL2() { return P2BoomerangboomerangL2; }
    [Foldout("Phase 2 Boomerang")][SerializeField] private GameObject P2BoomerangR1;
    [Foldout("Phase 2 Boomerang")][SerializeField] private GameObject P2BoomerangR2;
    public GameObject GetP2BRR1() { return P2BoomerangR1; }
    public GameObject GetP2BRR2() { return P2BoomerangR2; }
    [Foldout("Phase 2 Boomerang")][SerializeField] AttackData p2boomerangAttackData;
    public AttackData Getp2BoomerangAttackData() { return p2boomerangAttackData; }

    [Foldout("Phase 2 Boomerang")][SerializeField] float P2BoomeRangSpeed;
    public float GetP2BoomeRangSpeed() { return P2BoomeRangSpeed; }
    [Foldout("Phase 2 Boomerang")][SerializeField] float p2br_MaxDist;
    public float GetP2BR_MaxDist() { return p2br_MaxDist; }
    [Foldout("Phase 2 Boomerang")][SerializeField] float P2BoomeRangMaxStraightTime;
    public float GetP2BoomeRangMaxStraightTime() { return P2BoomeRangMaxStraightTime; }
    [Foldout("Phase 2 Boomerang")][SerializeField] Transform P2BoomeRangcurveMidL1,P2BoomeRangcurveMidL2;
    public Transform GetP2BoomeRangcurveMidL1() { return P2BoomeRangcurveMidL1; }
    public Transform GetP2BoomeRangcurveMidL2() { return P2BoomeRangcurveMidL2; }
    [Foldout("Phase 2 Boomerang")][SerializeField] Transform P2BoomeRangcurveMidR1,P2BoomeRangcurveMidR2;
    public Transform GetP2BoomeRangcurveMidR1() { return P2BoomeRangcurveMidR1; }
    public Transform GetP2BoomeRangcurveMidR2() { return P2BoomeRangcurveMidR2; }
    [Foldout("Phase 2 Boomerang")][SerializeField] float P2BoomeRangMaxCurveTime;
    public float GetP2BoomeRangMaxCurveTime() { return P2BoomeRangMaxCurveTime; }
    [Foldout("Phase 2 Boomerang")][SerializeField] float p2Br_startup;
    public float GetP2BR_Startup() { return p2Br_startup; }
    #endregion
        #region Dash
    [Foldout("Phase 2 Dash"), SerializeField] Transform p2sDash_preview;
    public Transform GetP2sD_Preview() { return p2sDash_preview; }
    [Foldout("Phase 2 Dash"), SerializeField] float p2sDash_MaxDashTime;
    public float GetP2sD_MDashT() { return p2sDash_MaxDashTime; }
    [Foldout("Phase 2 Dash"), SerializeField] int p2sDash_MaxDashCount;
    public int GetP2sD_MDCount() { return p2sDash_MaxDashCount; }
    [Foldout("Phase 2 Dash"), SerializeField] float p2sDash_DashSpeed;
    public float GetP2sD_DashSpeed() { return p2sDash_DashSpeed; }
    [Foldout("Phase 2 Dash"), SerializeField] float p2sDash_MaxChargingTime;
    public float GetP2sD_ChargingTime() { return p2sDash_MaxChargingTime; }
    [Foldout("Phase 2 Dash"), SerializeField] float p2Dash_dashModifier;
    public float GetP2MD_dMod() { return p2Dash_dashModifier; }
    [Foldout("Phase 2 Dash"), SerializeField] AttackData p2Dash_AttackData;
    public AttackData GetP2DashAttackData() { return p2Dash_AttackData; }
    #endregion
        #region TelePortation
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
    #endregion
        #region Phase 1 Rosace
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
        #endregion
        #region Phase 2 Rosace
    [Foldout("Rosace"), SerializeField] int p2rosaceNumber;
    public int GetP2RosaceNumber(){return p2rosaceNumber;}    
    [Foldout("Rosace"), SerializeField] int p2bulletNumber;
    public int GetP2RosaceBulletNB(){return p2bulletNumber;}
    [Foldout("Rosace"), SerializeField] float p2rosaceDelay;
    public float GetP2RosaceDelay(){return p2rosaceDelay;}
        #endregion
    #endregion
    #region ProjectilesInit
    private List<GameObject> baseProjectiles = new List<GameObject>();
    public void AddProjectile()
    {
        GameObject go = Instantiate(agent.GetProjectile(), transform.position, transform.rotation);
        go.GetComponent<Projectiles>().SetTarget(agent.GetPlayer());
    }
    public GameObject GetProjectile()
    {
        return baseProjectiles[baseProjectiles.Count - 1];
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

    public void AddState(Danu_State state)
    {
        state.fsm = this;
        this.m_states[state.name] = state;
        state.Init();
    }
    public void RemoveState(Danu_State state)
    {
        m_states.Remove(state.name);
    }

    public void ChangeState(string nextStateName, float idleTime = 0f)
    {
        Danu_State state = null;
        this.m_states.TryGetValue(nextStateName, out state);
        if (state == null)
        {
            Debug.LogError($"[FSM] We don't have a state with the name {nextStateName}");
            return;
        }

        /*if (state == this.curr)
            return; // already in this state*/

        if (this.curr != null)
            this.curr.End();

        this.prev = curr;
        this.curr = state;
        this.curr.Begin();
        //Debug.Log($"[FSM] Started state {this.curr.name}");
    }

    void Update()
    {
        if (agent.isStun)
        {
            return;
        }
        if (this.curr != null)
        {
            this.curr.Update();
        }
    }

    public GameObject InstantiateStaticProjectile(Vector3 pos)
    {
        GameObject go = Instantiate(staticProj, pos, Quaternion.identity, P1globalGO.transform);
        return go;
    }
}
