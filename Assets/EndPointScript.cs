using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyNavMeshAgent>(out EnemyNavMeshAgent agent))
        {
            GameManager.Instance.ModifyLives(-1);

            agent.Celebrate();
            agent.Disappear();
        }
    }
}
