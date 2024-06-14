using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class IntValue : Data
{
    public int value;
    public int initialValue;

    public new void Reset()
    {
        value = initialValue;
    }

    public new void Save()
    {
        string path = dataIndex.ToString() + ".save";
        fileName = name;
        IntData data = new IntData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load(int index)
    {
        string path = index.ToString() + ".save";
        IntData data = SaveSystem.Load<IntData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            fileName = data.fileName;
            value = data.value;
            initialValue = data.initialValue;
        }
    }
}