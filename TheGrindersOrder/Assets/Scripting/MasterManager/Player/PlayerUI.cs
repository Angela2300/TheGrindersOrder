using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    // Drag all the heart images here in the Inspector
    public GameObject[] hearts;

    // Drag all the shield images here in the Inspector
    public GameObject[] shields;

    // Drag the coin text object here
    public TMP_Text coinsText;

    // This function updates what the player sees on screen
    // It does NOT change the player's stats
    // It only shows them
    public void UpdateUI(int heartCount, int shieldCount, int coins)
    {
        // Go through every heart image one by one
        for (int i = 0; i < hearts.Length; i++)
        {
            // If i is smaller than heartCount, show the heart
            // Otherwise hide it
            hearts[i].SetActive(i < heartCount);
        }

        // Do the same thing for shields
        for (int i = 0; i < shields.Length; i++)
        {
            shields[i].SetActive(i < shieldCount);
        }
       
        Debug.Log("Updating coin text: " + coins);

        if (coinsText != null)
        {
            coinsText.text = "Coins: " + coins;
        }
    }
}