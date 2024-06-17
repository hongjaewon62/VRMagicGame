using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportOnButtonPressTwo : MonoBehaviour
{
    InputDevice left;
    InputDevice right;

    Transform shootpoint;


    public bool isGrabbed = false;

    private StaffSound sound;

    [SerializeField]
    private GameObject effectPrefab_001;
    [SerializeField]
    private GameObject effectPrefab_002;
    [SerializeField]
    private GameObject effectPrefab_003;
    [SerializeField]
    private GameObject effectPrefab_010;
    [SerializeField]
    private GameObject effectPrefab_011;
    [SerializeField]
    private GameObject effectPrefab_012;
    [SerializeField]
    private GameObject effectPrefab_013;
    [SerializeField]
    private GameObject effectPrefab_020;
    [SerializeField]
    private GameObject effectPrefab_021;
    [SerializeField]
    private GameObject effectPrefab_022;
    [SerializeField]
    private GameObject effectPrefab_023;
    [SerializeField]
    private GameObject effectPrefab_030;
    [SerializeField]
    private GameObject effectPrefab_031;
    [SerializeField]
    private GameObject effectPrefab_032;
    [SerializeField]
    private GameObject effectPrefab_033;
    [SerializeField]
    private GameObject effectPrefab_100;
    [SerializeField]
    private GameObject effectPrefab_101;
    [SerializeField]
    private GameObject effectPrefab_102;
    [SerializeField]
    private GameObject effectPrefab_103;
    [SerializeField]
    private GameObject effectPrefab_110;
    [SerializeField]
    private GameObject effectPrefab_111;
    [SerializeField]
    private GameObject effectPrefab_112;
    [SerializeField]
    private GameObject effectPrefab_113;
    [SerializeField]
    private GameObject effectPrefab_120;
    [SerializeField]
    private GameObject effectPrefab_121;
    [SerializeField]
    private GameObject effectPrefab_122;
    [SerializeField]
    private GameObject effectPrefab_123;
    [SerializeField]
    private GameObject effectPrefab_130;
    [SerializeField]
    private GameObject effectPrefab_131;
    [SerializeField]
    private GameObject effectPrefab_132;
    [SerializeField]
    private GameObject effectPrefab_133;
    [SerializeField]
    private GameObject effectPrefab_200;
    [SerializeField]
    private GameObject effectPrefab_201;
    [SerializeField]
    private GameObject effectPrefab_202;
    [SerializeField]
    private GameObject effectPrefab_203;
    [SerializeField]
    private GameObject effectPrefab_210;
    [SerializeField]
    private GameObject effectPrefab_211;
    [SerializeField]
    private GameObject effectPrefab_212;
    [SerializeField]
    private GameObject effectPrefab_213;
    [SerializeField]
    private GameObject effectPrefab_220;
    [SerializeField]
    private GameObject effectPrefab_221;
    [SerializeField]
    private GameObject effectPrefab_222;
    [SerializeField]
    private GameObject effectPrefab_223;
    [SerializeField]
    private GameObject effectPrefab_230;
    [SerializeField]
    private GameObject effectPrefab_231;
    [SerializeField]
    private GameObject effectPrefab_232;
    [SerializeField]
    private GameObject effectPrefab_233;
    [SerializeField]
    private GameObject effectPrefab_300;
    [SerializeField]
    private GameObject effectPrefab_301;
    [SerializeField]
    private GameObject effectPrefab_302;
    [SerializeField]
    private GameObject effectPrefab_303;
    [SerializeField]
    private GameObject effectPrefab_310;
    [SerializeField]
    private GameObject effectPrefab_311;
    [SerializeField]
    private GameObject effectPrefab_312;
    [SerializeField]
    private GameObject effectPrefab_313;
    [SerializeField]
    private GameObject effectPrefab_320;
    [SerializeField]
    private GameObject effectPrefab_321;
    [SerializeField]
    private GameObject effectPrefab_322;
    [SerializeField]
    private GameObject effectPrefab_323;
    [SerializeField]
    private GameObject effectPrefab_330;
    [SerializeField]
    private GameObject effectPrefab_331;
    [SerializeField]
    private GameObject effectPrefab_332;
    [SerializeField]
    private GameObject effectPrefab_333;

    private Dictionary<int, GameObject> effectPrefabs = new Dictionary<int, GameObject>();

    private int MaxNum = 3;
    public List<int> magicList = new List<int>(new int[3]);


    public int magicID = 0; // ������ ���� ��ȣ 

    private bool isButtonA = false;
    private bool isButtonB = false;
    private bool isButtonX = false;
    private bool isButtonY = false;

    private bool prevButtonA;
    private bool prevButtonB;
    private bool prevButtonX;
    private bool prevButtonY;

    private int BtnNumA = 0;
    private int BtnNumB = 1;
    private int BtnNumX = 2;
    private int BtnNumY = 3;


    public float bulletSpeed = 20f;
    public float fireRate = 0.5f; // �Ѿ� �߻� ����


    private bool isTriggerEnabled = true;
    private float nextFire; // ���� �߻� �ð�

    GameObject VFX;
    RaycastHit hit;

    void Start()
    {
        prevButtonA = false;
        prevButtonB = false;
        prevButtonX = false;
        prevButtonY = false;

        // �ʱ� magicList�� [0,0,0]���� ����
        magicList.Add(0);
        magicList.Add(0);
        magicList.Add(0);

        // ������ ����
        effectPrefabs[001] = effectPrefab_001;
        effectPrefabs[002] = effectPrefab_002;
        effectPrefabs[003] = effectPrefab_003;
        effectPrefabs[010] = effectPrefab_010;
        effectPrefabs[011] = effectPrefab_011;
        effectPrefabs[012] = effectPrefab_012;
        effectPrefabs[013] = effectPrefab_013;
        effectPrefabs[020] = effectPrefab_020;
        effectPrefabs[021] = effectPrefab_021;
        effectPrefabs[022] = effectPrefab_022;
        effectPrefabs[023] = effectPrefab_023;
        effectPrefabs[030] = effectPrefab_030;
        effectPrefabs[031] = effectPrefab_031;
        effectPrefabs[032] = effectPrefab_032;
        effectPrefabs[033] = effectPrefab_033;
        effectPrefabs[100] = effectPrefab_100;
        effectPrefabs[101] = effectPrefab_101;
        effectPrefabs[102] = effectPrefab_102;
        effectPrefabs[103] = effectPrefab_103;
        effectPrefabs[110] = effectPrefab_110;
        effectPrefabs[111] = effectPrefab_111;
        effectPrefabs[112] = effectPrefab_112;
        effectPrefabs[113] = effectPrefab_113;
        effectPrefabs[120] = effectPrefab_120;
        effectPrefabs[121] = effectPrefab_121;
        effectPrefabs[122] = effectPrefab_122;
        effectPrefabs[123] = effectPrefab_123;
        effectPrefabs[130] = effectPrefab_130;
        effectPrefabs[131] = effectPrefab_131;
        effectPrefabs[132] = effectPrefab_132;
        effectPrefabs[133] = effectPrefab_133;
        effectPrefabs[200] = effectPrefab_200;
        effectPrefabs[201] = effectPrefab_201;
        effectPrefabs[202] = effectPrefab_202;
        effectPrefabs[203] = effectPrefab_203;
        effectPrefabs[210] = effectPrefab_210;
        effectPrefabs[211] = effectPrefab_211;
        effectPrefabs[212] = effectPrefab_212;
        effectPrefabs[213] = effectPrefab_213;
        effectPrefabs[220] = effectPrefab_220;
        effectPrefabs[221] = effectPrefab_221;
        effectPrefabs[222] = effectPrefab_222;
        effectPrefabs[223] = effectPrefab_223;
        effectPrefabs[230] = effectPrefab_230;
        effectPrefabs[231] = effectPrefab_231;
        effectPrefabs[232] = effectPrefab_232;
        effectPrefabs[233] = effectPrefab_233;
        effectPrefabs[300] = effectPrefab_300;
        effectPrefabs[301] = effectPrefab_301;
        effectPrefabs[302] = effectPrefab_302;
        effectPrefabs[303] = effectPrefab_303;
        effectPrefabs[310] = effectPrefab_310;
        effectPrefabs[311] = effectPrefab_311;
        effectPrefabs[312] = effectPrefab_312;
        effectPrefabs[313] = effectPrefab_313;
        effectPrefabs[320] = effectPrefab_320;
        effectPrefabs[321] = effectPrefab_321;
        effectPrefabs[322] = effectPrefab_322;
        effectPrefabs[323] = effectPrefab_323;
        effectPrefabs[330] = effectPrefab_330;
        effectPrefabs[331] = effectPrefab_331;
        effectPrefabs[332] = effectPrefab_332;
        effectPrefabs[333] = effectPrefab_333;


        shootpoint = GameObject.Find("MagicShootPoint").GetComponent<Transform>();

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

        // ���������� A, B, X, Y�� ��ư �ν� �� �۵�
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonA);
        if (isButtonA && !prevButtonA)
        {
            AddInput(BtnNumA);
        }
        prevButtonA = isButtonA;

        right.TryGetFeatureValue(CommonUsages.secondaryButton, out isButtonB);
        if (isButtonB && !prevButtonB)
        {
            AddInput(BtnNumB);
        }
        prevButtonB = isButtonB;

        left.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonX);
        if (isButtonX && !prevButtonX)
        {
            AddInput(BtnNumX);
        }
        prevButtonX = isButtonX;

        left.TryGetFeatureValue(CommonUsages.secondaryButton, out isButtonY);
        if (isButtonY && !prevButtonY)
        {
            AddInput(BtnNumY);
        }
        prevButtonY = isButtonY;

        right.TryGetFeatureValue(CommonUsages.triggerButton, out bool isRTriggerPressed);
        //left.TryGetFeatureValue(CommonUsages.triggerButton, out bool isLTriggerPressed);
        if (isRTriggerPressed )
        {
            TriggerPressed();
        }
    }

    void AddInput(int input)
    {
        // ����Ʈ�� �ִ� ũ�⺸�� ũ�� ó�� �Էµ� ���� �����մϴ�
        if (magicList.Count >= MaxNum)
        {
            magicList.RemoveAt(0);
        }
        // ���ο� �Է� ���� ����Ʈ�� �߰��մϴ�
        magicList.Add(input);

        magicID = ConvertListToInt(magicList);
    }

    // ����Ʈ�� ���ڸ� ���������� ����
    int ConvertListToInt(List<int> list)
    {
        int result = 0;
        foreach (int num in list)
        {
            result = result * 10 + num;
        }
        return result;
    }

    void TriggerPressed()
    {
        if (isTriggerEnabled)
        {

            Debug.DrawRay(shootpoint.position, shootpoint.forward * 30f, Color.red, 1f);
            if (Physics.Raycast(shootpoint.position, shootpoint.forward, out hit, 30f))
            {
                if (effectPrefabs.ContainsKey(magicID))
                {
                    VFX = Instantiate(effectPrefabs[magicID], hit.point + effectPrefabs[magicID].transform.position, effectPrefabs[magicID].transform.rotation);

                    MasicSound();

                    Destroy(VFX, VFX.GetComponent<ParticleSystem>().main.duration);
                    nextFire = VFX.GetComponent<ParticleSystem>().main.duration;
                    StartCoroutine(DisableInputForSeconds(nextFire));
                }
            }
        }


    }
    IEnumerator DisableInputForSeconds(float seconds)
    {
        isTriggerEnabled = false; // ���� ��Ȱ��ȭ
        yield return new WaitForSeconds(seconds); // ������ �ð� ���� ���
        isTriggerEnabled = true; // ���� ��Ȱ��ȭ
    }


  

    public void Shoot()
    {
        // �Ѿ� ������ ����
        VFX = Instantiate(effectPrefabs[magicID], transform.position, transform.rotation);

        // �Ѿ� �߻�
        VFX.GetComponent<Rigidbody>().velocity = VFX.transform.forward * bulletSpeed;

        // 2�� �ڿ� �ı�
        Destroy(VFX, VFX.GetComponent<ParticleSystem>().main.duration);
    }


    public void onStaff()
    {
        isGrabbed = true;
    }
    public void offStaff()
    {
        isGrabbed = false;
    }

    public void MasicSound()
    {
        if(magicID <=023)
        {
            sound.stackState = StaffSound.StackSoundState.Arance;
        }
        else if(magicID >= 030 && magicID <= 111)
        {
            sound.stackState = StaffSound.StackSoundState.Positive;
        }
        else if (magicID >= 112 && magicID <= 201)
        {
            sound.stackState = StaffSound.StackSoundState.Fire;
        }
        else if (magicID >= 202 && magicID <= 230)
        {
            sound.stackState = StaffSound.StackSoundState.Ice;
        }
        else if (magicID >= 231 && magicID <= 314)
        {
            sound.stackState = StaffSound.StackSoundState.Lightning;
        }
        else if (magicID >= 320 && magicID <= 333)
        {
            sound.stackState = StaffSound.StackSoundState.Nature;
        }
        else
        {
            return;
        }

        sound.StackPlaySound();
    }
}
