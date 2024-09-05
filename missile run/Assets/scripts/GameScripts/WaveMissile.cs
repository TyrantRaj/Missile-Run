using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMissile : MonoBehaviour
{
    [SerializeField] private float missile_speed;
    [SerializeField] private Rigidbody2D rb_wave;
    [SerializeField] public Animator anim;
    [SerializeField] public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb_wave.velocity = transform.up * missile_speed;
        Vector2 direction = (player.position - transform.position).normalized;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        missile_speed = 0;
        if (collision.tag == "Player")
        {
            anim.Play("Explosion");
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else if (collision.tag == "Missile")
        {
            //Destroy(collision.gameObject);
            anim.Play("Explosion");
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else if (collision.tag == "Border")
        {
            anim.Play("Explosion");
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }



    }
}
