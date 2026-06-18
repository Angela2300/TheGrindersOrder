using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableInventoryItem : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    // Stores where the item originally was before dragging
    Vector3 startPosition;

    // Stores the original parent object (inventory slot)
    Transform startParent;

    // Reference to the sell area in the shop UI
    // Drag the ShopSellPanel RectTransform into this in the Inspector
    public RectTransform shopSellArea;

    // Called once when the player starts dragging the item
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Remember where the item started
        startPosition = transform.position;

        // Remember which slot the item belongs to
        startParent = transform.parent;
    }

    // Called continuously while dragging
    public void OnDrag(PointerEventData eventData)
    {
        // Move the item with the mouse
        transform.position = eventData.position;
    }

    // Called when the player releases the mouse button
    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if the item was dropped inside the shop sell area
        if (shopSellArea != null &&
            RectTransformUtility.RectangleContainsScreenPoint(
                shopSellArea,
                eventData.position))
        {
            // ===========================================
            // SHOP HANDOVER SECTION
            // ===========================================
            //
            // Next developer should implement:
            //
            // 1. Determine what item was dropped
            //    (Meat, Weapon, Medkit, etc.)

            // Maybe medkit no need put in inventory can just buy from shop instantd replenish health and shields 
            //
            // 2. Remove the item from the inventory upon purchase (meat count is part of inventory code)
            //
            // 3. Give the player coins  (check playerstats cs)
            //
            // Example:
            //
            // inventory.RemoveMeat();
            // playerStats.AddCoins(5);
            //
            // or
            //
            // inventory.RemoveWeapon();
            // playerStats.AddCoins(20);
            //
            // 4. Refresh inventory UI
            //
            // inventoryUI.UpdateInventory(...);
            //
            // ===========================================

            Debug.Log("Sell item here");
        }
        else
        {
            // If not dropped on the sell area,
            // return the item back to its original slot
            transform.position = startPosition;

            // Restore original parent slot
            transform.SetParent(startParent);
        }
    }
}