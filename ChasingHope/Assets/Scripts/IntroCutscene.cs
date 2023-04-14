using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : DialogueTrigger
{
    [Header("CGs")]
    public Sprite aliceCG;
    public Sprite rabbitCG1;
    public Sprite rabbitCG2;
    public Sprite rabbitCG3;
    public Sprite rabbitCG4;
    public Sprite chasingHopeCG;
    public Sprite CG7;      // Alice looking down
    public Sprite CG8;      // Alice jumping 1
    public Sprite CG9;      // Alice jumping 2

    [Header("Audio")]
    public AudioManager audioManager;
    public AudioClip footStepAudio;
    public AudioClip heartbeatSingle;
    public AudioClip heartbeatFlat;
    public GameObject chasingHopePrompt;
    public CursorInput cursor;
    public float cutsceneTime;      // How long each cutscene is on screen before transitioning
    public float cutsceneFadeSpeed;
    
    [Header("Heartbeat")]
    public float pulseTime;         // How long until heartbeat flats
    public float startFrequency;
    public float slowAmount;        // How much the heartbeats slow down

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
                    StartCoroutine(RabbitCutscene(rabbitCG1));
                }
                break;
            case 3:
                if (!inCutscene) {
                    StartCoroutine(RabbitCutscene(rabbitCG2));
                }
                break;
            case 4:
                if (!inCutscene) {
                    StartCoroutine(RabbitCutscene(rabbitCG3));
                }
                break;
            case 5:
                if (!inCutscene) {
                    StartCoroutine(RabbitCutscene(rabbitCG4));
                }
                break;
            case 6:
                if (!inCutscene) {
                    StartCoroutine(ChaseHopeScene());
                }
                break;
            case 7: // Game over
                if (!inCutscene) {
                    StartCoroutine(EndGameCutscene());
                }
                break;
            case 8:
                if (!inCutscene) {
                    StartCoroutine(RabbitCutscene(CG7));
                }
                break;
            case 9:
                if (!inCutscene) {
                    StartCoroutine(RabbitCutscene(CG8));
                }
                break;
            case 10:
                if (!inCutscene) {
                    StartCoroutine(RabbitCutscene(CG9));
                }
                break;
            case 11:
                SceneManager.LoadScene(1);
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
        audioManager.ToggleLoop();
        audioManager.PlayClip(footStepAudio);

        yield return new WaitForSeconds(cutsceneTime);
        CanvasManager.Instance.FadeToBlack(cutsceneFadeSpeed * Time.deltaTime);
        audioManager.Stop();

        // Don't do anything while fading to black
        while (CanvasManager.Instance.isFading) {
            yield return null;
        }

        // Wait a little bit in black screen before transitioning to next phase
        yield return new WaitForSeconds(cutsceneTime / 5);

        inCutscene = false;
        phase++;
    }

    private IEnumerator RabbitCutscene(Sprite cg) {
        inCutscene = true;
        CanvasManager.Instance.LoadCG(cg);
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

        while (cursor.currPosition == -1) {
            yield return null;
        }

        if (cursor.currPosition == 1) {
            // Game over
            phase++;
        }
        else if (cursor.currPosition == 0) {
            phase = 8;
        }

        chasingHopePrompt.SetActive(false);
        inCutscene = false;
    }

    private IEnumerator EndGameCutscene() {
        inCutscene = true;

        float fadeSpeed = Time.deltaTime / (pulseTime - 3.0f);
        CanvasManager.Instance.FadeToBlack(fadeSpeed);

        float heartbeatTimer = startFrequency;
        float heartbeatFrequency = startFrequency;
        float totalTimer = 0;
        while (totalTimer < pulseTime) {
            if (heartbeatTimer >= heartbeatFrequency) {
                audioManager.PlayClip(heartbeatSingle, 0.5f);
                heartbeatTimer = 0;
                heartbeatFrequency += slowAmount;
            }
            else {
                heartbeatTimer += Time.deltaTime;
            }

            totalTimer += Time.deltaTime;
            yield return null;
        }

        totalTimer = 0;
        audioManager.ToggleLoop();
        audioManager.PlayClip(heartbeatFlat, 0.5f);

        while (totalTimer < 5) {
            yield return null;
        }

        audioManager.Stop();

        phase = -1;
        inCutscene = false;
    }
}
