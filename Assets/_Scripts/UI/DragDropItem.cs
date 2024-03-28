using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private RectTransform rectTransform;

    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent <RectTransform> ();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
