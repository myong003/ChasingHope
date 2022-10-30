using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueLoader : MonoBehaviour
{
    public List<string> dialogues;
    public TextMeshProUGUI screenText;
    public GameObject dialoguePopup;

    public float scrollSpeed;
    private float scrollTimer;
    private bool moveToNextDialogue;
    private bool dialogueStarted;
    private int dialogueIndex;
    private int characterIndex;

    void Start() {
        scrollTimer = 0;
        scrollSpeed = 1 / scrollSpeed;
        dialogueStarted = false;

        StartDialogue();
    }

    public void LoadText(List<string> dialogue) {
        this.dialogues = dialogue;
    }

    public void StartDialogue() {
        dialoguePopup.SetActive(true);
        dialogueStarted = true;
        dialogueIndex = 0;
        characterIndex = 0;
        screenText.text = "";
        scrollTimer = 0;
    }

    void Update() {
        if (dialogueStarted && dialogueIndex < dialogues.Count) {
            // Get the current dialogue in the list of dialogues
            string currentDialogue = dialogues[dialogueIndex];

            // If the current dialogue hasn't finished yet
            if (characterIndex < currentDialogue.Length) {
                char currentCharacter = currentDialogue[characterIndex];

                // Add the next character in the dialogue to the screen after specific time
                if (scrollTimer >= scrollSpeed) {
                    screenText.text += currentCharacter;
                    scrollTimer = 0;
                    characterIndex++;
                }
                else {
                    scrollTimer += Time.deltaTime;
                }

                // If user presses x while text is scrolling, skip scrolling
                if (Input.GetKeyDown(KeyCode.X)) {
                    screenText.text = currentDialogue;
                    characterIndex = currentDialogue.Length;
                    scrollTimer = 0;
                }
            } 
            else {
                // Wait for user to press x to continue to next dialogue
                if (Input.GetKeyDown(KeyCode.X)) {
                    characterIndex = 0;
                    dialogueIndex++;
                    screenText.text = "";
                }
            }
        }
        else {
            dialoguePopup.SetActive(false);
        }
    }
}
