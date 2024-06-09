using System.Collections;
using UnityEngine;

public class Luigi : Npc
{
    public FirstQuest quest;
    public Item sword, bow;
    public GameObject house;
    public BoolValue houseStatus;
    public Status queenStatus;

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
        // Se non ho completato il dialogo con Luigi e non ho ancora la spada
        else if (quest.status.isActive && !quest.status.condition && !PlayerController.Instance.inventory.hasSword)
        {
            quest.UpdateQuest(0);
        }
        // Se ho già parlato con Luigi e non ho completato la quest
        else if (quest.status.isActive && !quest.status.condition)
        {
            quest.UpdateQuest(1);
        }
        // Se ho già parlato con Luigi e ho completato la quest
        else if (quest.status.isActive && quest.status.condition)
        {
            quest.UpdateQuest(2);
        }
        // Se ho già parlato con la regina -> ha priorità sullo stato 3
        else if (queenStatus.isActive || queenStatus.isCompleted)
        {
            quest.UpdateQuest(4);
        }
        // Se non vado subito al castello
        else if (quest.status.isCompleted && quest.status.dialog.HasReadDialogUntilCheckPoint(3))
        {
            quest.UpdateQuest(3);
        }

        base.Interact();
    }

    public override void ContinueInteraction()
    {
        if (quest.status.dialog.HasReadDialogUntilCheckPoint(1) && speechEnded)
        {
            if (!PlayerController.Instance.inventory.hasSword)
            {
                // Sblocco la spada
                dialogText.text = sword.description;
                PlayerController.Instance.ObtainItem(sword);
                PlayerController.Instance.inventory.hasSword = true;
                PlayerController.Instance.CreateAttack();
            }
            else
                base.ContinueInteraction();
        }
        else if (quest.status.dialog.HasReadDialogUntilCheckPoint(3) && speechEnded)
        {
            if (!PlayerController.Instance.inventory.hasBow)
            {
                // Sblocco la casa di Luigi
                house.SetActive(true);
                houseStatus.value = true;

                // Sblocco l'arco
                dialogText.text = bow.description;
                PlayerController.Instance.ObtainItem(bow);
                PlayerController.Instance.inventory.hasBow = true;

                quest.CompleteQuest();
            }
            else
                base.ContinueInteraction();
        }
        else
            base.ContinueInteraction();
    }
}