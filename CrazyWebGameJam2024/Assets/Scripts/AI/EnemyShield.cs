using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private Health enemyHealth;

    public void ShieldHit(bool canPenetrate)
    {
        if(canPenetrate == true)
        {
            enemyHealth.DeductHealth(1);
        }
    }
}
