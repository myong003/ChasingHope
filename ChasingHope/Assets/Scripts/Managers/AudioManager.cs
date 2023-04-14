using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;

    public void PlayClip(AudioClip clip) {
        source.clip = clip;
        source.Play();
    }

    public void ToggleLoop() {
        source.loop = !source.loop;
    }

    public void Stop() {
        source.Stop();
        source.loop = false;
    }
}
