[System.Serializable]
public class InventoryData : DataClass
{
    public int numberOfKeys;
    public int numberOfCoins;
    public int numberOfArrows;
    public bool hasSword;   
    public InventoryData(Inventory data) : base(data)
    {
        numberOfKeys = data.numberOfKeys;
        numberOfCoins = data.numberOfCoins;
        numberOfArrows = data.numberOfArrows;
        hasSword = data.hasSword;
    }
}