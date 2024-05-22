using UnityEngine;

[CreateAssetMenu]
public class IntValue : ScriptableObject, IResettable
{
    public int value;
    public int initialValue;

    public void Reset()
    {
        value = initialValue;
    }
}