using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class GameManager : LocalManager<GameManager>
{
    [SerializeField] SOMeshes _configSOMeshes;

    void Start()
    {
        #if UNITY_EDITOR
        SOMeshes.Init(_configSOMeshes);
        #endif
    }
}
