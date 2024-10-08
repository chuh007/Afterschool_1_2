using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharKind { Player, CharName0, CharName1, CharName2 };

public class scrTalkSys : MonoBehaviour
{
    bool[] ShowSCG = new bool[2];
    public CharKind[] charKind;
    bool CanSkip = false;
    int TalkCount = 0;
    Color colLScg= Color.white;
    Color colRScg = Color.white;
    Vector3 LSCGPos;
    Vector3 RSCGPos;
    string strCharName;
    [SerializeField]string[] strTalk;
    [HideInInspector] public Quest quest;
    Transform[] trScg = new Transform[2];
    Text txt;
    Text txtName;
    Image imgLScg;
    Image imgRScg; 
    GameManager manager;
    private void Awake()
    {
        txt = transform.Find("imgTalkBox").Find("txtTalk").GetComponent<Text>();
        txtName= transform.Find("imgTalkBox").Find("txtName").GetComponent<Text>();
        imgLScg= transform.Find("imgLScg").GetComponent< Image>();
        imgRScg = transform.Find("imgRScg").GetComponent< Image>();
        trScg[0] = imgLScg.transform;
        trScg[1] = imgRScg.transform;
        manager = transform.root.GetComponent<GameManager>(); 
    }
    private void OnEnable()
    {
        ShowSCG[0] = false;
        ShowSCG[1] = false;
        colLScg.a = 0;
        imgLScg.color = colLScg;
        colRScg.a = 0;
        imgRScg.color = colRScg;
        manager.isEventPlay = true;
        TalkCount = 0;
        Talk();
    }
    public void Set(CharKind[] talk, string[] str)
    {
        charKind = talk;
        strTalk = str;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))  
        {
            if (CanSkip)
            {
                if (TalkCount < strTalk.Length - 1)
                {
                    if (txt.text.Length == strTalk[TalkCount].Length)
                    {
                        TalkCount++;
                        Talk();
                    }
                    else if (txt.text.Length < strTalk[TalkCount].Length)
                    {
                        StopAllCoroutines(); 
                        txt.text = strTalk[TalkCount]; 
                    }
                }
                else gameObject.SetActive(false);
                CanSkip = false;
            }
        }
        else if (Input.GetButtonUp("Submit"))
        { if (!CanSkip) CanSkip = true; }
        trScg[0].localPosition = Vector3.Lerp(trScg[0].localPosition, LSCGPos, 0.2f);
        trScg[1].localPosition = Vector3.Lerp(trScg[1].localPosition, RSCGPos, 0.2f);

    }
    public void Talk()
    {
        StartCoroutine(this.TextOut());
        StartCoroutine(this.MoveSCG());
    }
    IEnumerator MoveSCG()
    { 
        yield return new WaitForSeconds(0.1f);
        if (charKind[TalkCount] == CharKind.Player)
        { 

            ShowSCG[0] = true;
            LSCGPos = new Vector3(-600, -200, 0);
            RSCGPos = new Vector3(600 + (200), -200 + (-150), 0);
        }

        colLScg = (charKind[TalkCount] == CharKind.Player) ? Color.white : Color.gray; 
        colLScg.a = ShowSCG[0] ? 1 : 0;

        if (charKind[TalkCount] != CharKind.Player)
        { 
            ShowSCG[1] = true;
            LSCGPos = new Vector3(-600 + (-200), -200 + (-150), 0);
            RSCGPos = new Vector3(600, -200, 0);
        }

        colRScg = (charKind[TalkCount] != CharKind.Player) ? Color.white : Color.gray; 
        colRScg.a = ShowSCG[1] ? 1 : 0;

        float i = 0;
        while (i < 1)
        {
            imgLScg.color = Color.Lerp(imgLScg.color, colLScg, 0.05f);
            imgRScg.color = Color.Lerp(imgRScg.color, colRScg, 0.05f);// SCG 색 변경
             
            yield return new WaitForSeconds(Time.deltaTime);
            i += 0.01f;
        }
    }
    IEnumerator TextOut(float TalkDelay = 0.001f)
    {
        switch(charKind[TalkCount])
        {
            case CharKind.Player: strCharName = "에스텔"; break;
            case CharKind.CharName0: strCharName = "루이스"; break;
            case CharKind.CharName1: strCharName = "제인"; break;
        }
        txtName.text = strCharName;
        txt.text = "";
        yield return new WaitForSecondsRealtime(0.1f);
        int len = 0;

        while (len < strTalk[TalkCount].Length)
        {
            txt.text += (strTalk[TalkCount][len]);
            len++;
            yield return new WaitForSecondsRealtime(TalkDelay);
        }
        txt.text = strTalk[TalkCount];
        yield return new WaitForSecondsRealtime(1);
    }
    private void OnDisable()
    {
        if(quest.Index>-1)
        {
            manager.playerInfo.GetQuest(quest);
        }
        manager.isEventPlay = false;
    }
}
