using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public SpriteRenderer cg;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void LoadCG(Sprite background) {
        cg.sprite = background;
        cg.enabled = true;
    }

    public void EndCG() {
        cg.sprite = null;
        cg.enabled = false;
    }
}
