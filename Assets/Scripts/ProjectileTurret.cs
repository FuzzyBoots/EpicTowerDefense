using GameDevHQ.FileBase.Missile_Launcher;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Missile_Launcher))]
public class ProjectileTurret : Emplacement
{
    [SerializeField] float _attackRange = 3f;
    [SerializeField] float _turnSpeed = 5f;
    [SerializeField] float _targetingArc = 5f;

    [SerializeField] GameObject _turretObject;
    [SerializeField] Missile_Launcher _launcher;

    [SerializeField] GameObject _closestEnemy;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    private void Start()
    {
        _launcher = GetComponent<Missile_Launcher>();
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
            return;
        }

        float distance = Vector3.Distance(transform.position, _closestEnemy.transform.position);
        if (distance > _attackRange)
        {
            Debug.Log("Lost sight of " + _closestEnemy.name, this);
            _closestEnemy = null;
            return;
        }

        Vector3 targetVector = _closestEnemy.transform.position - transform.position;
        // Rotate toward the hit
        _turretObject.transform.forward = Vector3.RotateTowards(_turretObject.transform.forward, targetVector, _turnSpeed * Time.deltaTime, 0f);

        // If in target arc, fire
        if (Vector3.Angle(_turretObject.transform.forward, targetVector) < _targetingArc)
        {
            _launcher.FireMissile(_closestEnemy.transform);
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
            return;
        }

        _closestEnemy = contacts[0].gameObject;
        float _closestDistance = Vector3.Distance(transform.position, _closestEnemy.transform.position);

        for (int i = 1; i < contacts.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, contacts[i].gameObject.transform.position);
            if (distance < _closestDistance)
            {
                _closestDistance = distance;
                _closestEnemy = contacts[i].gameObject;
            }
        }
    }
}