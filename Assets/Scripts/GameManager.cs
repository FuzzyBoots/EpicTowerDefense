using QFSW.QC;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class GameManager : MonoBehaviour
{
    [SerializeField] Transform _enemyGoalPoint;
    
    public int Cash { get { return _currency; } }
    public Transform EnemyGoalPoint { get { return _enemyGoalPoint; } }

    public static GameManager Instance { get; private set; }

    [SerializeField] int _currency;
    
    [SerializeField] int _lives;
    
    [SerializeField] int _waveNum;
    
    [SerializeField] string _version = "1.0";

    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] GameObject _missilePrefab;
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one GameManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        UIManager.Instance.SetInitialText(_currency, _waveNum, _lives, _version);

        ObjectPoolManager.Instance.CreatePool(_explosionPrefab, 30, 100);
        ObjectPoolManager.Instance.CreatePool(_missilePrefab, 30, 100);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    [Command]
    public void ModifyCash(int amount)
    {
        _currency += amount;
        UIManager.Instance.SetCurrency(_currency);
    }

    [Command]
    public void ModifyLives(int amount)
    {
        _lives += amount;
        UIManager.Instance.SetLives(_lives);
    }

    [Command]
    public void SetWaveNumber(int number)
    {
        _waveNum = number;
        UIManager.Instance.SetWave(_waveNum);
    }

}
