using UnityEngine;

public class CooldownBar : MonoBehaviour
{
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private float offsetY = 40f; // distância da barra em relação ao cursor

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        if (myCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // Em Overlay, a posição do mouse já está em coordenadas do canvas
            rectTransform.position = mousePosition + new Vector2(0, offsetY);
        }
        else
        {
            // Para Camera/World Space, precisamos converter
            Camera uiCamera = myCanvas.worldCamera;
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                myCanvas.transform as RectTransform,
                mousePosition,
                uiCamera,
                out canvasPos
            );
            rectTransform.anchoredPosition = canvasPos + new Vector2(0, offsetY);
        }
    }
}
