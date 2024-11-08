using UnityEngine;
public class EnemyKillable : Killable
{
    [SerializeField] private bool destroyAfterDeath = false;
    [SerializeField] private float destroyAfterDeathTime = 2f;
    
    public override void Death()
    {
        base.Death();
        GameManager.Instance().AddKillCount();
        if (destroyAfterDeath)
        {
            Invoke("DestroyEnemy", destroyAfterDeathTime);
        }
    }

    protected virtual void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
