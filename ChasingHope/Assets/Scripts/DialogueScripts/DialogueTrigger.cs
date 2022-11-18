using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset text;

    public void TriggerDialogue() {
        if (!DialogueLoader.Instance.IsInDialogue()) {
            DialogueLoader.Instance.LoadDialogue(text.text);
            DialogueLoader.Instance.StartDialogue();
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if (other.gameObject.name == "InteractTrigger"){
            if (Input.GetKey(KeyCode.X)){
                TriggerDialogue();
            }
        }
    }
}
