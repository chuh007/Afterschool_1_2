using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrResetFall : MonoBehaviour
{
    bool Chk = true;
    scrFollowCam cam;
    GameManager manager;
    IEnumerator CallReset()
    {
        if (cam == null) cam = manager.transform.Find("Main Camera").GetComponent<scrFollowCam>();
        cam.SetReset();
        manager.isEventPlay = true;
        manager.playerInfo.Hp = 0;
        yield return new WaitForSeconds(1f);
        manager.GetComponent<scrObjPool>().CreateGameOver();
        manager.isEventPlay = false;
        Chk = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Chk)
            {
                Chk = false;
                if (manager == null) manager = GameManager.instance;
                manager.CamRelease(collision.transform.position);
                StartCoroutine(this.CallReset());
            }
        }
        else if (collision.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);
        }
    }
}
