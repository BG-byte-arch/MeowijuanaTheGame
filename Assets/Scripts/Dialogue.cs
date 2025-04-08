using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogues/Basic Dialogue")]
public class Dialogue : BaseDialogue
{
    public bool unskippable;
    public bool autoEnd;
}