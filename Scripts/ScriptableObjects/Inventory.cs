using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject, IResettable
{
    public Item currentItem;
    public List<Item> items = new ();
    public Signals coinSignal;
    public int numberOfKeys;
    public int numberOfCoins;

    public void AddItem(Item item)
    {
        if (item.isKey)
            numberOfKeys += item.quantity;
        else if (item.isCoin)
        {
            numberOfCoins += item.quantity;
            coinSignal.Raise();
        }
        else if (item.isHealth)
        {
            PlayerController.Instance.Heal(item.quantity);
        }
        else{
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemName == item.itemName)
                {
                    items[i].quantity += item.quantity;
                    return;
                }
            }
            items.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        if (item.isKey)
            numberOfKeys -= item.quantity;
        else if (item.isCoin)
        {
            numberOfCoins -= item.quantity;
            coinSignal.Raise();
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemName == item.itemName)
                {
                    items[i].quantity -= item.quantity;
                    if (items[i].quantity <= 0)
                        items.Remove(items[i]);
                    return;
                }
            }
        }
    }

    public bool Pay(int price)
    {
        if (numberOfCoins >= price)
        {
            numberOfCoins -= price;
            coinSignal.Raise();
            return true;
        }
        return false;
    }

    public void UseKey()
    {
        if (numberOfKeys > 0)
            numberOfKeys--;
    }

    public void Reset()
    {
        currentItem = null;
        items.Clear();
        numberOfKeys = 0;
        numberOfCoins = 0;
    }
}
