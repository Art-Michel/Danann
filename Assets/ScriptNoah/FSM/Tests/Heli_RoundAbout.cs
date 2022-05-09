using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class Heli_RoundAbout : MonoBehaviour
{

    [SerializeField] private Transform dSphereN;
    [SerializeField] private Transform  dSphereSW;
    [SerializeField] private Transform  dSphereSE;
    [SerializeField] private GameObject Proj;
    public float Dist { get { return dist; } private set {  RepositionxDist(dist); }}
    //[SerializeField] private float radius= 0;
    [SerializeField] private float dist;
    [SerializeField]float rotationSpeed;
    [SerializeField]bool turningRight;
    Vector3 n;
    Vector3 w;
    Vector3 e;
    List<GameObject> nblades=new List<GameObject>();
    List<GameObject> wblades=new List<GameObject>();
    List<GameObject> eblades=new List<GameObject>();
    private bool wait;
    private float waitTime;
    [SerializeField]private float maxWaitTime;

    private void Start() {
        n=dSphereN.position;
        w=dSphereSW.position;
        e=dSphereSE.position;
        float delta = 1/dist;
        for (int i =0;i<dist;i++)
        {
            Vector3 nPos;
            Vector3 wPos;
            Vector3 ePos;
            nPos=Vector3.Lerp(transform.position,dSphereN.position,delta*i);
            wPos=Vector3.Lerp(transform.position,dSphereSW.position,delta*i);
            ePos=Vector3.Lerp(transform.position,dSphereSE.position,delta*i);
            nblades.Add(Instantiate(Proj,nPos,Quaternion.identity,transform));
            wblades.Add(Instantiate(Proj,wPos,Quaternion.identity,transform));
            eblades.Add(Instantiate(Proj,ePos,Quaternion.identity,transform));
            Debug.Log("e");
        }
        wait=true;
    }

    // Update is called once per frame
    void Update()
    {
        if (wait)
        {
            waitTime+=Time.deltaTime;
            if (waitTime>=maxWaitTime)
            {
                wait=false;
            }
            else
                return;
        }
        Rotate();
        

    }

    private void Rotate()
    {
        if (turningRight)
            transform.Rotate(new Vector3(0,rotationSpeed,0));
        else
            transform.Rotate(new Vector3(0,-rotationSpeed,0));    }

    /*[Button]
void Reposition()
{
   sphereN.localPosition=Vector3.Normalize(sphereN.localPosition);
   sphereSE.localPosition=Vector3.Normalize(sphereSE.localPosition);
   sphereSW.localPosition=Vector3.Normalize(sphereSW.localPosition);
}*/
    [Button]
    void RepositionxDist(float value=0)
    {
        
        value=dist;
        dSphereN.localPosition=Vector3.Normalize(dSphereN.localPosition)*value;
        dSphereSE.localPosition=Vector3.Normalize(dSphereSE.localPosition)*value;
        dSphereSW.localPosition=Vector3.Normalize(dSphereSW.localPosition)*value;
    }
    [Button]
    void CalculateDist()
    {
        Vector2 vN= new Vector2(dSphereN.localPosition.x,dSphereN.localPosition.z);
        Vector2 vSe= new Vector2(dSphereSE.localPosition.x,dSphereSE.localPosition.z);
        Vector2 vSw= new Vector2(dSphereSW.localPosition.x,dSphereSW.localPosition.z);
        float nToSW=Vector2.Angle(vN,vSw);
        float nToSE=Vector2.Angle(vN,vSe);
        float sWToSE=Vector2.Angle(vSw,vSe);
        Debug.Log("dist between North and SouthWest : "+nToSW+
        "\n dist between North and SouthEast : "+nToSE+
        "\n dist between SouthWest and SouthEast : "+sWToSE);
       //Debug.Log(Vector3.Angle(Vector3.forward,Vector3.left));
    }
    private void OnDisable() {
        dSphereN.position=n;
        dSphereSW.position=w;
        dSphereSE.position=e;
        float delta=1/dist;
        for (int i=0;i<nblades.Count;i++)
        {
            nblades[i].SetActive(false);
            nblades[i].transform.position=Vector3.Lerp(transform.position,n,delta*i);
            wblades[i].SetActive(false);
            wblades[i].transform.position=Vector3.Lerp(transform.position,w,delta*i);
            eblades[i].SetActive(false);
            eblades[i].transform.position=Vector3.Lerp(transform.position,e,delta*i);
        }
    }
    private void OnEnable() {
        wait=true;
        waitTime=0;
        for (int i=0;i<nblades.Count;i++)
        {
            nblades[i].SetActive(true);
            wblades[i].SetActive(true);
            eblades[i].SetActive(true);
        }
    }
}
