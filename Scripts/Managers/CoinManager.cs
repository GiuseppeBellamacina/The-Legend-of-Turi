using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TMP_Text coinText;
    public Inventory playerInventory;

    void Start()
    {
        SetCoins();
    }

    public void SetCoins()
    {
        coinText.text = playerInventory.numberOfCoins.ToString();
    }
}