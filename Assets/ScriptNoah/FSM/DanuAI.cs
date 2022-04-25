using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class DanuAI : MonoBehaviour
{
    private FSM m_fsm=null;
    [Range(1,2),SerializeField]private int phase;
    private int patternIndex;
    private float dist;
    [SerializeField] private Transform player;
    [SerializeField] private AnimationCurve distEvaluator;
    [SerializeField] private float revenge;
    [SerializeField] private float waitingTime;
    [SerializeField] private GameObject projectile;
    
    private void Awake() {
        if (m_fsm==null)
            m_fsm=GetComponent<FSM>();
        m_fsm.agent=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_fsm.AddState(new P1DShoot());
        m_fsm.AddState(new P1Idle());
        this.m_fsm.ChangeState( StateNames.P1IDLE);

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    [Button]
    public void NextPattern() 
    {
        dist = Vector3.Distance(transform.position, player.position);
        Debug.Log(dist);
        Debug.Log(distEvaluator.Evaluate(dist));
        float mod = distEvaluator.Evaluate(dist);
        if (mod < 0.9f) //short ranged patterns
        {
            m_fsm.ChangeState(StateNames.P1D_SHOOT);
            Debug.Log("smol");
        }
        else if (mod > 1.3f) //long ranged patterns
        {
                        m_fsm.ChangeState(StateNames.P1D_SHOOT);

            Debug.Log("loooong");

        }
        else //all patterns possible
        {
            m_fsm.ChangeState(StateNames.P1D_SHOOT);
            Debug.Log("midwest");

        }
    }
    public void NextPhase()
    {
        if (phase != 1)
            return;
        phase++;
        //Add all P2 states and remove all P1 states
        NextPattern();
        
    }
    public int GetPhase() { return phase; }
    public Transform GetPlayer(){return player;}
    public GameObject GetProjectile(){return projectile;}
    public void SpawnProjectile()
    {
        Instantiate(projectile,transform.position,transform.rotation).GetComponent<Projectiles>().SetTarget(GetPlayer());
    }
    public void ToIdle()
    {
        m_fsm.ChangeState(StateNames.P1IDLE);
    }
    public float GetWaitingTime()
    {
        return waitingTime;
    }
    public void SetWaitingTime(float newWT)
    {
        waitingTime=newWT;
    }
}
