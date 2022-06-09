using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class Laser : MonoBehaviour
{
    
    [SerializeField]List<Transform> children=new List<Transform>();
    [SerializeField]float maxRange;
    [SerializeField] float range;
    [SerializeField] GameObject laserTip;
    float t;
    [SerializeField] float speed;
    private float size;
    float delay;
    [SerializeField] GameObject gather;
    [SerializeField] GameObject gather1;
    [SerializeField] AttackData aData;
    [SerializeField] float lifetime;
    [SerializeField] List<ParticleSystem> vfx;
    // Start is called before the first frame update
    [Button]
    void Start()
    {
        var main = this.GetComponent<ParticleSystem>().main;
        delay=main.startDelay.constant;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (delay>=0)
        {
            Delay();
            return;
        }
        MoveTip();
        Live();
        
    }
    void Delay()
    {
            delay-=Time.deltaTime;
            if (delay<=0)
            {
                gather.SetActive(false);
                gather1.SetActive(false);
                aData.LaunchAttack();
            }
    }
    void Live()
    {
        lifetime-=Time.deltaTime;
        if (lifetime<=0)
        {
            aData.StopAttack();
            foreach(ParticleSystem ps in vfx)
            {
                ps.Stop();
            }
        }
    }

    private void ExtendHitbox()
    {
        float delta = 1f/children.Count;
        for (int i=0;i<children.Count;i++)
        {
            float nX=Mathf.Lerp(0,range*2.8f,delta*i);
            children[i].localPosition=new Vector3(0,0,nX);
        }  
    }

    void MoveTip()
    {
        if (laserTip.transform.localPosition.x<=maxRange)
        {
            t+=Time.deltaTime*speed;
            Debug.Log(t);
            float nX=Mathf.Lerp(0,maxRange,t);
            range=nX;
            Vector3 nPos=new Vector3(nX,0,0);
            laserTip.transform.localPosition=nPos;
            ExtendHitbox();
        }
    }
}
