using UnityEngine;

public class DoubleScore : MonoBehaviour
{
    private SpecialPowers SpScript;
    private SurviveTime timer;
    [SerializeField] private Sprite Icon;

    private void Start()
    {
        SpScript = FindAnyObjectByType<SpecialPowers>();
        timer = FindAnyObjectByType<SurviveTime>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (timer != null)
                timer.ResetNoPowerUpTimer();

            foreach (var mission in MissionManager.Instance.currentMissions)
            {
                if (mission.missionType == Mission.MissionType.DoubleScore && !mission.isCompleted)
                {
                    mission.currentValue++;
                    if (mission.currentValue >= mission.targetValue)
                        mission.isCompleted = true;
                }
            }
            float duration = UpgradeItem.GetDuration(UpgradeItem.UpgradeItems.DoubleScore);
            SpScript.ActivateDoubleScore(duration,Icon); // Use our new method
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
