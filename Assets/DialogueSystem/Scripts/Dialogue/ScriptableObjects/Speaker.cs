using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls setting up a speaker to be used by the dialogue system
/// A speaker controls the name of the character speaking, the sprite that represents them in dialogue, the position they prefer to have on the screen,
/// and the scaing of their image to make it fit on the screen.
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue/Speaker")]
public class Speaker : ScriptableObject
{
    [Header("General Settings")]
    [Tooltip("The name of the speaker")]
    public string speakerName;
    [Tooltip("The sprite to use for this speaker")]
    public Sprite speakerSprite;
    public enum Position { Left, Right };
    [Header("Positioning Settings")]
    [Tooltip("The side of the screen this speaker would prefer to be on")]
    public Position preferredPosition = Position.Left;
    [InspectorName("Base Sprite Faces")]
    [Tooltip("The direction the speaker's sprite faces in its import")]
    public Position baseSpriteFacesDirection = Position.Right;
    [Tooltip("How should the speaker's sprite be offset in relation to the screen's height? 0.5 would put the bottom of the sprite at half the screens height, -0.5 would put it below the edge of the screen.")]
    public float fractionalPositionOffsetY = 0;
    [Tooltip("How should the speaker's sprite be offset in relation to the screen's width? 0.5 would move it to the right (or left when the speaker enters from screen right) by half the screen width.")]
    public float fractionalPositionOffsetX = 0;
    [Tooltip("How much should the sprite be scaled down or up? 1 is native aspect ratio, 0.5 would be half the regular aspect ratio for example")]
    public float scaleMultiplier = 1;

}
