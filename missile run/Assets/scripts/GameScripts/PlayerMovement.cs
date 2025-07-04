using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour

{
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
    public float right_rotate_speed;
    public float left_rotate_speed;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawn = GameObject.FindWithTag("SPspawn");
        spawnsp = enemySpawn.GetComponent<SpawnSP>();

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
        player_rb.velocity = transform.up * speed;

        if (moveLeft)
        {
            float adjustedSpeed = right_rotate_speed;

            if (Damaged && (Damage_Side == 0 || both_Damaged))
            {
                adjustedSpeed *= 0.5f; // reduce speed by 50% on damaged side
            }

            transform.Rotate(new Vector3(0, 0, 1) * adjustedSpeed, Space.World);
        }
        else if (moveRight)
        {
            float adjustedSpeed = left_rotate_speed;

            if (Damaged && (Damage_Side == 1 || both_Damaged))
            {
                adjustedSpeed *= 0.5f; // reduce speed by 50% on damaged side
            }

            transform.Rotate(new Vector3(0, 0, -1) * adjustedSpeed, Space.World);
        }


        float bendAmount = 0.1f;

        if (moveLeft)
        {
            transform.localScale = new Vector3(1f - bendAmount, 1f + bendAmount * 0.5f, 1f); // Lean left
        }
        else if (moveRight)
        {
            transform.localScale = new Vector3(1f - bendAmount, 1f - bendAmount * 0.5f, 1f); // Lean right (inverted squish)
        }
        else
        {
            transform.localScale = Vector3.one; // Reset when not turning
        }



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

}
