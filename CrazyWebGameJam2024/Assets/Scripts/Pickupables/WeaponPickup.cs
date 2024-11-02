using System;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponPickup : Pickupable
{
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
            PlayerManager.Instance().ReplaceEquippedGunWith(this.weapon);
        }
    }
}