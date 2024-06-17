using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageComparer : MonoBehaviour
{
    public RawImage userDrawnImage; // 유저가 그린 이미지
    public Texture2D referenceTexture; // 비교할 대상 이미지
    public Button compareButton;
    public Text resultText;
    public RawImage overlayImage; // 반투명하게 덮을 RawImage

    [Range(1, 1000)]
    public int sampleSize = 500; // 샘플링할 픽셀 수
    [Range(0, 10)]
    public int neighborhoodSize = 1; // 비교할 범위

    void Start()
    {
        compareButton.onClick.AddListener(CompareImages);

        // OverlayImage 설정
        if (overlayImage != null && referenceTexture != null)
        {
            overlayImage.texture = referenceTexture;
            overlayImage.color = new Color(1f, 1f, 1f, 0.5f); // 반투명 설정
        }
    }

    public void CompareImages()
    {
        if (userDrawnImage == null)
        {
            resultText.text = "UserDrawnImage가 설정되지 않았습니다.";
            return;
        }
        if (referenceTexture == null)
        {
            resultText.text = "ReferenceTexture가 설정되지 않았습니다.";
            return;
        }

        Texture2D userTexture = userDrawnImage.texture as Texture2D;

        if (userTexture == null)
        {
            resultText.text = "UserDrawnImage의 텍스처가 유효하지 않습니다.";
            return;
        }

        // 두 이미지를 같은 크기로 리사이즈
        int targetWidth = referenceTexture.width;
        int targetHeight = referenceTexture.height;

        Texture2D resizedUserTexture = ResizeTexture(userTexture, targetWidth, targetHeight);

        float similarity = CalculateSimilarity(resizedUserTexture, referenceTexture, sampleSize);
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

    private float CalculateSimilarity(Texture2D tex1, Texture2D tex2,int sampleSize)
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

        return (float)similarCount / sampleSize;
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
                if (pixelColor.a != 0 && pixelColor.r == 0 && pixelColor.g == 0 && pixelColor.b == 0) // 검정색 픽셀을 확인
                {
                    paintedPixels.Add(new Vector2Int(x, y));
                }
            }
        }
        return paintedPixels;
    }
}
