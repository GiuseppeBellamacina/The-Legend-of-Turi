using UnityEngine;

public class FirstQuest : Quest
{
    public Room room;
    public Enemy[] enemies;

    void Start()
    {
        int enemiesCount = 0;
        foreach (GameObject obj in room.objectsToSpawn)
        {
            if (obj.GetComponent<Enemy>())
            {
                enemiesCount++;
            }
        }
        enemies = new Enemy[enemiesCount];
        enemiesCount = 0;
        foreach (GameObject obj in room.objectsToSpawn)
        {
            if (obj.GetComponent<Enemy>())
            {
                enemies[enemiesCount] = obj.GetComponent<Enemy>();
                enemiesCount++;
            }
        }
    }

    public void CheckCondition()
    {
        status.condition = true;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                status.condition = false;
                break;
            }
        }
    }

    void Update()
    {
        if (status.isActive)
        {
            CheckCondition();
        }
    }
}