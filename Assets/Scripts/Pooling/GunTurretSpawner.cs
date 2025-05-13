using UnityEngine;
using UnityEngine.Pool;

public class GunTurretSpawner : MonoBehaviour
{
    private ObjectPool<HitscanTurret> _hitscanTurretObjectPool;
    [SerializeField] private int _amountToPool;
    [SerializeField] private int _maxCapacity;
    [SerializeField] private HitscanTurret _gunTurretPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _hitscanTurretObjectPool = new ObjectPool<HitscanTurret>(createTurretFunc,
        getTurretFunc,
        releaseTurretFunc,
        destroyTurretFunc,
        true,
        _amountToPool,
        _maxCapacity);
    }

    private HitscanTurret createTurretFunc()
    {
        HitscanTurret tmp = Instantiate(_gunTurretPrefab);
        tmp.gameObject.SetActive(false);

        return tmp;
    }

    private void getTurretFunc(HitscanTurret turret)
    {
        turret.gameObject.SetActive(true);
    }

    private void releaseTurretFunc(HitscanTurret turret)
    {
        turret.gameObject.SetActive(false);
    }

    private void destroyTurretFunc(HitscanTurret turret)
    {
        Destroy(turret);
    }
}
