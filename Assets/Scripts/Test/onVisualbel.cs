using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class onVisualbel : MonoBehaviour
{
    private StaffSound sound;

    [SerializeField] GameObject staff;
    [SerializeField] DrawingManager drawingManager;

    public bool isGrabbed = false;

    InputDevice left;
    InputDevice right;
    private bool onDraw = false;
    private bool isButtonA = false;
    [SerializeField] GameObject GameObject;
    private bool isButtonB = false;
    private bool isButtonX = false;
    private bool isButtonY = false;
    private bool isLTriggerPressed = false;

    private bool prevButtonA;
    private bool prevButtonB;
    private bool prevButtonX;
    private bool prevButtonY;
    private bool prevButtonLT;

    [SerializeField]
    private GameObject Set;
    [SerializeField]
    private GameObject FiringVFX1;
    [SerializeField]
    private GameObject FiringVFX2;
    [SerializeField]
    private GameObject FiringVFX3;
    [SerializeField]
    private GameObject FiringVFX4;
    [SerializeField]
    private GameObject FiringVFX;


    public Texture2D referenceTexture1;
    public Texture2D referenceTexture2;
    public Texture2D referenceTexture3;
    public Texture2D referenceTexture4;
    public Texture2D referenceTexture5;

    public Texture2D referenceTexture;

    [SerializeField]
    private GameObject ChargePoint;
    [SerializeField]
    private GameObject ChargePoint1;
    [SerializeField]
    private GameObject ChargePoint2;
    [SerializeField]
    private GameObject ChargePoint3;
    [SerializeField]
    private GameObject ChargePoint4;

    private float stayTime = 0.0f;
    public float requiredTime = 5.0f;
    public float similarity;
    public int ran = 4;
    public int count;
    private bool isTriggerEnabled = true;
   
    GameObject chargingVFXInstance;
    RaycastHit hit;
   
   
    public RawImage userDrawnImage; // ������ �׸� �̹���
     // ���� ��� �̹���
    public Text resultText;
    public RawImage overlayImage; // �������ϰ� ���� RawImage

    private Texture2D texture;
   
    [Range(1, 1000)]
    public int sampleSize = 500; // ���ø��� �ȼ� ��
    [Range(0, 10)]
    public int neighborhoodSize = 1; // ���� ����

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<StaffSound>();
        prevButtonA = false;
        prevButtonB = false;
        prevButtonX = false;
        prevButtonY = false;
        prevButtonLT = false;

        if (overlayImage != null && referenceTexture != null)
        {
            overlayImage.texture = referenceTexture;
            overlayImage.color = new Color(1f, 1f, 1f, 0.5f); // ������ ����
        }
        
    }

    // Update is called once per frame
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
            if (overlayImage != null && referenceTexture != null)
            {
                overlayImage.texture = referenceTexture = ran-- == 5 ? referenceTexture4 : (ran == 4 ? referenceTexture3 : (ran == 3 ? referenceTexture3 : (ran == 2 ? referenceTexture2 : referenceTexture1)));
                if (ran < 1)
                    ran = 1;
                overlayImage.color = new Color(1f, 1f, 1f, 0.5f); // ������ ����
            }
        }
        prevButtonA = isButtonA;

        right.TryGetFeatureValue(CommonUsages.secondaryButton, out isButtonB);
        if (isButtonB && !prevButtonB)
        {
            if (overlayImage != null && referenceTexture != null)
            {
                overlayImage.texture = referenceTexture = ran++ == 1 ? referenceTexture1 : (ran == 2 ? referenceTexture2 : (ran == 3 ? referenceTexture3 : (ran == 4 ? referenceTexture4 : referenceTexture5)));
                if (ran > 5)
                    ran = 4;
                overlayImage.color = new Color(1f, 1f, 1f, 0.5f); // ������ ����
            }
        }
        prevButtonB = isButtonB;

        left.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonX);
        if (isButtonX && !prevButtonX)
        {
            
        }
        prevButtonX = isButtonX;

        left.TryGetFeatureValue(CommonUsages.secondaryButton, out isButtonY);
        if (isButtonY && !prevButtonY)
        {
            TriggerPressed();
        }
        prevButtonY = isButtonY;

        right.TryGetFeatureValue(CommonUsages.triggerButton, out bool isRTriggerPressed);
        if (isRTriggerPressed)
        {
            ImageDrawing();
        }
        left.TryGetFeatureValue(CommonUsages.triggerButton, out isLTriggerPressed);
        if (isLTriggerPressed && !prevButtonLT)
        {
            CompareImages();
            new WaitForSeconds(1.5f);
            MagicShot();
        }
        prevButtonLT = isLTriggerPressed;

    }
    void TriggerPressed()
    {
        if (GameObject != null)
        {
            GameObject.SetActive(!GameObject.activeSelf);
        }
        
    }
    void ImageDrawing()
    {

    }
    void MagicShot()
    {
        count = similarity > 0.75 ? 3 : (similarity > 0.5 ? 2 : (similarity > 0.25 ? 1 : 0));
        if (count > 0 && onDraw)
        {
            Magicshot2(count);
            onDraw = false;
            similarity = 0;
        }
        drawingManager.ClearCanvas();
    }
    void Magicshot2(int count)
    {
        switch (ran)
        {
            case 1:
                Set = FiringVFX;
                sound.paintState = StaffSound.PaintSoundState.Ice;
                break;
            case 2:
                Set = FiringVFX1;
                sound.paintState = StaffSound.PaintSoundState.Stome;
                break;
            case 3:
                Set = FiringVFX2;
                sound.paintState = StaffSound.PaintSoundState.Dark;
                break;
            case 4:
                Set = FiringVFX3;
                sound.paintState = StaffSound.PaintSoundState.Light;
                break;
            case 5:
                Set = FiringVFX4;
                sound.paintState = StaffSound.PaintSoundState.Fire;
                break;
        }
        
        while (count > 0)
        {
            switch (count)
            {
                case 1:
                    ChargePoint = ChargePoint1;
                    break;
                case 2:
                    ChargePoint = ChargePoint2;
                    break;
                case 3:
                    ChargePoint = ChargePoint3;
                    break;
                case 4:
                    ChargePoint = ChargePoint4;
                    break;
            }
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

                sound.PaintPlaySound();

                // Instantiate the firing VFX at the charge point
                GameObject firingVFXInstance = Instantiate(Set, ChargePoint.transform.position, ChargePoint.transform.rotation);

                // Set the velocity of the firing VFX to move it forward
                Rigidbody rb = firingVFXInstance.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = 10f * ChargePoint.transform.forward;
                }

                // Destroy the firing VFX after 5 seconds
                Destroy(firingVFXInstance, 5f);

                // Disable trigger input for a short duration
            }
            count--;
        }
    }

    IEnumerator DisableInputForSeconds(float seconds)
    {
        isTriggerEnabled = false; // ���� ��Ȱ��ȭ
        yield return new WaitForSeconds(seconds); // ������ �ð� ���� ���
        isTriggerEnabled = true; // ���� ��Ȱ��ȭ
    }

    public void CompareImages()
    {
        if (userDrawnImage == null)
        {
            resultText.text = "UserDrawnImage�� �������� �ʾҽ��ϴ�.";
            return;
        }
        if (referenceTexture == null)
        {
            resultText.text = "ReferenceTexture�� �������� �ʾҽ��ϴ�.";
            return;
        }

        Texture2D userTexture = userDrawnImage.texture as Texture2D;

        if (userTexture == null)
        {
            resultText.text = "UserDrawnImage�� �ؽ�ó�� ��ȿ���� �ʽ��ϴ�.";
            return;
        }

        // �� �̹����� ���� ũ��� ��������
        int targetWidth = referenceTexture.width;
        int targetHeight = referenceTexture.height;

        Texture2D resizedUserTexture = ResizeTexture(userTexture, targetWidth, targetHeight);

        similarity = CalculateSimilarity(resizedUserTexture, referenceTexture, sampleSize);
        resultText.text = "���絵: " + (similarity * 100f).ToString("F2") + "%";
        onDraw = true;

        

    }

    private Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float scaleX = (float)source.width / targetWidth;
        float scaleY = (float)source.height / targetHeight;

        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                float px = x * scaleX;
                float py = y * scaleY;
                result.SetPixel(x, y, source.GetPixelBilinear(px / source.width, py / source.height));
            }
        }

        result.Apply();
        return result;
    }

    private float CalculateSimilarity(Texture2D tex1, Texture2D tex2, int sampleSize)
    {
        List<Vector2Int> paintedPixels1 = GetPaintedPixels(tex1);
        List<Vector2Int> paintedPixels2 = GetPaintedPixels(tex2);

        if (paintedPixels1.Count == 0 || paintedPixels2.Count == 0)
        {
            return 0f;
        }

        int similarCount = 0;
        int samples = Mathf.Min(sampleSize, paintedPixels1.Count, paintedPixels2.Count);

        for (int i = 0; i < samples; i++)
        {
            Vector2Int pos1 = paintedPixels1[Random.Range(0, paintedPixels1.Count)];
            Vector2Int pos2 = paintedPixels2[Random.Range(0, paintedPixels2.Count)];

            if (PositionsAreSimilar(pos1, tex1, tex2))
            {
                similarCount++;
            }
        }

        return (float)similarCount / samples;
    }

    private bool PositionsAreSimilar(Vector2Int pos1, Texture2D tex1, Texture2D tex2)
    {
        bool foundSimilar = false;
        for (int y = -neighborhoodSize; y <= neighborhoodSize; y++)
        {
            for (int x = -neighborhoodSize; x <= neighborhoodSize; x++)
            {
                int nx = pos1.x + x;
                int ny = pos1.y + y;

                if (nx >= 0 && nx < tex2.width && ny >= 0 && ny < tex2.height)
                {
                    bool dot1 = tex1.GetPixel(pos1.x, pos1.y).a != 0;
                    bool dot2 = tex2.GetPixel(nx, ny).r == 0 && tex2.GetPixel(nx, ny).g == 0 && tex2.GetPixel(nx, ny).b == 0;

                    if (dot1 && dot2)
                    {
                        foundSimilar = true;
                        break;
                    }
                }
            }
            if (foundSimilar) break;
        }

        return foundSimilar;
    }

    private List<Vector2Int> GetPaintedPixels(Texture2D texture)
    {
        List<Vector2Int> paintedPixels = new List<Vector2Int>();
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color pixelColor = texture.GetPixel(x, y);
                if (pixelColor.a != 0 && pixelColor.r == 0 && pixelColor.g == 0 && pixelColor.b == 0) // ������ �ȼ��� Ȯ��
                {
                    paintedPixels.Add(new Vector2Int(x, y));
                }
            }
        }
        return paintedPixels;
    }

    public void onStaff()
    {
        isGrabbed = true;
    }
    public void offStaff()
    {
        isGrabbed = false;
        TriggerPressed();
    }
}
