
using System.Collections;
using UnityEngine;

public class GameOverDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject gameOverDisplay;

    [SerializeField] private DialogueTrigger dialogueTrigger;
    
    public void Display()
    {
        StartCoroutine(DisplayCoroutine());
    }

    private IEnumerator DisplayCoroutine()
    {
        
        dialogueTrigger.TriggerDialogue();
        
        while (HUDManager.Instance().DialogueSystem.isDialogueActive)
        {
            yield return new WaitForEndOfFrame();
        }
        
        gameOverDisplay.SetActive(true);
        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameOverDisplay.activeSelf)
        {
            SceneManager.Instance().LoadCurrentScene();
        }
    }
}
