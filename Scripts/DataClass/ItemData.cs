[System.Serializable]
public class ItemData : DataClass
{
    public bool hasBeenPickedUp;

    public ItemData(Item data) : base(data)
    {
        hasBeenPickedUp = data.hasBeenPickedUp;
    }
}