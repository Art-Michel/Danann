using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class EntityHP : MonoBehaviour
{
    public float HealthPoints { get; protected set; }
    protected float _maxHealthPoints = 1;
    protected bool _isInvulnerable = false;
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
            HealthPoints = Mathf.Clamp(HealthPoints -= amount, 0, _maxHealthPoints);
            if (_healthBar) UpdateHealthBar();
            DamageFeedback();
            if (HealthPoints <= 0) Die();
        }
    }

    [Button]
    public void Heal(int amount = 5)
    {
        HealthPoints = Mathf.Clamp(HealthPoints += amount, 0, _maxHealthPoints);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float value = Mathf.InverseLerp(0, _maxHealthPoints, HealthPoints);
        value = Mathf.Lerp(0, 1, value);
        _healthBar.fillAmount = value;
    }

    protected void Die()
    {
        Debug.Log(name + " just died");
    }

    protected virtual void DamageFeedback()
    {

    }
}
