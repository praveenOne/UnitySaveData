[System.Serializable]
public class MetaData{

    public int PlayerHelth;
    public float[] PlayerPosition;
    public float[] PlayerRotation;

    public MetaData(int playerHelth, float[] playerPosition, float[] playerRotation)
    {
        this.PlayerHelth = playerHelth;
        this.PlayerPosition = playerPosition;
        this.PlayerRotation = playerRotation;
    }
}
