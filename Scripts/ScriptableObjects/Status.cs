using UnityEngine;

[CreateAssetMenu]
public class Status : ScriptableObject, IResettable
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
        statusCheckpoint = dialog.currentCheckpoint;
    }

    public void GoToStatus(int index)
    {
        dialog.SetCheckpoint(index);
        statusCheckpoint = dialog.currentCheckpoint;
    }

    public bool InStatus(int index)
    {
        return statusCheckpoint == index;
    }

    public void Reset()
    {
        isActive = false;
        isCompleted = false;
        condition = false;
    }
}