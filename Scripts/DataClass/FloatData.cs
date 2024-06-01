[System.Serializable]
public class FloatData : DataClass
{
    public float value;
    public float initialValue;

    public FloatData(FloatValue data) : base(data)
    {
        value = data.value;
        initialValue = data.initialValue;
    }
}