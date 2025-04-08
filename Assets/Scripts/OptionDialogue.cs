using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "NewOptionDialogue", menuName = "Dialogues/Option Dialogue")]
public class OptionDialogue : BaseDialogue
{
    public List<DialogueQueue> options;

    public OptionDialogue(DialogueCharacter character, string sentence, float letterDelay, List<DialogueQueue> options)
    {
        this.character = character;
        this.sentence = sentence;
        this.letterDelay = letterDelay;
        this.options = options;
    }
}