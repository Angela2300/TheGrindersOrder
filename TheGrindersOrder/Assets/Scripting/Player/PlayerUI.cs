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