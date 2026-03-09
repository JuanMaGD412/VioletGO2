using UnityEngine;
using UnityEngine.UI;

public class AlphaRaycastImage : Image
{
    public float alphaThreshold = 0.1f;

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (sprite == null) return true;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out Vector2 local);
        
        Rect rect = rectTransform.rect;

        float x = (local.x - rect.x) / rect.width;
        float y = (local.y - rect.y) / rect.height;

        try
        {
            Color color = sprite.texture.GetPixelBilinear(x, y);
            return color.a >= alphaThreshold;
        }
        catch
        {
            return true;
        }
    }
}
