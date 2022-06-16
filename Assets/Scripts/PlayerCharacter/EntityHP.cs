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
    [SerializeField] protected float _maxHealthPoints = 1;
    public bool IsInvulnerable { get { return _isInvulnerable; } set { _isInvulnerable = value; } }


    bool _isInvulnerable;
    [Required][SerializeField] protected PlayerPlasma _playerplasma;
    [Required][SerializeField] Image _healthBar;
    [Required][SerializeField] public Image _healthBarRemnant;
    [SerializeField] Pooler _billboardsPool;
    //parry
    public bool IsParrying;
    public bool activateRemnant;
    public float remnantTime;
    public float maxRemnantTime;

    void Start()
    {
        maxRemnantTime=0.7f;
        remnantTime=0;
        activateRemnant=false;
        HealthPoints = _maxHealthPoints;
        _isInvulnerable = false;
        IsParrying = false;
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(int amount, string attackName, int plasmaRegainValue, int revengeGain = 0,GameObject obj = null)
    {
        if (!_isInvulnerable)
        {
            HealthPoints = Mathf.Clamp(HealthPoints -= amount, 0, _maxHealthPoints);
            bool isBoss = _maxHealthPoints > 100;
            if (isBoss) GetComponent<DanuAI>().BuildUpRevenge(revengeGain);
            if (_healthBar) UpdateHealthBar();
            DamageFeedback(attackName, plasmaRegainValue);
            if (HealthPoints <= 0) Die();
            return true;
        }
        else if (IsParrying)
        {
            Parry(obj, plasmaRegainValue, attackName);
            return false;
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

    protected void UpdateHealthBar()
    {
        float value = Mathf.InverseLerp(0, _maxHealthPoints, HealthPoints);
        value = Mathf.Lerp(0, 1, value);
        _healthBar.fillAmount = value;
        activateRemnant=true;
    
    }
    protected virtual void Die()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        Invoke("Quit",2);
        Debug.Log(name + " just died");
    }
    void Quit()
    {
        Application.Quit();
    }
    protected virtual void DamageFeedback(string attackName = "", int plasmaRegainValue = 0)
    {
    }

    protected virtual void Parry(GameObject obj, int plasmaRegainValue, string attackName)
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
