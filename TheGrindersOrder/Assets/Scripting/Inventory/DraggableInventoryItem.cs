using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Item Identity")]
    [Tooltip("Which inventory slot this UI element represents. Meat is always 0.")]
    [SerializeField] private int slotIndex = 0;

    public int SlotIndex => slotIndex;

    [Header("Drag Settings")]
    [SerializeField] private CanvasGroup canvasGroup; // Used to fade the icon and let raycasts pass through while dragging

    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Vector2 originalAnchoredPosition;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (rectTransform == null)
            Debug.LogWarning($"[DraggableInventoryItem] No RectTransform found on {gameObject.name}.");

        if (parentCanvas == null)
            Debug.LogWarning($"[DraggableInventoryItem] No parent Canvas found for {gameObject.name}. Dragging may not work correctly.");

        if (canvasGroup == null)
            Debug.LogWarning($"[DraggableInventoryItem] No CanvasGroup found on {gameObject.name}. Add one so raycasts pass through while dragging.");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return;

        originalAnchoredPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        // Move to the top of the canvas hierarchy so it renders above other UI while dragging
        if (parentCanvas != null)
            transform.SetParent(parentCanvas.transform, true);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false; // lets OnDrop on SellDropZone fire correctly
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null || parentCanvas == null) return;

        // Move with the pointer, scaled correctly for the canvas's render mode
        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent, true);

        if (rectTransform != null)
            rectTransform.anchoredPosition = originalAnchoredPosition;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
