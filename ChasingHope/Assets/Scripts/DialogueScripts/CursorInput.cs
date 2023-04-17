using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInput : MonoBehaviour
{
    public List<GameObject> positions;
    public AudioManager audioManager;
    public AudioClip buttonClickNoise;
    public float xOffset;
    public int startingPos=0;
    public int currPosition = -1;
    
    void OnEnable() {
        currPosition = -1;
        this.transform.position = positions[startingPos].transform.position;
        this.transform.position -= new Vector3(xOffset, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (startingPos > 0) {
                startingPos--;
                this.transform.position = positions[startingPos].transform.position;
                this.transform.position -= new Vector3(xOffset, 0, 0);
                audioManager.PlayClip(buttonClickNoise);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (startingPos < positions.Count-1) {
                startingPos++;
                this.transform.position = positions[startingPos].transform.position;
                this.transform.position -= new Vector3(xOffset, 0, 0);
                audioManager.PlayClip(buttonClickNoise);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Z)) {
            currPosition = startingPos;
            audioManager.PlayClip(buttonClickNoise);
        }
    }
}
