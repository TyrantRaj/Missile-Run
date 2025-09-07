using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpecialPowers SPscript;
    public float moveSpeed = 5f;
    public float pickupDistance = 2f;
    private Transform player;
    private bool isAttracting = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SPscript = FindAnyObjectByType<SpecialPowers>();
        pickupDistance = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().CoinpickupDistance;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Start attracting if within pickupDistance
        if (distance < pickupDistance)
            isAttracting = true;

        if (isAttracting)
        {
            // Move towards player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Optional: destroy & add coin when very close
            if (distance < 0.2f)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        if (SPscript != null) {
            if (SPscript.Is2xCoin)
            {
                CurrencyManager.instance.AddCoin(2);
            }
            else
            {
                CurrencyManager.instance.AddCoin(1);
            }
        }

        

        foreach (var mission in MissionManager.Instance.currentMissions)
        {
            if (mission.missionType == Mission.MissionType.Coin && !mission.isCompleted)
            {
                mission.currentValue++;
                if (mission.currentValue >= mission.targetValue)
                {
                    mission.isCompleted = true;
                    //SoundManager.PlaySound(SoundManager.Sound.Archivement);       
                }
                    

            }
        }

        MissionManager.Instance.SaveMissionProgress();
        // Play sound or animation here if needed
        SoundManager.PlaySound(SoundManager.Sound.CoinPickUp);
        Destroy(gameObject);
    }
}
