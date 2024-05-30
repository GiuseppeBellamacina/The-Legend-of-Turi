using UnityEngine;

public class QueenQuest : Quest
{
    public Dialog knightDialog;
    public Dialog luigiDialog;
    public BoolValue bossDefeated;

    public bool CheckCondition()
    {
        return bossDefeated.value;
    }
}