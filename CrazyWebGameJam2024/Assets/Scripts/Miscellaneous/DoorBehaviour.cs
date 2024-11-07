using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private int nextSceneIndex;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.Instance().LoadScene(nextSceneIndex);
        }
    }
}