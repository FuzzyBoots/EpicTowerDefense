using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavMeshAgent : MonoBehaviour
{
    [SerializeField] Transform _start;
    [SerializeField] Transform _end;

    NavMeshAgent _agent;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        transform.position = _start.position;
        _agent.SetDestination(_end.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
