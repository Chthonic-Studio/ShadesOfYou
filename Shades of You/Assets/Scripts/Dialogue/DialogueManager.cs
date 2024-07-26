using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject dialogueUIPrefab;
    private DialogueUIController dialogueUIController;
    private DialogueLine currentDialogueLine;
    private bool isDialogueFinished = false;
    private bool activeUI = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GameObject dialogueUI = GameObject.Find("DialogueUICanvas");
        if (dialogueUI != null)
        {
            GameObject dialogueUIInstance = Instantiate(dialogueUIPrefab);
            if (dialogueUIInstance != null)
            {
                dialogueUIInstance.transform.SetParent(dialogueUI.transform, false);
                dialogueUIInstance.transform.localScale = Vector3.one; // Reset the local scale
                dialogueUIController = dialogueUIInstance.GetComponent<DialogueUIController>();
                dialogueUIController.HideDialogueUI();
            }
            else
            {
                Debug.LogError("Failed to instantiate DialogueUI prefab");
            }
        }
        else
        {
            Debug.LogError("dialogueUI GameObject not found");
        }
        Debug.Log("Dialogue Manager initialized");
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (activeUI)
            {
                Debug.Log("activeUI is true");
                if (currentDialogueLine != null && dialogueUIController != null)
                {
                    Debug.Log("currentDialogueLine and dialogueUIController are not null");
                    if (isDialogueFinished)
                    {
                        Debug.Log("Calling ShowNextDialoguePart");
                        ShowNextDialoguePart();
                    }
                    else
                    {
                        StopAllCoroutines();
                        isDialogueFinished = true;
                        dialogueUIController.ShowWholeDialogue(currentDialogueLine.text);
                    }
                }
                else
                {
                    Debug.Log("currentDialogueLine or dialogueUIController is null");
                }
            }
            else
            {
                Debug.Log("activeUI is false");
            }
        }
    }

    public void StartCharacterDialogue(DialogueLine dialogueLine)
    {
        if (activeUI)
        {
            Debug.Log("Dialogue already active");
            return;
        }

        Debug.Log("StartCharacterDialogue called by Dialogue Manager");
        activeUI = true;
        dialogueUIController.gameObject.SetActive(true);
        currentDialogueLine = dialogueLine;
        Debug.Log("CurrentDialogueLine set to:" + currentDialogueLine);

        // Activate the dialogueBox, dialogueNameBox, and portraitBox GameObjects
        dialogueUIController.ActivateDialogue();
        dialogueUIController.ShowCharacterDialogue(dialogueLine);
    }

    public void ContinueDialogue(DialogueLine dialogueLine)
    {
        currentDialogueLine = dialogueLine;
        if (currentDialogueLine.dialogueType == DialogueLine.DialogueType.Choice)
        {
            StartChoiceDialogue(currentDialogueLine);
        }
        else
        {
            StartCoroutine(TypeDialogue(dialogueLine.text));
        }
    }

    public void ShowNextDialoguePart()
    {
        if (currentDialogueLine.continuation != null)
        {
            ContinueDialogue(currentDialogueLine.continuation);
        }
        else if (currentDialogueLine.dialogueEnd)
        {
            EndDialogue();
        }
        else if (currentDialogueLine.dialogueType == DialogueLine.DialogueType.Choice)
        {
            StartChoiceDialogue(currentDialogueLine);
        }
    }

    public bool IsDialogueFinished()
    {
        return dialogueUIController.IsDialogueFinished();
    }

    public void StartChoiceDialogue(DialogueLine dialogueLine)
    {
        dialogueUIController.gameObject.SetActive(true);
        dialogueUIController.ShowChoiceDialogue(dialogueLine);


    }

    public void ChoiceSelected(int choiceIndex)
    {
        dialogueUIController.ChoiceSelected(choiceIndex);
    }

    public void EndDialogue()
    {
        if (dialogueUIController != null)
        {
            dialogueUIController.HideDialogueUI();
            dialogueUIController.gameObject.SetActive(false);
        }

        activeUI = false;
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        isDialogueFinished = false;
        dialogueUIController.ClearDialogueText();

        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueUIController.AppendDialogueText(letter);
            yield return null; // Wait for one frame
        }

        isDialogueFinished = true;    
    }


}
