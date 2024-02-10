using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeTime;


    public void OnFade()
    {
        StartCoroutine(FadeOut(fadeTime));
    }
    
    IEnumerator FadeOut(float intervalWait)
    {
        bool check = false;
        while (true)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b,
                fadeImage.color.a + 0.05f);

            if (fadeImage.color.a >= 1)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
                break;
            }

            yield return new WaitForSeconds(intervalWait);
        }

        StartCoroutine(FadeIn(fadeTime));
    }

    IEnumerator FadeIn(float intervalWait)
    {
        while (true)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a - 0.05f);
            if (fadeImage.color.a <= 0)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
                break;
            }
            yield return new WaitForSeconds(intervalWait);
        }
    }
}
