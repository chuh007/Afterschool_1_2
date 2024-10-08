using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrPause : MonoBehaviour
{
    private void OnEnable()
    { 
        Time.timeScale = 0.0001f;
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
