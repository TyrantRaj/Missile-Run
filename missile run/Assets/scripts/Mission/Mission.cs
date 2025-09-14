[System.Serializable]
public class Mission
{
    public string description;
    public bool isCompleted;
    public bool isCollected;       
    public int targetValue;
    public int currentValue;
    public int coinReward;      

    public enum MissionType { Score, Coin, destroymissiles, speed, repair, emf, survive1min, survive10min, withoutpower, withoutSlowmotion, Magnet, DoubleCoin, DoubleScore}
    public MissionType missionType;

    public void AddProgress(int amount)
    {
        if (isCompleted) return; // already done
        currentValue += amount;
        if (currentValue >= targetValue)
        {
            currentValue = targetValue;
            isCompleted = true;
        }
    }
}
