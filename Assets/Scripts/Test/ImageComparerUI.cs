using UnityEngine;
using UnityEngine.UI;

public class ImageComparerUI : MonoBehaviour
{
    public Texture2D image2;
    public Button compareButton;
    public RawImage image1Raw;
    public Text resultText;
    public float similarityThreshold = 0.3f; // ���缺 �Ӱ谪

    void Start()
    {
        compareButton.onClick.AddListener(CompareImages);
    }

    void CompareImages()
    {
        if (image1Raw.texture == null || image2 == null)
        {
            Debug.LogError("�̹����� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        Texture2D image1 = (Texture2D)image1Raw.texture;

        // ��� �κ��� �����ϰ� ����ϴ�.
        image1 = MakeWhiteTransparent(image1);

        int matchingPixels = 0;
        int totalPixels = 0;
        int drawnPixels = 0;

        for (int x = 0; x < image1.width; x++)
        {
            for (int y = 0; y < image1.height; y++)
            {
                Color color1 = image1.GetPixel(x, y);
                Color color2 = image2.GetPixel(x, y);

                if (color1.a >= 0.5f && color2.a >= 0.5f)
                {
                    totalPixels++;
                    if (color1 != Color.clear)
                    {
                        drawnPixels++;
                        if (color1 == color2)
                        {
                            matchingPixels++;
                        }
                    }
                }
            }
        }

        float similarity = (float)matchingPixels / drawnPixels;
        string result = (similarity >= similarityThreshold) ? "�̹����� �����մϴ�." : "�̹����� �ٸ��ϴ�.";
        resultText.text = "�̹��� ���絵: " + similarity.ToString("P2") + "\n" + result;
    }

    Texture2D MakeWhiteTransparent(Texture2D texture)
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color pixelColor = texture.GetPixel(x, y);
                if (pixelColor.r >= 0.9f && pixelColor.g >= 0.9f && pixelColor.b >= 0.9f)
                {
                    pixelColor.a = 0f; // ��� �ȼ��� ���� ���� 0���� �����Ͽ� �����ϰ� ����ϴ�.
                    texture.SetPixel(x, y, pixelColor);
                }
            }
        }
        texture.Apply();
        return texture;
    }
}
