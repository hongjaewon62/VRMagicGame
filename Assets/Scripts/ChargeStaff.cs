using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ChargeStaff : MonoBehaviour
{
    InputDevice left;
    InputDevice right;

    public bool isGrabbed = false;
    public bool isCharging = false;

    [SerializeField]
    private GameObject ChargingVFX;
    [SerializeField]
    private GameObject FiringVFX;
    [SerializeField]
    private GameObject ChargePoint;

    private bool isButtonX = false;
    private bool isButtonY = false;

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

        left.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonX);
        left.TryGetFeatureValue(CommonUsages.secondaryButton, out isButtonY);

        

        // ��¡�ڵ� 5�ʵ��� �ݶ��̴��� ���� �������� Ȱ��ȭ Ʈ���� ������ �߻�
        right.TryGetFeatureValue(CommonUsages.triggerButton, out bool isRTriggerPressed);

        if (isRTriggerPressed && isCharging)
        {
            TriggerPressed();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LeftHand") && isButtonY && isButtonX) 
        {
            stayTime += Time.deltaTime;
            left.SendHapticImpulse(1, 0.4f, 0.5f);

            sound.chargeState = StaffSound.ChargeSoundState.Charge;
            sound.ChargePlaySound();

            if (stayTime >= requiredTime)
            {
                isCharging = true;

                if (chargingVFXInstance == null)
                {
                    chargingVFXInstance = Instantiate(ChargingVFX, ChargePoint.transform.position, ChargingVFX.transform.rotation);
                    chargingVFXInstance.transform.parent = transform;
                }
                else
                {
                    chargingVFXInstance.transform.position = ChargePoint.transform.position;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LeftHand"))
        {
            stayTime = 0;
            // isCharging ���¿� chargingVFXInstance�� �ı����� ����
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

            sound.chargeState = StaffSound.ChargeSoundState.Projectile;
            sound.ChargePlaySound();

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
            isCharging = false;
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
