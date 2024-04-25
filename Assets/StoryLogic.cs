using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLogic : MonoBehaviour
{
    public Image fadeImage;

    public DialogueHolder dialouge;

    public Canvas dialogueCanvas;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut(fadeImage));
        dialouge.TriggerDialogue();
    }

    public void ImageFadeOut(Image image)
    {
        StartCoroutine(FadeOut(image));
    }

    public void ResetPlaneDistance()
    {
        dialogueCanvas.planeDistance = 100;
    }

    private IEnumerator FadeOut(Image image)
    {
        while (image.color.a > 0)
        {
            float alpha = image.color.a;
            alpha -= 0.1f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return new WaitForSeconds(0.07f);
        }
        
    }
}
