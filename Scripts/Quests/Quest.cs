using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    public string questName;
    public Status status;

    public virtual void StartQuest()
    {
        if (!status.isActive && !status.isCompleted)
        {
            status.isActive = true;
        }
    }

    public virtual void UpdateQuest()
    {
        if (status.isActive || status.isCompleted)
        {
            status.GoToNextStatus();
        }
    }

    public virtual void UpdateQuest(int index)
    {
        if (status.isActive || status.isCompleted)
        {
            status.GoToStatus(index);
        }
    }

    public virtual void CompleteQuest()
    {
        status.isCompleted = true;
        status.isActive = false;
    }
}