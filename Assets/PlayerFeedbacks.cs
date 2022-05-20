using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using NaughtyAttributes;
using Cinemachine;

public class PlayerFeedbacks : MonoBehaviour
{
    #region Health
    [SerializeField] AudioClip _playerHurt;
    #endregion

    #region Aiming
    [SerializeField] Volume _volume;
    Vignette _vignette;

    [SerializeField] AudioClip _zoomin;
    [SerializeField] AudioClip _zoomout;
    #endregion

    #region Throwing
    [SerializeField] AudioClip _throw;
    [SerializeField] AudioClip _recall;
    #endregion

    #region DodgeRoll
    [SerializeField] TrailRenderer _bodyTrailRenderer;
    [SerializeField] AudioClip _dodge;
    #endregion

    #region Light Attack
    [SerializeField] AudioClip _punch0;
    [SerializeField] AudioClip _punch1;
    [SerializeField] AudioClip _punch2;
    #endregion
  
    #region Audio
    [Required][SerializeField] AudioSource _audioSource;
    #endregion
    
    [Required][SerializeField] CinemachineTargetGroup _targetGroup;
    
    void Awake()
    {
        //aiming
        if (_volume) _volume.profile.TryGet<Vignette>(out _vignette);
    }

    #region Aiming Feedbacks
    public void ZoomCamera()
    {
        _vignette.intensity.Override(0.6f);
    }

    public void UnzoomCamera()
    {
        _vignette.intensity.Override(0f);
    }
    #endregion

    #region Dodgeroll Feedbacks
    public void SetTrailRenderer(bool enabled, bool emitting)
    {
        _bodyTrailRenderer.enabled = enabled;
        _bodyTrailRenderer.emitting = emitting;
    }
    #endregion

    #region SFX
    private void PlaySound(AudioClip clip, float volume)
    {
        _audioSource.PlayOneShot(clip, volume);
    }
    
    public void PlayPunch0()
    {
        PlaySound(_punch0, 5f);
    }

    public void PlayPunch1()
    {
        PlaySound(_punch1, 8f);
    }

    public void PlayPunch2()
    {
        PlaySound(_punch2, 5f);
    }

    public void PlayZoomIn()
    {
        PlaySound(_zoomin, 5f);
    }

    public void PlayZoomOut()
    {
        PlaySound(_zoomout, 5f);
    }

    public void PlayThrow()
    {
        PlaySound(_throw, 0.8f);
    }

    public void PlayRecall()
    {
        PlaySound(_recall, 2f);
    }

    public void PlayDodge()
    {
        PlaySound(_dodge, 2f);
    }

    public void PlayPlayerHurt()
    {
        PlaySound(_playerHurt, 2f);
    }
    #endregion

    public void SetCameraTargetWeight(int target, int weight)
    {
        _targetGroup.m_Targets[target].weight = weight;
    }
}
