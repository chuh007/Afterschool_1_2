using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrOcculusion : MonoBehaviour
{
    [SerializeField] private bool show = false;
    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        ObjHide(show);
    }

    private void ObjHide(bool show)
    {
        for(int i = 0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(show);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MainCamera")
        {
            if (!show)
            {
                show = true;
                ObjHide(show);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MainCamera")
        {
            if (show)
            {
                show = false;
                ObjHide(show);
            }
        }
    }
}
