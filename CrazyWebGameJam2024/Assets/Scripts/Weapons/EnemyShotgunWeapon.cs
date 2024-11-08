using System.Threading.Tasks;
using UnityEngine;

public class EnemyShotgunWeapon : Weapon
{
    [SerializeField] protected float spread;
    public float Spread
    {
        get => spread;
    }

    [SerializeField] protected int pelletsPerShot;
    public int PelletsPerShot
    {
        get => pelletsPerShot;
    }
    
    

    public async override void Fire()
    {
        if (canFire == false)
            return;

        canFire = false;
        AudioManager.Instance().PlaySFX(gunshotClip, 0.5f);
        SpawnBullets();
        
        fireTimer = fireRate * 0.001f;
    }

    protected override void SpawnBullets()
    {
        for (int i = 0; i < pelletsPerShot; i++)
        {
            BulletBehaviour spawnedBullet
                = Instantiate
                    (
                        bullet,
                        center.position,
                        Quaternion.identity
                    )
                    .GetComponent<BulletBehaviour>();
            
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            Vector2 spreadVector = (Vector2)center.right + new Vector2(x, y);
            
            Vector2 newBulletDir = spreadVector.normalized;
            newBulletDir = Vector2.ClampMagnitude(newBulletDir, 1);
            
            spawnedBullet.SetBulletDirection(newBulletDir);
            spawnedBullet.SetDamage(damage);
        }
        
    }
}
