[System.Serializable]
public class StatusData : DataClass
{
    public bool isActive;
    public bool condition;
    public bool isCompleted;
    public int statusCheckpoint;

    public StatusData(Status data) : base(data)
    {
        isActive = data.isActive;
        condition = data.condition;
        isCompleted = data.isCompleted;
        statusCheckpoint = data.statusCheckpoint;
    }
}