using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustMovingEnemy : EnemyNavMeshAgent
{
    protected override PlayerAttackable[] GetTargets()
    {
        return new PlayerAttackable[] { };
    }

    protected override void PerformAttack(PlayerAttackable nearestTarget)
    {
        // Not Attacking
    }
}
