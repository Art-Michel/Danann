using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_ShockWave : MonoBehaviour
{
    Hitbox me=new Hitbox();
    [SerializeField] float expansionRate;
    [SerializeField] float expansionLimits;
    SphereCollider sphere;
    [SerializeField] bool isVFX;
    // Start is called before the first frame update
    void Start()
    {
        if (!isVFX)
        sphere=GetComponent<SphereCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isVFX && transform.localScale.x<=expansionLimits)
        {
            float x=expansionRate*Time.deltaTime;
            Vector3 nScale=new Vector3(x,x,0);
            transform.localScale+=nScale;
        }
        if (transform.localScale.x<=expansionLimits)
        {
            float x=expansionRate*Time.deltaTime;
            sphere.radius+=x;
        }
    }
    private void OnDisable() {
        transform.localScale=Vector3.one;
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("e");
        Spear_FSM spear=other.GetComponent<Spear_FSM>();
        
        if (spear==null)
        {
            Debug.Log("noSpear :" + other.name);
            return;
        }
        SpearAI ai=other.GetComponent<SpearAI>();
        Debug.Log("isSpear : " + other.name);
        bool cond=spear.currentState.Name!=Spear_StateNames.ATTACHED ;
        cond=cond && spear.currentState.Name!=Spear_StateNames.TRIANGLING ;
        if (!cond)
        {
            Debug.Log("not thrown");
            return;
        }
            Debug.Log("willBeReturned");
            spear.ChangeState(Spear_StateNames.RECALLED);
    }
}
