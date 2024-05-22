using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject, IResettable
{
    public Sprite sprite;
    public string itemName;
    [TextArea] public string description;
    public bool isKey, isCoin, isHealth;
    public int quantity;
    public bool hasBeenPickedUp;

    public void Reset()
    {
        hasBeenPickedUp = false;
    }
}
