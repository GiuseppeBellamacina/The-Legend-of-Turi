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
        PlayerController.Instance.DeactivateInput();
        TransferScene();
        StartCoroutine(ReactivateInput());
    }

    IEnumerator ReactivateInput()
    {
        yield return new WaitForSeconds(1);
        PlayerController.Instance.ActivateInput();
    }

    public override void ContinueInteraction(){}
    public override void StopInteraction(){}
}
