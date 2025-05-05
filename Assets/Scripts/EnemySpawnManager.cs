using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPoolManager))]
public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] WaveObject[] _waves;

    private WaitForSeconds _pauseDelay = new(0.1f);

    [SerializeField] bool _spawnActive = false;
    [SerializeField] Transform _spawnPos;

    [SerializeField] GameObject _toiletMechPrefab;
    [SerializeField] GameObject _dronePrefab;

    ObjectPoolManager _poolManager;

    private void Start()
    {
        _poolManager = GetComponent<ObjectPoolManager>();
        // Let's initialize the ObjectPoolManager
        _poolManager.CreatePool(_toiletMechPrefab, 15, 30);
        _poolManager.CreatePool(_dronePrefab, 10, 30);


        StartCoroutine(SpawnUnits());

        StartSpawning();
    }

    private void StartSpawning()
    {
        _spawnActive = true;
    }

    public IEnumerator SpawnUnits()
    {
        foreach (WaveObject wave in _waves)
        {
            Debug.Log("Wave " + wave.name);
            foreach (SpawnObject spawn in wave._spawns)
            {
                Debug.Log("Spawn " + spawn.name + " Count: " + spawn._count);
                for (int i = 0; i < spawn._count; i++)
                {
                    while (_spawnActive == false)
                    {
                        yield return _pauseDelay;
                    }
                    Debug.Log($"Spawning {spawn._agent.name} #{i+1}");

                    GameObject agent = _poolManager.GetFromPool(spawn._agent.gameObject);
                    agent.transform.position = _spawnPos.position;
                    EnemyNavMeshAgent agentComponent = agent.GetComponent<EnemyNavMeshAgent>();
                    if (agentComponent != null)
                    {
                        agentComponent.SetEnd(GameManager.Instance.EnemyGoalPoint.position);
                    }

                    yield return new WaitForSeconds(spawn._delay);
                }
            }
        }
    }
}
