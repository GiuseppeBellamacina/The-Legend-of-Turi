using Unity.VisualScripting;
using UnityEngine;

public class Luigi : Npc
{
    public FirstQuest quest;
    public Item sword;

    protected override void Awake()
    {
        base.Awake();
        quest = GetComponent<FirstQuest>();
    }

    public override void Interact()
    {
        // Se non ho ancora parlato con Luigi e non ho completato la quest
        if (!quest.status.isActive && !quest.status.isCompleted)
        {
            quest.StartQuest();
        }
        // Se ho già parlato con Luigi ho completato la quest
        else if (quest.status.isActive && quest.status.condition)
        {
            quest.UpdateQuest(2);
            quest.CompleteQuest();
        }
        // Se ho già parlato con Luigi e non ho completato la quest
        else if (quest.status.isActive && !quest.status.condition)
        {
            quest.UpdateQuest(1);
        }

        base.Interact();
    }

    public override void ContinueInteraction()
    {
        if (!PlayerController.Instance.inventory.items.Contains(sword) && !quest.status.isCompleted)
        {
            if (quest.status.dialog.dialogIndex == quest.status.dialog.dialogCheckpoints[1])
            {
                dialogText.text = sword.description;
                npcTitle.text = "";
                npcDialog.text = "";
                PlayerController.Instance.ObtainItem(sword);
                PlayerController.Instance.EnableAttack();
            }
            else
                base.ContinueInteraction();
        }
        else
            base.ContinueInteraction();
    }
}