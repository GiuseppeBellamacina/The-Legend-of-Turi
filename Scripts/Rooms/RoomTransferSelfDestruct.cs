using System.Collections;
using UnityEngine;

public class RoomTransferSelfDestruct : RoomTransfer
{
    public BoolValue isPlaced;

    protected override void Start()
    {
        base.Start();
        if (isPlaced.value)
            Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger && !isPlaced.value)
        {
            isPlaced.value = true;
            RoomLocator.Instance.SetMinMaxPositionObjects(minPosObject, maxPosObject);
            other.transform.position = new Vector2(other.transform.position.x + offset.x, other.transform.position.y + offset.y);
            text.SetActive(false);
            StartCoroutine(PlaceText());
        }
    }
    
    protected override IEnumerator PlaceText()
    {
        transferIndex.value++;
        myTransferIndex = transferIndex.value;
        text.SetActive(true);
        placeText.text = nextRoomName;
        yield return new WaitForSeconds(4f);
        if (myTransferIndex == transferIndex.value)
            text.SetActive(false);
        Destroy(gameObject);
    }
}
