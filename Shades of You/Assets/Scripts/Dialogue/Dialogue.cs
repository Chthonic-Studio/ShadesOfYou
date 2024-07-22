using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueLine", menuName = "Dialogue/DialogueLine", order = 1)]
public class DialogueLine : ScriptableObject
{
    public enum DialogueType { Dialogue, Choice }
    public enum Character { Blake, Iara, SooYeon, Xesus }

    [System.Serializable]
    public class Choice
    {
        public string text;
        public DialogueLine continuation;
        public bool specificCharacter;
        public bool isActive;
        public Character specificCharacterChoice;
    }

    public DialogueType dialogueType;
    public Actor actor;
    public string text;
    public bool specificCharacter;
    public Character specificCharacterChoice;
    public DialogueLine continuation;
    public bool dialogueEnd;

    public Choice[] choices;

    public bool finishesQuest;
    public NPCDialogue questNPC;
}

