using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Afterimage : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] player_AfterImage;

    private void Start()
    {
        spriteRenderer = GetComponent <SpriteRenderer>();

        if (player_AfterImage.Length > 0)
        {
            spriteRenderer.sprite = player_AfterImage[0];
        }
    }

    private void Update()
    {
        if (player_AfterImage.Length > 0)
        {
            for (int i = 0; i < player_AfterImage.Length; i++)
            {
                if (spriteRenderer.sprite == player_AfterImage[i])
                {
                    // 스프라이트에 따라 알파 값을 설정
                    float alpha = 1.0f - (0.1f * i);
                    SetAlpha(alpha);
                }
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    void AfterImageDestroy()
    {
        Destroy(gameObject);
    }
}
