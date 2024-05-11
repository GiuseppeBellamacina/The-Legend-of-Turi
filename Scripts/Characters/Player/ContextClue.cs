using UnityEngine;

public class ContextClue : MonoBehaviour
{
    public GameObject contextClue;
    
    public void Enable()
    {
        contextClue.SetActive(true);
    }

    public void Disable()
    {
        contextClue.SetActive(false);
    }
}