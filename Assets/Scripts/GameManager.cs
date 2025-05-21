using UnityEngine;

class GameManager : MonoBehaviour
{
    [SerializeField] Transform _enemyGoalPoint;
    [SerializeField] int _currency;

    public int Cash { get { return _currency; } }
    public Transform EnemyGoalPoint { get { return _enemyGoalPoint; } }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one GameManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void ModifyCash(int amount)
    {
        _currency += amount;
        // Update the UI
    }
}
