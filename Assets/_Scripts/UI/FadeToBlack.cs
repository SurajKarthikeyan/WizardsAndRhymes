using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float fadeInTime;

    private void Start()
    {
        OnFadeIn();
    }

    public void OnFadeOut()
    {
        StartCoroutine(FadeOut(fadeOutTime));
    }

    public void OnFadeIn()
    {
        StartCoroutine(FadeIn(fadeInTime));
    }
    
    IEnumerator FadeOut(float intervalWait)
    {
        fadeImage.gameObject.SetActive(true);
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
        fadeImage.gameObject.SetActive(false);
    }
}
