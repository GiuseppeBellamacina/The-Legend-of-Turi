using UnityEngine;

[CreateAssetMenu]
public class FloatValue : ScriptableObject, IResettable
{
    public float value;
    public float initialValue;

    public void Reset()
    {
        value = initialValue;
    }
}