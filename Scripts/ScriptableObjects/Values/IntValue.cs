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
        IntData data = new IntData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load()
    {
        string path = dataIndex.ToString() + ".save";
        IntData data = SaveSystem.Load<IntData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            value = data.value;
            initialValue = data.initialValue;
        }
    }
}