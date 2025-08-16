using UnityEngine;

public class TwoxCoin : MonoBehaviour
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
        if (collision.tag == "Player")
        {
            if (timer != null)
            {
                timer.ResetNoPowerUpTimer();
            }

            foreach (var mission in MissionManager.Instance.currentMissions)
            {
                if (mission.missionType == Mission.MissionType.DoubleCoin && !mission.isCompleted)
                {
                    mission.currentValue++;
                    if (mission.currentValue >= mission.targetValue)
                        mission.isCompleted = true;
                }
            }
            float duration = UpgradeItem.GetDuration(UpgradeItem.UpgradeItems.DoubleCoin);
            SpScript.ActivateDoubleCoin(duration, Icon);
            FindObjectOfType<IndicatorManager>().RemoveTarget(gameObject);
            gameObject.SetActive(false);

        }
        else if (collision.tag == "Missile")
        {
            Animator animator = collision.GetComponent<Animator>();
            animator.Play("Explosion");

            Destroy(collision.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        }
    }
}
