using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatDialogue : MonoBehaviour
{
    private DialogueLoader dLoad;

    // Start is called before the first frame update
    void Start()
    {
        dLoad = FindObjectOfType<DialogueLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other){
        if (other.gameObject.name == "InteractTrigger"){
            if (Input.GetKey(KeyCode.X)){
                dLoad.StartDialogue();
            }
        }
    }
}
