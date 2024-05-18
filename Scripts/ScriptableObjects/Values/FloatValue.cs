using UnityEngine;

[CreateAssetMenu]
public class FloatValue : Manageable
{
    public float value;
    public float initialValue;

    public override void Reset()
    {
        value = initialValue;
    }
}