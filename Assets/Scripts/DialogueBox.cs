using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public bool dialogueActive;
    bool dialogueEnded;
    private int characterCount;
    private float timer;
    public LinkedList<BaseDialogue> sentences;
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
        sentences = new LinkedList<BaseDialogue>();
        continueAction = InputSystem.actions.FindAction("ContinueDialogue");
    }

    // Update is called once per frame
    public void ContinueDialogue()
    {
        sentences.RemoveFirst();
        characterCount = 0;
        dialogueEnded = false;
    }

    public void AddDialogue(DialogueQueue queue)
    {
        for (int i = queue.dialogues.Count - 1; i >= 0; i--)
        {
            sentences.AddFirst(queue.dialogues[i]);
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
        panel.enabled = dialogueActive && sentences.First() is Dialogue;
        sentence.enabled = dialogueActive && sentences.First() is Dialogue;
        optionPanel.enabled = dialogueActive && sentences.First() is OptionDialogue;
        optionSentence.enabled = dialogueActive && sentences.First() is OptionDialogue;
        if (sentences.Count == 0) return; 
        name.text = sentences.First().character.name;
        if (!dialogueEnded && timer >= sentences.First().letterDelay)
        {
            if (sentences.First() is Dialogue dialogue)
            {
                if (characterCount == sentences.First().sentence.Length || (!dialogue.unskippable && continueAction.WasPressedThisFrame()))
                {
                    dialogueEnded = true;
                    if (dialogue.autoEnd) ContinueDialogue();
                }
            }
            else if (sentences.First() is OptionDialogue optionDialogue)
            {
                if (characterCount == sentences.First().sentence.Length || continueAction.WasPressedThisFrame())
                {
                    dialogueEnded = true;
                    foreach (var option in optionDialogue.options)
                    {
                        Button optionObj = Instantiate(dialogueOptionButton, optionGameObject.transform).GetComponent<Button>();
                        TMP_Text optionText = optionObj.GetComponentInChildren<TMP_Text>();
                        optionObj.onClick.AddListener(() =>
                        {
                            ContinueDialogue();
                            AddDialogue(option);
                            foreach (Transform child in optionGameObject.transform)
                            {
                                Destroy(child.gameObject);
                            }
                        });
                        optionText.text = option.name;
                    }
                }
            }
            if (characterCount < sentences.First().sentence.Length)
            {
                toggle = !toggle;
                characterCount++;
                timer = 0;
                if (sentences.First().letterDelay == 0)
                {
                    characterCount = sentences.First().sentence.Length;
                }
            }
            portrait.sprite = toggle ? sentences.First().character.portrait : sentences.First().character.portraitTalk;
        }
        else if (dialogueEnded)
        {
            portrait.sprite = sentences.First().character.portrait;
            if (sentences.First() is Dialogue dialogue)
            {
                if (continueAction.WasPressedThisFrame() && !dialogue.unskippable) ContinueDialogue();
            }
        }
        sentence.text = sentences.First().sentence.Substring(0, characterCount);
        optionSentence.text = sentences.First().sentence.Substring(0, characterCount);
    }
}
