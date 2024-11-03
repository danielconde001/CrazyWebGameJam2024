using UnityEngine;

public class ShotgunWeapon : Weapon
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
            Vector2 spreadVector = PlayerManager.Instance().GetPlayerAim().GetAimDirection() + new Vector2(x, y);
            
            Vector2 newBulletDir = spreadVector.normalized;
            newBulletDir = Vector2.ClampMagnitude(newBulletDir, 1);
            
            spawnedBullet.SetBulletDirection(newBulletDir);
        }
        
    }
}
