using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrAniSound : MonoBehaviour
{ 
    scrSEManager seManager;
    public void CreateSound(int Index)
    {
        if (seManager == null) 
            seManager = GameManager.instance.GetComponent<scrSEManager>();
        seManager.CreateSound(transform.position, Index);
    }
}
