using System.Threading.Tasks;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    public async override void Fire()
    {
        if (canFire == false)
            return;

        canFire = false;
        
        if (PlayerManager.Instance().CurrentlyEquippedWeapon.CurrentMagCapacity <= 0)
        {
            GameManager.Instance().GetAudioSource().PlayOneShot(emptyMagClip, 1f);
        }
        else
        {
            currentMagCapacity -= 1;
            GameManager.Instance().GetAudioSource().PlayOneShot(gunshotClip, 0.1f);
            ShowMuzzle();
            SpawnBullets();
        }
        
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
