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

    private CanvasGroup canvasGroup;

    public bool canMove = true;

    private void Awake()
    {
        rectTransform = GetComponent <RectTransform> ();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
   
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .6f;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canMove)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}
