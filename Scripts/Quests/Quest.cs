using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    public string questName;
    public Status status;

    public virtual void StartQuest()
    {
        status.isActive = true;
    }

    public virtual void UpdateQuest()
    {
        if (status.isActive)
        {
            status.GoToNextStatus();
        }
    }

    public virtual void UpdateQuest(int index)
    {
        if (status.isActive)
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