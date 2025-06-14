using AmazingAssets.AdvancedDissolve.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public enum EnemyState
{
    Moving,
    Attacking
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public abstract class EnemyNavMeshAgent : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyState _state = EnemyState.Moving;
    [SerializeField] Vector3 _end;

    [SerializeField] PlayerAttackable _nearestTarget;

    protected Animator _animator;

    [SerializeField] float _health = 50f;

    [SerializeField] int _reward = 100;

    NavMeshAgent _agent;

    bool _isDead = false;

    [SerializeField] float _deathDelay = 0.5f;

    [SerializeField] GameObject _offsetTarget;
    public GameObject OffsetTarget { get { return _offsetTarget; } private set { _offsetTarget = value; } }

    public void Damage(float damage)
    {
        _health -= damage;

        if (_health <= 0 && !_isDead)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        GameManager.Instance.ModifyCash(_reward);
        _animator.SetTrigger("Die");
        _isDead = true;

        // Set it to default so that the enemy-seeking script doesn't waste cycles
        gameObject.layer = LayerMask.NameToLayer("Default");

        Disappear();
    }

    public bool IsDead()
    {
        return _isDead;
    }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        Assert.IsNotNull(_agent, "NavMeshAgent not set up!");

        _animator = GetComponent<Animator>();
        Assert.IsNotNull(_animator, "No animator found");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead())
        {
            return;
        }

        if (_nearestTarget == null)
        {
            // Try fetching one
            PlayerAttackable[] targets = GetTargets();
            if (targets.Length > 0)
            {
                Debug.Log($"Detected {targets.Length}");
            }
            float closestDistance = Mathf.Infinity;
            foreach (PlayerAttackable target in targets)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.gameObject.transform.position);
                if (distanceToTarget < closestDistance)
                {
                    NavMeshPath path = new NavMeshPath();
                    _nearestTarget = target;
                    closestDistance = distanceToTarget;
                }
            }
        }

        _state = _nearestTarget == null ? EnemyState.Moving : EnemyState.Attacking;

        switch (_state) {
            case EnemyState.Moving:
                if (_agent)
                {
                    _agent.SetDestination(_end);
                }
                break;
            case EnemyState.Attacking:
                if (_nearestTarget.IsDead())
                {
                    _nearestTarget = null;
                    break;
                }
                _agent.SetDestination(_nearestTarget.gameObject.transform.position);
                PerformAttack(_nearestTarget);
                break;
        }
               
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    protected abstract void PerformAttack(PlayerAttackable nearestTarget);

    protected abstract PlayerAttackable[] GetTargets();

    internal void SetEnd(Vector3 position)
    {
        _end = position;
    }

    internal void Celebrate()
    {
        _animator.SetBool("Celebrating", true);
    }

    internal void Disappear()
    {
        // Disable the colliders
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        _agent.isStopped = true;
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            AnimateCutoutAndDestroy script = gameObject.AddComponent<AnimateCutoutAndDestroy>();

            //Instantiate material and assign it to the script
            script.material = renderer.material;
        }
    }
}
