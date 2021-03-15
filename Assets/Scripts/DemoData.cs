public enum DataType
{
    A,
    B,
    C,
}

public abstract class BaseData
{
    public abstract DataType Type { get; }
}

public class AData : BaseData
{
    public double _aDouble;

    public override DataType Type
    {
        get { return DataType.A; }
    }

    public override string ToString()
    {
        return string.Format("Type: {0}, _args: {1}", Type, _aDouble);
    }
}

public class BData : BaseData
{
    public string _bString;

    public override DataType Type
    {
        get { return DataType.B; }
    }

    public override string ToString()
    {
        return string.Format("Type: {0}, _args: {1}", Type, _bString);
    }
}

public class CData : BaseData
{
    public bool _cBool;

    public override DataType Type
    {
        get { return DataType.C; }
    }

    public override string ToString()
    {
        return string.Format("Type: {0}, _args: {1}", Type, _cBool);
    }
}