using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject secondPrefab;
    [SerializeField] int baseCount;
    [SerializeField] int secondBaseCount;

    Queue<GameObject> items = new Queue<GameObject>();
    Queue<GameObject> secondItems = new Queue<GameObject>();
    [SerializeField] DanuAI boss;
    [SerializeField] Danu_FSM fsm;
    
    void Start()
    {
        AddCount(baseCount);
        SecondAddCount(secondBaseCount);
    }

    public GameObject Get()
    {
        Debug.Log(items.Count);
        if (items.Count == 0)
            AddCount(1);
       
        return items.Dequeue();
        
    }

    public void Back(GameObject obj)
    {
        obj.SetActive(false);
        items.Enqueue(obj);
    }

    public void AddCount(int nb)
    {
        for (int i = 0; i < nb; i++)
        {
                GameObject go = Instantiate(prefab, fsm.transform.position,fsm.transform.rotation,transform);
                go.GetComponent<Projectiles>().SetOrigin(this);
                go.GetComponent<Projectiles>().SetTarget(boss.GetPlayer());
                go.GetComponent<Projectiles>().SetLifeTime(fsm.GetP1d_ProjLifeTime());
                go.SetActive(false);
                items.Enqueue(go);
        }
    }    
    public GameObject SecondGet()
    {
        if (secondItems.Count == 0)
            SecondAddCount(1);
       
        return secondItems.Dequeue();
        
    }

    public void SecondBack(GameObject obj)
    {
        obj.SetActive(false);
        secondItems.Enqueue(obj);
    }

    public void SecondAddCount(int nb)
    {
        for (int i = 0; i < nb; i++)
        {
                GameObject go = Instantiate(secondPrefab, fsm.transform.position,fsm.transform.rotation,fsm.GetP1GlobalGO().transform);
                go.SetActive(false);
                secondItems.Enqueue(go);
        }
    }
}