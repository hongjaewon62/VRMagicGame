using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffSound : MonoBehaviour
{
    public enum StackSoundState
    {
        Arance,
        Fire,
        Ice,
        Lightning,
        Nature,
        Positive
    }

    public enum ChargeSoundState
    {
        Charge,
        Projectile
    }

    public enum PaintSoundState
    {
        Ice,
        Stome,
        Dark,
        Light,
        Fire
    }

    public StackSoundState stackState;
    public ChargeSoundState chargeState;
    public PaintSoundState paintState;
    private AudioSource audioSource;
    public AudioClip[] audioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StackPlaySound()
    {
        switch(stackState)
        {
            case StackSoundState.Arance:
                audioSource.clip = audioClip[0];
                audioSource.Play();
                break;
            case StackSoundState.Fire:
                audioSource.clip = audioClip[1];
                audioSource.Play();
                break;
            case StackSoundState.Ice:
                audioSource.clip = audioClip[2];
                audioSource.Play();
                break;
            case StackSoundState.Lightning:
                audioSource.clip = audioClip[3];
                audioSource.Play();
                break;
            case StackSoundState.Nature:
                audioSource.clip = audioClip[4];
                audioSource.Play();
                break;
            case StackSoundState.Positive:
                audioSource.clip = audioClip[5];
                audioSource.Play();
                break;
        }
    }

    public void ChargePlaySound()
    {
        switch (chargeState)
        {
            case ChargeSoundState.Charge:
                audioSource.clip = audioClip[0];
                audioSource.Play();
                break;
            case ChargeSoundState.Projectile:
                audioSource.clip = audioClip[1];
                audioSource.Play();
                break;
        }
    }
    public void PaintPlaySound()
    {
        switch (paintState)
        {
            case PaintSoundState.Ice:
                audioSource.clip = audioClip[0];
                audioSource.Play();
                break;
            case PaintSoundState.Stome:
                audioSource.clip = audioClip[1];
                audioSource.Play();
                break;
            case PaintSoundState.Dark:
                audioSource.clip = audioClip[2];
                audioSource.Play();
                break;
            case PaintSoundState.Light:
                audioSource.clip = audioClip[3];
                audioSource.Play();
                break;
            case PaintSoundState.Fire:
                audioSource.clip = audioClip[4];
                audioSource.Play();
                break;
        }
    }

    public void PlaySound()
    {
        audioSource.clip = audioClip[0];
        audioSource.Play();
    }
}
