using System.Collections;
using UnityEngine;
using TMPro;

public enum Direction{
    up,
    down,
    left,
    right,
    none
}

public class RoomTransfer : MonoBehaviour
{
    // Questi servono per spostare il giocatore nella stanza successiva
    protected GameObject minPosObject, maxPosObject;
    protected Vector2 offset;
    [Header("Position Transfer")]
    public float offValue = 1.8f;
    // Questi servono per mostrare il nome della stanza
    protected GameObject canvas, text;
    protected TMP_Text placeText;
    [Header("Transfer Index")]
    public IntValue transferIndex;
    protected float myTransferIndex;
    private int numberOfTransfers;
    // Qua c'è la roba per la stanza successiva
    [Header("Next Room Info")]
    public GameObject oldRoom;
    public GameObject nextRoom;
    public Direction direction;
    public string nextRoomName;
    public bool doNotActivate;

    void Awake()
    {
        minPosObject = nextRoom.transform.Find("minPos").gameObject;
        maxPosObject = nextRoom.transform.Find("maxPos").gameObject;
        canvas = GameObject.FindWithTag("Canvas");
        text = canvas.transform.Find("Place Text").gameObject;
        placeText = text.GetComponent<TMP_Text>();
    }

    protected virtual void Start()
    {
        text.SetActive(false);
        SetDirection(direction);
        myTransferIndex = 0;
        numberOfTransfers = 0;
    }

    protected void SetDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.up:
                offset = new Vector2(0, offValue);
                break;
            case Direction.down:
                offset = new Vector2(0, -offValue);
                break;
            case Direction.left:
                offset = new Vector2(-offValue, 0);
                break;
            case Direction.right:
                offset = new Vector2(offValue, 0);
                break;
            case Direction.none:
                offset = new Vector2(0, 0);
                break;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            oldRoom.GetComponent<Room>().DeactivateObjects();
            if (!doNotActivate)
                nextRoom.GetComponent<Room>().SpawnObjects();
            RoomLocator.Instance.SetMinMaxPositionObjects(minPosObject, maxPosObject);
            other.transform.position = new Vector2(other.transform.position.x + offset.x, other.transform.position.y + offset.y);
            text.SetActive(false); // resetta un'eventuale scritta precedente
            numberOfTransfers++;
            StartCoroutine(PlaceText());
            RoomLocator.Instance.SetCurrentRoomByPos();
        }
    }

    protected virtual IEnumerator PlaceText()
    {
        transferIndex.value++;
        myTransferIndex = transferIndex.value;
        int currentTransfer = numberOfTransfers;
        text.SetActive(true);
        placeText.text = nextRoomName;
        yield return new WaitForSeconds(4f);
        if (myTransferIndex == transferIndex.value && currentTransfer == numberOfTransfers)
            text.SetActive(false);
    }
}
