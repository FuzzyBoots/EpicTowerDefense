using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MeleeEnemy : EnemyNavMeshAgent
{
    [SerializeField] float _detectRange = 2f;
    [SerializeField] float _attackRange = 0.5f;

    protected override PlayerAttackable[] GetTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _detectRange, LayerMask.GetMask(""));
        List<PlayerAttackable> playerAttackables = new List<PlayerAttackable>();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<PlayerAttackable>(out PlayerAttackable attackable))
            {
                playerAttackables.Add(attackable);
            }
        }

        return playerAttackables.ToArray();
    }

    protected override void PerformAttack(PlayerAttackable nearestTarget)
    {
        _animator.SetBool("Punching", Vector3.Distance(transform.position, nearestTarget.transform.position) <= _attackRange);        
    }
}
