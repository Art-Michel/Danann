using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Replace : MonoBehaviour
{
    float radius=0;
    [SerializeField]float maxRadius;

    [SerializeField] int angle;
    List<Transform> children=new List<Transform>();
    [SerializeField] float expansionRate;
    bool isplaying=false;

    // Start is called before the first frame update
    void Start()
    {
        isplaying=true;
        for (int i=0;i<transform.childCount;i++)
        {
            if (transform.GetChild(i).GetComponent<Hitbox>())
            children.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (maxRadius>=radius)
            radius+=Time.deltaTime*expansionRate;
        else
            gameObject.SetActive(false);
        Place();
        /*
        if (radius!=oldRadius)
            Place();
        oldRadius=radius;
        */
    }
    [Button]
    void Place()
    {
        /*if (!isplaying)
        {
            for (int i=0;i<transform.childCount;i++)
            {
                children.Add(transform.GetChild(i));
            }
        }
        }*/
        int index=0;
        foreach(Transform tr in children)
        {
            Vector2 pos;
            float rad= angle*Mathf.Deg2Rad;
            pos=new Vector2(Mathf.Cos(rad*index),Mathf.Sin( rad*index))*radius;
            tr.position=transform.position+new Vector3(pos.x,tr.position.y,pos.y);
            index++;
        }
    }   
}
