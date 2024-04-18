using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class that represents items that can be dragged and dropped on a UI canvas
/// </summary>
public class DragDropItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [Tooltip("Boolean stating whether or not this item is currently able to move")]
    public bool canMove = true;
    
    [Tooltip("Canvas that this item is a part of")]
    [SerializeField] private Canvas canvas;

    [Tooltip("Canvasgroup that this item has")]
    private CanvasGroup canvasGroup;
    
    [Tooltip("Item's transform")]
    private RectTransform rectTransform;

    public Vector3 startPosition;

    /// <summary>
    /// Function called on scene start
    /// </summary>
    private void Awake()
    {
        rectTransform = GetComponent <RectTransform>();
        startPosition = rectTransform.position;
        Debug.Log(startPosition);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Event handler for a pointer clicking on this item
    /// </summary>
    /// <param name="eventData">Data held in the pointer down event</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    /// <summary>
    /// Event handler for a pointer beginning to drag on this item
    /// </summary>
    /// <param name="eventData">Data held in the pointer begin drag event</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
   
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .6f;
        
    }

    /// <summary>
    /// Event handler for a pointer ending the drag on this item
    /// </summary>
    /// <param name="eventData">Data held in the pointer end drag event</param>
    public void OnEndDrag(PointerEventData eventData)
    {

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        
    }

    /// <summary>
    /// Event handler for a pointer dragging an item
    /// </summary>
    /// <param name="eventData">Data held in the pointer drag event</param>
    public void OnDrag(PointerEventData eventData)
    {
        if (canMove)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}
