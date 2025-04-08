using System;
using System.Collections.Generic;

[Serializable]
public class DialogueQueue
{
    public string name;
    public List<BaseDialogue> dialogues;

    public DialogueQueue(List<BaseDialogue> dialogues, string name)
    {
        this.dialogues = dialogues;
        this.name = name;
    }
}