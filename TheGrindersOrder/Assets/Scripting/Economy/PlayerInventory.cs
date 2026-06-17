using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public void AddItem(string resourceType, int amount)
    {
        Debug.Log($"Picked up {amount} {resourceType}");
    }
}