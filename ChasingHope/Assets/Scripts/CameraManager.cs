using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float cameraMoveSpeed;

    private bool isPanning;

    public void CallPanCamera(Vector3 newPos) {
        IEnumerator coroutine = PanCamera(newPos);
        StartCoroutine(coroutine);
    }

    private IEnumerator PanCamera(Vector3 newPos) {
        isPanning = true;

        while (this.transform.position != newPos) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, newPos, cameraMoveSpeed);
            yield return null;
        }

        isPanning = false;
    }
}
