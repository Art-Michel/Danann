using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class PlayerPlasma : MonoBehaviour
{
    public float PlasmaPoints { get; private set; }
    const float _maxPlasmaPoints = 100;
    [SerializeField] Image _plasmaBar;

    void Start()
    {
        PlasmaPoints = 0;
        UpdatePlasmaBar();
    }

    [Button]
    public void DecreasePlasma(int amount = 30)
    {
        PlasmaPoints = Mathf.Clamp(PlasmaPoints -= amount, 0, _maxPlasmaPoints);
        UpdatePlasmaBar();
    }

    [Button]
    public void IncreasePlasma(int amount = 5)
    {
        PlasmaPoints = Mathf.Clamp(PlasmaPoints += amount, 0, _maxPlasmaPoints);
        UpdatePlasmaBar();
    }

    private void UpdatePlasmaBar()
    {
        float value = Mathf.InverseLerp(0, _maxPlasmaPoints, PlasmaPoints);
        value = Mathf.Lerp(0, 1, value);
        _plasmaBar.fillAmount = value;
    }
}
