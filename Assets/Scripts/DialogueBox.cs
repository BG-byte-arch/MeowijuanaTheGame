using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

struct DialogueBox{}

public class DialogueManager : MonoBehaviour
{
    bool dialogueActive;
    Queue<String> sentences;
    InputAction continueAction;
    private TMP_Text name;
    private TMP_Text sentence;
    void Start()
    {
        continueAction = InputSystem.actions.FindAction("ContinueDialogue");
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive)
        {
            if (sentences.Count == 0) dialogueActive = false;
            else
            {
                name.text = sentences.Dequeue();
                sentence.text = sentences.Peek();
                if (continueAction.IsPressed())
                {
                    sentences.Dequeue();
                }
            }
        }
    }
}
