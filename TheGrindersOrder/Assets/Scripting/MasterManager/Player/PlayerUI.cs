//using UnityEngine;
//using TMPro;

//public class PlayerUI : MonoBehaviour
//{
//    // Drag all the heart images here in the Inspector
//    public GameObject[] hearts;

//    // Drag all the shield images here in the Inspector
//    public GameObject[] shields;

//    // Drag the coin text object here
//    public TMP_Text coinsText;

//    // This function updates what the player sees on screen
//    // It does NOT change the player's stats
//    // It only shows them
//    public void UpdateUI(int heartCount, int shieldCount, int coins)
//    {
//        Debug.Log($"UI Received -> Hearts: {heartCount} | Shields: {shieldCount}");
//        // Update Hearts: Show only if index < heartCount

//        for (int i = 0; i < hearts.Length; i++)
//        {
//            //hearts[i].SetActive(i < heartCount);
//            bool shouldBeVisible = (i < heartCount);
//            hearts[i].SetActive(shouldBeVisible); // Is this actually turning things off?
//            Debug.Log("Heart " + i + " set to " + shouldBeVisible); // Add this inside the loop
//        }

//        // Update Shields: Show only if index < shieldCount
//        for (int i = 0; i < shields.Length; i++)
//        {
//            shields[i].SetActive(i < shieldCount);
//        }

//        Debug.Log("Updating coin text: " + coins);

//        if (coinsText != null)
//        {
//            coinsText.text = "Coins: " + coins;
//        }
//    }
//}


using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public GameObject[] hearts;
    public GameObject[] shields;
    public TMP_Text coinsText;

    public void UpdateUI(int heartCount, int shieldCount, int coins)
    {
        // Update Hearts visibility
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < heartCount);
        }

        // Update Shields visibility
        for (int i = 0; i < shields.Length; i++)
        {
            shields[i].SetActive(i < shieldCount);
        }

        // Update Coin Text
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + coins;
        }
    }
}