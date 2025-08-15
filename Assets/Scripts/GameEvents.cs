using System;
using UnityEngine;

public static class GameEvents
{
    // Event fires when any entity should take damage.
    // Parameters: GameObject to be damaged, float amount of damage.
    public static event Action<GameObject, float> onDamageTaken;
    public static void DamageTaken(GameObject target, float damageAmount)
    {
        onDamageTaken?.Invoke(target, damageAmount);
    }

    // Event fires when any entity dies.
    // Parameter: GameObject that died.
    public static event Action<GameObject> onEntityDied;
    public static void EntityDied(GameObject deadEntity)
    {
        onEntityDied?.Invoke(deadEntity);
    }
}