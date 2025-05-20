using GameDevHQ.FileBase.Gatling_Gun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanTurret : Turret, IDamageable
{
    [SerializeField] float _attackRange = 1f;
    [SerializeField] float _damagePerSecond = 10f;
    [SerializeField] float _turnSpeed = 1f;
    [SerializeField] float _targetingArc = 5f;

    [SerializeField] GameObject _turretObject;

    [SerializeField] GameObject _closestEnemy;
    [SerializeField] float _idleTurnSpeed = 0.2f;

    [SerializeField] Gatling_Gun _gun;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_active) { return; }

        FindEnemy();

        AimAtAndShootAtCurrentEnemy();
    }

    private void AimAtAndShootAtCurrentEnemy()
    {
        if (_closestEnemy == null)
        {
            _gun.Firing = false;
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
            _gun.Firing = true;
            
            // Play the animation for firing
            if (_closestEnemy.TryGetComponent<IDamageable>(out IDamageable target))
            {
                target.Damage(_damagePerSecond * Time.deltaTime);
                
                if (target.IsDead()) { _closestEnemy = null; Debug.Log("Enemy Dead"); }
            }
        } else
        {
            _gun.Firing = false;
        }
    }

    private void FindEnemy()
    {
        if (_closestEnemy != null)
        {
            return;
        }
        
        Collider[] contacts = Physics.OverlapSphere(transform.position, _attackRange, LayerMask.GetMask("Enemy"));

        if (contacts.Length < 1)
        {
            // General spin
            _turretObject.transform.Rotate(Vector3.up, _idleTurnSpeed * Time.deltaTime);
            _gun.Firing = false;
            return;
        }

        _closestEnemy = contacts[0].gameObject;
        float _closestDistance = Vector3.Distance(transform.position, _closestEnemy.transform.position);

        for (int i = 1; i < contacts.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, contacts[i].gameObject.transform.position);
            if (distance < _closestDistance) {
                _closestDistance = distance;
                _closestEnemy = contacts[i].gameObject;
            }
        }        
    }
}
