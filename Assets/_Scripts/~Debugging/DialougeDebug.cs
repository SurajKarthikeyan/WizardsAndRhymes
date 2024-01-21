using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine.UIElements;

public class DialougeDebug : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    public char specialChar;

    public GameObject glyph;

    public Transform canvas;

    public TextMeshProTypewriterEffect typewriterEffect;
    
    private void Start()
    {
        typewriterEffect.OnLeaveSourceCode += RealTimeCharacterReplacement;
    }




    public void RealTimeCharacterReplacement(string currentString)
    {
        RectTransform textRectTransform = textMeshPro.GetComponent<RectTransform>();
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        int charIndex = currentString.Length - 1;
        int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;
        Vector3 localPosition = textInfo.meshInfo[0].vertices[vertexIndex];
        Vector3 worldPosition = textRectTransform.TransformPoint(localPosition);
        Debug.Log(worldPosition);
        GameObject instantiatedGlyph = Instantiate(glyph, canvas);
        instantiatedGlyph.GetComponent<RectTransform>().position = worldPosition;
    }

    void DisplayTMPPos() 
    {
        RectTransform textRectTransform = textMeshPro.GetComponent<RectTransform>();
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        List<int> charIndices = new List<int>();
        for (int i = 0; i < textMeshPro.text.Length; i++)
        {
            if (textMeshPro.text[i] == specialChar)
            {
                charIndices.Add(i);
            }
        }
        foreach (int charIndex in charIndices)
        {
            int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;
            Vector3 localPosition = textInfo.meshInfo[0].vertices[vertexIndex];
            Vector3 worldPosition = textRectTransform.TransformPoint(localPosition);
            Debug.Log(worldPosition);
            GameObject instantiatedGlyph = Instantiate(glyph, canvas);
            instantiatedGlyph.GetComponent<RectTransform>().position = worldPosition;
        }
    }
}
