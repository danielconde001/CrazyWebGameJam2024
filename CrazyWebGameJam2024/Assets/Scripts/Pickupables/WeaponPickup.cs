using System;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponPickup : Pickupable
{
    [SerializeField] private AudioClip pickupSFX;

    private Weapon weapon;
    
    protected override void Awake()
    {
        base.Awake();
        weapon = GetComponent<Weapon>();
    }
    
    protected override void Pickup(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            AudioManager.Instance().PlaySFX(pickupSFX, 1.0f);
            PlayerManager.Instance().ReplaceEquippedGunWith(this.weapon);
        }
    }
}