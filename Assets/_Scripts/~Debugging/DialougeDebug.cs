using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
public class DialougeDebug : MonoBehaviour
{

    #region Variables

    private SayDialog _sayDialog; 
    
    [SerializeField] private Flowchart flowchart;
    [SerializeField] private string textBeforeGlyph;
    [SerializeField] private string curString;
    [SerializeField] private bool once = true;
    
    #endregion

    private void Update()
    {
        //if (SayDialog.ActiveSayDialog != null)
        //{
        //    curString = SayDialog.ActiveSayDialog.StoryText;
        //    Debug.Log(curString);
        //    if (curString.Contains(textBeforeGlyph) && once)
        //    {
        //        Debug.Log("glyph here bro");
        //        once = false;
        //    }
        //}
    }
}
