using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System;

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

                    if (dialogueIndex >= dialogue.sentences.Length) {
                        EndDialogue();
                    }
                    else {
                        if (CheckForAction(dialogue.sentences[dialogueIndex])) {
                            dialogueIndex++;
                            if (dialogueIndex >= dialogue.sentences.Length) {
                                EndDialogue();
                                return;
                            }
                        }
                        ParseSpeaker(dialogue.sentences[dialogueIndex]);
                        ParseExpression(dialogue.sentences[dialogueIndex]);
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
            if (PlayerController.Instance != null) {
                PlayerController.Instance.FreezePlayer();
            }

            // while (CheckForAction(dialogue.sentences[dialogueIndex])) {
            //     dialogueIndex++;
            // }

            ParseSpeaker(dialogue.sentences[dialogueIndex]);
            ParseExpression(dialogue.sentences[dialogueIndex]);
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
        if (PlayerController.Instance != null) {
            PlayerController.Instance.UnfreezePlayer();
        }
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

    /// <summary>
    /// Checks for an action marked by {} in the text, then parses and executes it
    /// Currently supported actions:
    /// {MoveCamera panSpeed (Vector3 newCameraPosition)}   :   Moves the camera to newCameraPosition
    /// {LoadCG cgName}                                     :   Loads a cg
    /// </summary>
    /// <param name="currentDialogue"></param>
    /// <returns>
    /// Returns true if action found, false otherwise
    /// </returns>
    private bool CheckForAction(string currentDialogue) {

        // Action found, do action
        if (currentDialogue[0] == '{') {
            int currIndex = 1;
            // Get function
            string function = GetWord(currentDialogue, ref currIndex);

            Debug.Log(function);
            switch (function) {
                case "MoveCamera":
                    float panSpeed = float.Parse(GetWord(currentDialogue, ref currIndex));
                    Vector3 newPos = GetVector(currentDialogue);
                    CameraManager cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
                    cm.CallPanCamera(newPos, panSpeed);
                    break;
                case "LoadCG":
                    string cg = GetWord(currentDialogue, ref currIndex);
                    Debug.Log(cg);
                    Sprite cgSprite = Resources.Load<Sprite>("CGs/" + cg);
                    CanvasManager.Instance.LoadCG(cgSprite);
                    break;
                default:
                    Debug.Log("Command not found " + function);
                    break;
            }

            return true;
        }
        
        // No action found
        return false;
    }

    private string GetWord(string currentDialogue, ref int currentIndex) {
        StringBuilder sb = new StringBuilder();

        // Get every character until reaching a space character
        while (currentIndex < currentDialogue.Length && currentDialogue[currentIndex] != ' ' && currentDialogue[currentIndex] != '}') {
            sb.Append(currentDialogue[currentIndex]);
            currentIndex++;
        }
        currentIndex++;

        return sb.ToString();
    }

    private Vector3 GetVector(string currentDialogue) {
        int currIndex = currentDialogue.IndexOf("(") + 1;
        string x, y, z;
        int xInt, yInt, zInt;
        StringBuilder sb = new StringBuilder();

        while (currIndex < currentDialogue.Length && currentDialogue[currIndex] != ',') {
            sb.Append(currentDialogue[currIndex]);
            currIndex++;
        }
        x = sb.ToString();
        sb.Clear();

        currIndex++;
        while (currIndex < currentDialogue.Length && currentDialogue[currIndex] != ',') {
            sb.Append(currentDialogue[currIndex]);
            currIndex++;
        }
        y = sb.ToString();
        sb.Clear();

        currIndex++;
        while (currIndex < currentDialogue.Length && currentDialogue[currIndex] != ')') {
            sb.Append(currentDialogue[currIndex]);
            currIndex++;
        }
        z = sb.ToString();
        sb.Clear();

        try {
            xInt = Int32.Parse(x);
            yInt = Int32.Parse(y);
            zInt = Int32.Parse(z);

            return new Vector3(xInt, yInt, zInt);
        }
        catch (FormatException e) {
            Debug.Log(e.Message);
            return Vector3.zero;
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
