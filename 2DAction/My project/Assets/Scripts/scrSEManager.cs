using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSEManager : MonoBehaviour
{
    [SerializeField] AudioClip[] clip;
    scrObjPool objPool;
    private void Awake()
    {
        objPool = GetComponent<scrObjPool>();
    }
    public void CreateSound(Vector3 pos, int Index)
    {
        objPool.CreateSound(pos, clip[Index]);
    }
}
