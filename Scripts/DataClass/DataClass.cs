using UnityEngine;

[System.Serializable]
public class DataClass
{
    public int dataIndex;
    public string fileName;

    public DataClass(Data data)
    {
        dataIndex = data.dataIndex;
        fileName = data.fileName;
    }
}