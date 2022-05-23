using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DASHSHSH : MonoBehaviour
{
[SerializeField] Transform target;
    [SerializeField] Transform preview;
    [SerializeField] float maxDashTime;
    [SerializeField] int dashCount;
    [SerializeField] int maxDashCount;
    [SerializeField] float dashSpeed;
    [SerializeField] float maxChargingTime;
    [SerializeField] AttackData dashAttackData;
    Vector3 dir;
    float dashTime;
    float chargingTime;
    bool isDashing;
    Vector3 maxArrival;
    private Vector3 startPos;
    // Start is called before the first frame update
    private void Start() 
    {            
        dashCount = maxDashCount;
        StartDash();
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
        if (dashCount == 0)
        {
            return;
        }
        if (chargingTime <= maxChargingTime)
        {
            preview.gameObject.SetActive(true);
            chargingTime += Time.deltaTime;
            //Vector3 arrival= transform.position+dir*dashSpeed*maxDashTime ;
            //arrival=new Vector3(arrival.x,3.72f,arrival.z);

            return;
        }
        dashTime += Time.deltaTime;
        transform.position += dir * dashSpeed * Time.deltaTime;
        if (dashTime >= maxDashTime)
        {
            preview.gameObject.SetActive(false);
            dashCount--;
            dashTime = 0;
            dir = (-transform.position + target.position).normalized;
            if (dashCount==0)
            {
            Debug.Log("over");
            enabled=false;
            return;
            }

        }

    }
    void StartDash()
    {
        dashTime = 0;
        chargingTime = 0;
        isDashing = true;
        dir = (-transform.position + target.position).normalized;
        startPos = transform.position;
        maxArrival = transform.position + dir * dashSpeed * dashTime;
        preview.position = startPos + (dir * dashSpeed * maxDashTime) / 2;
        preview.LookAt(target);
        preview.localScale = new Vector3(transform.localScale.x, transform.localScale.y, maxDashTime * dashSpeed);
        dashAttackData.LaunchAttack();
    }
}
