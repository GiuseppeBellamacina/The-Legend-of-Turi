using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject, IResettable
{
    public Item currentItem;
    public List<Item> items = new ();
    public Signals coinSignal;
    public Signals arrowSignal;
    public Item Arrow;
    public int numberOfKeys;
    public int numberOfCoins;
    public int numberOfArrows;

    public bool IsAwaible(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
                return true;
        }
        return false;
    }

    public int GetQuantity(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
                return item.quantity;
        }
        return 0;
    }

    public void SetCurrentItem(Item item)
    {
        currentItem = item;
    }

    public void RemoveCurrentItem()
    {
        currentItem = null;
    }

    public Item GetItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
                return item;
        }
        return null;
    }

    public void AddArrow(int quantity)
    {
        if (numberOfArrows == 0){
            AddItem(Arrow);
        }
        
        foreach (Item item in items)
        {
            if (item.itemName == "Frecce")
            {
                item.quantity += quantity;
                numberOfArrows = item.quantity;
                return;
            }
        }
    }

    public void UseArrow()
    {
        foreach (Item item in items)
        {
            if (item.itemName == "Frecce")
            {
                item.quantity--;
                numberOfArrows = item.quantity;
                arrowSignal.Raise();
                if (item.quantity <= 0)
                    items.Remove(item);
                return;
            }
        }
    }

    public void AddItem(Item item)
    {
        if (item.isKey)
            numberOfKeys += item.quantity;
        else if (item.isCoin)
        {
            numberOfCoins += item.quantity;
            coinSignal.Raise();
        }
        else if (item.isArrow)
        {
            numberOfArrows += item.quantity;
            arrowSignal.Raise();
        }
        else if (item.isHealth)
        {
            PlayerController.Instance.Heal(item.quantity);
        }
        else if (item.isContainer)
        {
            PlayerController.Instance.IncreaseMaxHealth(item.quantity);
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
        else if (item.isArrow)
        {
            numberOfArrows -= item.quantity;
            arrowSignal.Raise();
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
        numberOfArrows = 0;
        Arrow.quantity = 0;
    }
}
