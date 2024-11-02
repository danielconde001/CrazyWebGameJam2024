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

    [SerializeField] private bool isEquppiedByPlayer = false;
    public bool IsEquppiedByPlayer
    {
        get => isEquppiedByPlayer;
    }
    
    public void Fire()
    {
        if (currentMagCapacity > 0)
        {
            currentMagCapacity -= 1;
            
        }
    }
}
