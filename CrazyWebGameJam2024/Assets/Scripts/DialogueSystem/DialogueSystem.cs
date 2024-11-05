using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;
 
[RequireComponent(typeof(AudioSource))]
public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueBox;
    public RectTransform anchor;
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
 
    private Queue<DialogueLine> lines;
    private AudioSource audioSource;
    
    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;
    
    private void Awake()
    {
        lines = new Queue<DialogueLine>();
        audioSource = GetComponent<AudioSource>();
    }
    
    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

        PlayerManager.Instance().LosePlayerControl();
        
        lines.Clear();
        
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }
        
        characterIcon.sprite = dialogue.dialogueLines[0].character.icon;
        characterName.text = dialogue.dialogueLines[0].character.name;
        
        dialogueBox.gameObject.SetActive(true);
        anchor.DOMoveY(0f, 1f).OnComplete(() =>
        {
            DisplayNextDialogueLine();
        });
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
 
        DialogueLine currentLine = lines.Dequeue();
 
        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;
 
        StopAllCoroutines();
 
        StartCoroutine(TypeSentence(currentLine));
    }
 
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            audioSource.PlayOneShot(dialogueLine.character.dialogueClip, 1f);
            yield return new WaitForSeconds(typingSpeed);
        }
    }
 
    void EndDialogue()
    {
        isDialogueActive = false;
        anchor.DOMoveY(Screen.height, .5f).OnComplete(() =>
        {
            dialogueBox.gameObject.SetActive(false);
            PlayerManager.Instance().RegainPlayerControl();
        });
    }
}