[System.Serializable]
public class BoolData : DataClass
{
    public bool value;
    public bool initialValue;

    public BoolData(BoolValue data) : base(data)
    {
        value = data.value;
        initialValue = data.initialValue;
    }
}