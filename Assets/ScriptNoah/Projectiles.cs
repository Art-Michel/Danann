using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    Transform target;
    [SerializeField] float speed;
    [SerializeField] float turnRate;
    [SerializeField] float turnTime;
    [SerializeField] int nbTurn;
    float lifeTime;
    float dist;
    Vector3 end;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(target);
        end=target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position+=transform.forward*speed*Time.deltaTime;
        turnTime+=Time.deltaTime;
        lifeTime+=Time.deltaTime;
        if (turnTime>turnRate && nbTurn>0)
        {
            turnTime=0;
            Vector3 newTarget=Vector3.Lerp(transform.position+transform.forward,target.position,0.25f);
            transform.LookAt(newTarget);
            nbTurn--;
           

        }
        if (Vector3.Distance(transform.position,target.position)<=2.5f)
            nbTurn=0;
        if (lifeTime>2) Destroy(gameObject);
    }
    public void SetTarget(Transform newTarget)
    {
        target=newTarget;
    }
}
