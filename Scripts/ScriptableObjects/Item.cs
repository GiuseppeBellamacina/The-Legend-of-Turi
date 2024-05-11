using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject, ISerializationCallbackReceiver
{
    public Sprite sprite;
    public string itemName;
    [TextArea] public string description;
    public bool isKey;
    public bool hasBeenPickedUp;

    public void OnAfterDeserialize()
    {
        hasBeenPickedUp = false;
    }

    public void OnBeforeSerialize(){}
}
