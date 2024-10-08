using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrTalkChar : MonoBehaviour
{
    [SerializeField] CharKind[] CharIndex;
    [SerializeField] Quest quest;
    [SerializeField] string[] strIndex;
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
                if (objPool == null) objPool = GameManager.instance.GetComponent<scrObjPool>();
                if (!objPool.ExistTalk())
                {
                    objPool.CreateEvntKey(false);
                    objPool.CreateTalk(CharIndex, strIndex, quest);
                    evnt.Open = false;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
