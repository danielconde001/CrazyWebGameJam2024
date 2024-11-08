using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private bool hasDialogue = false;
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    [SerializeField] private string sceneName;
    
    private bool entered = false;
    private bool isLoadingAScene = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out CapsuleCollider2D capsule))
        {
            if (other != capsule) return;
            
            entered = true;
            if (!hasDialogue)
            {
                GameManager.Instance().TimeManipulator.NormalizeTime();
                SceneManager.Instance().LoadSceneWithFade(sceneName);
            }
            else
            {
                _dialogueTrigger.TriggerDialogue();
            }
        }
    }

    private void Update()
    {
        if (entered && !isLoadingAScene)
        {
            if (hasDialogue)
            {
                if (!HUDManager.Instance().DialogueSystem.isDialogueActive)
                {
                    isLoadingAScene = true;
                    GameManager.Instance().TimeManipulator.NormalizeTime();
                    SceneManager.Instance().LoadSceneWithFade(sceneName);
                }
            }
        }
    }
}