using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangedHitscanEnemy : EnemyNavMeshAgent
{
    [SerializeField] private float _attackRange = 4f;
    private GameObject _closestEnemy;
    [SerializeField] private GameObject _turretObject;

    [SerializeField] private float _idleTurnSpeed = 0.2f;
    [SerializeField] private float _turnSpeed = 1f;
    [SerializeField] private float _targetingArc = 10f;
    [SerializeField] private float _damagePerSecond = 1f;
    private object _gun;

    protected override PlayerAttackable[] GetTargets()
    {        
        List<Collider> contacts = Physics.OverlapSphere(transform.position, _attackRange, LayerMask.GetMask("Enemy")).ToList<Collider>();

        // if (contacts.Length < 1)
        // {
        //     // General spin
        //     _turretObject.transform.forward = Vector3.RotateTowards(transform.forward, _turretObject.transform.forward, _idleTurnSpeed * Time.deltaTime, 0f);
        //     _gun.Firing = false;
        //     return;
        // }

        // _closestEnemy = contacts[0].gameObject;
        // float _closestDistance = Vector3.Distance(transform.position, _closestEnemy.transform.position);

        // for (int i = 1; i < contacts.Length; i++)
        // {
        //     float distance = Vector3.Distance(transform.position, contacts[i].gameObject.transform.position);
        //     if (distance < _closestDistance) {
        //         _closestDistance = distance;
        //         _closestEnemy = contacts[i].gameObject;
        //     }
        // }   
        List<PlayerAttackable> targets = new List<PlayerAttackable>();
        foreach (Collider c in contacts)
        {
            if (c.gameObject.TryGetComponent<PlayerAttackable>(out PlayerAttackable target))
            {
                targets.Add(target);
            }
        }
        return targets.ToArray();
    }

    protected override void PerformAttack(PlayerAttackable nearestTarget)
    {
        if (_closestEnemy == null)
        {
            // _gun.Firing = false;
            return;
        }

        float distance = Vector3.Distance(transform.position, _closestEnemy.transform.position);
        if ( distance > _attackRange)
        {
            _closestEnemy = null;
            return;
        }

        Vector3 targetVector = _closestEnemy.transform.position - transform.position;
        // Rotate toward the hit
        _turretObject.transform.forward = Vector3.RotateTowards(_turretObject.transform.forward, targetVector, _turnSpeed * Time.deltaTime, 0f);

        // If in target arc, fire
        if (Vector3.Angle(_turretObject.transform.forward, targetVector) < _targetingArc)
        {
            // _gun.Firing = true;
            
            // Play the animation for firing
            if (_closestEnemy.TryGetComponent<IDamageable>(out IDamageable target))
            {
                target.Damage(_damagePerSecond * Time.deltaTime);
                
                if (target.IsDead()) { _closestEnemy = null; Debug.Log("Enemy Dead"); }
            }
        } else
        {
            // _gun.Firing = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
