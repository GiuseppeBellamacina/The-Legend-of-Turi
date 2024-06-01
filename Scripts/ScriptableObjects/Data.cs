using UnityEngine;

public class Data : ScriptableObject, IResettable, ISaveLoad
{
    public int dataIndex = 0;

    public void Reset(){}
    public void Save(){}
    public void Load(){}
}