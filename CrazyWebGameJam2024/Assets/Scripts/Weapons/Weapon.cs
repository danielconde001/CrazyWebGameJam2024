using UnityEngine;
using System.Threading.Tasks;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        SHOTGUN,
        PISTOL,
        ASSAULT,
        SNIPER,
        COUNT
    }

    protected Collider2D weaponCollider = new Collider2D();
    public Collider2D GetCollider2D()
    {
        return weaponCollider;
    }

    [SerializeField] protected Transform center;
    public Transform Center
    {
        get => center;
    }

    [SerializeField] protected WeaponType type;
    public WeaponType Type
    {
        get => type;
    }

    [SerializeField] protected Vector3 defaultLocalPosition;
    public Vector3 DefaultLocalPosition
    {
        get => defaultLocalPosition;
    }

    [SerializeField] protected int damage = 1;
    public int Damage
    {
        get => damage;
    }

    [SerializeField] protected int fireRate = 200;
    public int FireRate
    {
        get => fireRate;
    }
    
    [SerializeField] protected bool canPenetrate = false;
    public bool CanPenetrate
    {
        get => canPenetrate;
    }

    [SerializeField] protected int maxMagCapacity = 4;
    public int MaxMagCapacity
    {
        get => maxMagCapacity;
    }

    [SerializeField] protected int currentMagCapacity = 4;
    public int CurrentMagCapacity
    {
        get => currentMagCapacity;
    }
    
    [SerializeField] protected float cameraShakeForce = 1f;
    public float CameraShakeForce
    {
        get => cameraShakeForce;
    }
    
    [SerializeField] protected AudioClip gunshotClip;
    public AudioClip GunshotClip
    {
        get => gunshotClip;
    }
    
    [SerializeField] protected AudioClip emptyMagClip;

    public AudioClip EmptyMagClip
    {
        get => emptyMagClip;
    }

    [SerializeField] protected GameObject bullet;
    public GameObject Bullet
    {
        get => bullet;
    }
    
    [SerializeField] protected SpriteRenderer spriteRenderer;
    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
    }
    
    protected bool canFire = true;
    protected float fireTimer = 0f;
    
    protected virtual void Awake()
    {
        weaponCollider = GetComponent<Collider2D>();
        
        if (center == null)
        {
            center = transform.Find("Center");
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    protected virtual void Update()
    {
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
        else
        {
            canFire = true;
        }
    }

    public async virtual void Fire()
    {
        if (canFire == false)
        {
            return;
        }

        canFire = false;
        
        if (PlayerManager.Instance().CurrentlyEquippedWeapon.CurrentMagCapacity <= 0)
        {
            GameManager.Instance().GetAudioSource().PlayOneShot(emptyMagClip, 1f);
        }
        else
        {
            currentMagCapacity -= 1;
            GameManager.Instance().GetAudioSource().PlayOneShot(gunshotClip, 0.5f);
            
            ShakeCamera();
            SpawnBullets();
        }
        
        HUDManager.Instance().UpdateAmmoInfo();
        
        fireTimer = fireRate * 0.001f;
    }

    public void Equip()
    {
        GetCollider2D().enabled = false;
        gameObject.transform.SetParent(PlayerManager.Instance().WeaponAnchor, false);
        gameObject.transform.localPosition = DefaultLocalPosition;
    }
    
    public void Unequip(Vector3 pDropPosition)
    {
        transform.SetParent(null);
        transform.position = pDropPosition;
        transform.localEulerAngles = Vector3.zero;
        Invoke("EnableCollider", 3f);
    }

    protected void EnableCollider()
    {
        GetCollider2D().enabled = true;
    }
    
    protected void ShakeCamera()
    {
        CameraShake.Instance().ShakeCamera(.5f,cameraShakeForce);
    }

    protected virtual void SpawnBullets()
    {
        BulletBehaviour spawnedBullet
            = Instantiate
                (
                    bullet,
                    center.position,
                    Quaternion.identity
                )
                .GetComponent<BulletBehaviour>();
        
        Vector2 newBulletDir = PlayerManager.Instance().GetPlayerAim().GetAimDirection().normalized;
        newBulletDir = Vector2.ClampMagnitude(newBulletDir, 1);
        spawnedBullet.SetBulletDirection(newBulletDir);
        spawnedBullet.SetDamage(damage);
        spawnedBullet.SetCanPenetrate(canPenetrate);
    }
}
