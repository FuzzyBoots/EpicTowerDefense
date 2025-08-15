using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
        // Subscribe to the damage event
        GameEvents.onDamageTaken += TakeDamage;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks
        GameEvents.onDamageTaken -= TakeDamage;
    }

    private void TakeDamage(GameObject target, float damageAmount)
    {
        // Check if this health component is on the game object that should take damage
        if (target != this.gameObject) return;

        _currentHealth -= damageAmount;

        // Debug.Log($"{gameObject.name} took {damageAmount} damage. Health is now {_currentHealth}");

        if (_currentHealth <= 0)
        {
            // Announce that this game object has died
            GameEvents.EntityDied(this.gameObject);
            // Optionally, destroy the object after announcing death
            Destroy(gameObject);
        }
    }
}