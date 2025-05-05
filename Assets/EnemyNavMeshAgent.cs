using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavMeshAgent : MonoBehaviour
{
    NavMeshAgent _agent;

    Vector3 _end;
    
    public void SetEnd(Vector3 end)
    {
        _end = end;
    }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(_end);
    }
}
