using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class targeting_missile : MonoBehaviour
{
    [SerializeField] private GameObject bigCoinPrefab;
    [SerializeField, Range(0f, 1f)] private float bigCoinSpawnChance = 0.1f; // 10% chance by default


    SurviveTime timer;
    GameOver gameoverscript;
    [SerializeField] GameObject coinPrefab;
    private PlayerMovement playerMovement;
    private Transform target;
    private GameObject player;
    private Rigidbody2D missile_rb;
    public float missile_speed;
    public float rotate_speed;
    [SerializeField]public float missile_minSpeed;
    [SerializeField] public float missile_MaxSpeed;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        timer = FindAnyObjectByType<SurviveTime>();
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        gameoverscript = GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOver>();
        playerMovement = player.GetComponent<PlayerMovement>();
        missile_rb = GetComponent<Rigidbody2D>();

        // --- Core dynamic speed logic ---
        float playerSpeed = playerMovement.orginalSpeed;

        // Adjust missile speed based on player speed
        // You can tweak multipliers to balance difficulty
        float speedMultiplier = 1.2f; // missile is 20% faster than player
        missile_speed = Mathf.Clamp(playerSpeed * speedMultiplier, missile_minSpeed, missile_MaxSpeed);

        // Adjust rotation speed based on player speed (higher speed = lower rotation, harder to track)
        float rotationBase = 100f;
        float rotationFactor = 5f;
        rotate_speed = Mathf.Clamp(rotationBase - playerSpeed * rotationFactor, 100f, 500f); // Keep rotation within bounds

        // Optional: Pause thrust particles
        playerMovement.RightPS?.Pause();
        playerMovement.LeftPS?.Pause();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        missile_rb.linearVelocity = transform.up * missile_speed;
        Vector2 direction = (target.position - transform.position).normalized;
        float rotate_amount = Vector3.Cross(transform.up, direction).z;
        missile_rb.angularVelocity = rotate_amount * rotate_speed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        missile_speed = 0;
        rotate_speed = 0;

        if (collision.tag == "Player")
        {
            if(timer != null)
            {
                timer.ResetNoHitTimer();
            }
            collision.GetComponent<ShakeTrigger>()?.TriggerShake();
            Damage_Player();
            Explode();

        }
        else if (collision.tag == "Missile")
        {
            Vector3 explosionPos = collision.transform.position;
            ScoreManager.Instance?.AddScore(100, transform.position);

            if (Random.value < bigCoinSpawnChance && bigCoinPrefab != null)
            {
                Instantiate(bigCoinPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }


            Explode();
        }
        else if (collision.tag == "Border")
        {


            Explode();

        }
        else if (collision.tag == "SP")
        {
            //spawnsp.CURRENT_SP -= 1;
            gameObject.SetActive(false);
            
            Invoke("Destroy_Go", 12);
        }

    }


    private void Damage_Player()
    {
        if(playerMovement.isGod) { return; }

        //0 means left side damage
        //1 means right side damage
        if (playerMovement.both_Damaged)
        {
            Restart_Game();
            Debug.Log("Game Over");
        }
        else if (playerMovement.Damaged)
        {
            if (playerMovement.Damage_Side == 0)
            {
                playerMovement.play_right_PS = true;
                playerMovement.RightTR.enabled = false;
                playerMovement.both_Damaged = true;

            }
            else if (playerMovement.Damage_Side == 1)
            {
                playerMovement.play_left_PS = true;
                playerMovement.LeftTR.enabled = false;
                playerMovement.both_Damaged = true;
            }
        }
        else
        {
            playerMovement.Damage_Side = UnityEngine.Random.Range(0, 2); // 0 or 1


            if (playerMovement.Damage_Side == 0)
            {
                playerMovement.play_left_PS = true;
                playerMovement.LeftTR.enabled = false;
            }
            else
            {
                playerMovement.play_right_PS = true;
                playerMovement.RightTR.enabled = false;
            }
            playerMovement.Damaged = true;
        }
    }

    void Restart_Game()
    {
        gameoverscript.GameOverFunction();
    }



  

    void Explode()
    {
        foreach (var mission in MissionManager.Instance.currentMissions)
        {
            if (mission.missionType == Mission.MissionType.destroymissiles && !mission.isCompleted)
            {
                mission.currentValue++;
                if (mission.currentValue >= mission.targetValue)
                    mission.isCompleted = true;
            }
        }
        FindObjectOfType<IndicatorManager>().RemoveTarget(gameObject);
        anim.Play("Explosion");
        SoundManager.PlaySound(SoundManager.Sound.Explosion);
        Destroy(gameObject, 0.5f); // Destroy missile itself shortly after
    }



    void Destroy_Go()
    {
        Destroy(gameObject);
    }

}