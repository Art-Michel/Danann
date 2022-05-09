using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [SerializeField] int baseCount;

    Queue<GameObject> items = new Queue<GameObject>();

    void Start()
    {
        AddCount(baseCount);
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
                GameObject go = Instantiate(prefab, GetComponent<Danu_FSM>().transform);
                go.GetComponent<Projectiles>().SetOrigin(this);
                go.GetComponent<Projectiles>().SetTarget(GetComponent<DanuAI>().GetPlayer());
                go.GetComponent<Projectiles>().SetLifeTime(GetComponent<Danu_FSM>().GetP1d_ProjLifeTime());
                go.SetActive(false);
                items.Enqueue(go);
        }
    }
}