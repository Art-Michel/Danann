using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System;

public class PlasmaBar : MonoBehaviour
{
    [SerializeField] List<Image> _segments;
    [SerializeField] Image _plasmaGauge;
    int _currentSegment;

    [OnValueChanged("OnPlasmaChanged")]
    [MinValue(0), MaxValue(100)] float _plasma;
    const float _segmentPlasma = 20;
    float _maxPlasma;

    void Start()
    {
        _maxPlasma = _segmentPlasma * _segments.Count;
        _currentSegment = 0;
        _plasma = 0;
    }

    void OnPlasmaChanged()
    {
        _plasmaGauge.fillAmount = Mathf.Lerp(0, _maxPlasma, _plasma);
        CheckForSegmentUpdate();
    }

    private void CheckForSegmentUpdate()
    {
        if (_plasma > 20 * (_currentSegment + 1))
        {
            NewSegmentFilled();
            CheckForSegmentUpdate();
        }
    }

    private void NewSegmentFilled()
    {
        //_segments[_currentSegment]
        _currentSegment++;
    }

    int _filledSegments;

}
