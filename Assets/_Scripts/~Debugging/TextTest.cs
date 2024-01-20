using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    //public Text textComp;
    //public int charIndex;
    //public Transform canvas;
    //public TextMeshProUGUI tmpro;
    //public GameObject glyph;
    //public string compareString;
    //private bool once = true;
    //public int offset;



    //public void WEAREOUT(string outString, int index, Text outText)
    //{
    //    if (outString != null && index != null)
    //    {
    //        charIndex = index;
    //        textComp = outText;
    //        textComp.text = outString;
    //        /*Debug.Log(charIndex);
    //        Debug.Log(textComp.text.Length);
    //        Debug.Log("______");*/
    //        MaybePleaseHelpOurSanity();
    //    }
    //}
    //private void Update()
    //{
    //    if (SayDialog.ActiveSayDialog != null)
    //    {
    //        if (once)
    //        {
    //            once = false;
    //            SayDialog.ActiveSayDialog.enumeratorWriter.OnOutOfSourceCode += WEAREOUT;
    //        }
    //        /*tmpro.text = SayDialog.ActiveSayDialog.enumeratorWriter.leftString.ToString();
    //        Debug.Log(tmpro.text);
    //        if (tmpro.text.Contains(compareString) && once)
    //        {
    //            Debug.Log("PoggChamp");
    //            once = true;
    //        }*/
    //    }
    //}

    //void PrintPos()
    //{
    //    string text = textComp.text;
    //    if (charIndex >= text.Length)
    //        return;

    //    TextGenerator textGen = new TextGenerator(text.Length);
    //    Vector2 extents = textComp.gameObject.GetComponent<RectTransform>().rect.size;
    //    textGen.Populate(text, textComp.GetGenerationSettings(extents));

    //    int newLine = text.Substring(0, charIndex).Split('\n').Length - 1;
    //    int whiteSpace = text.Substring(0, charIndex).Split(' ').Length - 1;
    //    int indexOfTextQuad = (charIndex * 4) + (newLine * 4) - 4;
    //    if (indexOfTextQuad < textGen.vertexCount)
    //    {
    //        Vector3 avgPos = (textGen.verts[indexOfTextQuad].position +
    //        textGen.verts[indexOfTextQuad + 1].position +
    //        textGen.verts[indexOfTextQuad + 2].position +
    //        textGen.verts[indexOfTextQuad + 3].position) / 4f; 
    //        /*print(avgPos);*/
    //        PrintWorldPos(avgPos);
            
            
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Out of text bound");
    //    }
    //}

    //void PrintWorldPos(Vector3 testPoint)
    //{
    //    Vector3 worldPos = textComp.transform.TransformPoint(testPoint);
    //    Debug.Log(worldPos + " WorldPosition");
    //    //new GameObject("point").transform.position = worldPos + new Vector3(10, 0, 0);
    //    //Debug.DrawRay(worldPos, Vector3.up, Color.red, 50f);
        
    //    Instantiate(glyph, canvas);
    //    glyph.transform.position = worldPos;
    //}

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10, 10, 100, 80), "Test"))
    //    {
    //        MaybePleaseHelpOurSanity();
    //    }
    //}

    //public void MaybePleaseHelpOurSanity()
    //{
    //    RectTransform textBoxRect = textComp.GetComponent<RectTransform>();
    //    int charIndex = -10; // Replace with the index of the desired character
    //    TextGenerator textGenerator = textComp.cachedTextGenerator;
    //    UICharInfo[] charInfo = textGenerator.GetCharactersArray(); 
    //    Vector3 charPosition = charInfo[charIndex].cursorPos;
    //    Vector3 worldPosition = textBoxRect.TransformPoint(charPosition);
    //    Instantiate(glyph, canvas);
    //    glyph.transform.position = worldPosition;
    //    Debug.Log(worldPosition);
    //    /*if (textComp != null && charIndex >= 0 && charIndex < textComp.text.Length)
    //    {
    //        RectTransform rectTransform = textComp.rectTransform;
    //        TextGenerator textGenerator = textComp.cachedTextGenerator;

    //        // Ensure the text generator is up-to-date
    //        textGenerator.Invalidate();

    //        // Get the position of the character in local space
    //        UICharInfo[] charactersInfo = textGenerator.GetCharactersArray();
    //        Vector2 localPos = charactersInfo[charIndex].cursorPos;
    //        //Debug.Log(localPos + " Local Pos");

    //        // Convert local position to world position
    //        Vector3 worldPos;
    //        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, localPos, Camera.main, out worldPos);
    //        Instantiate(glyph, canvas);
    //        glyph.transform.position = localPos;
    //        //Debug.Log("World Position of character " + charIndex + ": " + worldPos);
            
    //    }
        
    //    else
    //    {
    //        Debug.LogError("Ensure the Text component and valid character index are set.");
    //    }*/
    //}
}
