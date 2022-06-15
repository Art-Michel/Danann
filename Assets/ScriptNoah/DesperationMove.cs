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
    public Transform GetP1sD_Preview() { return p1sDash_preview; }
    public Pool GetPool() { return pool; }
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
    State curr;
    // Start is called before the first frame update
    void Start()
    {
        states.Add(new DM_Shoot());

    }

    // Update is called once per frame
    void Update()
    {
        states[index].Update();
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
