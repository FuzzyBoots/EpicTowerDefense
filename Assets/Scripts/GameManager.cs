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
    [SerializeField] Text _currencyText;

    [SerializeField] int _lives;
    [SerializeField] Text _livesText;

    [SerializeField] int _waveNum;
    [SerializeField] Text _waveText;

    [SerializeField] string _version = "1.0";
    [SerializeField] Text _versionText;

    [SerializeField] Text _status;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one GameManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetInitialText();
    }

    private void SetInitialText()
    {
        _currencyText.text = _currency.ToString();
        _waveText.text = _waveNum.ToString();
        _livesText.text = _lives.ToString();
        _versionText.text = _version;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    [Command]
    public void ModifyCash(int amount)
    {
        _currency += amount;
        _currencyText.text = _currency.ToString();
    }

    [Command]
    public void ModifyLives(int amount)
    {
        _lives += amount;
        _livesText.text = _lives.ToString();

        if (_lives > 60) {
            _status.text = "Good";
            _status.color = Color.blue;
        }
        else if (_lives > 20)
        {
            _status.text = "So-So";
            _status.color = Color.yellow;
        } else
        {
            _status.text = "Dire";
            _status.color = Color.red;
        }

    }

    [Command]
    public void SetWaveNumber(int number)
    {
        _waveNum = number;
        _waveText.text = _waveNum.ToString();
    }

}
