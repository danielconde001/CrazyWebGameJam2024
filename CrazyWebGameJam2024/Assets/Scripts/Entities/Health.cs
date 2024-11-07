using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int currentHealth;

    [SerializeField] protected UnityEvent onHurtEvent;
    [SerializeField] protected UnityEvent onDeathEvent;
    
    protected bool isDead = false;
    
    protected void Awake()
    {
        currentHealth = maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    
    public virtual void DeductHealth(int damage)
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
