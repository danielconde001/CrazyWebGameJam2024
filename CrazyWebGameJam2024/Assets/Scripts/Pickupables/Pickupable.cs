using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pickupable : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {
        
    }

    protected virtual void Awake()
    {
        
    }

    protected virtual void Pickup(Collider2D collision)
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Pickup(collision);
    }
    
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
