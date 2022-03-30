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
    
    private void Awake() {
        if (m_fsm==null)
            m_fsm=GetComponent<FSM>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    [Button]
    void NextPattern() 
    {
        dist = Vector3.Distance(transform.position, player.position);
        Debug.Log(dist);
        Debug.Log(distEvaluator.Evaluate(dist));
        float mod = distEvaluator.Evaluate(dist);
        if (mod < 0.9f) //short ranged patterns
        {
            Debug.Log("smol");
        }
        else if (mod > 1.3f) //long ranged patterns
        {
            Debug.Log("loooong");

        }
        else //all patterns possible
        {
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
}
