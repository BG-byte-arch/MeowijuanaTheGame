using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

[Serializable]
public abstract class BaseDialogue: ScriptableObject
{
    public DialogueCharacter character;
    public string sentence;
    public float letterDelay;
}
    

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogues/Basic Dialogue")]
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


[CreateAssetMenu(fileName = "NewOptionDialogue", menuName = "Dialogues/Option Dialogue")]
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

[CreateAssetMenu(fileName = "NewDialogueCharacter", menuName = "Dialogues/Dialogue Character")]
public class DialogueCharacter : ScriptableObject
{
    new public string name;
    public Sprite portrait;
    public Sprite portraitTalk;
}

public class DialogueBox : MonoBehaviour
{
    public bool dialogueActive;
    bool dialogueEnded;
    private int characterCount;
    private float timer;
    public Queue<BaseDialogue> sentences;
    InputAction continueAction;
    new public TMP_Text name;
    public TMP_Text sentence;
    public TMP_Text optionSentence;
    public Image panel;
    public Image optionPanel;
    public Image portrait;
    public GameObject optionGameObject;
    public GameObject dialogueOptionButton;
    void Start()
    {
        sentences = new Queue<BaseDialogue>();
        continueAction = InputSystem.actions.FindAction("ContinueDialogue");
    }

    // Update is called once per frame
    public void ContinueDialogue()
    {
        sentences.Dequeue();
        characterCount = 0;
        dialogueEnded = false;
    }

    public void AddDialogue(DialogueQueue queue)
    {
        foreach (var dialogue in queue.dialogues)
        {
            sentences.Enqueue(dialogue);
        }

        dialogueActive = true;
    }

    private bool toggle;
    void Update()
    {
        timer += Time.deltaTime;
        if (sentences.Count == 0) dialogueActive = false;
        name.enabled = dialogueActive;
        portrait.enabled = dialogueActive;
        panel.enabled = dialogueActive && sentences.Peek() is Dialogue;
        sentence.enabled = dialogueActive && sentences.Peek() is Dialogue;
        optionPanel.enabled = dialogueActive && sentences.Peek() is OptionDialogue;
        optionSentence.enabled = dialogueActive && sentences.Peek() is OptionDialogue;
        if (sentences.Count == 0) return; 
        name.text = sentences.Peek().character.name;
        if (!dialogueEnded && timer >= sentences.Peek().letterDelay)
        {
            if (sentences.Peek() is Dialogue dialogue)
            {
                if (characterCount == sentences.Peek().sentence.Length || (!dialogue.unskippable && continueAction.WasPressedThisFrame()))
                {
                    dialogueEnded = true;
                    if (dialogue.autoEnd) ContinueDialogue();
                }
            }
            else if (sentences.Peek() is OptionDialogue optionDialogue)
            {
                if (characterCount == sentences.Peek().sentence.Length || continueAction.WasPressedThisFrame())
                {
                    dialogueEnded = true;
                    foreach (var option in optionDialogue.options)
                    {
                        Button optionObj = Instantiate(dialogueOptionButton, optionGameObject.transform).GetComponent<Button>();
                        TMP_Text optionText = optionObj.GetComponentInChildren<TMP_Text>();
                        optionObj.onClick.AddListener(() =>
                        {
                            AddDialogue(option);
                            ContinueDialogue();
                            foreach (Transform child in optionGameObject.transform)
                            {
                                Destroy(child.gameObject);
                            }
                        });
                        optionText.text = option.name;
                    }
                }
            }
            if (characterCount < sentences.Peek().sentence.Length)
            {
                toggle = !toggle;
                characterCount++;
                timer = 0;
            }
            portrait.sprite = toggle ? sentences.Peek().character.portrait : sentences.Peek().character.portraitTalk;
        }
        else if (dialogueEnded)
        {
            portrait.sprite = sentences.Peek().character.portrait;
            if (sentences.Peek() is Dialogue dialogue)
            {
                if (continueAction.WasPressedThisFrame() && !dialogue.unskippable) ContinueDialogue();
            }
        }
        sentence.text = sentences.Peek().sentence.Substring(0, characterCount);
        optionSentence.text = sentences.Peek().sentence.Substring(0, characterCount);
    }
}
