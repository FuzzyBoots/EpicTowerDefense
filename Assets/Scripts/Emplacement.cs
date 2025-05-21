using UnityEngine;

public class Emplacement : PlayerAttackable, IDamageable
{
    [SerializeField] protected bool _active = false;

    [SerializeField] protected int _cost;

    public int Cost { get { return _cost; } protected set { _cost = value; } }

    public void SetActive(bool active)
    {
        _active = active;
    }
}