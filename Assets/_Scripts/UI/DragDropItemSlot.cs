using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Item slot for a UI canvas
/// </summary>
public class DragDropItemSlot : MonoBehaviour, IDropHandler
{
    [Tooltip("String that is the correct word for this item slot")]
    [SerializeField]
    private string correctWord;

    [Tooltip("Boolean stating if this is the correct slot for an item")]
    public bool correctSlot;
    
    [Tooltip("Reference to the WordCanvasManager")]
    public WordCanvasManager manager;

    [Tooltip("Slot Sound event")] 
    [SerializeField] private AK.Wwise.Event slotSoundEffect;

    
    /// <summary>
    /// Event handler for dropping an item into an item slot
    /// </summary>
    /// <param name="eventData">Pointer data for the item dropping event handler</param>
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
                slotSoundEffect.Post(this.gameObject);
            }
        }
        else
        {
            correctSlot = false;
        }
    }

    /// <summary>
    /// Function that calculates if this slot has the correct word
    /// </summary>
    /// <param name="word">String with the word we are comparing to</param>
    /// <returns></returns>
    private bool HasValidWord(string word)
    {
        return word == correctWord;
    }
}
