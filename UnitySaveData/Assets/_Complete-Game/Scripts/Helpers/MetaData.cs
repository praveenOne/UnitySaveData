[System.Serializable]
public struct MyTransform
{
    public float X;
    public float Y;
    public float Z;

    public MyTransform(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
}

[System.Serializable]
public struct MyCharacter
{
    public int Helth;
    public MyTransform Position;
    public MyTransform Rotation;

    public MyCharacter(int helth, MyTransform position, MyTransform rotation)
    {
        this.Helth = helth;
        this.Position = position;
        this.Rotation = rotation;
    }
}



[System.Serializable]
public class MetaData{

    public MyCharacter Player;
    public MyCharacter[] ZomBunny;
    public MyCharacter[] ZomBear;
    public MyCharacter[] Helliphant;

    public MetaData(MyCharacter player, MyCharacter[] zomBunny, MyCharacter[] zomBear, MyCharacter[] helliphant)
    {
        this.Player = player;
        this.ZomBunny = zomBunny;
        this.ZomBear = zomBear;
        this.Helliphant = helliphant;
    }
}
