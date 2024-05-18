using UnityEngine;

[CreateAssetMenu]
public class CharacterData : Manageable
{
    public float health;
    public float maxHealth;
    public float damage;
    public float speed;

    public override void Reset()
    {
        health = maxHealth;
    }
}