using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class FSM : MonoBehaviour
{
    private Dictionary<string, State> m_states = new Dictionary<string, State>();

    [HideInInspector] public DanuAI agent = null;
    [Foldout("Phase 1 Distance")]
    [SerializeField] private int p1d_nbShot;
    public int GetP1d_nbShot(){return p1d_nbShot;}
    [SerializeField] private float p1d_delay;
    public float GetP1d_delay(){return p1d_delay;}
    
    [Foldout("Phase 1 Close Combat")][SerializeField]GameObject boombox;
    public GameObject GetBoomBox(){return boombox;}
    [Foldout("Phase 1 Close Combat")] [SerializeField] Vector3 frames;
    public Vector3 AttackFrames(){return frames;}
    [Foldout("Phase 2")]
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
}
