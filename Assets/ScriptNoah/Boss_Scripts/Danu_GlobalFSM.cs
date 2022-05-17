using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Danu_GlobalFSM : MonoBehaviour
{
    private Dictionary<string, GlobalStates> m_states = new Dictionary<string, GlobalStates>();
    [HideInInspector] public DanuAI agent = null;
    
    
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
    
    [Expandable]
    public PhaseStats stats;
    public PhaseStats GetPhaseStats(){return stats;}
    public GlobalStates curr { get; private set; }
    public GlobalStates prev { get; private set; }
    [Button]
    public void QuickDebug()
    {

    }

    public void AddState( GlobalStates state )
    {
        state.gFSM = this;
        this.m_states[state.name] = state;
    }
    public void RemoveState(GlobalStates state)
    {
        m_states.Remove(state.name);
    }

    public void ChangeState( string nextStateName)
    {
        GlobalStates state = null;
        this.m_states.TryGetValue( nextStateName, out state );
        if( state == null )
        {
            Debug.LogError( $"[FSM] We don't have a state with the name {nextStateName}" );
            return;
        }

        if( state == this.curr )
            return; // already in this state

        if( this.curr != null ) 
            this.curr.End();

        this.prev = curr;
        this.curr = state;
        this.curr.Begin();
        Debug.Log( $"[FSM] Started state {this.curr.name}" );
    }

    void Update()
    {
        if (agent.isStun)
        {
            return;
        }
        if( this.curr != null ) 
        {
            this.curr.Update();
        }
    }
    /*public void SetTPDest(P1CTeleportation.destPoints newDest)
    {
        p1TP_destination=newDest;
    }*/

}
