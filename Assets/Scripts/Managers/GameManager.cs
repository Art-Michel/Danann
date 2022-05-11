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
#if UNITY_EDITOR
        SOMeshes.Init(_configSOMeshes);
        #endif

        Hurtboxes = new Hurtbox[EnemyHurtboxes.Length + AllyHurtboxes.Length];
        EnemyHurtboxes.CopyTo(Hurtboxes, 0);
        AllyHurtboxes.CopyTo(Hurtboxes, EnemyHurtboxes.Length);
    }
}
