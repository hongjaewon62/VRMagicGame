using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class DrawingManager : MonoBehaviour
{
    public RawImage canvasImage;
    public Color drawColor = Color.black;
    public int brushSize = 2;

    private Texture2D texture;
    private bool isDrawing = false;
    private Vector2 collisionPoint;

    // XR Input reference
    private InputDevice rightController;
    public GameObject cane;
    private CapsuleCollider caneCollider;

    void Start()
    {
        // Create a new texture and apply it to the RawImage
        texture = new Texture2D((int)canvasImage.rectTransform.rect.width, (int)canvasImage.rectTransform.rect.height);
        canvasImage.texture = texture;
        ClearCanvas();

        // Get the capsule collider from the cane object
        caneCollider = cane.GetComponent<CapsuleCollider>();

        // Get the right hand controller device
        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count > 0)
        {
            rightController = rightHandDevices[0];
        }
    }

    void Update()
    {
        if (rightController.isValid)
        {
            rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool isRTriggerPressed);

            if (isRTriggerPressed && !isDrawing)
            {
                isDrawing = true;
            }
            if (!isRTriggerPressed && isDrawing)
            {
                isDrawing = false;
            }

            // If drawing, cast a ray from the cane's position
            if (isDrawing)
            {
                RaycastHit hit;
                if (Physics.Raycast(caneCollider.transform.position, -caneCollider.transform.up, out hit))
                {
                    // Check if the hit object is the canvas
                    if (hit.collider.gameObject == canvasImage.gameObject)
                    {
                        // Convert the hit point to local canvas coordinates
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasImage.rectTransform, hit.point, null, out collisionPoint);
                        Draw(collisionPoint);
                    }
                }
            }
        }
    }

    void Draw(Vector2 position)
    {
        // Convert local point to texture coordinates
        Vector2 textureCoord = LocalToTextureCoords(position);
        int x = (int)textureCoord.x-50;
        int y = (int)textureCoord.y-50;

        if (IsWithinBounds(x, y))
        {
            for (int i = 0; i < brushSize; i++)
            {
                for (int j = 0; j < brushSize; j++)
                {
                    texture.SetPixel(x , y , drawColor);
                }
            }
            texture.Apply();
        }
    }

    Vector2 LocalToTextureCoords(Vector2 localPoint)
    {
        float width = canvasImage.rectTransform.rect.width;
        float height = canvasImage.rectTransform.rect.height;

        // Translate local coordinates to texture coordinates
        float x = (localPoint.x + width / 2) / width * texture.width;
        float y = (localPoint.y + height / 2) / height * texture.height;

        return new Vector2(x, y);
    }

    bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < texture.width &&
               y >= 0 && y < texture.height;
    }

    public void ClearCanvas()
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, Color.white);
            }
        }
        MakeWhiteTransparent(texture);
        texture.Apply();
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
                    pixelColor.a = 0f; // 흰색 픽셀의 알파 값을 0으로 설정하여 투명하게 만듭니다.
                    texture.SetPixel(x, y, pixelColor);
                }
            }
        }
        texture.Apply();
        return texture;
    }
}
