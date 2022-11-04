using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueLoader : MonoBehaviour
{
    public List<string> dialogues;
    public TextMeshProUGUI screenText;
    public TextMeshProUGUI autoText;
    public GameObject dialoguePopup;
    public GameObject continueTriangle;

    public float scrollSpeed;   // How fast the text moves
    public bool isAuto;
    public float autoDelay;     // How long before the text automatically continues in auto

    private float scrollTimer;
    private bool moveToNextDialogue;
    private int dialogueIndex;
    private int characterIndex;
    private float autoTimer;

    void Start() {
        scrollTimer = 0;
        autoTimer = 0;
        scrollSpeed = 1 / scrollSpeed;
    }

    void Update() {

        if (dialoguePopup.activeInHierarchy && dialogueIndex < dialogues.Count) {

            if (Input.GetKeyDown(KeyCode.A)) {
                ToggleAuto();
            }

            if (Input.GetKeyDown(KeyCode.S)) {
                EndDialogue();
            }

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
               continueTriangle.SetActive(true);

                // Wait for user to press x to continue to next dialogue
                if (Input.GetKeyDown(KeyCode.X) || (isAuto && autoTimer > autoDelay)) {
                    characterIndex = 0;
                    dialogueIndex++;
                    screenText.text = "";
                    autoTimer = 0;
                    continueTriangle.SetActive(false);

                    if (dialogueIndex >= dialogues.Count) {
                        EndDialogue();
                    }
                }
                else if (isAuto) {
                    autoTimer += Time.deltaTime;
                }
            }
        }
    }

    public void StartDialogue() {
        if (!dialoguePopup.activeInHierarchy) {
            dialoguePopup.SetActive(true);
            dialogueIndex = 0;
            characterIndex = 0;
            screenText.text = "";
            scrollTimer = 0;
            PlayerController.Instance.FreezePlayer();
        }
    }

    public void EndDialogue() {
        dialoguePopup.SetActive(false);
        PlayerController.Instance.UnfreezePlayer();
    }

    public void LoadText(List<string> dialogue) {
        this.dialogues = dialogue;
    }

    public void ToggleAuto() {
        isAuto = !isAuto;
        autoTimer = 0;

        if (isAuto) {
            autoText.color = Color.yellow;
        }
        else {
            autoText.color = Color.white;
        }
    }

}
