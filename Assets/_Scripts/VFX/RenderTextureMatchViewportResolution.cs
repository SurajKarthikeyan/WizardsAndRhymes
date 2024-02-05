using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Matches a RenderTexture's resolution to the screen resolution when the game starts
/// </summary>
public class RenderTextureMatchViewportResolution : MonoBehaviour
{
    #region Variables
    [Tooltip("The render texture to set the resolution of")]
    [SerializeField] RenderTexture renderTexture;


    [Tooltip("Stores to original dimensions of the render texture to restore it later")]
    Vector2Int originalDimensions;
    #endregion

    #region Unity Methods
    //Might run into issues if the screen is resized mid-game
    private void Awake()
    {
        //Save the render texture's original dimensions to restore them later
        originalDimensions.x = renderTexture.width;
        originalDimensions.y = renderTexture.height;

        //Match the render texture's dimensions to match the viewport
        renderTexture.width = Screen.width;
        renderTexture.height = Screen.height;
    }

    private void OnDestroy()
    {
        //Reset the render texture to avoid unnecessary version control merge conflicts
        renderTexture.Release();
        renderTexture.width = originalDimensions.x;
        renderTexture.height = originalDimensions.y;
    }
    #endregion
}
