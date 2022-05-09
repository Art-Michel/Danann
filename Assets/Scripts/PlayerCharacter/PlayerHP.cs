using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Cinemachine;

public class PlayerHP : EntityHP
{

    [SerializeField] CinemachineVirtualCamera _vcam;
    CinemachineBasicMultiChannelPerlin _vcamPerlin;
    Hurtbox _hurtbox;

    void Awake()
    {
        _maxHealthPoints = 100;
        _vcamPerlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _hurtbox = GetComponent<Hurtbox>();
    }

    override protected IEnumerator PostDamage()
    {
        SoundManager.Instance.PlayPlayerHurt();
        _vcamPerlin.m_AmplitudeGain = 1f;
        _isInvulnerable = true;
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.2f);

        _vcamPerlin.m_AmplitudeGain = 0f;
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.7f);

        _isInvulnerable = false;
        _hurtbox.ResetIds();
        yield return null;
    }
}
