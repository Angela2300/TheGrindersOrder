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
            Inventory inventory = startParent.GetComponentInParent<Inventory>();
            PlayerStats playerStats = inventory.GetComponent<PlayerStats>();

            if (inventory == null || playerStats == null)
            {
                Debug.LogWarning("Missing Inventory or PlayerStats.");
                return;
            }

            int slotIndex = startParent.GetSiblingIndex();

            if (slotIndex < 0 || slotIndex >= inventory.maxSlots)
            {
                Debug.LogWarning("Invalid inventory slot.");
                return;
            }

            InventorySlotType slotType = inventory.slotTypes[slotIndex];

            if (slotType == InventorySlotType.Meat)
            {
                if (inventory.meatCount > 0)
                {
                    inventory.meatCount--;
                    playerStats.AddCoins(5);
                    Debug.Log("Sold 1 meat for 5 coins.");
                }
            }
            else if (slotType == InventorySlotType.Weapon)
            {
                inventory.slotUsed[slotIndex] = false;
                inventory.slotTypes[slotIndex] = InventorySlotType.Empty;
                inventory.weapons[slotIndex] = default;

                playerStats.AddCoins(20);
                Debug.Log("Sold weapon for 20 coins.");
            }

            if (inventory.inventoryUI != null)
            {
                inventory.inventoryUI.UpdateInventory(
                    inventory.weapons,
                    inventory.slotUsed,
                    inventory.slotTypes,
                    inventory.meatCount
                );
            }

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