using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EntityHP : MonoBehaviour
{
    public float HealthPoints { get; protected set; }
    protected float _maxHealthPoints = 1;
    public bool _isInvulnerable = false;
    [Required][SerializeField] PlayerPlasma _playerplasma;
    [Required][SerializeField] Image _healthBar;


    void Start()
    {
        HealthPoints = _maxHealthPoints;
        UpdateHealthBar();
    }

    [Button]
    public void TakeDamage(int amount, string attackName, int plasmaRegainValue)
    {
        if (!_isInvulnerable)
        {
            HealthPoints = Mathf.Clamp(HealthPoints -= amount, 0, _maxHealthPoints);
            if (_healthBar) UpdateHealthBar();
            DamageFeedback(attackName);
            if (HealthPoints <= 0) Die();
            if (plasmaRegainValue > 0) _playerplasma.IncreasePlasma(plasmaRegainValue);
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
        Time.timeScale = 1;
        if (_maxHealthPoints > 100)
            Application.Quit();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log(name + " just died");
    }

    protected virtual void DamageFeedback(string attackName = "")
    {
    }

    internal void TakeDamage(object attackDamage)
    {
        throw new NotImplementedException();
    }
}
