using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rosace : MonoBehaviour
{
    [SerializeField] GameObject spin;
    [SerializeField] int nb;
    [SerializeField] float arenaRadius;
    [SerializeField] Vector3 arenaCenter;
    [SerializeField] GameObject proj;
    // Start is called before the first frame update
    void Start()
    {
        int delta =360/nb;
        for (int i=0;i<nb;i++)
        {
            Vector3 pos = arenaCenter;
            Debug.Log(pos);
            float rad= delta*Mathf.Deg2Rad;
            Vector3 dest=new Vector3(Mathf.Cos(rad*i),0,Mathf.Sin(rad*i)).normalized;
            Debug.Log(dest);
            pos+=dest*arenaRadius;
            GameObject go= Instantiate(spin,pos,Quaternion.identity);
            go.GetComponent<Pool>().SetUp(null,null,proj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
