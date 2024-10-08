using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrMoneySys : MonoBehaviour
{
    Text txt;
    PlayerInfo playerInfo;
    private void Awake()
    {
        txt = transform.Find("Text").GetComponent<Text>();
    }
    private void OnEnable()
    {
        if (playerInfo == null)
            playerInfo = transform.root.GetComponent<GameManager>().playerInfo;
        if (playerInfo.Money == 0)
            txt.text = "0";
        else
            txt.text = string.Format("{0:#,###}", playerInfo.Money);
    }
    public void PlusMoney(int Plus)
    {
        StartCoroutine(this.SetMoney(Plus));
    }

    IEnumerator SetMoney(float plus)
    {
        float money = (float)playerInfo.Money;
        playerInfo.Money += (int)plus;
        float PlusVar = (plus) *0.05f;

        while (money < playerInfo.Money)
        {
            money += PlusVar;
            txt.text = string.Format("{0:#,###}", money);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        txt.text = string.Format("{0:#,###}", playerInfo.Money);
    }
}
