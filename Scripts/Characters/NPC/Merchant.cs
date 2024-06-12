using UnityEngine;

enum ShopState { First, ShowPrice, SoldOut, NoMoney, Sold, Stop };

public class Merchant : Npc
{
    [Header("Shop")]
    public Item article;
    public int price;
    public int stock;
    public bool infiniteStock;
    bool soldOut;
    string lastSentence;

    protected override void Awake()
    {
        base.Awake();
        
        if (infiniteStock)
            soldOut = false;
        else
        {
            if (stock <= 0)
                soldOut = true;
            else
                soldOut = false;
        }
    }

    public override void Interact()
    {
        if (dialog.currentCheckpoint == (int)ShopState.Stop)
        {
            if (soldOut)
                dialog.SetCheckpoint((int)ShopState.SoldOut);
            else
                dialog.SetCheckpoint((int)ShopState.ShowPrice);
        }
            
        base.Interact();
        lastSentence = dialog.GetSentence(dialog.currentCheckpoint);

        if (PlayerController.Instance.inventory.numberOfCoins < price && dialog.currentCheckpoint != (int)ShopState.First)
            dialog.SetCheckpoint((int)ShopState.NoMoney);
    }

    public override void ContinueInteraction()
    {
        if (!speechEnded)
        {
            TextDisplacer(lastSentence);
            return;
        }

        lastSentence = dialog.GetSentence(dialog.currentCheckpoint);

        // Se non ci ho mai parlato
        if (dialog.currentCheckpoint == (int)ShopState.First)
        {
            dialog.SetNextCheckpoint();
            StopInteraction();
        }
        // Mi dice il costo e vende
        else if (dialog.currentCheckpoint == (int)ShopState.ShowPrice)
        {
            Sell();
            dialogText.text = article.description;
            PlayerController.Instance.ObtainItem(article);
            dialog.SetCheckpoint((int)ShopState.Sold);
        }
        // Mi dice che è finito
        else if (dialog.currentCheckpoint == (int)ShopState.SoldOut)
        {
            TextDisplacer(dialog.GetSentence((int)ShopState.SoldOut));
            dialog.currentCheckpoint = (int)ShopState.Stop;
        }
        // Non ho abbastanza soldi
        else if (dialog.currentCheckpoint == (int)ShopState.NoMoney)
        {
            TextDisplacer(dialog.GetSentence((int)ShopState.NoMoney));
            dialog.currentCheckpoint = (int)ShopState.Stop;
        }
        // Mi dice che ho fatto l'acquisto
        else if (dialog.currentCheckpoint == (int)ShopState.Sold)
        {
            TextDisplacer(dialog.GetSentence((int)ShopState.Sold));
            dialog.currentCheckpoint = (int)ShopState.Stop;
        }
        // Serve per fermare l'interazione
        else if (dialog.currentCheckpoint == (int)ShopState.Stop)
            StopInteraction();
    }

    public override void StopInteraction()
    {
        base.StopInteraction();

        speechEnded = true;
        StopAllCoroutines();

        if (soldOut)
            dialog.SetCheckpoint((int)ShopState.SoldOut);
        else
            dialog.SetCheckpoint((int)ShopState.ShowPrice);
    }

    void Sell()
    {
        if (PlayerController.Instance.inventory.Pay(price))
        {
            if (!infiniteStock)
                stock--;
            if (stock <= 0 && !infiniteStock)
                soldOut = true;

            if (soldOut)
                dialog.SetCheckpoint(2);
        }
    }
}