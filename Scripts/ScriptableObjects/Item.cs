using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Item : Data
{
    public Sprite sprite;
    public string itemName;
    [TextArea] public string description;
    public bool isKey, isCoin, isHearth, isArrow, isHeartContainer;
    public int quantity;
    public bool hasBeenPickedUp;

    public new void Reset()
    {
        hasBeenPickedUp = false;
    }

    public new void Save()
    {
        string path = dataIndex.ToString() + ".save";
        fileName = name;
        ItemData data = new ItemData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load(int index)
    {
        string path = index.ToString() + ".save";
        ItemData data = SaveSystem.Load<ItemData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            fileName = data.fileName;
            hasBeenPickedUp = data.hasBeenPickedUp;
        }
    }
}
