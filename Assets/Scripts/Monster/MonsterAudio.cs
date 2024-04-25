using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class MonsterAudio : MonoBehaviour
{
    public AudioClip[] footStepSounds;
    public AudioClip[] growlSounds;
    private AudioSource soundSource;

    void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }

    public void LeftFoot()
    {
        int n = Random.Range(1, footStepSounds.Length);
        soundSource.clip = footStepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);

        footStepSounds[n] = footStepSounds[0];
        footStepSounds[0] = soundSource.clip;
    }

    public void RightFoot()
    {
        int n = Random.Range(1, footStepSounds.Length);
        soundSource.clip = footStepSounds[n];
        soundSource.PlayOneShot(soundSource.clip);

        footStepSounds[n] = footStepSounds[0];
        footStepSounds[0] = soundSource.clip;
    }

    public void GrowlSound()
    {
        int n = Random.Range(1, growlSounds.Length);
        soundSource.clip = growlSounds[n];
        soundSource.PlayOneShot(soundSource.clip);

        growlSounds[n] = growlSounds[0];
        growlSounds[0] = soundSource.clip;
    }
}