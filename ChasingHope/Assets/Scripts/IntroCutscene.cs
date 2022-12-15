using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : DialogueTrigger
{
    public Sprite aliceCG;
    public Sprite rabbitCG;
    public Sprite chasingHopeCG;
    public AudioSource footStepAudio;
    public GameObject chasingHopePrompt;
    public float cutsceneTime;      // How long each cutscene is on screen before transitioning
    public float cutsceneFadeSpeed;

    public int phase = 1;
    private bool inCutscene = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(phase) {
            case 1:
                if (!inCutscene) {
                    StartCoroutine(AliceCutscene());
                }
                break;
            case 2:
                if (!inCutscene) {
                    StartCoroutine(RabbitCutscene());
                }
                break;
            case 3:
                if (!inCutscene) {
                    StartCoroutine(ChaseHopeScene());
                }
                break;
            default:
                Debug.Log("Scene finished at phase " + phase);
                break;
        }
    }

    // private IEnumerator StartDialogue() {
    //     inCutscene = true;
    //     TriggerDialogue();

    //     while (DialogueLoader.Instance.IsInDialogue()) {
    //         yield return null;
    //     }

    //     inCutscene = false;
    //     phase++;
    // }

    private IEnumerator AliceCutscene() {
        inCutscene = true;
        TriggerDialogue();
        CanvasManager.Instance.LoadCG(aliceCG);
        footStepAudio.Play();

        yield return new WaitForSeconds(cutsceneTime);
        CanvasManager.Instance.FadeToBlack(cutsceneFadeSpeed * Time.deltaTime);
        footStepAudio.Stop();

        // Don't do anything while fading to black
        while (CanvasManager.Instance.isFading) {
            yield return null;
        }

        // Wait a little bit in black screen before transitioning to next phase
        yield return new WaitForSeconds(cutsceneTime / 5);

        inCutscene = false;
        phase++;
    }

    private IEnumerator RabbitCutscene() {
        inCutscene = true;
        CanvasManager.Instance.LoadCG(rabbitCG);
        CanvasManager.Instance.FadeIn(cutsceneFadeSpeed * Time.deltaTime);

        // Don't do anything while fading in
        while (CanvasManager.Instance.isFading) {
            yield return null;
        }

        // Wait on rabbit scene before fading to black
        yield return new WaitForSeconds(cutsceneTime);
        CanvasManager.Instance.FadeToBlack(cutsceneFadeSpeed * Time.deltaTime);

        // Don't do anything while fading to black
        while (CanvasManager.Instance.isFading) {
            yield return null;
        }

        // Wait a little bit in black screen before transitioning to next phase
        yield return new WaitForSeconds(cutsceneTime / 5);

        inCutscene = false;
        phase++;
    }

    private IEnumerator ChaseHopeScene() {
        inCutscene = true;
        CanvasManager.Instance.LoadCG(chasingHopeCG);
        CanvasManager.Instance.FadeIn(cutsceneFadeSpeed * Time.deltaTime);

        while (CanvasManager.Instance.isFading) {
            yield return null;
        }

        chasingHopePrompt.SetActive(true);

        inCutscene = false;
        phase++;
    }
}
