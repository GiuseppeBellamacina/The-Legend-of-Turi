using System.Collections;
using UnityEngine;

public class Madame : Npc
{
    public QueenQuest quest;
    public SceneTransfer sceneTransfer;
    public BoolValue dungeonUnlocked;

    protected override void Awake()
    {
        base.Awake();
        quest = GetComponent<QueenQuest>();
    }

    public override void Interact()
    {
        // Se non ho ancora parlato con la regina e non ho completato la quest
        if (!quest.status.isActive && !quest.status.isCompleted)
        {
            quest.StartQuest();
            quest.knightDialog.SetNextCheckpoint();
        }
        // Se non ho completato il dialogo con la regina e non ho ancora sbloccato il dungeon
        else if (quest.status.isActive && !quest.CheckCondition() && !dungeonUnlocked.value)
        {
            quest.UpdateQuest(0);
        }
        // Se ho già parlato con la regina e non ho completato la quest
        else if (quest.status.isActive && !quest.CheckCondition())
        {
            quest.UpdateQuest(1);
        }
        // Se ho già parlato con la regina e ho completato la quest
        else if (quest.status.isActive && quest.CheckCondition())
        {
            quest.UpdateQuest(2);
            // Sblocco il dungeon
            quest.CompleteQuest();
            // QUI FINISCE IL GIOCO
        }

        base.Interact();
    }

    public override void ContinueInteraction()
    {
        if (!dungeonUnlocked.value)
        {
            if (quest.status.dialog.HasReadDialogUntilCheckPoint(1))
            {
                dungeonUnlocked.value = true;
                base.ContinueInteraction();
            }
            else
                base.ContinueInteraction();
        }
        else
            base.ContinueInteraction();
    }

    IEnumerator WaitAndTransfer()
    {
        yield return new WaitForSeconds(3f);
        sceneTransfer.Interact();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (playerInRange && PlayerController.Instance.IsState(State.attack))
        {
            PlayerController.Instance.SetState(State.none);
            string sentence = "Regina:\nGuardie! Buttate fuori questo viddanazzo!";
            suggestionBox.SetActive(false);
            contextOff.Raise();
            TextDisplacer(sentence);
            PlayerController.Instance.DeactivateInput();
            StartCoroutine(WaitAndTransfer());
        }
    }
}