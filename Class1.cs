using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour, IDamage
{
    public enum E_States
    {
        NONE,
        IDLE,
        CHASING,
        RUNNING_AWAY,
        ATTACK,
        DEAD,
    }

    public enum EnemyType
    {
        NONE,
        MELEE,
        RANGE,
        SPECIAL,
    }


    [Space(2), Header("ENEMY TYPE"), Tooltip("THE TYPE MAY CHANGE THE ENEMY BEHAVIOUR")]
    [SerializeField] protected EnemyType _eType;

    [Space(2), Header("CURRENT STATE"), Tooltip("THE CURRENT ACTIVE BEHAVIOUR")]
    [SerializeField] protected E_States _currentState;

    public Action OnEnemyDeath;

    [Header("Scriptable Objects")]
    [SerializeField] protected CharacterClass _enemyStats;
    [SerializeField] protected SO_DropRate _dropRate;

    [Space(5)]
    protected Animator _anim;
    protected GameObject _player;
    protected bool _isDead;
    protected Collider _collider;
    protected DamageTrigger _hitbox;
    protected Rigidbody _rigidbody;
    protected bool _canMove = true;

    [Header("Enemy Stats")]
    [SerializeField] protected float _statsMultiplier = 3f;
    [SerializeField] protected float _level;
    protected float _health;
    protected float _power;
    public float power { get { return _power; } }
    protected float _speed;
    protected float _defense;

    [Space(5)]

    [Header("Random Position")]
    [SerializeField] protected bool _randomSpawn;
    [Tooltip("THIS VECTOR3 IS USED TO SET A POSITION FOR THE ENEMY SPAWN")]
    [SerializeField] protected Vector3 _fixedPosition;
    [SerializeField, Range(-50f, 50f)] protected float _xPos;
    [SerializeField, Range(-50f, 50f)] protected float _zPos;

    [Header("Enemy sight"), Tooltip("THE HIGHER THE VALUES, THE FAR THE INTERACTION WILL BE")]
    [SerializeField, Range(0f, 100f)] protected float _distanceToFindPlayer;
    [Header("Attack Distance"), Tooltip("THE HIGHER THE VALUES, THE FAR THE INTERACTION WILL BE")]
    [SerializeField, Range(0f, 50f)] protected float _distanceToAttack;
    protected NavMeshAgent _agent;

    protected virtual void Awake()
    {
        _health = _enemyStats.health;
        _power = _enemyStats.power;
        _speed = _enemyStats.speed;
        _defense = _enemyStats.defense;

        _hitbox = GetComponentInChildren<DamageTrigger>();
        _anim = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = _distanceToAttack;

        _collider = GetComponent<Collider>();
        _collider.enabled = true;
    }

    protected virtual void Start()
    {
        _player = FindAnyObjectByType<PlayerBase>().gameObject;

        UpdateStats();

        _currentState = E_States.IDLE;
    }

    protected virtual void Update()
    {
        switch (_currentState)
        {
            case E_States.IDLE:
                State_IDLE();
                break;

            case E_States.CHASING:
                State_CHASING();
                break;

            case E_States.RUNNING_AWAY:
                State_RUNNING_AWAY();
                break;

            case E_States.ATTACK:
                State_ATTACK();
                break;

            case E_States.DEAD:
                State_DEAD();
                break;
        }
    }

    protected virtual void UpdateStats()
    {
        _health += _level * _statsMultiplier;
        _power += _level * _statsMultiplier;
        _defense += _level * _statsMultiplier;
        _speed += _level * _statsMultiplier;
        _agent.speed = _speed;

        _hitbox?.SetPowerValue(power);
    }

    public void SetFixedPosition(Vector3 pos)
    {
        _fixedPosition = pos;
    }

    #region - Statemachine Methods -
    protected virtual void State_IDLE()
    {
        if (PlayerDistance() < _distanceToFindPlayer)
        {
            _currentState = E_States.CHASING;
        }
    }

    protected virtual void State_CHASING()
    {
        _agent.SetDestination(_player.transform.position);

        if (PlayerDistance() >= _distanceToFindPlayer)
        {
            _agent.ResetPath();
            _agent.SetDestination(_fixedPosition);
            _currentState = E_States.IDLE;
        }

        if (PlayerDistance() <= _distanceToAttack)
        {
            _agent.ResetPath();
            _currentState = E_States.ATTACK;
        }
    }

    protected virtual void State_RUNNING_AWAY()
    {
        Vector3 fleeDirection = (transform.position - _player.transform.position).normalized;
        Vector3 fleePoint = transform.position + fleeDirection * _distanceToFindPlayer;

        _agent.SetDestination(fleePoint);

        if (PlayerDistance() >= _distanceToFindPlayer)
        {
            _agent.ResetPath();
            _currentState = E_States.IDLE;
        }
    }

    protected virtual void State_ATTACK()
    {
        if (_eType == EnemyType.MELEE)
        {
            if (PlayerDistance() > _distanceToAttack)
            {
                _currentState = E_States.CHASING;
            }
        }

        if (_eType == EnemyType.RANGE)
        {
            if (PlayerDistance() > _distanceToAttack && PlayerDistance() < _distanceToFindPlayer)
            {
                _currentState = E_States.CHASING;
            }

            if (PlayerDistance() < _distanceToAttack - 2)
            {
                _currentState = E_States.RUNNING_AWAY;
            }
        }
    }

    protected virtual void State_DEAD()
    {
        Death();
    }
    #endregion

    #region - Interface Methods -
    //METHOD TO BE CALLED FOR SLOW OBJECT SPEED
    public void Slowdown(float speed, float time)
    {
        StartCoroutine(SlowSpeed(speed, time));
    }

    public void Damage(float damage)
    {
        float dmgCalc = damage - _defense;

        if (dmgCalc <= 0)
        {
            dmgCalc = 1;
        }

        _health -= dmgCalc;

        if (_health <= 0)
        {
            _health = 0;
            _currentState = E_States.DEAD;
        }
    }

    #endregion

    public void Anim_CanMove()
    {
        _canMove = !_canMove;
        if (_canMove) _agent.speed = _speed;
        else _agent.speed = 0;
    }

    void Death()
    {
        _isDead = true;
        _agent.speed = 0;
        _anim.SetTrigger("Death");
        _collider.enabled = false;
        StartCoroutine(DeathSequence());
    }

    protected float PlayerDistance()
    {
        if (_player != null)
            return Vector3.Distance(transform.position, _player.transform.position);
        else return 0;
    }

    protected virtual void SpawnPosition()
    {
        if (!_randomSpawn)
        {
            transform.position = _fixedPosition;
        }

        Vector3 _randomSpawnPos = new Vector3(Random.Range(-_xPos, _xPos), 0, Random.Range(-_zPos, _zPos));

        Vector3 cameraPos = Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, Camera.main.nearClipPlane));

        if (Vector3.Distance(_randomSpawnPos, cameraPos) > 20 && Vector3.Distance(_randomSpawnPos, cameraPos) < 50)
        {
            transform.position = _randomSpawnPos;
        }
        else
        {
            _randomSpawnPos = new Vector3(Random.Range(-_xPos, _xPos), 0, Random.Range(-_zPos, _zPos));
            transform.position = _randomSpawnPos;
        }
    }

    IEnumerator SlowSpeed(float speed, float time)
    {
        float speedInitialValue = _agent.speed;

        _agent.speed -= speed;

        yield return new WaitForSeconds(time);

        _agent.speed = speedInitialValue;
    }

    IEnumerator DeathSequence()
    {
        OnEnemyDeath?.Invoke();
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2);
        _dropRate.CoinSpawner(Random.Range(1, 3), transform, _player.transform);
        _dropRate.ExpSpawner(Random.Range(1, 3), transform, _player.transform);
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        _health = _enemyStats.health;

        SpawnPosition();

        if (_player != null)
        {
            Vector3 _playerPos = _player.transform.position;

            float _distance = Vector3.Distance(_playerPos, _player.transform.position);

            _currentState = E_States.IDLE;
        }

        GetComponent<Collider>().enabled = true;

        _isDead = false;
    }
}
