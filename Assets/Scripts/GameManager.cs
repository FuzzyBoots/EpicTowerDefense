using UnityEngine;

class GameManager : MonoBehaviour
{
    [SerializeField] Transform _enemyGoalPoint;
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
}
