using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button clearButton;
    public DrawingManager drawingManager;

    void Start()
    {
        clearButton.onClick.AddListener(ClearCanvas);
    }

    void ClearCanvas()
    {
        drawingManager.ClearCanvas();
    }
}
