using UnityEngine;

public class EnemyWeapon : Weapon
{
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
