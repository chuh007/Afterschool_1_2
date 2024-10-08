using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrBGMChange : MonoBehaviour
{
    [SerializeField] int BgmIndex = 0;
    scrBGMManager BgmManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            if (BgmManager == null) 
                BgmManager = GameManager.instance.GetComponent<scrBGMManager>();
            BgmManager.SetChangeBgm(BgmIndex);
        }
    }
}
