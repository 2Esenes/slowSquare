using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Vector2 cursorHotSpot = new Vector2(50, 50);

    public void ChooseCursor(Texture2D _curTex)
    {
        Cursor.SetCursor(_curTex, cursorHotSpot , CursorMode.Auto);
    }
}
