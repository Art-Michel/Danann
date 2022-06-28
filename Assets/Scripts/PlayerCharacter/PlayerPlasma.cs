using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class PlayerPlasma : MonoBehaviour
{
    public float PlasmaPoints { get; private set; }
    float _maxPlasmaPoints;
    PlayerFeedbacks _playerFeedbacks;
    [SerializeField] BinoParticlePooler _binoParticlePooler;

    [SerializeField] Image _plasmaBar;
    [SerializeField] List<Image> _segments;
    int _currentSegment;
    const int _plasmaPerSegment = 20;

    public Dictionary<string, float> _plasmaCost = new Dictionary<string, float>()
    {
        {Ccl_Attacks.TRIANGLEBOOM, 3},
        {"TriangleBreak", 2},
        {"TriangleStop", 1},
        {Ccl_Attacks.DASHONSPEAR, 1},
        {Ccl_Attacks.SHIELD, 1},
        {"Renvoi", 0.2f}
    };

    void Awake()
    {
        _playerFeedbacks = GetComponent<PlayerFeedbacks>();
    }

    void Start()
    {
        PlasmaPoints = 60f;
        _currentSegment = 0;
        _maxPlasmaPoints = _segments.Count * _plasmaPerSegment;
        foreach (Image segment in _segments) segment.enabled = false;
        UpdatePlasmaBar();
    }

    public bool VerifyPlasma(string skill)
    {
        if (PlasmaPoints >= _plasmaCost[skill] * _plasmaPerSegment)
            return true;
        else
        {
            _playerFeedbacks.PlayErrorSfx();
            _playerFeedbacks.NotEnoughPlasmaText();
        }
        return false;
    }

    public void SpendPlasma(string skill)
    {
        DecreasePlasma((_plasmaCost[skill]));
    }

    public void DecreasePlasma(float amount)
    {
        PlasmaPoints = Mathf.Clamp(PlasmaPoints -= amount * _plasmaPerSegment, 0, _maxPlasmaPoints);
        UpdatePlasmaBar();
    }

    public void IncreasePlasma(float amount)
    {
        PlasmaParticle particle = _binoParticlePooler.Get() as PlasmaParticle;
        particle.Enable(amount);
    }
    public void ActuallyIncreasePlasma(float amount)
    {
        PlasmaPoints = Mathf.Clamp(PlasmaPoints += amount, 0, _maxPlasmaPoints);
        UpdatePlasmaBar();
        _playerFeedbacks.PlayPlasmaGainedSfx((int)(PlasmaPoints + amount) - (_currentSegment * _plasmaPerSegment));
    }

    private void UpdatePlasmaBar()
    {
        float value = Mathf.InverseLerp(0, _maxPlasmaPoints, PlasmaPoints);
        value = Mathf.Lerp(0, 1, value);
        _plasmaBar.fillAmount = value;
        CheckForSegmentUpdate();
    }

    private void CheckForSegmentUpdate()
    {
        if (PlasmaPoints >= _plasmaPerSegment * (_currentSegment + 1) && _currentSegment <= _segments.Count)
        {
            NewSegmentFilled();
            CheckForSegmentUpdate();
        }
        else if (PlasmaPoints < _plasmaPerSegment * _currentSegment && _currentSegment > 0)
        {
            DepleteSegment();
            CheckForSegmentUpdate();
        }
    }

    private void NewSegmentFilled()
    {
        _segments[_currentSegment].enabled = true;
        UiManager.Instance.CheckIfUltReady();
        _currentSegment++;
        _playerFeedbacks.PlaySegmentFill();
        if (_currentSegment == 1)
            UiManager.Instance.OnePlasmaFilled();
    }

    private void DepleteSegment()
    {
        _currentSegment--;
        UiManager.Instance.CheckIfUltReady();
        _segments[_currentSegment].enabled = false;
        if (_currentSegment == 0)
            UiManager.Instance.OnePlasmaEmptied();
    }
}
