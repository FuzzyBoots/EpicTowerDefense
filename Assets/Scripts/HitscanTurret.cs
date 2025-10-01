using GameDevHQ.FileBase.Gatling_Gun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class HitscanTurret : Emplacement, IDamageable
{
    [SerializeField] float _attackRange = 1f;
    [SerializeField] float _damagePerSecond = 10f;
    [SerializeField] float _turnSpeed = 1f;
    [SerializeField] float _targetingArc = 5f;

    [SerializeField, ReadOnly(true)] Collider _triggerCollider;

    [SerializeField] GameObject _turretObject;

    [SerializeField] GameObject _closestEnemy;
    [SerializeField] float _idleTurnSpeed = 0.2f;

    [SerializeField] Gatling_Gun _gun;
    Dictionary<int, GameObject> _enemyDict;
    List<GameObject> _availableEnemies;

    private void Start()
    {
        _enemyDict = new Dictionary<int, GameObject>();

        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = _attackRange;
        collider.isTrigger = true;

        _triggerCollider = collider;
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyTarget"))
        {
            _enemyDict.Add(other.gameObject.GetInstanceID(), other.gameObject);

            if (_closestEnemy != null)
            {
                _closestEnemy = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int instanceID = other.gameObject.GetInstanceID();
        if (other.CompareTag("EnemyTarget") && _enemyDict.ContainsKey(instanceID))
        {
            _enemyDict.Remove(instanceID);

            if (_closestEnemy && _closestEnemy.GetInstanceID() == instanceID)
            {
                _closestEnemy = null;   // We'll find them in the Aim
                // EventManager.Instance.StopFiring(_closestEnemy);
            }
        }
    }

    private int? GetClosestEnemyID()
    {
        GameObject enemy = _closestEnemy;
        while (!enemy.transform.root)
        {
            enemy = enemy.transform.root.gameObject;
            if (enemy.CompareTag("Enemy"))
            {
                return enemy.GetInstanceID();
            }
        }

        return null;
    }

    private void AimAtAndShootAtCurrentEnemy()
    {
        if (_closestEnemy == null)
        {
            _gun.Firing = false;
            return;
        }

        // Shouldn't need to do distance check since our trigger volume handles it

        Vector3 targetVector = _closestEnemy.transform.position - _turretObject.transform.position;
        // Rotate toward the hit
        _turretObject.transform.forward = Vector3.RotateTowards(_turretObject.transform.forward, targetVector, _turnSpeed * Time.deltaTime, 0f);

        // If in target arc, fire
        if (Vector3.Angle(_turretObject.transform.forward, targetVector) < _targetingArc && !_gun.Firing)
        {
            // Play the animation for firing
            _gun.Firing = true;

            int? enemyID = GetClosestEnemyID();
            if (enemyID != null)
            {
                EventManager.Instance.DispatchAttackStart(enemyID, _damagePerSecond);
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
        
        if (_enemyDict.Count < 1)
        {
            // General spin
            _turretObject.transform.Rotate(Vector3.up, _idleTurnSpeed * Time.deltaTime);
            _gun.Firing = false;
            return;
        }

        _closestEnemy = _enemyDict[0];
        float _closestDistance = Vector3.Distance(transform.position, _closestEnemy.transform.position);

        for (int i = 1; i < _enemyDict.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, _enemyDict[i].transform.position);
            if (distance < _closestDistance) {
                _closestDistance = distance;
                _closestEnemy = _enemyDict[i];
            }
        }        
    }
}
