using UnityEngine;

[CreateAssetMenu]
public class Dialog : ScriptableObject, IResettable
{
    [Tooltip("Indice di lettura del dialogo")]
    public int dialogIndex; // indice di lettura del dialogo
    [Tooltip("Indice da cui iniziare a leggere il dialogo")]
    public int currentCheckpoint; // attuale indice da cui iniziare a leggere il dialogo
    [Tooltip("Indici da cui iniziare a leggere il dialogo")]
    public int[] dialogCheckpoints; // indici da cui iniziare a leggere il dialogo
    [Tooltip("Indice di dialogCheckpoints")]
    public int checkPointIndex; // indice di dialogCheckpoints
    [Tooltip("Frasi del dialogo")]
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
        if (checkPointIndex + 1 < dialogCheckpoints.Length)
        {
            currentCheckpoint = dialogCheckpoints[checkPointIndex++];
        }
    }

    public void SetCheckpoint(int index)
    {
        if (index < dialogCheckpoints.Length)
        {
            currentCheckpoint = dialogCheckpoints[index];
            checkPointIndex = index;
        }
        else
        {
            currentCheckpoint = dialogCheckpoints[^1]; // dialogCheckpoints.Length - 1
            checkPointIndex = dialogCheckpoints.Length - 1;
        }
    }

    public void Reset()
    {
        dialogIndex = 0;
        currentCheckpoint = 0;
        checkPointIndex = 0;
    }
}