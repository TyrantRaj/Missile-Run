using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WaveMissile : MonoBehaviour
{
    [SerializeField] private GameObject bigCoinPrefab;
    [SerializeField, Range(0f, 1f)] private float bigCoinSpawnChance = 0.1f;

    SurviveTime timer;
    GameOver gameoverscript;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] public float missile_speed = 10f;
    [SerializeField] private Rigidbody2D rb_wave;
    [SerializeField] private Animator anim;


    private Vector2 moveDirection;
    private bool isMoving = false;

    private GameObject player;
    private PlayerMovement playerMovement;


    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
        isMoving = true;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    void Start()
    {
        timer = FindAnyObjectByType<SurviveTime>();
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        gameoverscript = GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOver>();
        Invoke("Destroy_Go", 15f);
        playerMovement.RightPS.Pause();
        playerMovement.LeftPS.Pause();
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb_wave.linearVelocity = moveDirection * missile_speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isMoving = false;
        rb_wave.linearVelocity = Vector2.zero;

        if (collision.CompareTag("Player"))
        {
            if (timer != null)
            {
                timer.ResetNoHitTimer();
            }
            collision.GetComponent<ShakeTrigger>()?.TriggerShake();
            Damage_Player();
            Explode();
        }
        else if (collision.CompareTag("Missile"))
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

        else if (collision.CompareTag("SP"))
        {
            //spawnsp.CURRENT_SP -= 1;
            gameObject.SetActive(false);
            Invoke("Destroy_Go", 12f);
        }
    }

    private void Damage_Player()
    {
        if (playerMovement.isGod) { return; }

        if (playerMovement.both_Damaged)
        {
            Restart_Game();
        }
        else if (playerMovement.Damaged)
        {
            if (playerMovement.Damage_Side == 0)
            {
                playerMovement.play_right_PS = true;
                playerMovement.RightTR.enabled = false;
                playerMovement.both_Damaged = true;
            }
            else
            {
                playerMovement.play_left_PS = true;
                playerMovement.LeftTR.enabled = false;
                playerMovement.both_Damaged = true;
            }
        }
        else
        {
            playerMovement.Damage_Side = Random.Range(0, 2);

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

    private void Restart_Game()
    {
        gameoverscript.GameOverFunction();
    }

    private void Explode()
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
        FindObjectOfType<IndicatorManager>()?.RemoveTarget(gameObject);
        anim.Play("Explosion");
        SoundManager.PlaySound(SoundManager.Sound.Explosion);
        Destroy(gameObject, 0.5f);
    }

   

    void Destroy_Go()
    {
        Destroy(gameObject);
    }
}
