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
    public bool IsInvulnerable { get { return _isInvulnerable; } set { _isInvulnerable = value; } }
    bool _isInvulnerable;
    [Required][SerializeField] PlayerPlasma _playerplasma;
    [Required][SerializeField] Image _healthBar;
    [SerializeField] Pooler _billboardsPool;
    //parry
    public bool IsParrying;

    void Start()
    {
        HealthPoints = _maxHealthPoints;
        _isInvulnerable = false;
        IsParrying = false;
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(int amount, string attackName, int plasmaRegainValue, int revengeGain = 0)
    {
        if (!_isInvulnerable)
        {
            HealthPoints = Mathf.Clamp(HealthPoints -= amount, 0, _maxHealthPoints);
            bool isBoss = _maxHealthPoints > 100;
            if (isBoss) GetComponent<DanuAI>().BuildUpRevenge(revengeGain);
            if (_healthBar) UpdateHealthBar();
            DamageFeedback(attackName);
            if (HealthPoints <= 0) Die();
            if (plasmaRegainValue > 0) _playerplasma.IncreasePlasma(plasmaRegainValue);
            return true;
        }
        else if (IsParrying)
        {
            Parry();
            return true;
        }
        else
        {
            SoundManager.Instance.PlayBlockedHit();
        }
        return false;
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

    protected virtual void Parry()
    {

    }

    internal void TakeDamage(object attackDamage)
    {
        throw new NotImplementedException();
    }

    public void InvulnerableText()
    {
        Billboard billboard = _billboardsPool.Get() as Billboard;
        billboard.Enable("Invulnerable!");
    }

    public void ParryingText()
    {
        Billboard billboard = _billboardsPool.Get() as Billboard;
        billboard.Enable("Parrying!");
    }
}
