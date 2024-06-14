using UnityEngine;

public class Data : ScriptableObject, IResettable, ISaveLoad
{
    public int dataIndex;
    public string fileName;

    public void Reset(){}
    public void Save(){}
    public void Load(int index){}
}