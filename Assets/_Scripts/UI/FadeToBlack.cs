using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script to alter the alpha of an image to fade to black or fade to transparency
/// </summary>
public class FadeToBlack : MonoBehaviour
{
    #region Variables
    [Tooltip("Image that is used to change its alpha")]
    public Image fadeImage;
    [Tooltip("Timer to fade to black")]
    public float fadeOutTime;
    [Tooltip("Timer to fade to transparency")]
    public float fadeInTime;
    #endregion

    #region UnityMethods
    /// <summary>
    /// When the game starts, fade in on first frame
    /// </summary>
    private void Start()
    {
        OnFadeIn();
    }
    #endregion

    #region CustomMethods
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
    /// intervalWait is the time it yields between each change in alpha shade
    /// </summary>
    /// <param name="intervalWait"></param>
    /// <returns></returns>
    IEnumerator FadeOut(float intervalWait)
    {
        fadeImage.gameObject.SetActive(true);
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
    /// intervalWait is the time it yields between each change in alpha shade
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
    #endregion

}
