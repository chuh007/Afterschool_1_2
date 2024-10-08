using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrEventObj : MonoBehaviour
{
    public bool Open = false;
    bool EnterPlayer = false;
    scrPlayerCtrl player;
    scrObjPool objPool;
    GameManager manager;

    private void Update()
    {
        if(EnterPlayer )
        {
            if (!manager.isEventPlay&&!objPool.ExistEvntKey())
            {
                player.CallEvntBtn(EnterPlayer, this);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!EnterPlayer)
            {
                if (manager == null) manager = GameManager.instance;
                if (objPool == null) objPool = manager.GetComponent<scrObjPool>();
                if (player == null) player = collision.transform.GetComponent<scrPlayerCtrl>();
                 EnterPlayer = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (EnterPlayer)
            {
                EnterPlayer = false;
                player.CallEvntBtn(EnterPlayer, this);
            }
        }
    }
}
