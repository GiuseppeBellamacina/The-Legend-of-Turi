using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject, ISerializationCallbackReceiver
{
    public Item currentItem;
    public List<Item> items = new ();
    public int numberOfKeys;

    public void AddItem(Item item)
    {
        if (item.isKey)
            numberOfKeys++;
        else{
            if(!items.Contains(item))
                items.Add(item);
        }
    }

    public void OnAfterDeserialize()
    {
        currentItem = null;
        items.Clear();
        numberOfKeys = 0;
    }

    public void OnBeforeSerialize(){}
}
