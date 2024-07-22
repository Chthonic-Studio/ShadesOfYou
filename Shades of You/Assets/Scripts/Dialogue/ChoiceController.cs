using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ChoiceButtonController : MonoBehaviour
{
    public DialogueUIController dialogueUIController;
    public int choiceIndex;
    public TextMeshProUGUI choiceText; // Add this line to reference the TextMeshPro element

    private void Awake()
    {
        // Assuming the TextMeshProUGUI component is on a direct child of this game object
        choiceText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnChoiceClicked()
    {
        string choiceValue = choiceText.text; // Get the text from the TextMeshPro element
        Debug.Log("Choice " + choiceIndex + " selected: " + choiceValue);
        DialogueManager.Instance.ChoiceSelected(choiceIndex); // Send only the choice index to the DialogueManager
    }
}