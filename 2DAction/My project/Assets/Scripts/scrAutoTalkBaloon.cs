using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrAutoTalkBaloon : MonoBehaviour
{
    [SerializeField] string str;
    [SerializeField] Vector3 posPlus;
    scrObjPool objPool;
    private void OnEnable()
    {
        StartCoroutine(this.ShowTalk());
    }
    IEnumerator ShowTalk()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (objPool == null) objPool = GameManager.instance.GetComponent<scrObjPool>();
            objPool.CreateTalk(transform.position + posPlus, str);
            yield return new WaitForSeconds(4);
        }
    }
     
}
