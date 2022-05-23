using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class TP_TheRevenge : MonoBehaviour
{
    [SerializeField]GameObject arrival, fakeArrival;
    [SerializeField]GameObject boomBox,fakeBoomBox;
    [SerializeField]AttackData boomBoxAttackData,fakeBoomBoxAttackData;
    [SerializeField] Transform target;
    [SerializeField] destPoints destination;
    [SerializeField] float MaxFadeTime;
    [SerializeField] float MaxSartup;
    [SerializeField] float offsetValue;
    [SerializeField] float maxReco;
    [SerializeField] float maxActive;
    [SerializeField] float farDist;
    float fadeTime;
    float startup;
    float active;
    [SerializeField]Vector3 arenaCenter;
    public enum destPoints
    {
        FAR,
        CLOSE
    }
    float reco;

    [SerializeField]float arenaRadius;
    // Start is called before the first frame update
    private void Start() 
    {        
        Init();
        arrival.SetActive(false);
        fakeArrival.SetActive(false);
        if (destination == destPoints.FAR)
        {
            float dist = farDist / Vector3.Distance(transform.position, target.position);
            Vector3 dir = transform.position - target.position;
            dir.Normalize();
            dir *= farDist;
            Vector2 rand = Random.insideUnitCircle;
            Vector3 offset = new Vector3(rand.x, 0, rand.y) * offsetValue;
            arrival.transform.position = transform.position + dir-offset;
            fakeArrival.transform.position = transform.position + dir+offset;
            if (Vector3.Distance(arrival.transform.position, arenaCenter) >= arenaRadius)
            {
                dir = transform.position - target.position;
                dir.Normalize();
                arrival.transform.position =arenaCenter+ arenaRadius * dir;
            }            
            if (Vector3.Distance(fakeArrival.transform.position, arenaCenter) >= arenaRadius)
            {
                dir = transform.position - target.position;
                dir.Normalize();
                fakeArrival.transform.position =arenaCenter+ arenaRadius * dir;
            }
        }
        else
        {
            Vector2 rand = Random.insideUnitCircle;
            Vector3 offset = new Vector3(rand.x, 0, rand.y) * offsetValue;
            arrival.transform.position = target.position + offset;
            fakeArrival.transform.position = target.position - offset;
        }
        startup=0;
        fadeTime=0;
        active=0;
        reco=0;
    }
    void Init()
    {
    }
    // Update is called once per frame
    private void Update() 
    {
        TP();
    }

    void TP()
    {
        if (startup <= MaxSartup)
        {
            startup += Time.deltaTime;
        }
        else if (fadeTime <= MaxFadeTime)
        {
            arrival.SetActive(true);
            fakeArrival.SetActive(true);
            fadeTime += Time.deltaTime;
        }
        else if (active <= maxActive)
        {
            //boomBox.SetActive(true);
            boomBoxAttackData.LaunchAttack();
            fakeBoomBoxAttackData.LaunchAttack();
            transform.position = arrival.transform.position;
            active += Time.deltaTime;
        }
        else if (reco <= maxReco)
        {
            boomBoxAttackData.StopAttack();
            fakeBoomBoxAttackData.StopAttack();
            arrival.SetActive(false);
            fakeArrival.SetActive(false);
            reco += Time.deltaTime;
        }
        else
        {
            enabled=false;
            Debug.Log("over");
        }
    }


    
}