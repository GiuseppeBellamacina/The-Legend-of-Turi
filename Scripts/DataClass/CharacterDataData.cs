[System.Serializable]
public class CharacterDataData : DataClass
{
    public float health;
    public float maxHealth;
    public float initialMaxHealth;
    public float absoluteMaxHealth;
    public float damage;
    public float speed;

    public CharacterDataData(CharacterData data) : base(data)
    {
        health = data.health;
        maxHealth = data.maxHealth;
        initialMaxHealth = data.initialMaxHealth;
        absoluteMaxHealth = data.absoluteMaxHealth;
        damage = data.damage;
        speed = data.speed;
    }
}