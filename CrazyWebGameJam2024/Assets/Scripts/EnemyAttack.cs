using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyAttack : MonoBehaviour
{
    private EnemyAI_OLD enemyAI;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI_OLD>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyAI.IsStopped == false)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Health>().DeductHealth(1);
            }
        }
    }
}
