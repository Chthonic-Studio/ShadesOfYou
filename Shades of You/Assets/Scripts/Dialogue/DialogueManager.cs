using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject dialogueUIPrefab;
    private DialogueUIController dialogueUIController;

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
                dialogueUIController.HideDialogueUIInstant();
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

    public void StartCharacterDialogue(DialogueLine dialogueLine)
    {
        Debug.Log("StartCharacterDialogue called by Dialogue Manager");
        dialogueUIController.gameObject.SetActive(true);

        // Activate the dialogueBox, dialogueNameBox, and portraitBox GameObjects
        dialogueUIController.ActivateDialogue();
        dialogueUIController.ShowCharacterDialogue(dialogueLine);
    }

    public void ShowNextDialoguePart(DialogueLine dialogueLine)
    {
        dialogueUIController.ShowNextDialoguePart(dialogueLine);
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
    }
}
