[System.Serializable]
public class Mission
{
    public string description;
    public bool isCompleted;
    public bool isCollected;       
    public int targetValue;
    public int currentValue;
    public int coinReward;      

    public enum MissionType { Score, Coin, SurviveTime, UsePowerUp }
    public MissionType missionType;
}
