using UnityEngine;

[System.Serializable]
public class DataClass
{
    public int dataIndex;

    public DataClass(Data data)
    {
        dataIndex = data.dataIndex;
    }
}