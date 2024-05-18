using UnityEngine;

[CreateAssetMenu]
public class Item : Manageable
{
    public Sprite sprite;
    public string itemName;
    [TextArea] public string description;
    public bool isKey, isCoin, isHealth;
    public int quantity;
    public bool hasBeenPickedUp;

    public override void Reset()
    {
        hasBeenPickedUp = false;
    }
}
