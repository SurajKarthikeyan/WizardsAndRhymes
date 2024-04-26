using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLogic : MonoBehaviour
{
    public Image fadeImage;

    public DialogueHolder dialouge;

    public Canvas dialogueCanvas;

    public Collider dialougeCollider;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("in story logic start");
        Debug.Log(FlagManager.hasReloaded);
        if (!FlagManager.hasReloaded)
        {
            StartCoroutine(FadeOut(fadeImage));
            dialouge.TriggerDialogue();
        }
        else 
        { 
            GetComponent<Canvas>().enabled = false;
            ResetPlaneDistance();
            if (!FlagManager.instance.GetFlag("enemyWave1Completed"))
            {
                dialougeCollider.enabled = true;
            }
            //else if (FlagManager.instance.GetFlag("enemyWave1Completed"))
            //{
            //    Debug.Log("enabling player controls");
            //    PlayerController.instance.EnablePlayerControls();
            //    PlayerController.instance.EnablePlayerControls();
            //    PlayerController.instance.EnablePlayerAttackControls();
            //}
        }
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
