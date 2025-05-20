using UnityEngine;

public class Turret : PlayerAttackable, IDamageable
{
    [SerializeField] protected bool _active = false;

    public void SetActive(bool active)
    {
        _active = active;
    }
}