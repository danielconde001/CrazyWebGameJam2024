using System.Collections;
using UnityEngine;

public class LevelConclusionDisplayer : MonoBehaviour
{
    [SerializeField] protected GameObject display;
    [SerializeField] protected DialogueTrigger dialogueTrigger;
    
    public void Display()
    {
        StartCoroutine(DisplayCoroutine());
    }

    protected IEnumerator DisplayCoroutine()
    {

        if (dialogueTrigger != null)
        {
            dialogueTrigger.TriggerDialogue();
        }

        GameManager.Instance().TimeManipulator.NormalizeTime();
        
        while (HUDManager.Instance().DialogueSystem.isDialogueActive)
        {
            yield return new WaitForEndOfFrame();
        }
        
        display.SetActive(true);
        yield return null;
    }

    protected virtual void Update()
    {
        
    }
}
