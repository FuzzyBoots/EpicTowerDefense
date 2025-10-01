using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    static public EventManager Instance;
    
    void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    public Action<int> OnEnemyEnterRange;
    public Action<int> OnEnemyExitRange;
    public Action<int, float> OnAttackStart;
    public Action<int> OnAttackEnd;
    public Action<int> OnEnemyDead;

    public void DispatchAttackStart(int id, float damageAmount)
    {
        OnAttackStart?.Invoke(id, damageAmount);
    }

    public void DispatchAttackEnd(int id)
    {
        OnAttackEnd?.Invoke(id);
    }
}
