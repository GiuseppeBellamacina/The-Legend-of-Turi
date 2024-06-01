using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class VectorValue : Data
{
    public Vector2 value;

    public new void Reset(){}

    public new void Save()
    {
        string relPath = SaveSystem.path + "/VectorValues/";
        string path = relPath + dataIndex.ToString() + ".save";
        VectorData data = new VectorData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load()
    {
        string relPath = SaveSystem.path + "/VectorValues/";
        string path = relPath + dataIndex.ToString() + ".save";
        VectorData data = SaveSystem.Load<VectorData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            value.x = data.values[0];
            value.y = data.values[1];
        }
    }
}
