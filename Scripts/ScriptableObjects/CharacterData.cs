using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class CharacterData : Data
{
    public float health;
    public float maxHealth;
    public float initialMaxHealth;
    public float absoluteMaxHealth;
    public float damage;
    public float speed;

    public new void Reset()
    {
        maxHealth = initialMaxHealth;
        health = maxHealth;
    }

    public new void Save()
    {
        string path = dataIndex.ToString() + ".save";
        fileName = name;
        CharacterDataData data = new CharacterDataData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load(int index)
    {
        string path = index.ToString() + ".save";
        CharacterDataData data = SaveSystem.Load<CharacterDataData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            fileName = data.fileName;
            health = data.health;
            maxHealth = data.maxHealth;
            initialMaxHealth = data.initialMaxHealth;
            absoluteMaxHealth = data.absoluteMaxHealth;
            damage = data.damage;
            speed = data.speed;
        }
    }
}