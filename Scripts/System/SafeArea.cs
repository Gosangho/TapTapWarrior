using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    void Start()
    {
        UpdateSafeArea();
    }

    void UpdateSafeArea()
    {
        Rect safeArea = Screen.safeArea;

        float x = safeArea.x;
        float y = safeArea.y;
        float width = safeArea.width;
        float height = safeArea.height;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(x, y);
        rectTransform.offsetMax = new Vector2(Screen.width - (x + width), Screen.height - (y + height));
    }
}
