using System;
using UnityEngine;

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
    
    public enum ShootingType
    {
        SINGLE,
        AUTO,
        COUNT
    }

    private void Awake()
    {
        weaponCollider = GetComponent<Collider2D>();
        
        if (center == null)
        {
            center = transform.Find("Center");
        }
    }
    
    private Collider2D weaponCollider = new Collider2D();

    public Collider2D GetCollider2D()
    {
        return weaponCollider;
    }

    [SerializeField] private Transform center;

    public Transform Center
    {
        get => center;
    }

    [SerializeField] private WeaponType type;
    public WeaponType Type
    {
        get => type;
    }

    [SerializeField] private ShootingType shootingType;
    public ShootingType GetShootingType()
    {
        return shootingType;
    }

    [SerializeField] private Vector3 defaultLocalPosition;
    public Vector3 DefaultLocalPosition
    {
        get => defaultLocalPosition;
    }

    [SerializeField] private int damage = 1;
    public int Damage
    {
        get => damage;
    }

    [SerializeField] private bool canPenetrate = false;
    public bool CanPenetrate
    {
        get => canPenetrate;
    }

    [SerializeField] private int maxMagCapacity = 4;
    public int MaxMagCapacity
    {
        get => maxMagCapacity;
    }

    [SerializeField] private int currentMagCapacity = 4;
    public int CurrentMagCapacity
    {
        get => currentMagCapacity;
    }
    
    public void Fire()
    {
        currentMagCapacity -= 1;
    }

    public void Equip()
    {
        GetCollider2D().enabled = false;
        gameObject.transform.SetParent(GameManager.Instance().WeaponAnchor, false);
        gameObject.transform.localPosition = DefaultLocalPosition;
    }
    
    public void Unequip(Vector3 pDropPosition)
    {
        transform.SetParent(null);
        transform.position = pDropPosition;
        transform.localEulerAngles = Vector3.zero;
        Invoke("EnableCollider", 3f);
    }

    private void EnableCollider()
    {
        GetCollider2D().enabled = true;
    }
}