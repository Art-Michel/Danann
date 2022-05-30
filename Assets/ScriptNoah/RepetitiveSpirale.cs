using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class RepetitiveSpirale : MonoBehaviour
{
    [SerializeField,Foldout("Spirale Initialisation")]Pool pool;
    [SerializeField,Foldout("Spirale Initialisation")]int nbBullet;
    [SerializeField,Foldout("Spirale Initialisation")]bool goLeft;
    [SerializeField,Foldout("Spirale Initialisation")]int angle;
    [SerializeField,Foldout("Spirale Initialisation")]float maxDelay;
    [SerializeField,Foldout("Repetition")]int spiraleNumber;
    [SerializeField,Foldout("Repetition")]bool reverse;
    [SerializeField,Foldout("Repetition")]float MaxSpiraleDelay;
    [SerializeField,Foldout("Repetition")] bool decrementBullet;
    float spiraleDelay;
    int index;
    List<ProjSpirale> spirales=new List<ProjSpirale>();


    // Start is called before the first frame update
    void Start()
    {
        index=0;
        spiraleDelay=0;
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
        if (index<=0)
            return;
        foreach(ProjSpirale sp in spirales)
        {
            sp.InheritedUpdate();
        }
    }

    private void Spawn()
    {
        if (index==spiraleNumber)
            return;
        spiraleDelay+=Time.deltaTime;
        if (spiraleDelay<MaxSpiraleDelay)
            return;
        ProjSpirale spirale=new ProjSpirale();
        spirale.Init(pool,nbBullet,goLeft,angle,maxDelay);
        spirales.Add(spirale);
        if (reverse)
            goLeft=!goLeft;
        spiraleDelay=0;
        index++;
        if (decrementBullet)
            nbBullet--;
    }
}
