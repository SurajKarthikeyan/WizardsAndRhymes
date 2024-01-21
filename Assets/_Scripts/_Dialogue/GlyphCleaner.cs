using System;
using System.Collections;
using System.Collections.Generic;
using MonKey.Extensions;
using UnityEngine;
using TMPro;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class GlyphCleaner : MonoBehaviour
{
    #region Variables

    [Header("TextMesh and related")] 
    [Tooltip("Text Mesh Pro of the Subtitle object in the current existing dialogue")]
    [SerializeField] private TextMeshProUGUI subtitleTMPro;
    [Tooltip("Canvas that the dialogue lies on")]
    [SerializeField] private Transform dialogueCanvasTransform;
    [SerializeField] private TextMeshProTypewriterEffect subtitleTypewriterEffect;


    [Header("Glyph GameObject")] 
    [SerializeField] private GameObject glyphGameObject1;
    [SerializeField] private GameObject glyphGameObject2;
    [SerializeField] private GameObject glyphGameObject3;
    [SerializeField] private int xOffSet;
    [SerializeField] private int yOffSet;

    [Header("Glyph Click Menu")] 
    [Tooltip("Opacity Panel when glyph in rap is clicked")]
    [SerializeField] private GameObject subtitleOpacityPanel;
    [Tooltip("Target Opacity Value, between 0 and 1")] [Range(0f, 1f)]
    [SerializeField] private float opacityPanelValue;

    [NonSerialized] private RectTransform subtitleRectTransform;
    [NonSerialized] private TMP_TextInfo subtitleTextInfo;
    [NonSerialized] private List<GameObject> subtitleGlyphList;
    [NonSerialized] private GameObject currentGlyphGO;
    #endregion

    #region UnityMethods

    private void Start()
    {
        subtitleTypewriterEffect.OnLeaveSourceCode += RealTimeCharacterReplacement;
        subtitleRectTransform = subtitleTMPro.GetComponent<RectTransform>();
        subtitleGlyphList = new List<GameObject>();
    }

    #endregion

    #region PrivateMethods

    /// <summary>
    /// Function that takes in the current real time string displayed on the canvas
    /// Accessed through PixelCrusher source code
    /// </summary>
    /// <param name="realTimeString"></param>
    public void RealTimeCharacterReplacement(string realTimeString, char symbolIndicator)
    {
        TMP_TextInfo subtitleTextInfo = subtitleTMPro.textInfo; // Get the information from TMPro (only TMPro, not available on regular text)
        SetGlyphGameObject(symbolIndicator);
        int charIndex = realTimeString.Length - 1;
        int vertexIndex = subtitleTextInfo.characterInfo[charIndex].vertexIndex;    // Get the vertex of the char at charIndex
        
        Vector3 localPosition = subtitleTextInfo.meshInfo[0].vertices[vertexIndex];
        Vector3 worldPosition = subtitleRectTransform.TransformPoint(localPosition);    // Local position to world position conversion
        
        worldPosition.x += xOffSet;
        worldPosition.y += yOffSet; // Apply centering offsets 
        
        GameObject instantiatedGlyph = Instantiate(currentGlyphGO, dialogueCanvasTransform);   // Instantiate and place glyph at location
        instantiatedGlyph.GetComponent<RectTransform>().position = worldPosition;

        Button glyphButton = instantiatedGlyph.GetComponent<Button>();  //Glyph has button so it can be clicked
        glyphButton.onClick.AddListener(OnGlyphClick);  // Add listener to glyph 
        subtitleGlyphList.Add(instantiatedGlyph);
        
    }

    public void SetGlyphGameObject(char indicator)
    {
        switch (indicator)
        {
            case '@':
                currentGlyphGO = glyphGameObject1;
                break;
            case '#':
                currentGlyphGO = glyphGameObject2;
                break;
            case '$':
                currentGlyphGO = glyphGameObject3;
                break;
        }
    }

    public void OnGlyphClick()
    {
        Color opacityColor = subtitleOpacityPanel.GetComponent<Image>().color;
        opacityColor.a = opacityPanelValue;
        subtitleOpacityPanel.GetComponent<Image>().color = opacityColor;
        subtitleOpacityPanel.SetActive(true);
    }
    
    public void OnContinue()
    {
        for (int i = 0; i < subtitleGlyphList.Count; i++)
        {
            Destroy(subtitleGlyphList[i]);
        }
        subtitleGlyphList.Clear();
    }

    #endregion
}
