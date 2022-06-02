using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class PlayerPlasma : MonoBehaviour
{
    public int PlasmaPoints { get; private set; }
    int _maxPlasmaPoints;
    PlayerFeedbacks _playerFeedbacks;

    [SerializeField] Image _plasmaBar;
    [SerializeField] List<Image> _segments;
    int _currentSegment;
    const int _plasmaPerSegment = 20;

    public Dictionary<string, int> _plasmaCost = new Dictionary<string, int>()
    {
        {Ccl_Attacks.TRIANGLE, 3},
        {Ccl_Attacks.DASHONSPEAR, 1},
        {Ccl_Attacks.PARRY, 1}
    };

    void Awake()
    {
        _playerFeedbacks = GetComponent<PlayerFeedbacks>();
    }

    void Start()
    {
        PlasmaPoints = 0;
        _currentSegment = 0;
        _maxPlasmaPoints = _segments.Count * _plasmaPerSegment;
        foreach (Image segment in _segments) segment.enabled = false;
        UpdatePlasmaBar();
    }

    public bool VerifyPlasma(string skill)
    {
        if (PlasmaPoints > _plasmaCost[skill] * _plasmaPerSegment)
            return true;
        else
            _playerFeedbacks.PlayErrorSfx();
        _playerFeedbacks.NotEnoughPlasmaText();
        return false;
    }

    public void SpendPlasma(string skill)
    {
        DecreasePlasma(_plasmaCost[skill]);
    }

    public void DecreasePlasma(int amount)
    {
        PlasmaPoints = Mathf.Clamp(PlasmaPoints -= amount * _plasmaPerSegment, 0, _maxPlasmaPoints);
        UpdatePlasmaBar();
    }

    public void IncreasePlasma(int amount)
    {
        PlasmaPoints = Mathf.Clamp(PlasmaPoints += amount, 0, _maxPlasmaPoints);
        UpdatePlasmaBar();
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
        _currentSegment++;
        _playerFeedbacks.PlaySegmentFill();
    }

    private void DepleteSegment()
    {
        _currentSegment--;
        _segments[_currentSegment].enabled = false;
    }
}
