using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrFollowObj : MonoBehaviour
{
    [SerializeField] Vector3 pos; 
    void Update()
    {
        transform.localPosition = pos;
    }
}
