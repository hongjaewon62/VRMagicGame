using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;

public class FireMagic : MonoBehaviour
{
    InputDevice left;
    InputDevice right;

    public bool isGrabbed = false;
    public bool isCharging = false;

    [SerializeField]
    private GameObject FiringVFX;
    [SerializeField]
    private GameObject ChargePoint;



    private float stayTime = 0.0f;
    public float requiredTime = 5.0f;

    private bool isTriggerEnabled = true;

    GameObject chargingVFXInstance;
    RaycastHit hit;

    private StaffSound sound;

    private void Start()
    {
        sound = GetComponent<StaffSound>();
    }

    void Update()
    {
        if (isGrabbed)
        {
            ButtonInput();
        }
    }

    void ButtonInput()
    {
        // �¿� ����̽� �ν�
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        // ��¡�ڵ� 5�ʵ��� �ݶ��̴��� ���� �������� Ȱ��ȭ Ʈ���� ������ �߻�
        right.TryGetFeatureValue(CommonUsages.triggerButton, out bool isRTriggerPressed);

        if (isRTriggerPressed)
        {
            TriggerPressed();
        }
    }


    void TriggerPressed()
    {
        if (isTriggerEnabled)
        {
            Debug.DrawRay(ChargePoint.transform.position, ChargePoint.transform.forward * 30f, Color.red, 1f);

            // isCharging ���¸� ����
            // Destroy the existing charging VFX
            if (chargingVFXInstance != null)
            {
                Destroy(chargingVFXInstance);
                chargingVFXInstance = null;
            }

            sound.PlaySound();
            // Instantiate the firing VFX at the charge point
            GameObject firingVFXInstance = Instantiate(FiringVFX, ChargePoint.transform.position, ChargePoint.transform.rotation);

            // Set the velocity of the firing VFX to move it forward
            Rigidbody rb = firingVFXInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = 10f * ChargePoint.transform.forward;
            }

            // Destroy the firing VFX after 5 seconds
            Destroy(firingVFXInstance, 5f);

            // Disable trigger input for a short duration
            StartCoroutine(DisableInputForSeconds(1.0f));
        }
    }

    IEnumerator DisableInputForSeconds(float seconds)
    {
        isTriggerEnabled = false; // ���� ��Ȱ��ȭ
        yield return new WaitForSeconds(seconds); // ������ �ð� ���� ���
        isTriggerEnabled = true; // ���� ��Ȱ��ȭ
    }

    public void onStaff()
    {
        isGrabbed = true;
    }
    public void offStaff()
    {
        isGrabbed = false;
    }

}
