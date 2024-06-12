using UnityEngine;

public class Data : ScriptableObject, IResettable, ISaveLoad
{
    public int dataIndex;

    public void Reset(){}
    public void Save(){}
    public void Load(int index){}
}