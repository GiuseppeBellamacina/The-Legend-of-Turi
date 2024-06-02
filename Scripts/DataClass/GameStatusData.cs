using UnityEngine.SceneManagement;

[System.Serializable]
public class GameStatusData : DataClass
{
    public int difficulty;
    public bool swordUnlocked;
    public float[] playerPosition;
    public string currentScene;
    public bool isBounded;

    public GameStatusData(GameStatus data) : base(data)
    {
        difficulty = data.difficulty;
        swordUnlocked = PlayerController.Instance.inventory.hasSword;
        playerPosition = new float[] { PlayerController.Instance.transform.position.x, PlayerController.Instance.transform.position.y };
        currentScene = SceneManager.GetActiveScene().name;
        isBounded = CameraMovement.Instance.isBounded;
    }
}