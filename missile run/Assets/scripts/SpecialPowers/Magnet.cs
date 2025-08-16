using UnityEngine;

public class Magnet : MonoBehaviour
{
    private SpecialPowers spScript;

    [SerializeField] private Sprite Icon;

    private SurviveTime timer;

    void Start()
    {
        timer = FindAnyObjectByType<SurviveTime>();
        spScript = GameObject.FindWithTag("Player").GetComponent<SpecialPowers>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        if (timer != null)
            timer.ResetNoPowerUpTimer();
            float duration = UpgradeItem.GetDuration(UpgradeItem.UpgradeItems.Magnet);
            // Example: set magnet range to 10f for 15 seconds
            spScript.ActivateMagnet(12f,duration,Icon);

        // Update missions if needed
        foreach (var mission in MissionManager.Instance.currentMissions)
        {
            if (mission.missionType == Mission.MissionType.Magnet && !mission.isCompleted)
            {
                mission.currentValue++;
                if (mission.currentValue >= mission.targetValue)
                    mission.isCompleted = true;
            }
        }
            FindObjectOfType<IndicatorManager>().RemoveTarget(gameObject);
            Destroy(gameObject);
    }
    else if (collision.CompareTag("Missile"))
    {
        Animator animator = collision.GetComponent<Animator>();
        animator.Play("Explosion");
        Destroy(collision.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}

}
