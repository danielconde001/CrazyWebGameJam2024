using UnityEngine;

public class CaffeineDrinkPickup : Pickupable
{
    protected override void Pickup(Collider2D collision)
    {
         if(collision.tag == "Player")
         {
             GameManager.Instance().TimeManipulator.SlowTime();
             Destroy(gameObject);
         }
    }
 }
