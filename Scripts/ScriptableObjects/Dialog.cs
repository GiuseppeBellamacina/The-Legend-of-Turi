using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Dialog : Data
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

    public string GetSentence(int index)
    {
        if (index < sentences.Length)
        {
            return sentences[index];
        }
        return null;
    }

    public void SetNextCheckpoint()
    {
        if (checkPointIndex + 1 < dialogCheckpoints.Length)
        {
            currentCheckpoint = dialogCheckpoints[++checkPointIndex];
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

    public bool HasReadDialogUntilCheckPoint(int index)
    {
        return dialogIndex == dialogCheckpoints[index];
    }

    public new void Reset()
    {
        dialogIndex = 0;
        currentCheckpoint = 0;
        checkPointIndex = 0;
    }

    public new void Save()
    {
        string path = dataIndex.ToString() + ".save";
        DialogData data = new DialogData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load(int index)
    {
        string path = index.ToString() + ".save";
        DialogData data = SaveSystem.Load<DialogData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            dialogIndex = data.dialogIndex;
            currentCheckpoint = data.currentCheckpoint;
            dialogCheckpoints = data.dialogCheckpoints;
            checkPointIndex = data.checkPointIndex;
            sentences = data.sentences;
        }
    }
}