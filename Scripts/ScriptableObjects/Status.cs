using UnityEngine;

[CreateAssetMenu]
public class Status : ScriptableObject, IResettable
{
    public bool isActive;
    public bool condition;
    public bool isCompleted;
    [Header("Dialogs")]
    public Dialog dialog;

    public void GoToNextStatus()
    {
        dialog.SetNextCheckpoint();
    }

    public void GoToStatus(int index)
    {
        dialog.SetCheckpoint(index);
    }

    public void Reset()
    {
        isActive = false;
        isCompleted = false;
        condition = false;
    }
}