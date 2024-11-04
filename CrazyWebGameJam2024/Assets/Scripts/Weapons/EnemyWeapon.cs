using System.Threading.Tasks;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    public async override void Fire()
    {
        if (canFire == false)
            return;

        canFire = false;

        ShowMuzzle();
        SpawnBullets();
        
        await Task.Delay(fireRate);
        canFire = true;
    }
    protected override void SpawnBullets()
    {
        BulletBehaviour spawnedBullet
            = Instantiate
                (
                    bullet,
                    center.position,
                    Quaternion.identity
                )
                .GetComponent<BulletBehaviour>();
        
        Vector2 newBulletDir = center.right;
        newBulletDir = Vector2.ClampMagnitude(newBulletDir, 1);
        spawnedBullet.SetBulletDirection(newBulletDir);
    }
}
