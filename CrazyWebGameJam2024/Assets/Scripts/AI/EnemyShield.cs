using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private Health enemyHealth;

    public void ShieldHit(bool canPenetrate, int damage = 0)
    {
        if(canPenetrate == true)
        {
            enemyHealth.DeductHealth(damage);
        }
    }
}
