using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightShoot : MonoBehaviour
{
    [SerializeField]int row,nbBullet,index;
    [SerializeField]float delay,maxDelay;
    [SerializeField]Pool pool;
    [SerializeField]float gap;
    Vector3 startPos;
    [SerializeField]Transform player;
    // Start is called before the first frame update
    void Start()
    {
        index=0;
        delay=0;
        Vector3 target=new Vector3(player.position.x,transform.position.y,player.position.z);
        transform.LookAt(target);
        float offset;
        if (row%2==0)
            offset=-((gap/2)*(row/2));
        else
            offset=gap*(-row/2);
        startPos=transform.position+transform.right*offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (index>=nbBullet)
            return;
        delay+=Time.deltaTime;
        if (delay<maxDelay)
            return;
        Debug.Log("ee");
        for (int i=0;i<row;i++)
        {
            GameObject go= pool.Get();
            go.SetActive(true);
            if (row%2==0)
            go.transform.position=startPos+(transform.right*(gap))*i;
            else
            go.transform.position=startPos+(transform.right*gap)*i;
            go.transform.rotation=transform.rotation;
        }
        index++;
        delay=0;
    }
}
