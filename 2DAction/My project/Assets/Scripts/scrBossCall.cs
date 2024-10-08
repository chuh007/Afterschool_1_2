using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrBossCall : MonoBehaviour
{
    bool Call = true;
    [SerializeField] Transform trBoss;
    [SerializeField] Transform trCnvBoss;
    GameManager manager;
    IEnumerator CallBossCanvas()
    {
        if (manager == null) manager = GameManager.instance;
        manager.GetComponent<scrBGMManager>().SetChangeBgm(1);
        manager.isEventPlay = true;
        manager.transform.Find("Main Camera").GetComponent<scrFollowCam>().trBoss = trBoss;
        manager.transform.Find("objPlayer").GetComponent<scrCharMove>().MoveSpd = 0;
        trCnvBoss.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        trCnvBoss.Find("pnlBattleOpen").gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        StartCoroutine(trBoss.Find("Object").GetComponent<scrBossPattern>().PlayAttPtrn());
        manager.isEventPlay = false;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Call)
            {
                Call = false;
                StartCoroutine(this.CallBossCanvas());
            }
        }
    }
}
