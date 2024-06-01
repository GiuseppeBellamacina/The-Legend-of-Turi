[System.Serializable]
public class DialogData : DataClass
{
    public int dialogIndex;
    public int currentCheckpoint;
    public int[] dialogCheckpoints;
    public int checkPointIndex;
    public string[] sentences;

    public DialogData(Dialog data) : base(data)
    {
        dialogIndex = data.dialogIndex;
        currentCheckpoint = data.currentCheckpoint;
        dialogCheckpoints = data.dialogCheckpoints;
        checkPointIndex = data.checkPointIndex;
        sentences = data.sentences;
    }
}