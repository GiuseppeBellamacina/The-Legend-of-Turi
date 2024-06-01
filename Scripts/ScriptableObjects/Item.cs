using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Item : Data
{
    public Sprite sprite;
    public string itemName;
    [TextArea] public string description;
    public bool isKey, isCoin, isHealth, isArrow, isContainer;
    public int quantity;
    public bool hasBeenPickedUp;

    public new void Reset()
    {
        hasBeenPickedUp = false;
    }

    public new void Save()
    {
        string relPath = SaveSystem.path + "/Items/";
        string path = relPath + dataIndex.ToString() + ".save";
        ItemData data = new ItemData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load()
    {
        string relPath = SaveSystem.path + "/Items/";
        string path = relPath + dataIndex.ToString() + ".save";
        ItemData data = SaveSystem.Load<ItemData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            hasBeenPickedUp = data.hasBeenPickedUp;
        }
    }
}
