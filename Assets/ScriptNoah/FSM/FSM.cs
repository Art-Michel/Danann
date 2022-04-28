using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class FSM : MonoBehaviour
{
    private Dictionary<string, State> m_states = new Dictionary<string, State>();

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
        [Foldout("Phase 1 Slam")][SerializeField]GameObject boombox;
        public GameObject GetBoomBox(){return boombox;}
        [Foldout("Phase 1 Slam")] [SerializeField] Vector3[] boomFrames=new Vector3[3];
        public Vector3 GetAttackFrames(int index){return boomFrames[index];}
        [Foldout("Phase 1 Slam")][SerializeField] private float P1C_BoomWait;
        public float GetP1C_BoomWait(){return P1C_BoomWait;}
        [Foldout("Phase 1 Slam")][SerializeField] private Vector3[] slamScales=new Vector3[3];
        #endregion
        #region Spin
        [Foldout("Phase 1 Spin")][SerializeField] private GameObject staticProj;
        public GameObject GetStaticProj(){  return staticProj;  }
        [Foldout("Phase 1 Spin")][SerializeField] private GameObject globalGO;
        public GameObject GetGlobalGO(){return globalGO;}
        [Foldout("Phase 1 Spin")][SerializeField] private Transform[] nweMax=new Transform[3];
        public Transform[] GetNWEMax(){return nweMax;}
        [Foldout("Phase 1 Spin")][SerializeField] private float rotationSpeed;
        public float GetRotationSpeed(){return rotationSpeed;}
        [Foldout("Phase 1 Spin")][SerializeField] private bool turningRight;
        public bool GetTurningRight(){return turningRight;}
        [Foldout("Phase 1 Spin")][SerializeField] private float maxWaitTime;
        public float GetMaxWaitTime(){return maxWaitTime;}
        [Foldout("Phase 1 Spin")][SerializeField] private float SpinLifeTime;
        public float GetSpinLifeTime(){return SpinLifeTime;}
        #endregion
    #endregion
    int indexx;
    public State curr { get; private set; }
    public State prev { get; private set; }

    public void AddState( State state )
    {
        state.fsm = this;
        this.m_states[state.name] = state;
    }
    public void RemoveState(State state)
    {
        m_states.Remove(state.name);
    }

    public void ChangeState( string nextStateName, float idleTime=0f )
    {
        State state = null;
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
    public Vector3 GetSlamScale(int index)
    {
        return slamScales[index];
    }
    public GameObject InstantiateStaticProjectile(Vector3 pos)
    {
        return Instantiate(staticProj,pos,Quaternion.identity,globalGO.transform);
    }


}
