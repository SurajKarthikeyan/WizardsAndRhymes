using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Based on this forum post: https://discussions.unity.com/t/create-texture-from-current-camera-view/86847/3
/// <summary>
/// Editor tool for rendering text to a Texture asset
/// </summary>
public class EditorRenderTextToTexture : MonoBehaviour
{
    [Tooltip("The camera to render from")]
    [SerializeField] Camera renderCamera;
    [Tooltip("The camera's output render texture")]
    [SerializeField] RenderTexture renderTexture;
    [Tooltip("The resolution of the texture to render")]
    [SerializeField] Vector2Int outputResolution;
    [Tooltip("The folder to save the texture in")]
    [SerializeField] string folderPath;
    [Tooltip("The name of the texture asset to save")]
    [SerializeField] string outputTextureName;

    //Render a texture and save it as an asset
    [ContextMenu("Render To Texture")]
    private void RenderToTexture()
    {
        Texture2D texture = RTImage();
        System.IO.File.WriteAllBytes(folderPath + "/" + outputTextureName + ".png", texture.EncodeToPNG());
        //AssetDatabase.Refresh();
    }

    //Render the camera's current view to a texture
    private Texture2D RTImage()
    {
        Rect rect = new Rect(0, 0, outputResolution.x, outputResolution.y);

        renderTexture.Release();
        renderTexture.width = outputResolution.x;
        renderTexture.height = outputResolution.y;
        Texture2D screenShot = new Texture2D(outputResolution.x, outputResolution.y, TextureFormat.RGBA32, false);

        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();
        renderCamera.targetTexture = null;

        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);

        renderCamera.targetTexture = null;
        RenderTexture.active = null;

        return screenShot;
    }
}
