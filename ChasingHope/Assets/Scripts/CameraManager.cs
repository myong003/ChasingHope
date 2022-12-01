using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private bool isPanning;

    public void CallPanCamera(Vector3 newPos, float cameraMoveSpeed) {
        IEnumerator coroutine = PanCamera(newPos, cameraMoveSpeed);
        StartCoroutine(coroutine);
    }

    private IEnumerator PanCamera(Vector3 newPos, float cameraMoveSpeed) {
        isPanning = true;

        while (this.transform.position != newPos) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, newPos, cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }

        isPanning = false;
    }
}
