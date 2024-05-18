using UnityEngine;

[CreateAssetMenu]
public class IntValue : Manageable
{
    public int value;
    public int initialValue;

    public override void Reset()
    {
        value = initialValue;
    }
}