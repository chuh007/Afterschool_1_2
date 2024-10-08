using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrShopEvnt : MonoBehaviour
{
    scrEventObj evnt;
    scrObjPool objPool;
    private void Awake()
    {
        evnt = GetComponent<scrEventObj>();
    }
    private void OnEnable()
    {
        StartCoroutine(this.Chk());
    }
    IEnumerator Chk()
    {
        while (true)
        {
            if (evnt.Open)
            {
                if (objPool == null) 
                    objPool = GameManager.instance.GetComponent<scrObjPool>();
                if (!objPool.ExistsShop())
                {
                    objPool.CreateEvntKey(false);
                    objPool.CreateShop();
                    evnt.Open = false;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
