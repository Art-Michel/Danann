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
    [SerializeField] Hurtbox bossHB;
    float baseRadius=2.68f;
    float shieldRadius=4.72f;
    [SerializeField] List<GameObject> shieldsBreak;
    public void PlayShieldBreak(int ind)
    {
        shieldsBreak[ind].SetActive(false);
        shieldsBreak[ind].SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        agent=GetComponent<DanuAI>();
        bh=GetComponent<BossHealth>();   
        bh.ActivateShield();
        agent.ActivateShield(); 
        bh.InvulnerableShieldingText();

        bossHB.SetRadius(shieldRadius);
    }
    public void ReActivate()
    {
        timer=0;
        shieldActive=true;
        bh.InvulnerableShieldingText();
        bossHB.SetRadius(shieldRadius);
    }
    // Update is called once per frame
    void Update()
    {
        if (shieldActive)
        {
            bossHB.SetRadius(shieldRadius);
            return;
        }
        timer+= Time.deltaTime;
        if (timer>=maxTimer)
        {
            shieldActive=true;
            timer=0;
            bh.ActivateShield();
            agent.ActivateShield(); 
        }
    }
    public void DesactivateShield()
    {
        bossHB.SetRadius(baseRadius);
        shieldActive=false;
    }
}
