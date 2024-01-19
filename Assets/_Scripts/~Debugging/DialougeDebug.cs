using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using TMPro;

public class DialougeDebug : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    public char specialChar;

    public GameObject glyph;

    public Transform canvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            DisplayTMPPos();
        }
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
        

        //// Assuming you have the world position
        //worldPosition = /* Your world position here */;

        //// Convert world position to RectTransform position
        //Vector2 rectTransformPosition;

        //RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //    rectTransform,
        //    worldPosition,
        //    Camera.main, // Use the appropriate camera (e.g., Camera.main)
        //    out rectTransformPosition
        //);
    }
}
