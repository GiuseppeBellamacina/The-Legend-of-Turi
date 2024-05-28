using Unity.VisualScripting;
using UnityEngine;

public class Luigi : Npc
{
    public FirstQuest quest;
    public Item sword;
    public GameObject house;
    public BoolValue houseStatus;

    protected override void Awake()
    {
        base.Awake();
        quest = GetComponent<FirstQuest>();
        if (!houseStatus.value)
            house.SetActive(false);
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
            house.SetActive(true);
            houseStatus.value = true;
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
                PlayerController.Instance.inventory.hasSword = true;
                PlayerController.Instance.EnableAttack();
            }
            else
                base.ContinueInteraction();
        }
        else
            base.ContinueInteraction();
    }
}