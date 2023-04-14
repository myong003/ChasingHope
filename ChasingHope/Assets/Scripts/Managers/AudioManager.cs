using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    private float masterVolume;

    void Start() {
        masterVolume = source.volume;
    }

    public void PlayClip(AudioClip clip) {
        source.clip = clip;
        source.Play();
    }

    public void PlayClip(AudioClip clip, float volume) {
        source.clip = clip;
        source.volume = volume * masterVolume;
        source.Play();
        // source.volume = masterVolume;
    }

    public void ToggleLoop() {
        source.loop = !source.loop;
    }

    public void Stop() {
        source.Stop();
        source.loop = false;
    }
}
