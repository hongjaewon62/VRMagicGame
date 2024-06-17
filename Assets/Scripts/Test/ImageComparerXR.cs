using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using CommonUsages = UnityEngine.XR.CommonUsages;

public class ImageComparerXR : MonoBehaviour
{
    public RawImage userDrawnImage;
    public Texture2D referenceTexture;
    public Text resultText;
    public RawImage overlayImage;
    public GameObject drawingCanvas;

    [Range(1, 1000)]
    public int sampleSize = 500;
    [Range(0, 10)]
    public int neighborhoodSize = 1;

    private bool isDrawingMode = false;
    private XRController leftController;
    private XRController rightController;
    private Texture2D drawingTexture;

    void Start()
    {
        leftController = FindController(XRNode.LeftHand);
        rightController = FindController(XRNode.RightHand);

        if (overlayImage != null && referenceTexture != null)
        {
            overlayImage.texture = referenceTexture;
            overlayImage.color = new Color(1f, 1f, 1f, 0.5f);
        }

        drawingTexture = new Texture2D((int)userDrawnImage.rectTransform.rect.width, (int)userDrawnImage.rectTransform.rect.height, TextureFormat.RGBA32, false);
        userDrawnImage.texture = drawingTexture;
    }

    void Update()
    {
        HandleInput();
    }

    private XRController FindController(XRNode hand)
    {
        foreach (var controller in FindObjectsOfType<XRController>())
        {
            if (controller.controllerNode == hand)
            {
                return controller;
            }
        }
        return null;
    }

    private void HandleInput()
    {
        //if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool yButton) && yButton)
        //{
        //    ToggleDrawingMode();
        //}

        if (isDrawingMode)
        {
            DrawOnCanvas();
        }

        //if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool bButton) && bButton)
        //{
        //    CompareImages();
        //}
    }

    private void ToggleDrawingMode()
    {
        isDrawingMode = !isDrawingMode;
        drawingCanvas.SetActive(isDrawingMode);
    }

    private void DrawOnCanvas()
    {
        // Raycast from the controller to detect the canvas
        Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == userDrawnImage.transform)
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(userDrawnImage.rectTransform, hit.point, null, out localPoint);

                int x = Mathf.RoundToInt(localPoint.x + userDrawnImage.rectTransform.rect.width / 2);
                int y = Mathf.RoundToInt(localPoint.y + userDrawnImage.rectTransform.rect.height / 2);

                drawingTexture.SetPixel(x, y, Color.black);
                drawingTexture.Apply();
            }
        }
    }

    public void CompareImages()
    {
        if (userDrawnImage == null || referenceTexture == null)
        {
            resultText.text = "이미지가 설정되지 않았습니다.";
            return;
        }

        Texture2D userTexture = userDrawnImage.texture as Texture2D;
        if (userTexture == null)
        {
            resultText.text = "UserDrawnImage의 텍스처가 유효하지 않습니다.";
            return;
        }

        int targetWidth = referenceTexture.width;
        int targetHeight = referenceTexture.height;

        Texture2D resizedUserTexture = ResizeTexture(userTexture, targetWidth, targetHeight);
        float similarity = CalculateSimilarity(resizedUserTexture, referenceTexture);
        resultText.text = "유사도: " + (similarity * 100f).ToString("F2") + "%";
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

    private float CalculateSimilarity(Texture2D tex1, Texture2D tex2)
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
                if (pixelColor.a != 0 && pixelColor.r == 0 && pixelColor.g == 0 && pixelColor.b == 0)
                {
                    paintedPixels.Add(new Vector2Int(x, y));
                }
            }
        }
        return paintedPixels;
    }
}
