using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class MovingSpirale : MonoBehaviour
{
    Pool pool;
    [SerializeField] int nbBullet;
    public void SetBullets(int newnb){nbBullet=newnb;}
    [SerializeField] bool goLeft;
    [SerializeField] bool shootLeft;
    
    int index;
    float radius=27.5f;
    [SerializeField] int angle;
    [SerializeField] float maxDelay;
    public void SetDelay(float newMax ){maxDelay=newMax;}
    float delay;
    private float moveIndex;
    [SerializeField] Vector3 arenaCenter;
    [SerializeField,MinMaxSlider(-180,180)] Vector2 maxAngle;
    [SerializeField] float speed;
    Vector3 projdir;
    [SerializeField] float sinIndex;
    public float lifetime{get;private set;}
    List<GameObject> activeBullets=new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        pool=transform.parent.GetComponent<Pool>();
        Vector3 dir=(-arenaCenter+transform.position).normalized;
        transform.position=arenaCenter;
        dir*=radius;
        transform.position+=dir;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        //Move();
    }
    private void OnDisable() {
        sinIndex=0;
        delay=0;
        for (int i=activeBullets.Count-1;i>=0;i--)
        {
            pool.Back(activeBullets[i]);
            activeBullets[i].SetActive(false);
            activeBullets.RemoveAt(i);

        }
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
            activeBullets.Add(go);
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
