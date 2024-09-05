using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D bullet_rb;
    [SerializeField] float bullet_speed;
    //[SerializeField] Transform direction_trans;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Bullet_Des", 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bullet_rb.velocity = transform.up * bullet_speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Missile") {
            Destroy(collision.gameObject);
            
        }
        
    }

    void Bullet_Des() {
        Destroy(gameObject);
    }
}
