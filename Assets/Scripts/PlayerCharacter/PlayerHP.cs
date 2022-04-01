using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class PlayerHP : MonoBehaviour
{
    public float HealthPoints { get; private set; }
    const float _maxHealthPoints = 100;
    private bool _isInvulnerable = false;
    [SerializeField] Image _healthBar;

    void Start()
    {
        HealthPoints = _maxHealthPoints;
        UpdateHealthBar();
    }

    [Button]
    public void TakeDamage(int amount = 30)
    {
        if (!_isInvulnerable)
        {
            HealthPoints= Mathf.Clamp(HealthPoints -= amount, 0, _maxHealthPoints);
            UpdateHealthBar();
            if (HealthPoints <= 0) Die();
        }
    }

    [Button]
    public void Heal(int amount = 5)
    {
        HealthPoints=  Mathf.Clamp(HealthPoints += amount, 0, _maxHealthPoints);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float value = Mathf.InverseLerp(0, _maxHealthPoints, HealthPoints);
        value = Mathf.Lerp(0, 1, value);
        _healthBar.fillAmount = value;
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
