using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrTreasureBox : MonoBehaviour
{
    bool Open = false;
    [SerializeField] int BoxIndex = 0; 
    [SerializeField] int MaxMoney;
    scrEventObj evnt;
    GameManager manager;
    private void Awake()
    {
        evnt = GetComponent<scrEventObj>();
    }
    private void OnEnable()
    {
        StartCoroutine(this.CheckHide());
    }
    IEnumerator CheckHide()
    {
        yield return new WaitForSeconds(0.1f);
        if (manager == null) manager = GameManager.instance;
        if (manager.playerInfo.OpenBoxIndex[BoxIndex])
        {
            GetComponent<BoxCollider2D>().enabled = false;
            gameObject.layer = 0;
        }
        else StartCoroutine(this.CheckOpen());
    }
    IEnumerator CheckOpen()
    {
        while (true)
        {
            if (evnt.Open && !Open)
            { 
                manager.playerInfo.OpenBoxIndex[BoxIndex] = true;
                StartCoroutine(this.CreateMoney());
                Open = true;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator CreateMoney()
    {
        GetComponent<scrAutoAni>().enabled = true;
        scrObjPool objPool = manager.GetComponent<scrObjPool>();
        objPool.CreateEvntKey(false);
        GetComponent<BoxCollider2D>().enabled = false;
        gameObject.layer = 0;

        for (int i = 0; i < MaxMoney; i++)
        {
            objPool.CreateMoney(transform.position + Vector3.up);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
