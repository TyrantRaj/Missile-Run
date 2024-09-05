using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    public GameObject[] objects;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, objects.Length);
        GameObject gameobject = Instantiate(objects[rand], transform.position,Quaternion.identity);
        SpriteRenderer sr = gameobject.GetComponent<SpriteRenderer>();
        sr.color = new Color(1,1,1,Random.Range(0.325f,0.815f));
    }
}
