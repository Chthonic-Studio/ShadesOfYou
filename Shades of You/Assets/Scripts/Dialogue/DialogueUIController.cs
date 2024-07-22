using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUIController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueBoxText;
    [SerializeField] private GameObject dialogueName;
    [SerializeField] private TMP_Text dialogueNameText;
    [SerializeField] private GameObject choiceBoxParent;
    [SerializeField] private GameObject[] choiceBoxes;
    [SerializeField] private TMP_Text[] choiceTexts;
    [SerializeField] private GameObject portrait;
    [SerializeField] private Image portraitImage;

    public static DialogueUIController Instance { get; private set; }

    private DialogueLine currentDialogueLine;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateDialogue()
    {
        dialogueBox.SetActive(true);
        dialogueName.SetActive(true);
        portrait.SetActive(true);
    }

    public void ShowCharacterDialogue(DialogueLine dialogueLine)
    {
        currentDialogueLine = dialogueLine;

        dialogueNameText.text = dialogueLine.actor.name;
        dialogueBoxText.text = dialogueLine.text;
        portraitImage.sprite = dialogueLine.actor.portrait.sprite;

        if (dialogueLine.finishesQuest && dialogueLine.questNPC != null)
        {
            // Set hasFinishedQuest to true on the NPCDialogue
            dialogueLine.questNPC.hasFinishedQuest = true;
        }
    }

    public void ShowNextDialoguePart(DialogueLine dialogueLine)
    {
        if (currentDialogueLine.continuation != null)
        {
            ShowCharacterDialogue(currentDialogueLine.continuation);
        }    
    }

    public bool IsDialogueFinished()
    {
        return currentDialogueLine.continuation == null;
    }

    public void ShowChoiceDialogue(DialogueLine dialogueLine)
    {
        currentDialogueLine = dialogueLine;

        // Loop through each choice in the dialogue line
        for (int i = 0; i < dialogueLine.choices.Length; i++)
        {
            // Make sure the choice box is active
            choiceBoxes[i].SetActive(true);

            // Get the button component from the choice box
            Button choiceButton = choiceBoxes[i].GetComponent<Button>();

            // Set the button text to the choice text
            choiceButton.GetComponentInChildren<Text>().text = dialogueLine.choices[i].text;

            // Add a listener to the button click event
            choiceButton.onClick.AddListener(() => ChoiceSelected(i));
        }

        // Disable any unused choice boxes
        for (int i = dialogueLine.choices.Length; i < choiceBoxes.Length; i++)
        {
            choiceBoxes[i].SetActive(false);
        }
    }

    public void ChoiceSelected(int choiceIndex)
    {
        // Get the selected choice
        DialogueLine.Choice selectedChoice = currentDialogueLine.choices[choiceIndex];

        // Check if the choice has a continuation
        if (selectedChoice.continuation != null)
        {
            // Check the type of the continuation
            if (selectedChoice.continuation.dialogueType == DialogueLine.DialogueType.Dialogue)
            {
                // Show the continuation as a character dialogue
                ShowCharacterDialogue(selectedChoice.continuation);
            }
            else if (selectedChoice.continuation.dialogueType == DialogueLine.DialogueType.Choice)
            {
                // Show the continuation as a choice dialogue
                ShowChoiceDialogue(selectedChoice.continuation);
            }
        }
        else
        {
            // Hide the dialogue UI if there's no continuation
            HideDialogueUI();
        }
    }

    public void HideDialogueUI()
    {
        dialogueBox.SetActive(false);
        dialogueName.SetActive(false);
        portrait.SetActive(false);

        // Hide the choice boxes
        foreach (GameObject choiceBox in choiceBoxes)
        {
            choiceBox.SetActive(false);
        }
    }

    public void HideDialogueUIInstant()
    {
        dialogueBox.SetActive(false);
        dialogueName.SetActive(false);
        portrait.SetActive(false);

        // Hide the choice boxes
        foreach (GameObject choiceBox in choiceBoxes)
        {
            choiceBox.SetActive(false);
        }
    }
}
