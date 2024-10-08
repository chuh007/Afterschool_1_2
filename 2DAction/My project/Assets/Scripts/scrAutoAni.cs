using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrAutoAni : MonoBehaviour
{
    [SerializeField] bool Loop = true;
    [SerializeField] float AniSpd = 0.1f;
    int Index = 0;
    [SerializeField] Sprite[] spr;
    SpriteRenderer sprRen;
    private void Awake()
    {
        sprRen = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        StartCoroutine(this.AniPlay());
    } 
    IEnumerator AniPlay()
    {
        float CurrIndex = 0;
        while (Index < spr.Length)
        {
            CurrIndex += AniSpd;
            Index = Mathf.FloorToInt(CurrIndex);
            if (Index >= spr.Length)
            {
                if (Loop)
                {
                    Index = 0;
                    CurrIndex = 0;
                }
                else
                {
                    Index = spr.Length - 1;
                    break;
                }
            }
            sprRen.sprite = spr[Index];
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
