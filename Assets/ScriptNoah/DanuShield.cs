using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanuShield : MonoBehaviour
{
    float timer;
    [SerializeField]float maxTimer;
    bool shieldActive;
    BossHealth bh;
    DanuAI agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent=GetComponent<DanuAI>();
        bh=GetComponent<BossHealth>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldActive)
            return;
        timer+= Time.deltaTime;
        if (timer>=maxTimer)
        {
            shieldActive=true;
            bh.ActivateShield();
        }
    }
}
