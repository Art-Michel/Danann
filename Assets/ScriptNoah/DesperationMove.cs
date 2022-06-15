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
    State curr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(curr)
        {
            case State.SHOOT :
                ShootUpdate();
                break;
            case State.DOUBLETP :
                DoubleTpUpdate();
                break;                
            case State.ROSACE :
                RosaceUpdate();
                break;                
            case State.TP :
                TPUpdate();
                break;                
            case State.SLAM :
                SlamUpdate();
                break;                
            case State.LASTTP :
                LastTPUpdate();
                break;                
            case State.LASER :
                LaserUpdate();
                break;                
        }
    }

    private void LaserUpdate()
    {
        throw new NotImplementedException();
    }

    private void LastTPUpdate()
    {
        throw new NotImplementedException();
    }

    private void SlamUpdate()
    {
        throw new NotImplementedException();
    }

    private void TPUpdate()
    {
        throw new NotImplementedException();
    }

    private void RosaceUpdate()
    {
        throw new NotImplementedException();
    }

    private void DoubleTpUpdate()
    {
        throw new NotImplementedException();
    }

    private void ShootUpdate()
    {
        throw new NotImplementedException();
    }
}
