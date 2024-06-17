using UnityEngine;
using UnityEngine.UI;

public class ImageSimilarityChecker : MonoBehaviour
{
    public RawImage drawnImage;
    public Texture2D referenceImage;
    public Text resultText;
    public Button compareButton;

    void Start()
    {
        compareButton.onClick.AddListener(CompareImages);
    }

    void CompareImages()
    {
        if (drawnImage.texture == null || referenceImage == null)
        {
            Debug.LogError("그린 그림 또는 비교할 이미지가 없습니다.");
            return;
        }

        Texture2D drawnTexture = drawnImage.texture as Texture2D;

        // 그린 이미지의 크기를 비교할 이미지의 크기에 맞추기 위해 리사이즈
        ResizeTexture(referenceImage, drawnTexture.width, drawnTexture.height);

        float totalDifference = 0f;

        Color32[] drawnPixels = drawnTexture.GetPixels32();
        Color32[] referencePixels = referenceImage.GetPixels32();

        for (int i = 0; i < drawnPixels.Length; i++)
        {
            float difference = ColorDistance(drawnPixels[i], referencePixels[i]);
            totalDifference += difference;
        }

        float averageDifference = totalDifference / drawnPixels.Length;

        // 이미지의 유사도를 백분율로 계산
        float similarityPercentage = Mathf.Clamp(100f - (averageDifference / 255f) * 100f, 0f, 100f);


        resultText.text = "이미지 유사도: " + similarityPercentage.ToString("F2") + "%";
    }

    void ResizeTexture(Texture2D texture, int width, int height)
    {
        texture.Reinitialize(width, height);
        texture.Apply();
    }

    float ColorDistance(Color32 a, Color32 b)
    {
        float rDiff = a.r - b.r;
        float gDiff = a.g - b.g;
        float bDiff = a.b - b.b;
        return Mathf.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
    }
}
