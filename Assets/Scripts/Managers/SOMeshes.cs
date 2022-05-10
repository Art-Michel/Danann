using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SOMeshes", menuName = "e-artstup/meshes")]
public class SOMeshes : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField] Mesh _hitboxDebugSphere;
    public Mesh HitboxDebugSphere => _hitboxDebugSphere;

    public static SOMeshes Instance { get; private set; }
    public static void Init(SOMeshes config = null)
    {
        if (Instance != null)
            return;
        if (config != null)
        {
            Instance = config;
            return;
        }
        string[] assets = AssetDatabase.FindAssets("t:SOMeshes");
        if (assets.Length > 0)
            Instance = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0])) as SOMeshes;
    }
#endif
}