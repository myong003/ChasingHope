using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public Transform firstPos;
    public Transform secondPos;

    public float panSpeed;

    private Transform currPos;
    private CameraManager cameraManager;

    // Start is called before the first frame update
    void Awake()
    {
        currPos = firstPos;
        cameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player") {
            SwitchCameraPositions();
        }
    }

    private void SwitchCameraPositions() {
        if (currPos == firstPos) {
            currPos = secondPos;
        }
        else {
            currPos = firstPos;
        }

        cameraManager.CallPanCamera(currPos.position, panSpeed);
    }
}
