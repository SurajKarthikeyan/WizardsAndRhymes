using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureMatchViewportResolution : MonoBehaviour
{
    [Tooltip("The render texture to set the resolution of")]
    [SerializeField] RenderTexture renderTexture;

    int originalWidth;
    int originalHeight;

    //Might run into issues if the screen is resized mid-game
    private void Awake()
    {
        //Save the render texture's original dimensions to restore them later
        originalWidth = renderTexture.width;
        originalHeight = renderTexture.height;

        //Match the render texture's dimensions to match the viewport
        renderTexture.width = Screen.width;
        renderTexture.height = Screen.height;
    }

    private void OnDestroy()
    {
        //Reset the render texture to avoid unnecessary version control merge conflicts
        renderTexture.Release();
        renderTexture.width = originalWidth;
        renderTexture.height = originalHeight;
    }
}
