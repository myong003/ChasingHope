using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class DialogueLoader : MonoBehaviour
{
    public static DialogueLoader Instance { get; private set;}
    public Dialogue dialogue;
    public TextMeshProUGUI screenText;
    public TextMeshProUGUI autoText;
    public TextMeshProUGUI speakerText;
    public GameObject dialoguePopup;
    public GameObject continueTriangle;
    public SpriteRenderer leftSprite;
    public SpriteRenderer rightSprite;

    public float scrollSpeed;   // How fast the text moves
    public bool isAuto;
    public float autoDelay;     // How long before the text automatically continues in auto

    private float scrollTimer;
    private bool moveToNextDialogue;
    private int dialogueIndex;
    private int characterIndex;
    private float autoTimer;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
        }
        else{
            Instance = this;
        }
    }

    void Start() {
        scrollTimer = 0;
        autoTimer = 0;
        scrollSpeed = 1 / scrollSpeed;
    }

    void Update() {

        if (dialoguePopup.activeInHierarchy && dialogueIndex < dialogue.sentences.Length) {

            if (Input.GetKeyDown(KeyCode.A)) {
                ToggleAuto();
            }

            if (Input.GetKeyDown(KeyCode.S)) {
                EndDialogue();
            }

            // Get the current dialogue in the list of dialogues
            string currentDialogue = dialogue.sentences[dialogueIndex];

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
                    screenText.text += currentDialogue.Substring(characterIndex);
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
                    ParseSpeaker(dialogue.sentences[dialogueIndex]);
                    ParseExpression(dialogue.sentences[dialogueIndex]);

                    if (dialogueIndex >= dialogue.sentences.Length) {
                        EndDialogue();
                    }
                }
                else if (isAuto) {
                    autoTimer += Time.deltaTime;
                }
            }
        }
    }

    public void LoadDialogue(string text) {
        dialogue = new Dialogue(text);
    }

    public void StartDialogue() {
        if (!IsInDialogue()) {
            dialoguePopup.SetActive(true);
            dialogueIndex = 0;
            characterIndex = 0;
            screenText.text = "";
            scrollTimer = 0;
            PlayerController.Instance.FreezePlayer();

            ParseSpeaker(dialogue.sentences[0]);
            ParseExpression(dialogue.sentences[0]);
        }
    }

    public bool IsInDialogue() {
        if (dialoguePopup.activeInHierarchy) {
            return true;
        }

        return false;
    }

    public void EndDialogue() {
        dialoguePopup.SetActive(false);
        PlayerController.Instance.UnfreezePlayer();
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

    private void ParseSpeaker(string currentDialogue) {
        if (currentDialogue.Contains(':')) {
            StringBuilder sb = new StringBuilder();
            while (characterIndex < currentDialogue.Length && currentDialogue[characterIndex] != ':') {
                sb.Append(currentDialogue[characterIndex]);
                characterIndex++;
            }
            characterIndex += 2;    // Increment past the : and the space after
            speakerText.text = sb.ToString();
        }
    }

    private void ParseExpression(string currentDialogue) {
        if (currentDialogue.Contains('[')) {
            StringBuilder sb = new StringBuilder();
            if (currentDialogue[characterIndex] != '[') {
                characterIndex = currentDialogue.IndexOf('[');
            }
            characterIndex++;   // Increment 1 past initial "["
            
            // Get string inbetween [] to find expression
            while (currentDialogue[characterIndex] != ']') {
                if (currentDialogue[characterIndex] == ' ') {
                    // String doesn't contain an expression, don't need to do anything
                    return;
                }
                sb.Append(currentDialogue[characterIndex]);
                characterIndex++;
            }

            characterIndex += 2;    // Increment past the : and the space after
            Sprite tempSprite = GetCharacterSprite(speakerText.text, sb.ToString());
            if (tempSprite != null) {
                UpdateExpression(tempSprite, speakerText.text);
            }
        }
    }

    private void UpdateExpression(Sprite sprite, string currSpeaker) {
        if (currSpeaker == "Alice") {
            leftSprite.sprite = sprite;
        }
        else {
            rightSprite.sprite = sprite;
        }
    }

    private Sprite GetCharacterSprite(string character, string expression) {
        // Return parsed expression sprite
        DialogueCharacter characterExpression = Resources.Load<DialogueCharacter>("Characters/" + character + "/" + expression);
        if (characterExpression != null) {
            return characterExpression.sprite;
        }
        
        // Return neutral expression if character found, null otherwise
        characterExpression = Resources.Load<DialogueCharacter>("Characters/" + character + "/" + "neutral");
        if (characterExpression != null) {
            return characterExpression.sprite;
        }

        return null;
    }

}
