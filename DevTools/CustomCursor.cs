using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorNormal;
    public Texture2D cursorClick;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private bool isClicking = false;

    void Start()
    {
        Cursor.SetCursor(cursorNormal, hotspot, cursorMode);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorClick, hotspot, cursorMode);
            isClicking = true;
        }
        if (Input.GetMouseButtonUp(0) && isClicking)
        {
            Cursor.SetCursor(cursorNormal, hotspot, cursorMode);
            isClicking = false;
        }
    }
}
