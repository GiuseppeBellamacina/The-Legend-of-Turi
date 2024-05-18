using UnityEngine;

[CreateAssetMenu]
public class BoolValue : Manageable
{
    public bool value;

    public bool initialValue;

    public override void Reset()
    {
        value = initialValue;
    }
}