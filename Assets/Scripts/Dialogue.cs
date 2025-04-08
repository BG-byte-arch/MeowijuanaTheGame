using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogues/Basic Dialogue")]
public class Dialogue : BaseDialogue
{
    public bool unskippable;
    public bool autoEnd;
    public Dialogue(DialogueCharacter character, string sentence, float letterDelay = 0.05f, bool unskippable = false, bool autoEnd = false)
    {
        this.character = character;
        this.sentence = sentence;
        this.letterDelay = letterDelay;
        this.unskippable = unskippable; // manually use ContinueDialogue to continue
        this.autoEnd = autoEnd;
    }
}