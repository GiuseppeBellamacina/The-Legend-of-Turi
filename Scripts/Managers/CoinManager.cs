using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TMP_Text coinText;
    public Inventory playerInventory;
    public AudioClip coinSound;
    bool start;

    void Start()
    {
        start = true;
        SetCoins();
        start = false;
    }

    public void SetCoins()
    {
        if (!start)
            AudioManager.Instance.PlaySFX(coinSound);
        coinText.text = playerInventory.numberOfCoins.ToString();
    }
}