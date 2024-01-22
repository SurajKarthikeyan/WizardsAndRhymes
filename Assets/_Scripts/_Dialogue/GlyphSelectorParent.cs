using System;
using System.Collections;
using System.Collections.Generic;
using MonKey.Extensions;
using UnityEngine;

public class GlyphSelectorParent : MonoBehaviour
{
    public List<GlyphSelector> glyphSelectors;

    /*private void Start()
    {
        Transform[] childArray = GetComponentsInChildren<Transform>();
        for (int i = 0; i < childArray.Length; i++)
        {
            glyphSelectors.Add(childArray[i].gameObject.GetComponent<GlyphSelector>());
        }
    }*/
}
