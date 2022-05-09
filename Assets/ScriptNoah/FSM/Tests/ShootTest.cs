using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class ShootTest : MonoBehaviour
{
    public GameObject projectile;
    public int nbShot;
    int remainingShot;

    int index;
    public float delay;
    float timer;
    [SerializeField]Transform player;
    // Start is called before the first frame update
    [Button]
    void Start()
    {
        //remainingShot=nbShot;

        index=0;
        timer=0;
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if (timer>delay){
            timer=0;
            if (index>=nbShot)
            {
                //Debug.Log("End");
            }
            else
            {

            GameObject go = Instantiate(projectile,transform.position,transform.rotation);
            go.GetComponent<Projectiles>().SetTarget(player);
            
            index++;

            }
        }
    }
}
