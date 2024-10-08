using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrShopItem : MonoBehaviour
{
    public int ItemIndex = 0;
    scrShopSys sys; 
    public void Buy()
    {
        if (sys == null)
        {
            sys =
                transform.parent.parent.parent.parent.GetComponent<scrShopSys>();
        }
        sys.SetItem(ItemIndex);
    }
     
}
