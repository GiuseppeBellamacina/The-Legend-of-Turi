using System.Collections;
using UnityEngine;

public class SceneTransfer : Interactable
{
    public string sceneName;
    public VectorValue startingPosition;
    public bool willBeBounded;

    protected override void Start()
    {
        suggestionBox.SetActive(false);
        if (isContextClue)
            contextOff.Raise();
    }

    public void TransferScene()
    {
        suggestionBox.SetActive(false);
        GameController.Instance.startingPosition.value = startingPosition.value;
        GameController.Instance.LoadScene(sceneName, willBeBounded);
    }

    public override void Interact()
    {
        TransferScene();
    }

    public override void ContinueInteraction(){}
    public override void StopInteraction(){}
}
