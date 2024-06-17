using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    public GameObject horse;
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public BgmChange bgm;
    GameObject presser;
    bool isPressed;

    void Start()
    {
        isPressed = false;
        horse.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            onRelease.Invoke();
            isPressed = false;
        }
    }

    public void QuestStart()
    {
        horse.SetActive(true);
        bgm.QuestSound();
    }
}
