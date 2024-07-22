using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public DialogueLine initialDialogue;
    public DialogueLine standbyDialogue;
    public DialogueLine finishedDialogue;

    public bool hasShownInitialDialogue = false;
    public bool hasFinishedQuest = false;

    public void Interact()
    {
        if (!hasShownInitialDialogue)
        {
            DialogueManager.Instance.StartCharacterDialogue(initialDialogue);
            hasShownInitialDialogue = true;
        }
        else if (!hasFinishedQuest)
        {
            DialogueManager.Instance.StartCharacterDialogue(standbyDialogue);
        }
        else
        {
            DialogueManager.Instance.StartCharacterDialogue(finishedDialogue);
        }
    }


}
