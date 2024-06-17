using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmChange : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] audioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        MainSound();
    }

    public void MainSound()
    {
        audioSource.clip = audioClip[0];
        audioSource.Play();
    }

    public void QuestSound()
    {
        audioSource.clip = audioClip[1];
        audioSource.Play();
    }
}
