using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class MeleeEnemy : EnemyNavMeshAgent
{
    [SerializeField] float _detectRange = 2f;
    [SerializeField] float _attackRange = 1f;

    [SerializeField] float _damagePerSecond = 5f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _detectRange);
    }

    protected override PlayerAttackable[] GetTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _detectRange, LayerMask.GetMask("Turret"));
        List<PlayerAttackable> playerAttackables = new List<PlayerAttackable>();

        // Debug.Log($"Getting targets in {colliders.Length} colliders");
        foreach (Collider collider in colliders)
        {
            Debug.Log("Collider " + collider.gameObject.name);
            if (collider.TryGetComponent<PlayerAttackable>(out PlayerAttackable attackable))
            {
                Debug.Log($"Attacker Count: {attackable.AttackerCount()} Max Count: {attackable.MaxAttackers()}");
                if (attackable.AttackerCount() < attackable.MaxAttackers())
                {
                    playerAttackables.Add(attackable);
                    Debug.Log($"Adding attacker. Up to {attackable.AttackerCount()}");
                }
            }
        }
        Debug.Log($"Fetched {playerAttackables.Count} targets", this);

        return playerAttackables.ToArray();
    }

    protected override void PerformAttack(PlayerAttackable nearestTarget)
    {
        float attackDistance = Vector3.Distance(transform.position, nearestTarget.transform.position);
        if (attackDistance <= _attackRange)
        {
            transform.forward = Vector3.RotateTowards(transform.forward, nearestTarget.transform.position, 1f, 1f);
            _animator.SetBool("Punching", true);
            nearestTarget.Damage(_damagePerSecond * Time.deltaTime);
        } else
        {
            _animator.SetBool("Punching", false);
        }        
    }
}
