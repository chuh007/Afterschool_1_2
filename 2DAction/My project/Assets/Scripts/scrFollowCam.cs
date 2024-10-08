using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrFollowCam : MonoBehaviour
{ 
    Vector3 pos;
    [HideInInspector] public Vector3 PosSet;
    scrCharMove playerMove;
    [SerializeField] Transform trPlayer;
    public Transform trBoss;

    private void OnEnable()
    {
        playerMove = trPlayer.GetComponent<scrCharMove>();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if(trPlayer==null)
        {
            pos = PosSet;
        }
        else
        {
            if(trBoss==null)
            { pos = trPlayer.position + new Vector3(playerMove.Look*2 , 1.5f); }
            else
            { pos = trPlayer.position - (trPlayer.position - trBoss.position) * 0.5f; }
        }
        pos.z = -10;

        transform.position = Vector3.Lerp(transform.position, pos, 0.3f);
    }
    public void SetPlayer()
    {
        trPlayer = GameManager.instance.transform.Find("objPlayer");
    }
    public void SetReset()
    {
        trPlayer = null;
    }
    public void SetResetBoss()
    {
        trBoss = null;
    }
}
