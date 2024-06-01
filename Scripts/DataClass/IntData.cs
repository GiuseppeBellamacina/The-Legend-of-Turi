[System.Serializable]
public class IntData : DataClass
{
    public int value;
    public int initialValue;

    public IntData(IntValue data) : base(data)
    {
        value = data.value;
        initialValue = data.initialValue;
    }
}