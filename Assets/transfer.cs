using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class transfer : MonoBehaviour
{

    public RawImage image1Raw;

    void Start()
    {
        Texture2D image1 = (Texture2D)image1Raw.texture;
        image1 = MakeWhiteTransparent(image1);

        
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
                    pixelColor.a = 0f;
                    texture.SetPixel(x, y, pixelColor);
                }
            }
        }
        texture.Apply();
        return texture;
    }

}
