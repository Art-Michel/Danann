using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class TpOnPlayer : MonoBehaviour
{
    [SerializeField]GameObject arrival;
    [SerializeField]GameObject boomBox;
    [SerializeField]Transform target;
    enum destPoints
    {
        FAR,
        CLOSE
    }
    [SerializeField]destPoints destination;
    float fadeTime;
    [SerializeField]float MaxFadeTime;
    float startup;
    [SerializeField]float MaxSartup;
    [SerializeField]float offsetValue;
    float reco;
    [SerializeField]float maxReco;
    float active;
    [SerializeField]float maxActive;
    [SerializeField] float farDist;
    // Start is called before the first frame update
    void Start()
    {
        arrival.SetActive(false);
        if (destination==destPoints.FAR)
        {
            float dist=Vector3.Distance(transform.position,target.position)*farDist;
            Vector3 dir=transform.position-target.position;
            dir.Normalize();
            dir*=dist;
            arrival.transform.position=transform.position+dir;
        }
        else
        {
            Vector2 rand=Random.insideUnitCircle;
            Vector3 offset=new Vector3(rand.x,0,rand.y)*offsetValue;
            arrival.transform.position=target.position+offset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (startup<=MaxSartup)
        {
            startup+=Time.deltaTime;
        }
        else if (fadeTime<=MaxFadeTime)
        {
            arrival.SetActive(true);
            fadeTime+=Time.deltaTime;
        }
        else if (active<=maxActive)
        {

            boomBox.SetActive(true);
            transform.position=arrival.transform.position;
            active+=Time.deltaTime;
        }
        else if (reco<=maxReco)
        {
            boomBox.SetActive(false);
            arrival.SetActive(false);
            reco+=Time.deltaTime;
        }
        else
        {
            
        }
    }
    [Button]
    void TESTTTT()
    {
        Vector3 nee=Vector3.up*5;
        Debug.Log(nee);
        nee.Normalize();
        Debug.Log(nee);
    }
}
