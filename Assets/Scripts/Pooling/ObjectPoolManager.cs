using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<GameObject, ObjectPool<GameObject>> spawnPools = new Dictionary<GameObject, ObjectPool<GameObject>>();

    public static ObjectPoolManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ObjectPoolManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Creates a new ObjectPool for the given prefab if one doesn't already exist.
    /// </summary>
    /// <param name="prefab">The GameObject prefab to create a pool for.</param>
    /// <param name="defaultCapacity">The initial number of objects to pre-allocate.</param>
    /// <param name="maxSize">The maximum number of objects the pool can hold.</param>
    public void CreatePool(GameObject prefab, int defaultCapacity = 10, int maxSize = 100)
    {
        if (!spawnPools.ContainsKey(prefab))
        {
            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                () => Instantiate(prefab),
                (obj) => obj.SetActive(true),
                (obj) => obj.SetActive(false),
                (obj) => Destroy(obj.gameObject),
                true,
                defaultCapacity,
                maxSize
            );
            spawnPools.Add(prefab, pool);
            Debug.Log($"Created a new pool for prefab: {prefab.name}");
        }
        else
        {
            Debug.LogWarning($"Pool already exists for prefab: {prefab.name}");
        }
    }

    /// <summary>
    /// Gets an object from the pool associated with the given prefab.
    /// Ensure a pool has been created for this prefab first.
    /// </summary>
    /// <param name="prefab">The GameObject prefab to get an instance of.</param>
    /// <returns>An instance of the prefab from its pool, or null if the pool doesn't exist.</returns>
    public GameObject GetFromPool(GameObject prefab)
    {
        if (spawnPools.TryGetValue(prefab, out var pool))
        {
            return pool.Get();
            Debug.Log($"Retrieved a {prefab.name} from the pool.");
        }
        else
        {
            Debug.LogError($"No pool found for prefab: {prefab.name}. Ensure you have created a pool using CreatePool() first.");
            return null;
        }
    }

    /// <summary>
    /// Returns an object to its associated pool.
    /// </summary>
    /// <param name="pooledObject">The GameObject to return to the pool.</param>
    /// <param name="originalPrefab">The original prefab of the pooled object.</param>
    public void ReturnToPool(GameObject pooledObject, GameObject originalPrefab)
    {
        if (spawnPools.TryGetValue(originalPrefab, out var pool))
        {
            pool.Release(pooledObject);
        }
        else
        {
            Debug.LogWarning($"No pool found for prefab: {originalPrefab.name}. Destroying the object.");
            Destroy(pooledObject);
        }
    }

    /// <summary>
    /// Checks if a spawn pool exists for the given prefab.
    /// </summary>
    /// <param name="prefab">The GameObject prefab to check for.</param>
    /// <returns>True if a pool exists, false otherwise.</returns>
    public bool HasPoolFor(GameObject prefab)
    {
        return spawnPools.ContainsKey(prefab);
    }

    // Optional: Method to get the pool for a specific prefab if needed
    public ObjectPool<GameObject> GetPool(GameObject prefab)
    {
        if (spawnPools.TryGetValue(prefab, out var pool))
        {
            return pool;
        }
        return null;
    }
}