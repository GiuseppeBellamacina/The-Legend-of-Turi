[System.Serializable]
public class VectorData : DataClass
{
    public float[] values;

    public VectorData(VectorValue data) : base(data)
    {
        values = new float[2];
        values[0] = data.value.x;
        values[1] = data.value.y;
    }
}