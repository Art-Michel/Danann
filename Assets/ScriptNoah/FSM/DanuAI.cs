using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DanuAI : MonoBehaviour
{
    private Danu_FSM m_fsm=null;
    [Range(1,2),SerializeField]private int phase;
    private int patternIndex;
    private float dist;
    [SerializeField] private Transform player;
    [SerializeField] private AnimationCurve distEvaluator;
    [SerializeField] private float revenge;

    public float GetMovementRange()
    {
        return movementRange;
    }

    [SerializeField] private float waitingTime;
    [SerializeField] private float arenaRadius;
    [SerializeField] private Vector3 arenaCenter;
    [SerializeField] private float movementRange;
    [SerializeField] private float maxChain;
    float chain;
    [SerializeField] private GameObject projectile;
    private bool isPushed;
    private float hp;
    private float maxHP;
    [SerializeField] bool goingRandom;
    private void Awake() {
        if (m_fsm==null)
            m_fsm=GetComponent<Danu_FSM>();
        m_fsm.agent=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_fsm.AddState(new P1DShoot());
        m_fsm.AddState(new P1Idle());
        m_fsm.AddState(new P1CSlam());
        m_fsm.AddState(new P1DHelicopter());
        m_fsm.AddState(new P1DBoomerang());
        m_fsm.AddState(new P1CDash());
        m_fsm.AddState(new P1CTeleportation());
        m_fsm.AddState(new P1CMixDash());
        this.m_fsm.ChangeState( StateNames.P1C_SLAM);
    }

    // Update is called once per frame
    void Update()
    {
        float nx=Mathf.Clamp(transform.position.x,arenaCenter.x-arenaRadius,arenaCenter.x+arenaRadius);
        float nz=Mathf.Clamp(transform.position.z,arenaCenter.z-arenaRadius,arenaCenter.z+arenaRadius);
        transform.position=new Vector3(nx,arenaCenter.y,nz);
    }
    [Button]
    public void NextPattern() 
    {
        if (goingRandom)
        {
            int chance=Random.Range(1,8);
            for (int i=1;i<8;i++)
                switch (chance)
                {
                    case 1:
                        m_fsm.ChangeState(StateNames.P1C_DASH);
                        break;
                    case 2:
                        m_fsm.ChangeState(StateNames.P1C_SLAM);
                        break;
                    case 3:
                        m_fsm.ChangeState(StateNames.P1C_TELEPORTATION);
                        break;
                    case 4:
                        m_fsm.ChangeState(StateNames.P1D_BOOMERANG);
                        break;
                    case 5:
                        m_fsm.ChangeState(StateNames.P1D_SHOOT);
                        break;
                    case 6:
                        m_fsm.ChangeState(StateNames.P1D_SPIN);
                        break;                    
                    case 7:
                        m_fsm.ChangeState(StateNames.P1C_MIXDASH);
                        break;
                }
                return;
        }
        else
        {
            m_fsm.ChangeState(StateNames.P1C_MIXDASH);
            if (!goingRandom)
                return;
        }
        dist = Vector3.Distance(transform.position, player.position);
        Debug.Log(dist);
        Debug.Log(distEvaluator.Evaluate(dist));
        float mod = distEvaluator.Evaluate(dist);
        if (mod <= 1.1f) //short ranged patterns
        {
            if (m_fsm.prev.name==StateNames.P1C_TELEPORTATION)
            {
                m_fsm.ChangeState(StateNames.P1C_DASH);
                Debug.Log("smol");
            }
        }
        else if (mod > 1.1f) //long ranged patterns
        {
                        m_fsm.ChangeState(StateNames.P1D_SHOOT);

            Debug.Log("loooong");

        }
        else //all patterns possible
        {
            m_fsm.ChangeState(StateNames.P1D_SPIN);
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
    {/*
        chain=Mathf.Clamp(chain--,0,maxChain);
        if (phase==1)
        {
            if (chain==0)
                m_fsm.ChangeState(StateNames.P1IDLE);
            else
                NextPattern();
        }
        else if (phase==2)
            m_fsm.ChangeState(StateNames.P2IDLE);
    */
         m_fsm.ChangeState(StateNames.P1IDLE,5);
    }
    public float GetWaitingTime()
    {
        return waitingTime;
    }
    public void SetWaitingTime(float newWT)
    {
        waitingTime=newWT;
    }
    public void RestoreChain(){
        chain=maxChain;
    }
    public Vector3 GetArenaCenter(){return arenaCenter;}
    public float GetArenaRadius(){return arenaRadius;}
    public bool GetIsPushed(){return isPushed;}
}
