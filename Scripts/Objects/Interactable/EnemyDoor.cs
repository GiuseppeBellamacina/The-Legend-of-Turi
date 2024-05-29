using UnityEngine;

public class EnemyDoor : Door
{
    public GameObject room;
    public GameObject[] otherDoors;
    public BoolValue hasBeenInteracted;
    DoorType[] oldDoorTypes;
    GameObject[] enemies;


    protected override void Start()
    {
        base.Start();

        enemies = room.GetComponent<Room>().objectsToSpawn;
        oldDoorTypes = new DoorType[otherDoors.Length];
    }

    public override void Interact()
    {
        base.Interact();
        dialogText.text = "La porta <b><color=#FF0000FF>non si muove</color></b>, ma...\n<size=70>Cos'è questo rumore</size>?";

        if (doorType == DoorType.Enemies)
        {
            if (!hasBeenInteracted.value) // Così non si resetta ogni volta che si interagisce con la porta
            {
                foreach (GameObject enemy in enemies)
                {
                    enemy.SetActive(true);
                    enemy.GetComponent<IResettable>()?.Reset();
                }
            }
            hasBeenInteracted.value = true;

            int i = 0;
            foreach (GameObject otherDoor in otherDoors)
            {
                otherDoor.GetComponent<Door>().Close();
                oldDoorTypes[i++] = otherDoor.GetComponent<Door>().doorType;
                otherDoor.GetComponent<Door>().doorType = DoorType.Blocked;
            }
        }
    }

    int CountEnemies()
    {   
        foreach (GameObject otherDoor in otherDoors)
        {
            if (otherDoor.GetComponent<Door>().isOpen.value)
                return 1;
        }

        int count = 0;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf)
                count++;
        }
        return count;
    }

    void Update()
    {
        if (CountEnemies() == 0 && hasBeenInteracted.value)
        {
            Open();
            doorType = DoorType.Normal;
            int i = 0;
            foreach (GameObject otherDoor in otherDoors)
            {
                otherDoor.GetComponent<Door>().doorType = oldDoorTypes[i++];
                otherDoor.GetComponent<Door>().Open();
            }
        }
    }
}