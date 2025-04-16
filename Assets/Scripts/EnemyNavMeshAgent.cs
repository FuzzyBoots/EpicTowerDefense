using AmazingAssets.AdvancedDissolve.Examples;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] Transform _start;
    [SerializeField] Transform _end;

    [SerializeField] PlayerAttackable _nearestTarget;

    Animator _animator;

    [SerializeField] float _health = 50f;

    NavMeshAgent _agent;

    bool _isDead = false;

    [SerializeField] float _deathDelay = 0.5f;

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
        _animator.SetTrigger("Die");
        _isDead = true;

        gameObject.layer = LayerMask.NameToLayer("Default");

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

    public bool IsDead()
    {
        return _isDead;
    }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        transform.position = _start.position;
        _agent.SetDestination(_end.position);

        _animator = GetComponent<Animator>();
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
            float closestDistance = Mathf.Infinity;
            foreach (PlayerAttackable target in targets)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.gameObject.transform.position);
                if (distanceToTarget < closestDistance)
                {
                    NavMeshPath path = new NavMeshPath();
                    if (_agent.CalculatePath(target.transform.position, path) && path.status == NavMeshPathStatus.PathComplete)
                    {
                        // Do we allow partial paths? Might make sense for ranged attacks.
                        _nearestTarget = target;
                        closestDistance = distanceToTarget;
                    }
                }
            }
        }

        switch (_state) {
            case EnemyState.Moving:
                _agent.SetDestination(_end.position);
                break;
            case EnemyState.Attacking:
                _agent.SetDestination(_nearestTarget.gameObject.transform.position);
                PerformAttack(_nearestTarget);
                break;
        }

        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    protected abstract void PerformAttack(PlayerAttackable nearestTarget);

    protected abstract PlayerAttackable[] GetTargets();
}
