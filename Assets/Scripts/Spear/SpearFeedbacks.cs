using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using NaughtyAttributes;

public class SpearFeedbacks : MonoBehaviour
{

     #region Audio
    [Required][SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _reattach;

    private void PlaySound(AudioClip clip, float volume)
    {
        _audioSource.PlayOneShot(clip, volume);
    }
    
    public void PlayReattach()
    {
        PlaySound(_reattach, 1f);
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
