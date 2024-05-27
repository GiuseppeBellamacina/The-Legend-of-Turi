using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu]
public class Dialog : ScriptableObject, IResettable
{
    public int dialogIndex;
    public int currentCheckpoint;
    public int[] dialogCheckpoints;
    int checkPointIndex;
    [TextArea(3, 10)]
    public string[] sentences;

    public string GetFirstSentence()
    {
        dialogIndex = currentCheckpoint;
        return GetNextSentence();
    }

    public string GetNextSentence()
    {
        int nextCheckpoint = checkPointIndex + 1 < dialogCheckpoints.Length ? dialogCheckpoints[checkPointIndex + 1] : sentences.Length;
        if (dialogIndex < nextCheckpoint)
        {
            return sentences[dialogIndex++];
        }
        return null;
    }

    public void SetNextCheckpoint()
    {
        if (checkPointIndex < dialogCheckpoints.Length)
        {
            currentCheckpoint = dialogCheckpoints[checkPointIndex++];
        }
    }

    public void Reset()
    {
        dialogIndex = 0;
        currentCheckpoint = 0;
        checkPointIndex = 0;
    }
}