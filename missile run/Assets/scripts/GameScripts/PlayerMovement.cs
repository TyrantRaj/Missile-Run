using System.Collections;
using System.Collections.Generic;
//using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;



public class PlayerMovement : MonoBehaviour

{
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
 

    private float horizontalMove;
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

    void PS_Controller()
    {
        if (play_right_PS && play_left_PS)
        {
            RightPS.Play();
            LeftPS.Play();
        }
        if (play_right_PS)
        {
            RightPS.Play();
        } else if (play_left_PS)
        {
            LeftPS.Play();
        }
    }

    public void pointerDownLeft()
    {
        moveLeft = true;
        Debug.Log("working");
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
            transform.Rotate(new Vector3(0, 0, 1) * right_rotate_speed, Space.World);
        }
        else if (moveRight)
        {
            transform.Rotate(new Vector3(0, 0, -1) * left_rotate_speed, Space.World);
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 0) , Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "SP")
        {
            spawnsp.CURRENT_SP -= 1;
            if (spawnsp.CURRENT_SP <= 3)
            {
                spawnsp.CURRENT_SP += 1;
                spawnSP();
            }
        }
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
