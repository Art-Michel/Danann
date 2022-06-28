using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine.UI;

public class SpearFeedbacks : MonoBehaviour
{
    SpearAI _spearAi;
    [SerializeField] GameObject _targetArrow;
    #region anim
    [SerializeField] Transform _mesh;
    Vector3 _swingingRot = new Vector3(0, 90, 90);
    bool _isSwinging = false;
    float _swingAnimT = 0;
    [Required][SerializeField] TrailRenderer _bladeTrail;

    [Required][SerializeField] CinemachineTargetGroup _targetGroup;

    #endregion

    #region Audio
    [Required][SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _reattach;
    [SerializeField] AudioClip _swing;
    [SerializeField] AudioClip _target;
    [SerializeField] AudioClip _unTarget;

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

    public void PlayTargetSfx()
    {
        PlaySound(_target, 1f);
    }

    public void PlayUntargetSfx()
    {
        PlaySound(_unTarget, 1f);
    }
    #endregion

    #region ui
    [SerializeField] TextMeshProUGUI spearText;
    [SerializeField] Image spearImage;

    public void SetText(string str)
    {
        spearText.text = str;
    }
    #endregion

    void Awake()
    {
        _spearAi = GetComponent<SpearAI>();
    }

    void Start()
    {
        _isSwinging = false;
    }

    public void SetSpearCameraTargetWeight(bool isLeft, int weight)
    {
        if (isLeft) _targetGroup.m_Targets[2].weight = weight;
        else _targetGroup.m_Targets[3].weight = weight;
    }

    public void SetSpearCameraTargetRadius(bool isLeft, int radius)
    {
        if (isLeft) _targetGroup.m_Targets[2].radius = radius;
        else _targetGroup.m_Targets[3].radius = radius;
    }

    public void SetCameraTargetWeight(int target, int weight)
    {
        _targetGroup.m_Targets[target].weight = weight;
    }

    #region Targetting
    [SerializeField] GameObject _targettedCircle;

    public void TargettedFeedbacks()
    {
        SetSpearCameraTargetRadius(_spearAi.IsLeft, 6);
        _targetArrow.SetActive(true);
        _targettedCircle.SetActive(true);
        PlayTargetSfx();
        spearImage.transform.localScale = Vector3.one;
        if (_spearAi.IsLeft)
        {
            if (_spearAi._fsm.PlayerActions._rightSpear.currentState.Name == Spear_StateNames.IDLE || _spearAi._fsm.PlayerActions._rightSpear.currentState.Name == Spear_StateNames.ATTACKING || _spearAi._fsm.PlayerActions._rightSpear.currentState.Name == Spear_StateNames.THROWN)
                UiManager.Instance.RtText.text = "ULT";
        }
        else if (_spearAi._fsm.PlayerActions._leftSpear.currentState.Name == Spear_StateNames.IDLE || _spearAi._fsm.PlayerActions._leftSpear.currentState.Name == Spear_StateNames.ATTACKING || _spearAi._fsm.PlayerActions._rightSpear.currentState.Name == Spear_StateNames.THROWN)
            UiManager.Instance.LtText.text = "ULT";
    }

    public void UntargettedFeedbacks(bool gotRecalled)
    {
        spearImage.transform.localScale = Vector3.one * 0.8f;
        _targettedCircle.SetActive(false);
        SetSpearCameraTargetRadius(_spearAi.IsLeft, 4);
        _targetArrow.SetActive(false);
        if (gotRecalled)
        {
            PlayUntargetSfx();
            SetText("Target");
        }
        if (_spearAi.IsLeft)
        {
            if (_spearAi._fsm.PlayerActions._rightSpear.currentState.Name == Spear_StateNames.IDLE || _spearAi._fsm.PlayerActions._rightSpear.currentState.Name == Spear_StateNames.ATTACKING || _spearAi._fsm.PlayerActions._rightSpear.currentState.Name == Spear_StateNames.THROWN)
                UiManager.Instance.RtText.text = "Target";
        }
        else if (_spearAi._fsm.PlayerActions._leftSpear.currentState.Name == Spear_StateNames.IDLE || _spearAi._fsm.PlayerActions._leftSpear.currentState.Name == Spear_StateNames.ATTACKING || _spearAi._fsm.PlayerActions._rightSpear.currentState.Name == Spear_StateNames.THROWN)
            UiManager.Instance.LtText.text = "Target";
    }
    #endregion

    #region Aiming
    [SerializeField] GameObject _aimingVfx;

    public void AimedFeedbacks()
    {
        _aimingVfx.SetActive(true);
        spearImage.transform.localScale = Vector3.one;
    }

    public void UnaimedFeedbacks()
    {
        spearImage.transform.localScale = Vector3.one * 0.8f;
        _aimingVfx.SetActive(false);
    }
    #endregion

    #region animation
    [Button]
    public void StartSwingAnimation()
    {
        SetMeshRotation(_swingingRot);

        _swingAnimT = 0;
        _isSwinging = true;
        EnableTrail();
    }

    public void StopSwingAnimation()
    {
        _isSwinging = false;
        DisableTrail();
        SetMeshRotation(Vector3.zero);
    }

    public void EnableTrail()
    {
        _bladeTrail.emitting = true;
    }
    public void DisableTrail()
    {
        _bladeTrail.emitting = false;
    }
    const float rotationSpeed = 2000;
    void AnimateSwing()
    {
        //_swingAnimT += Time.deltaTime * 0.4f;
        SetMeshRotation(_mesh.rotation.eulerAngles + Vector3.up * rotationSpeed * Time.deltaTime);
    }

    public void SetMeshRotation(Vector3 rotation)
    {
        _mesh.rotation = Quaternion.Euler(rotation);
    }

    public void SetMeshForward(Vector3 direction)
    {
        _mesh.forward = direction;
    }

    public void SetMeshUp(Vector3 direction)
    {
        _mesh.up = direction;
    }
    #endregion

    void Update()
    {
        if (_isSwinging)
            AnimateSwing();
    }


}