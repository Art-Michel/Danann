using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using NaughtyAttributes;

public class SpearFeedbacks : MonoBehaviour
{
    [Required][SerializeField] Transform _spearTransformWhenAttached;
    public void ResetPositionAndRotation()
    {
        transform.SetPositionAndRotation(_spearTransformWhenAttached.position, _spearTransformWhenAttached.rotation);
    }

    [Required][SerializeField] CinemachineTargetGroup _targetGroup;
    public void SetCameraTargetWeight(int target, int weight)
    {
        _targetGroup.m_Targets[target].weight = weight;
    }

}
