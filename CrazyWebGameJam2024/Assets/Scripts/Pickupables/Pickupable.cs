using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pickupable : MonoBehaviour
{
    protected virtual void Pickup(Collider2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Pickup(collision);
    }
}
