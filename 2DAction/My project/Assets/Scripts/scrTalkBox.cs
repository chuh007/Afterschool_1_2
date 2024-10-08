using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrTalkBox : MonoBehaviour
{
    Text txt;
    RectTransform tr;
    private void Awake()
    {
    }
    private void OnEnable()
    { 
        StartCoroutine(this.Disable());
    }

    public void Sett(string ReplaceStr)
    {
        if(txt==null) txt = transform.Find("Text").GetComponent<Text>();
        if (tr == null) tr = GetComponent<RectTransform>();
        txt.text = ReplaceStr;
        int FontSize = txt.fontSize; //폰트 크기
        int MaxTxt = 20;//가로 최대 글자수
        //문자열의 가로 최대 글자 를 나눠 높이를 구한다
        int Height = ReplaceStr.Length / MaxTxt;
        //그 나머지 값
        int Etc = ReplaceStr.Length % MaxTxt;
        float maxWidth = 0;
        if (Height <= 0) { maxWidth = Etc; } //높이가 0이면 넓이는 나머지 글자수로
        else maxWidth = MaxTxt; //그 외엔 가로 최대글자수 만큼의 넓이로

        float Xscale = 64 + (maxWidth * FontSize);
        float Yscale = 64 + ((Height + 1) * FontSize);

        tr.sizeDelta = new Vector2(Xscale, Yscale);
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
