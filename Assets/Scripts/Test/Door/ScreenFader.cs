using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage; // 검정색 이미지

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeOut(float duration)
    {
        float t = 0;
        Color color = fadeImage.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / duration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1;
        fadeImage.color = color;
    }

    public IEnumerator FadeIn(float duration)
    {
        float t = 0;
        Color color = fadeImage.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / duration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0;
        fadeImage.color = color;
    }
}