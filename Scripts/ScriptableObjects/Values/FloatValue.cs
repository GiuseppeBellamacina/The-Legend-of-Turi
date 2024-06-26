using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class FloatValue : Data
{
    public float value;
    public float initialValue;

    public new void Reset()
    {
        value = initialValue;
    }

    public new void Save()
    {
        string path = dataIndex.ToString() + ".save";
        fileName = name;
        FloatData data = new FloatData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load(int index)
    {
        string path = index.ToString() + ".save";
        FloatData data = SaveSystem.Load<FloatData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            fileName = data.fileName;
            value = data.value;
            initialValue = data.initialValue;
        }
    }
}