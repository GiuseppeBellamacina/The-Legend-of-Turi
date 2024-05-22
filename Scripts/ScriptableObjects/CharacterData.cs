using UnityEngine;

[CreateAssetMenu]
public class CharacterData : ScriptableObject, IResettable
{
    public float health;
    public float maxHealth;
    public float damage;
    public float speed;

    public void Reset()
    {
        health = maxHealth;
    }
}