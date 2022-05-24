using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using NaughtyAttributes;
using System;

public class SpearFeedbacks : MonoBehaviour
{

     #region Audio
    [Required][SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _reattach;
    [SerializeField] AudioClip _swing;

    private void PlaySound(AudioClip clip, float volume)
    {
        _audioSource.PlayOneShot(clip, volume);
    }
    
    public void PlayReattach()
    {
        PlaySound(_reattach, 1f);
    }

    public void PlaySwing()
    {
        PlaySound(_swing, 1f);
    }
    #endregion


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
