using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public SpriteRenderer cg;
    public bool isFading = false;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void LoadCG(Sprite background) {
        Debug.Log("LOading cg " + background);
        cg.sprite = background;
        cg.enabled = true;
    }

    public void EndCG() {
        cg.sprite = null;
        cg.enabled = false;
    }

    public void FadeToBlack(float fadeSpeed) {
        if (!isFading) {
            StartCoroutine(FadeToBlackCoroutine(fadeSpeed));
        }
    }

    private IEnumerator FadeToBlackCoroutine(float fadeSpeed) {
        isFading = true;
        Color tempColor = cg.color;

        while (tempColor.a > 0) {
            Debug.Log(tempColor.a);
            tempColor.a -= fadeSpeed;
            cg.color = tempColor;
            yield return null;
        }

        isFading = false;
    }

    public void FadeIn(float fadeSpeed) {
        if (!isFading) {
            StartCoroutine(FadeInCoroutine(fadeSpeed));
        }
    }

    private IEnumerator FadeInCoroutine(float fadeSpeed) {
        isFading = true;
        Color tempColor = cg.color;

        while (tempColor.a < 1) {
            tempColor.a += fadeSpeed;
            cg.color = tempColor;
            yield return null;
        }

        isFading = false;
    }
}
