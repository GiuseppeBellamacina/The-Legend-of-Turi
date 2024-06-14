using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Status : Data
{
    public bool isActive;
    public bool condition;
    public bool isCompleted;
    [Header("Dialogs")]
    public Dialog dialog;
    public int statusCheckpoint;

    public void GoToNextStatus()
    {
        dialog.SetNextCheckpoint();
        statusCheckpoint = dialog.checkPointIndex;
    }

    public void GoToStatus(int index)
    {
        dialog.SetCheckpoint(index);
        statusCheckpoint = dialog.checkPointIndex;
    }

    public bool IsStatus(int index)
    {
        return statusCheckpoint == index;
    }

    public new void Reset()
    {
        isActive = false;
        isCompleted = false;
        condition = false;
        statusCheckpoint = 0;
    }

    public new void Save()
    {
        string path = dataIndex.ToString() + ".save";
        fileName = name;
        StatusData data = new StatusData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load(int index)
    {
        string path = index.ToString() + ".save";
        StatusData data = SaveSystem.Load<StatusData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            fileName = data.fileName;
            isActive = data.isActive;
            condition = data.condition;
            isCompleted = data.isCompleted;
            statusCheckpoint = data.statusCheckpoint;
        }
    }
}