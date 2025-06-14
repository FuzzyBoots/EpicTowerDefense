using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // The purpose of this class is to handle various bits of the UI
    [SerializeField] Text _currencyText;
    [SerializeField] Text _waveText;
    [SerializeField] Text _versionText;
    [SerializeField] Text _livesText;
    [SerializeField] Text _status;

    internal void SetCurrency(int currency)
    {
        _currencyText.text = currency.ToString();
    }

    internal void SetLives(int lives)
    {
        _livesText.text = lives.ToString();
        if (lives > 60)
        {
            _status.text = "Good";
            _status.color = Color.blue;
        }
        else if (lives > 20)
        {
            _status.text = "So-So";
            _status.color = Color.yellow;
        }
        else
        {
            _status.text = "Dire";
            _status.color = Color.red;
        }
    }

    internal void SetWave(int waveNum)
    {
        _waveText.text = waveNum.ToString();
    }

    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UIManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetInitialText(int currency, int waveNum, int lives, string version)
    {
        _currencyText.text = currency.ToString();
        _waveText.text = waveNum.ToString();
        _livesText.text = lives.ToString();
        _versionText.text = version;
    }
}
