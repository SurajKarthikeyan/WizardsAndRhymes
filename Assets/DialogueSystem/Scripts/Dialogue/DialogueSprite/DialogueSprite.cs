using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controls the animation of a dialogue sprite, which position themselves on the left or right of the dialogue display
/// </summary>
public class DialogueSprite : MonoBehaviour
{
    [Tooltip("The animator which controls animations to move the sprite on adn off the screen")]
    public Animator animator;
    [Tooltip("The image of the dialogue sprite that gets changed to match the spearker's sprite")]
    public Image image;
    [Tooltip("The name of the character currently using this dialgoue sprite")]
    public string currentCharacterName;
    [Tooltip("The position of this dialogue sprite on the screen, either left or right")]
    public Speaker.Position spritePosition = Speaker.Position.Left;

    /// <summary>
    /// Makes this dialogue sprite active in the display and makes its image use the sprite from the passed in dialogue.
    /// </summary>
    /// <param name="dialogue">The dialogue controlling this dialogue sprite</param>
    public void MakeActiveSprite(Dialogue dialogue)
    {
        if (dialogue.speaker.speakerSprite != null)
        {
            image.sprite = dialogue.speaker.speakerSprite;
            image.SetNativeSize();

            // Flip the sprite to make sure it faces towards the dialogue box (towards the other speaker)
            RectTransform imagesRectTransform = image.GetComponent<RectTransform>();
            // also apply any scaling for the spearker's sprite to the image
            imagesRectTransform.localScale = (spritePosition == dialogue.speaker.baseSpriteFacesDirection) ? new Vector3(-1 * dialogue.speaker.scaleMultiplier, 1 * dialogue.speaker.scaleMultiplier, 1) : new Vector3(1 * dialogue.speaker.scaleMultiplier, 1 * dialogue.speaker.scaleMultiplier, 1);

            // Apply any offset information to position the image
            RectTransform canvasImageIsOnRectTransform = imagesRectTransform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            float newXPosition = (spritePosition == Speaker.Position.Right) ? -dialogue.speaker.fractionalPositionOffsetX * canvasImageIsOnRectTransform.rect.width : dialogue.speaker.fractionalPositionOffsetX * canvasImageIsOnRectTransform.rect.width;
            imagesRectTransform.anchoredPosition = new Vector2(newXPosition, dialogue.speaker.fractionalPositionOffsetY * canvasImageIsOnRectTransform.rect.height);

            animator.SetBool("active", true);
            animator.SetBool("enter", true);
        }
        
    }

    /// <summary>
    /// Makes this dialogue sprite inactive, thus making it not the current speaker using the dialogue box
    /// </summary>
    public void MakeInactiveSprite()
    {
        animator.SetBool("active", false);
    }

    /// <summary>
    /// Resets the state of the dialogue sprite and removes it from being on the display
    /// </summary>
    public void ResetSprite()
    {
        animator.SetTrigger("reset");
        animator.SetBool("active", false);
        animator.SetBool("enter", false);
        currentCharacterName = "";
    }

    /// <summary>
    /// Returns whether or not the dialogue sprite is in the active position.
    /// </summary>
    /// <returns>Whether or not the dialogue sprite is in the active position</returns>
    public bool CheckIfActive()
    {
        return animator.GetBool("active");
    }
}
