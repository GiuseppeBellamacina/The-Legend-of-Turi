using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class GameStatus : Data
{
    public int difficulty;
    public Vector2 playerPosition;
    public string currentScene;
    public bool isBounded;

    public new void Reset()
    {
        difficulty = 0;
        playerPosition = Vector2.zero;
        currentScene = "";
        isBounded = false;
    }

    public new void Save()
    {
        string path = dataIndex.ToString() + ".save";
        GameStatusData data = new GameStatusData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load(int index)
    {
        string path = index.ToString() + ".save";
        GameStatusData data = SaveSystem.Load<GameStatusData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            difficulty = data.difficulty;
            playerPosition = new Vector2(data.playerPosition[0], data.playerPosition[1]);
            currentScene = data.currentScene;
            isBounded = data.isBounded;
        }
    }
}