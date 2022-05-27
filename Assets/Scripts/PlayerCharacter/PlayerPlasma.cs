using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class PlayerPlasma : MonoBehaviour
{
    public int PlasmaPoints { get; private set; }
    const int _maxPlasmaPoints = 100;
    PlayerFeedbacks _playerFeedbacks;
    [SerializeField] Image _plasmaBar;

    Dictionary<string, int> _plasmaCost = new Dictionary<string, int>()
    {
        {Ccl_Attacks.TRIANGLE, 90},
        {Ccl_Attacks.DASHONSPEAR, 30},
        {Ccl_Attacks.PARRY, 30},
        {Ccl_Attacks.SHOCKWAVE_CCL, 50},
    };

    void Awake()
    {
        _playerFeedbacks = GetComponent<PlayerFeedbacks>();
    }

    void Start()
    {
        PlasmaPoints = 0;
        UpdatePlasmaBar();
    }

    public bool VerifyPlasma(string skill)
    {
        if (PlasmaPoints > _plasmaCost[skill])
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
        PlasmaPoints = Mathf.Clamp(PlasmaPoints -= amount, 0, _maxPlasmaPoints);
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
    }
}
