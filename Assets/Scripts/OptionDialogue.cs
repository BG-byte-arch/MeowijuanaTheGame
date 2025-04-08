using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "NewOptionDialogue", menuName = "Dialogues/Option Dialogue")]
public class OptionDialogue : BaseDialogue
{
    public List<DialogueQueue> options;
}