using UnityEngine;

[CreateAssetMenu]
public class CharacterData : ScriptableObject, IResettable
{
    public float health;
    public float maxHealth;
    public float initialMaxHealth;
    public float absoluteMaxHealth;
    public float damage;
    public float speed;

    public void Reset()
    {
        maxHealth = initialMaxHealth;
        health = maxHealth;
    }
}