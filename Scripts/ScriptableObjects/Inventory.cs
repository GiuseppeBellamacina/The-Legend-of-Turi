using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Inventory : Data
{
    public Item currentItem;
    public List<Item> items = new ();
    public Signals coinSignal;
    public Signals arrowSignal;
    public Item Arrow;
    public int numberOfKeys;
    public int numberOfCoins;
    public int numberOfArrows;
    public bool hasSword, hasBow;

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
        numberOfArrows += quantity;
        arrowSignal.Raise();
    }

    public void UseArrow()
    {
        numberOfArrows--;
        arrowSignal.Raise();
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
            AddArrow(item.quantity);
        }
        else if (item.isHearth)
        {
            PlayerController.Instance.Heal(item.quantity);
        }
        else if (item.isHeartContainer)
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

    public new void Reset()
    {
        numberOfKeys = 0;
        numberOfCoins = 0;
        numberOfArrows = 0;
        Arrow.quantity = numberOfArrows;
        hasSword = false;
        hasBow = false;
    }

    public new void Save()
    {
        string path = dataIndex.ToString() + ".save";
        fileName = name;
        InventoryData data = new InventoryData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load(int index)
    {
        string path = index.ToString() + ".save";
        InventoryData data = SaveSystem.Load<InventoryData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            fileName = data.fileName;
            numberOfKeys = data.numberOfKeys;
            numberOfCoins = data.numberOfCoins;
            numberOfArrows = data.numberOfArrows;
            Arrow.quantity = numberOfArrows;
            hasSword = data.hasSword;
            hasBow = data.hasBow;
        }
    }
}
