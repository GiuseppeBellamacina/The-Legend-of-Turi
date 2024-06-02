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
        FloatData data = new FloatData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load()
    {
        string path = dataIndex.ToString() + ".save";
        FloatData data = SaveSystem.Load<FloatData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            value = data.value;
            initialValue = data.initialValue;
        }
    }
}