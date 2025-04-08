using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "NewDialogueCharacter", menuName = "Dialogues/Dialogue Character")]
public class DialogueCharacter : ScriptableObject
{
    new public string name;
    public Sprite portrait;
    public Sprite portraitTalk;
}