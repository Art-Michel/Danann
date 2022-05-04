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
        [Foldout("Phase 1 Shoot")] [SerializeField] private int P1D_nbShot;
        public int GetP1d_nbShot(){return P1D_nbShot;}
        [Foldout("Phase 1 Shoot")][SerializeField] private float P1D_delay;
        public float GetP1d_delay(){return P1D_delay;}
        [Foldout("Phase 1 Shoot")][SerializeField] private float P1D_wait;
        public float GetP1d_wait(){return P1D_wait;}
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
        #endregion
        #region Spin
        [Foldout("Phase 1 Spin")][SerializeField] private GameObject staticProj;
        public GameObject GetStaticProj(){  return staticProj;  }
        [Foldout("Phase 1 Spin")][SerializeField] private GameObject P1globalGO;
        public GameObject GetP1GlobalGO(){return P1globalGO;}
        [Foldout("Phase 1 Spin")][SerializeField] private Transform[] P1nweMax=new Transform[3];
        public Transform[] GetP1NWEMax(){return P1nweMax;}
        [Foldout("Phase 1 Spin")][SerializeField] private float P1rotationSpeed;
        public float GetP1RotationSpeed(){return P1rotationSpeed;}
        [Foldout("Phase 1 Spin")][SerializeField] private bool P1turningRight;
        public bool GetP1TurningRight(){return P1turningRight;}
        [Foldout("Phase 1 Spin")][SerializeField] private float P1maxWaitTime;
        public float GetP1MaxWaitTime(){return P1maxWaitTime;}
        [Foldout("Phase 1 Spin")][SerializeField] private float P1SpinLifeTime;
        public float GetP1SpinLifeTime(){return P1SpinLifeTime;}
        #endregion
        #region Boomerang
        [Foldout("Phase 1 Boomerang")][SerializeField] private GameObject P1BoomeRangboomerangL;
        public GameObject GetP1BoomeRangboomerangL(){return P1BoomeRangboomerangL;}
        [Foldout("Phase 1 Boomerang")][SerializeField] private GameObject P1BoomeRangboomerangR;
        public GameObject GetP1BoomeRangboomerangR(){return P1BoomeRangboomerangR;}

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
        #endregion
    #endregion
    int indexx;
    public Danu_State curr { get; private set; }
    public Danu_State prev { get; private set; }

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
        if (state.name==StateNames.P1IDLE)
        this.prev = curr;
        this.curr = state;
        this.curr.Begin();
        Debug.Log( $"[FSM] Started state {this.curr.name}" );
    }

    void Update()
    {
        if( this.curr != null ) {
            this.curr.Update();
        }
    }
    
    public GameObject InstantiateStaticProjectile(Vector3 pos)
    {
        return Instantiate(staticProj,pos,Quaternion.identity,P1globalGO.transform);
    }


}
