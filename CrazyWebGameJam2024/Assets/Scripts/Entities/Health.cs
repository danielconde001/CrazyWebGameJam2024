using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private uint maxHealth;
    private uint currentHealth;

    [SerializeField] private UnityEvent onHurtEvent;
    [SerializeField] private UnityEvent onDeathEvent;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public uint GetCurrentHealth()
    {
        return currentHealth;
    }

    public void DeductHealth(uint damage)
    {
        if (currentHealth == 0) 
            return;

        else
        {
            currentHealth -= damage;

            if (currentHealth == 0)
            {
                onDeathEvent.Invoke();
            }
            else
            {
                onHurtEvent.Invoke();
            }
        }
    }
}
