using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    [SerializeField] private UnityEvent onHurtEvent;
    [SerializeField] private UnityEvent onDeathEvent;
    
    private bool isDead = false;
    
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void DeductHealth(int damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            onDeathEvent.Invoke();
            isDead = true;
        }
        else
        {
            onHurtEvent.Invoke();
        }
    }
}
