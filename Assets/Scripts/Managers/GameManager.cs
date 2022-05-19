using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using System;

public class GameManager : LocalManager<GameManager>
{
    [SerializeField] SOMeshes _configSOMeshes;

    public Hurtbox[] EnemyHurtboxes;
    public Hurtbox[] AllyHurtboxes;
    [NonSerialized] public Hurtbox[] Hurtboxes;

    void Start()
    {
        Time.timeScale=1;
#if UNITY_EDITOR
        SOMeshes.Init(_configSOMeshes);
        #endif

        Time.timeScale=1;
        Hurtboxes = new Hurtbox[EnemyHurtboxes.Length + AllyHurtboxes.Length];
        EnemyHurtboxes.CopyTo(Hurtboxes, 0);
        AllyHurtboxes.CopyTo(Hurtboxes, EnemyHurtboxes.Length);
    }
}
