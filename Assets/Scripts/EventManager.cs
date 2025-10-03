using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    static public EventManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Set up instance");
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    public Action<int, int> OnEnemyEnterRange;
    public Action<int, int> OnEnemyExitRange;
    public Action<int, int, float> OnDamage;
    public Action<int> OnEnemyDead;

    public void TriggerDamage(int attackerId, int targetId, float damageAmount)
    {
        OnDamage?.Invoke(attackerId, targetId, damageAmount);
    }
}
