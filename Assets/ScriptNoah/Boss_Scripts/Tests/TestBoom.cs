using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoom : MonoBehaviour
{
    [SerializeField]GameObject boombox;
    [SerializeField] Transform player;
    [SerializeField] float startup;
    [SerializeField]float active;
    [SerializeField]float end;
    float timer;
    [SerializeField] float delay;
    float delta;
    int index=0;

    // Start is called before the first frame update
    void Start()
    {
        LookTowardPlayer();
        
    }

    private void LookTowardPlayer()
    {
        transform.LookAt(player);
        boombox.transform.rotation=transform.rotation;
        boombox.transform.position=transform.position+transform.forward*boombox.transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        Slam();
    }

    

    private void Slam()
    {
       delta+=Time.deltaTime;
        if (delta>delay)
        {

            switch(index)
            {
                case 0:
                    timer+=Time.deltaTime;
                    if (timer>startup)
                    {
                        index++;
                        timer=0;
                        ActivateHitBox();
                    }
                    else if (timer>startup/2)
                    {
                        LookTowardPlayer();

                    }
                    break;
                case 1:
                    timer+=Time.deltaTime;
                    if (timer>active)
                    {
                        index++;
                        timer=0;
                        DesactivateHitBox();
                    }
                    break;
                case 2:
                    timer+=Time.deltaTime;
                    if (timer>end)
                    {
                        index++;
                        timer=0;
                        ToIdle();
                    }
                    break;
                    default:
                        break;
            }
        }
        else
            LookTowardPlayer();
    }

    private void ToIdle()
    {
        Debug.Log("End");
    }

    private void DesactivateHitBox()
    {
        boombox.SetActive(false);
    }

    private void ActivateHitBox()
    {
        boombox.SetActive(true);
    }
}
