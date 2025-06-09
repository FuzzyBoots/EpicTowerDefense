using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackable : MonoBehaviour, IDamageable
{
    [SerializeField] float _health;

    [SerializeField] HashSet<EnemyNavMeshAgent> _attackers;

    [SerializeField] int _maxAttackers = 3;

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

    public int AttackerCount()
    {
        return _attackers.Count;
    }

    public int MaxAttackers()
    {
        return _maxAttackers;
    }

    public void AddAttacker(EnemyNavMeshAgent attacker)
    {
        _attackers.Add(attacker);
    }
}