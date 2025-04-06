using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTriggerer : MonoBehaviour
{
    [SerializeField] private DialogueQueue listOfDialogues;
    public DialogueBox dialogueBox;
    InputAction interactAction;
    bool triggered = false;

    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        if (dialogueBox == null)
        {
            dialogueBox = GameObject.Find("Canvas/Dialogue").GetComponent<DialogueBox>();
        }
    }

    private void Update()
    {
        if (!triggered) return;
        if (dialogueBox.dialogueActive) return;
        if (!interactAction.WasPressedThisFrame()) return;
        dialogueBox.AddDialogue(listOfDialogues);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        triggered = true;
        print("Triggered");
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        triggered = false;
        print("UnTriggered");
    }
}
