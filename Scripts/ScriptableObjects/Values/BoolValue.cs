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
        string path = dataIndex.ToString() + ".save";
        fileName = name;
        BoolData data = new BoolData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load(int index)
    {
        string path = index.ToString() + ".save";
        BoolData data = SaveSystem.Load<BoolData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            fileName = data.fileName;
            value = data.value;
            initialValue = data.initialValue;
        }
    }
}