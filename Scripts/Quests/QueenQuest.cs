using UnityEngine;

public class QueenQuest : Quest
{
    public Dialog knightDialog;
    public BoolValue bossDefeated;

    public bool CheckCondition()
    {
        return bossDefeated.value;
    }
}