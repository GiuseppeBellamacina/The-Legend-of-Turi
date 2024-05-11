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
    // Questi servono per mostrare il nome della stanza
    protected GameObject canvas, text;
    protected TMP_Text placeText;
    public FloatValue transferIndex;
    protected float myTransferIndex;
    // Qua c'è la roba per la stanza successiva
    public GameObject nextRoom;
    public Direction direction;
    public string nextRoomName;

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
    }

    protected void SetDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.up:
                offset = new Vector2(0, 1.8f);
                break;
            case Direction.down:
                offset = new Vector2(0, -1.8f);
                break;
            case Direction.left:
                offset = new Vector2(-1.8f, 0);
                break;
            case Direction.right:
                offset = new Vector2(1.8f, 0);
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
            RoomLocator.instance.SetMinMaxPositionObjects(minPosObject, maxPosObject);
            other.transform.position = new Vector2(other.transform.position.x + offset.x, other.transform.position.y + offset.y);
            text.SetActive(false); // resetta un'eventuale scritta precedente
            StartCoroutine(PlaceText());
        }
    }

    protected virtual IEnumerator PlaceText()
    {
        transferIndex.value++;
        myTransferIndex = transferIndex.value;
        text.SetActive(true);
        placeText.text = nextRoomName;
        yield return new WaitForSeconds(4f);
        if (myTransferIndex == transferIndex.value)
            text.SetActive(false);
    }
}
