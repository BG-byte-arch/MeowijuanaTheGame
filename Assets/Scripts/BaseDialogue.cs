using System;
using UnityEngine;

[Serializable]
public abstract class BaseDialogue: ScriptableObject
{
    public DialogueCharacter character;
    public string sentence;
    public float letterDelay;
}