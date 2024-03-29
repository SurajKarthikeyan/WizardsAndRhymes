using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropItemSlot : MonoBehaviour, IDropHandler
{
    public WordCanvasManager manager;

    public string correctWord;

    public bool correctSlot;
    
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                GetComponent<RectTransform>().anchoredPosition;

            correctSlot = HasValidWord(eventData.pointerDrag.gameObject.name);

            if (correctSlot)
            {
                eventData.pointerDrag.GetComponent<DragDropItem>().canMove = false;
                manager.ValidateCanvas();
            }
        }
        else
        {
            correctSlot = false;
        }
    }

    public bool HasValidWord(string word)
    {
        return word == correctWord;
    }
}
