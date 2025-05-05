using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn", menuName = "ScriptableObjects/SpawnObject", order = 1)]
public class SpawnObject : ScriptableObject
{
    public EnemyNavMeshAgent _agent;
    public int _count = 1;
    public float _delay = 1f;
}
