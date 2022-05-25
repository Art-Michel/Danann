using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjSpirale : MonoBehaviour
{
    Pool pool;
    [SerializeField] int nbBullet;
    [SerializeField] bool goLeft;
    [SerializeField] int index;
    float radius;
    [SerializeField] int angle;
    [SerializeField] float maxDelay;
    float delay;
    // Start is called before the first frame update
    void Start()
    {
        pool=GetComponent<Pool>();
    }

    // Update is called once per frame
    void Update()
    {
        delay+=Time.deltaTime;
        if (delay>maxDelay && index<=nbBullet)
        {
            delay=0;
            GameObject go= pool.Get();
            go.SetActive(true);
            go.GetComponent<Projectiles>().SetOrigin(pool);
            Vector2 pos=new Vector2(go.transform.position.x,go.transform.position.z);
            float rad= angle*Mathf.Deg2Rad;
            pos=new Vector2(Mathf.Cos(rad*index),Mathf.Sin( rad*index));
            if (goLeft)
            go.transform.Rotate(0,angle*index*-1,0);
            else
            go.transform.Rotate(0,angle*index,0);
            //go.transform.position=transform.position+new Vector3(pos.x,transform.position.y,pos.y);
            index++;
        }
    }
}
