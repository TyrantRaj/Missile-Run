using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followRadar : MonoBehaviour
{
    [SerializeField] Transform playerpos;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(playerpos.transform.position.x, playerpos.transform.position.y,0);
    }
}
