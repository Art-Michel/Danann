using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Laser : MonoBehaviour
{
    
    List<Transform> children=new List<Transform>();
    [SerializeField]float range;

    // Start is called before the first frame update
    [Button]
    void Start()
    {
        children.Clear();
        float delta = 1f/transform.childCount;
        for (int i=0;i<transform.childCount;i++)
        {
            children.Add(transform.GetChild(i));
            float nX=Mathf.Lerp(0,range,delta*i);
            Debug.Log(nX+" : : "+delta+" : : "+i);
            children[i].localPosition=new Vector3(nX,0,0);
        }    

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
