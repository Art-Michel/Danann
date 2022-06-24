using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRise : MonoBehaviour
{
    [SerializeField] Vector3 destPos;
    [SerializeField] Vector3 startPos;
    [SerializeField] AnimationCurve speed;
    [SerializeField] float startup;
    float t;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnEnable() 
    {
        t=0;
        startPos=transform.position;
        destPos=new Vector3(transform.position.x,destPos.y,transform.position.z);
    }
    private void OnDisable() 
    {
        transform.position=startPos;
        t=0;
    }
    // Update is called once per frame
    void Update()
    {
        t+=Time.deltaTime;
        Vector3 newPos=Vector3.Lerp(startPos,destPos, speed.Evaluate(t/startup));
        transform.position=newPos;
        transform.localPosition=new Vector3(0,transform.localPosition.y,0);
    }
}
