using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrInvenItem : MonoBehaviour
{
    [HideInInspector] public int ItemNum = 0;
    scrInvenSys sys;
    public void Detail()
    {
        if (sys == null) sys =
                transform.parent.parent.parent.parent.GetComponent<scrInvenSys>();
        sys.SetDetail(ItemNum);
        sys.OpenSelBox();
    }
}
