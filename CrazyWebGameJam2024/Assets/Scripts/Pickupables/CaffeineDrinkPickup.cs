using UnityEngine;

public class CaffeineDrinkPickup : Pickupable
{
    protected override void Pickup(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerTimeManipulation>().SlowTime();
            Destroy(gameObject);
        }
    }
}
