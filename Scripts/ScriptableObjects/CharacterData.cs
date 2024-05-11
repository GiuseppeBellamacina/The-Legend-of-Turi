using UnityEngine;

[CreateAssetMenu]
public class CharacterData : ScriptableObject, ISerializationCallbackReceiver
{
    public float health;
    public float maxHealth;
    public float damage;
    public float speed;

    public void OnAfterDeserialize()
    {
        health = maxHealth;
    }

    public void OnBeforeSerialize(){}
}