using UnityEngine;

[CreateAssetMenu]
public class BoolValue : ScriptableObject, IResettable
{
    public bool value;

    public bool initialValue;

    public void Reset()
    {
        value = initialValue;
    }
}