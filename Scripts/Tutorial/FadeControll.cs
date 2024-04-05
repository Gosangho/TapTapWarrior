using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeControll : MonoBehaviour
{
    [SerializeField]
    private float FadeTime = 0.5f;
    private Image FadeImage;
    private Image FadeImage2;

    private void Start()
    {
        FadeImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        StartCoroutine("FadeInOut");
    }

    private void OnDisable()
    {
        StopCoroutine("FadeInOut");
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0)); // Fade in

            yield return StartCoroutine(Fade(0, 1)); // Fade Out
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while (percent <1)
        {
            current += Time.deltaTime;
            percent = current / FadeTime;

            if(FadeImage != null) {
                Color color = FadeImage.color;
                color.a = Mathf.Lerp(start, end, percent);
                FadeImage.color = color;
            }
            yield return null;
        }
    }
}
