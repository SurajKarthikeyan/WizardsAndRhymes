using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    [Tooltip("Image that is used to change its alpha")]
    [SerializeField] private Image fadeImage;
    [Tooltip("Timer to fade to black")]
    [SerializeField] private float fadeOutTime;
    [Tooltip("Timer to fade to transparency")]
    [SerializeField] private float fadeInTime;

    /// <summary>
    /// When the game starts, fade in on first frame
    /// </summary>
    private void Start()
    {
        OnFadeIn();
    }

    /// <summary>
    /// Start the FadeOut Coroutine
    /// </summary>
    public void OnFadeOut()
    {
        StartCoroutine(FadeOut(fadeOutTime));
    }

    /// <summary>
    /// Start the FadeIn Coroutine
    /// </summary>
    public void OnFadeIn()
    {
        StartCoroutine(FadeIn(fadeInTime));
    }
    
    /// <summary>
    /// Coroutine that loops to increment the alpha of an image
    /// Initially activates the image and then begins a while loop
    /// At the end of each iteration, yield return waits for the interval time
    /// </summary>
    /// <param name="intervalWait"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Coroutine that loops to decrement the alpha of an image
    /// At the end of each loop, yield return waits for the interval time
    /// IMPORTANT --- The image must be set to inactive after fading out
    /// IMPORTANT --- This is so you can click through it to other UI elements
    /// </summary>
    /// <param name="intervalWait"></param>
    /// <returns></returns>
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
