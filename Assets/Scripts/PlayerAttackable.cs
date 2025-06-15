using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackable : MonoBehaviour, IDamageable
{
    [SerializeField] float _health;

    public void Damage(float damage)
    {
        _health -= damage;
        if (_health < 0)
        {
            // Run some kind of OnDeath event?
            Destroy(gameObject);
        }
    }

    public bool IsDead()
    {
        return _health <= 0;
    }
}