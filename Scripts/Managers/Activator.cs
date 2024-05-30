using UnityEngine;

public class Activator : MonoBehaviour
{
    public GameObject objectToActivate;
    
    public void Enable()
    {
        objectToActivate.SetActive(true);
    }

    public void Disable()
    {
        objectToActivate.SetActive(false);
    }
}