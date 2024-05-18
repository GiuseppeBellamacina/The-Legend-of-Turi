using UnityEngine;

public class SceneTransfer : Interactable
{
    public string sceneName;
    public VectorValue startingPosition;
    public bool willBeBounded;

    public void TransferScene()
    {
        GameController.Instance.startingPosition.value = startingPosition.value;
        GameController.Instance.LoadScene(sceneName, willBeBounded);
    }

    public override void Interact()
    {
        if (playerInRange && PlayerController.Instance.IsState(State.interact))
        {
            suggestionBox.SetActive(false);
            TransferScene();
        }
    }

    void FixedUpdate()
    {
        Interact();
    }
}
