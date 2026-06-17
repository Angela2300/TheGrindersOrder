using System;
using UnityEngine;

//     PlayerInventory must have:
//     public int  GetItemCount(string resourceType)
//     public void RemoveAllOfType(string resourceType)
//     public void AddItem(string resourceType, int amount)
public class BlendCircle : MonoBehaviour
{
    public static event Action<int> OnBlendComplete;

    [Header("Blend Settings")]
    [Tooltip("How many Meat Juice units are produced per Meat Chunk")]
    public float conversionRate = 3f;

    [Tooltip("Key the player presses to blend")]
    public KeyCode blendKey = KeyCode.E;

    [Header("UI")]
    [Tooltip("Drag the blend prompt panel here — it shows/hides when player enters/exits")]
    public GameObject blendPromptUI;

  
    PlayerInventory currentInventory;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Requires PlayerInventory on the Player
        currentInventory = other.GetComponent<PlayerInventory>();

        if (blendPromptUI != null)
            blendPromptUI.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        currentInventory = null;

        if (blendPromptUI != null)
            blendPromptUI.SetActive(false);
    }

    void Update()
    {
        if (currentInventory == null) return;
        if (!Input.GetKeyDown(blendKey)) return;

        int meatCount = currentInventory.GetItemCount("loot_meat");

        if (meatCount <= 0)
        {
            Debug.Log("[BlendCircle] No meat to blend.");
            return;
        }

        int juiceProduced = Mathf.RoundToInt(meatCount * conversionRate);

        currentInventory.RemoveAllOfType("loot_meat");
        currentInventory.AddItem("loot_meat_juice", juiceProduced);

        OnBlendComplete?.Invoke(juiceProduced);

        Debug.Log($"[BlendCircle] Blended {meatCount} meat → {juiceProduced} juice.");
    }
}
