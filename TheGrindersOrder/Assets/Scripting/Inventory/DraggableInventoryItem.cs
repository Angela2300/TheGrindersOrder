using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 startPosition;
    Transform startParent;

    public RectTransform shopSellArea;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (shopSellArea != null &&
            RectTransformUtility.RectangleContainsScreenPoint(shopSellArea, eventData.position))
        {
            Debug.Log("Sell meat here");
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }
}