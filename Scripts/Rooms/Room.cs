using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] objectsToSpawn;

    void Start()
    {
        DeactivateObjects();
    }

    public void SpawnObjects()
    {
        if (objectsToSpawn.Length == 0)
            return;
        foreach (GameObject obj in objectsToSpawn)
        {
            obj.SetActive(true);
            obj.GetComponent<IResettable>()?.Reset();
        }
    }

    public void DeactivateObjects()
    {
        if (objectsToSpawn.Length == 0)
            return;
        foreach (GameObject obj in objectsToSpawn)
        {
            obj.SetActive(false);
        }
    }
}