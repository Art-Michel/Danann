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

    Dictionary<string, float> _plasmaCost = new Dictionary<string, float>()
    {
        {Ccl_Attacks.TRIANGLE, 90},
        {Ccl_Attacks.DASHONSPEAR, 30},
        {Ccl_Attacks.PARRY, 30},
        {Ccl_Attacks.SHOCKWAVE_CCL, 50},
    };

    void Start()
    {
        PlasmaPoints = 0;
        UpdatePlasmaBar();
    }

    public bool VerifyPlasma(string skill)
    {
        return true;
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
