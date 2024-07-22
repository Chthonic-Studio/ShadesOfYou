using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueLine))]
public class DialogueLineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueLine dialogueLine = (DialogueLine)target;

        dialogueLine.dialogueType = (DialogueLine.DialogueType)EditorGUILayout.EnumPopup("Dialogue Type", dialogueLine.dialogueType);

        if (dialogueLine.dialogueType == DialogueLine.DialogueType.Dialogue)
        {
            dialogueLine.actor = (Actor)EditorGUILayout.ObjectField("Actor", dialogueLine.actor, typeof(Actor), true);
            dialogueLine.text = EditorGUILayout.TextArea(dialogueLine.text, GUILayout.Height(100));
            dialogueLine.specificCharacter = EditorGUILayout.Toggle("Specific Character", dialogueLine.specificCharacter);

            if (dialogueLine.specificCharacter)
            {
                dialogueLine.specificCharacterChoice = (DialogueLine.Character)EditorGUILayout.EnumPopup("Specific Character Choice", dialogueLine.specificCharacterChoice);
            }

            dialogueLine.continuation = (DialogueLine)EditorGUILayout.ObjectField("Continuation", dialogueLine.continuation, typeof(DialogueLine), true);
            dialogueLine.dialogueEnd = EditorGUILayout.Toggle("Dialogue End", dialogueLine.dialogueEnd); // Add this line

            dialogueLine.finishesQuest = EditorGUILayout.Toggle("Finishes Quest", dialogueLine.finishesQuest); // Add this line

            if (dialogueLine.finishesQuest)
            {
                dialogueLine.questNPC = (NPCDialogue)EditorGUILayout.ObjectField("Quest NPC", dialogueLine.questNPC, typeof(NPCDialogue), true); // Add this line
            }
        }
        else if (dialogueLine.dialogueType == DialogueLine.DialogueType.Choice)
        {
            int oldChoiceAmount = dialogueLine.choices.Length;
            int newChoiceAmount = EditorGUILayout.IntField("Choice Amount", oldChoiceAmount);

            if (oldChoiceAmount != newChoiceAmount)
            {
                System.Array.Resize(ref dialogueLine.choices, newChoiceAmount);

                for (int i = oldChoiceAmount; i < newChoiceAmount; i++)
                {
                    dialogueLine.choices[i] = new DialogueLine.Choice();
                }
            }

            for (int i = 0; i < dialogueLine.choices.Length; i++)
            {
                EditorGUILayout.LabelField($"Choice {i + 1}");
                dialogueLine.choices[i].text = EditorGUILayout.TextField("Text", dialogueLine.choices[i].text);
                dialogueLine.choices[i].continuation = (DialogueLine)EditorGUILayout.ObjectField("Continuation", dialogueLine.choices[i].continuation, typeof(DialogueLine), true);
                dialogueLine.choices[i].specificCharacter = EditorGUILayout.Toggle("Specific Character", dialogueLine.choices[i].specificCharacter);

                if (dialogueLine.choices[i].specificCharacter)
                {
                    dialogueLine.choices[i].specificCharacterChoice = (DialogueLine.Character)EditorGUILayout.EnumPopup("Specific Character Choice", dialogueLine.choices[i].specificCharacterChoice);
                }

                dialogueLine.choices[i].isActive = EditorGUILayout.Toggle("Is Active", dialogueLine.choices[i].isActive);
            }
        }
    
        if (GUI.changed)
        {
            EditorUtility.SetDirty(dialogueLine);
            AssetDatabase.SaveAssets();
        }

    }
}