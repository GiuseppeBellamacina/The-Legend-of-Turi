using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class BoolValue : Data
{
    public bool value;
    public bool initialValue;

    public new void Reset()
    {
        value = initialValue;
    }

    public new void Save()
    {
        string relPath = SaveSystem.path + "/BoolValues/";
        string path = relPath + dataIndex.ToString() + ".save";
        BoolData data = new BoolData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load()
    {
        string relPath = SaveSystem.path + "/BoolValues/";
        string path = relPath + dataIndex.ToString() + ".save";
        BoolData data = SaveSystem.Load<BoolData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            value = data.value;
            initialValue = data.initialValue;
        }
    }
}