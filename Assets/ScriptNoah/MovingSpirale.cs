using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class MovingSpirale : MonoBehaviour
{
    Pool pool;
    [SerializeField] int nbBullet;
    [SerializeField] bool goLeft;
    [SerializeField] bool shootLeft;
    
    int index;
    float radius=19.5f;
    [SerializeField] int angle;
    [SerializeField] float maxDelay;
    float delay;
    private float moveIndex;
    [SerializeField] Vector3 arenaCenter;
    [SerializeField,MinMaxSlider(-180,180)] public Vector2 maxAngle;
    [SerializeField] float speed;
    Vector3 projdir;
    [SerializeField] float sinIndex;
    // Start is called before the first frame update
    void Start()
    {
        pool=transform.parent.GetComponent<Pool>();
        Vector3 dir=(-arenaCenter+transform.position).normalized;
        Debug.Log(transform.position+":"+radius+":"+dir);
        transform.position=arenaCenter;
        dir*=radius;
        transform.position+=dir;
        Debug.Log(Vector3.Distance(transform.position,arenaCenter));
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        //Move();
    }

    private void Move()
    {
            Vector2 pos;
            float rad= angle*Mathf.Deg2Rad*speed;
            if (goLeft)
            {
                pos=new Vector2(Mathf.Cos(rad*-moveIndex),Mathf.Sin( rad*-moveIndex))*radius;

            }
            else
            {
                pos=new Vector2(Mathf.Cos(rad*moveIndex),Mathf.Sin( rad*moveIndex))*radius;

            }

            transform.position=arenaCenter + new Vector3(pos.x,transform.position.y,pos.y);
    }

    void Shoot()
    {
        delay+=Time.deltaTime;
        if (delay>maxDelay && sinIndex<=nbBullet)
        {
            sinIndex++;
            delay=0;

            GameObject go= pool.Get();
            go.transform.position=transform.position;
            go.SetActive(true);
            go.GetComponent<Projectiles>().SetOrigin(pool);

            Vector2 pos=new Vector2(go.transform.position.x,go.transform.position.z);
            float rad= angle*Mathf.Deg2Rad;
            pos=new Vector2(Mathf.Cos(rad*index),Mathf.Sin( rad*index));
            go.transform.forward=arenaCenter-transform.position;
            go.transform.Rotate(0,angle,0);
            if (shootLeft)
            {
                Vector3 baseDir=arenaCenter-transform.position;
                baseDir = Quaternion.Euler(0, angle, 0) * baseDir;
                go.transform.Rotate(0,angle*index,0);
                Vector3 projAngle=go.transform.forward;
                    index--;
                if (index<=maxAngle.x)
                {
                    Debug.Log(Vector3.Angle(baseDir,projAngle));
                    shootLeft=false;
                }
                projdir=projAngle;
            }
            else
            {
                Vector3 baseDir=arenaCenter-transform.position;
                baseDir = Quaternion.Euler(0, angle/2, 0) * baseDir;
                go.transform.Rotate(0,angle*index,0);
                Vector3 projAngle=go.transform.forward;            
                    index++;
                if (index>=maxAngle.y)
                {
                    Debug.Log(Vector3.Angle(baseDir,projAngle));
                    //go.transform.Rotate(0,maxAngle.y,0);
                    shootLeft=true;
                }
                projdir=projAngle;

            }
            //go.transform.position=transform.position+new Vector3(pos.x,transform.position.y,pos.y);
            moveIndex++;
        }
    }
}
