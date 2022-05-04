using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class SimpleDashTest : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 dir;
    float dashTime;
    [SerializeField]Transform preview;
    [SerializeField] float maxDashTime;
    [SerializeField] int dashCount;
    [SerializeField] int maxDashCount;
    [SerializeField] float dashSpeed;
    [SerializeField]float maxChargingTime;
    float chargingTime;
    bool isDashing;
    Vector3 maxArrival;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        StartDash();
        Debug.Log(transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        Dash();
    }

    private void Dash()
    {
        if (!isDashing)
        {
            return;
        }
        if (dashCount==0)
        {
            return;
        }
        if (chargingTime<=maxChargingTime)
        {
            preview.gameObject.SetActive(true);
            chargingTime+=Time.deltaTime;
            //Vector3 arrival= transform.position+dir*dashSpeed*maxDashTime ;
            //arrival=new Vector3(arrival.x,3.72f,arrival.z);
            preview.position=startPos+(dir*dashSpeed*maxDashTime)/2;
            preview.LookAt(target);
            preview.localScale=new Vector3(1,1,maxDashTime*dashSpeed);
            return;
        }
        dashTime+=Time.deltaTime;
        transform.position+=dir*dashSpeed*Time.deltaTime;
        if (dashTime>=maxDashTime)
        {
            preview.gameObject.SetActive(false);
            dashCount--;
            dashTime=0;
            dir=(-transform.position+target.position).normalized;
            if (dashCount==0)
                isDashing=false;
        }
        Debug.Log(transform.position);
    }

    [Button]
    void StartDash()
    {
        dashCount=maxDashCount;
        dashTime=0;
        chargingTime=0;
        isDashing=true;
        dir=(-transform.position+target.position).normalized;
        startPos=transform.position;
        maxArrival=transform.position+dir*dashSpeed*dashTime;
        
    }
    [Button]
    void Math()
    {
        /*Vector2 ene=new Vector2(32.2f,50.7f);
        Debug.Log(ene.normalized);
        Debug.Log(dir);*/
        Debug.Log(maxArrival);
        Debug.Log(transform.position);
    }
}
