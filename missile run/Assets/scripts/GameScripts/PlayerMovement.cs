using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    public SpriteRenderer playerSpriteRenderer;
    public bool isGod = false;
    private SpawnSP spawnsp;
    private GameObject enemySpawn;

    [SerializeField] float Time_Gap = 20;
    public GameObject[] SP_gameObjects;

    [SerializeField] public ParticleSystem RightPS;
    [SerializeField] public ParticleSystem LeftPS;
    [SerializeField] public TrailRenderer RightTR;
    [SerializeField] public TrailRenderer LeftTR;
    [SerializeField] public bool play_right_PS = false;
    [SerializeField] public bool play_left_PS = false;
 
    private bool moveRight;
    private bool moveLeft;


    public int Player_Life = 3;
    public int Damage_Side;
    public bool Damaged = false;
    public bool both_Damaged = false;

    public Rigidbody2D player_rb;
    public float speed;
    public float orginalSpeed;
    public float right_rotate_speed;
    public float left_rotate_speed;

    void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        AudioSource loopingAudio = SoundManager.PlayLoopingSound(SoundManager.Sound.PlaneSound);

        int index = PlayerPrefs.GetInt("SelectedCharacter", 0); // default to 0

        // Ensure stats are always set when starting the game
        speed = PlayerPrefs.GetFloat("JetSpeed_" + index, 3f); // fallback values
        orginalSpeed = PlayerPrefs.GetFloat("JetSpeed_" + index, 3f);
        right_rotate_speed = left_rotate_speed = PlayerPrefs.GetFloat("JetRot_" + index, 2f);

        JetDatabase db = Resources.Load<JetDatabase>("JetDatabase");
        SelectedJetStats.jetSprite = db.jetSkins[index].sprite;


        playerSpriteRenderer.sprite = SelectedJetStats.jetSprite;

        enemySpawn = GameObject.FindWithTag("SPspawn");
        spawnsp = enemySpawn.GetComponent<SpawnSP>();
        Application.targetFrameRate = 60;

        moveLeft = false;
        moveRight = false;
    }


    private void FixedUpdate()
    {
        Movement();
     
        PS_Controller();
    }

    void Stop_Ps_Play()
    {
        RightTR.enabled = true;
        LeftTR.enabled = true;
        play_left_PS = false ;
        play_right_PS = false;
        RightPS.Stop();
        LeftPS.Stop();
    }

    void PS_Controller()
    {
        if (play_right_PS && !RightPS.isPlaying)
        {
            RightPS.Play();
        }
        else if (!play_right_PS && RightPS.isPlaying)
        {
            RightPS.Stop();
        }

        if (play_left_PS && !LeftPS.isPlaying)
        {
            LeftPS.Play();
        }
        else if (!play_left_PS && LeftPS.isPlaying)
        {
            LeftPS.Stop();
        }
    }


    public void pointerDownLeft()
    {
        moveLeft = true;
    }

    public void pointerUpLeft()
    {
        moveLeft = false;
    }
    public void pointerDownRight()
    {
        moveRight = true;
    }

    public void pointerUpRight()
    {
        moveRight = false;
    }

    void Movement()
    {
        player_rb.linearVelocity = transform.up * speed;

        bool left = moveLeft || Input.GetKey(KeyCode.A);
        bool right = moveRight || Input.GetKey(KeyCode.D);

        if (left)
        {
            float adjustedSpeed = right_rotate_speed;
            if (Damaged && (Damage_Side == 0 || both_Damaged))
                adjustedSpeed *= 0.5f;
            transform.Rotate(new Vector3(0, 0, 1) * adjustedSpeed, Space.World);
        }
        else if (right)
        {
            float adjustedSpeed = left_rotate_speed;
            if (Damaged && (Damage_Side == 1 || both_Damaged))
                adjustedSpeed *= 0.5f;
            transform.Rotate(new Vector3(0, 0, -1) * adjustedSpeed, Space.World);
        }

        float bendAmount = 0.1f;

        if (left)
            transform.localScale = new Vector3(1f - bendAmount, 1f + bendAmount * 0.5f, 1f);
        else if (right)
            transform.localScale = new Vector3(1f - bendAmount, 1f - bendAmount * 0.5f, 1f);
        else
            transform.localScale = Vector3.one;
    }


    public void Repair()
    {
        // Restore full movement speed
        Damaged = false;
        both_Damaged = false;
        
        Stop_Ps_Play();

        // You can also add any visual/audio feedback for repair here
        Debug.Log("Player Repaired!");
    }


    public IEnumerator spawnSpecialPower(float Time_Gap, GameObject SP)
    {
        yield return new WaitForSeconds(Time_Gap);

        Vector3 whereToSpawn = new Vector3(Random.Range(-80f, 80f), Random.Range(-110f, 110f));

        GameObject newSpecialPower = Instantiate(SP, whereToSpawn, Quaternion.identity);

    }

    public void spawnSP()
    {


        int rand = Random.Range(0, SP_gameObjects.Length);

        StartCoroutine(spawnSpecialPower(Time_Gap, SP_gameObjects[rand]));


    }

    public void Die()
    {
        SoundManager.PauseAllLoopingSounds();
        playerSpriteRenderer.enabled = false;
        LeftPS.gameObject.SetActive(false);
        RightPS.gameObject.SetActive(false);
        player_rb.linearVelocity = Vector2.zero;
        player_rb.angularVelocity = 0f;
        player_rb.bodyType = RigidbodyType2D.Static;

    }


}
