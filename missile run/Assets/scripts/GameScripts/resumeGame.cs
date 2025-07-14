using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resumeGame : MonoBehaviour
{
    private void Start()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
}
