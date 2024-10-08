using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrSetting : MonoBehaviour
{
    [HideInInspector]
    public bool SubSel = false;
    bool CanChangeKey = false;
    bool CanPlaySE = true;
    int Sel = 0;
    int KeySel = -1; 
    Color colSubSel;
    AudioSource audio;
    Transform trKeyFolder;
    Transform trKeySel;
    Transform trSel;
    Slider sldBgm;
    Slider sldSE;
    PlayerInfo playerInfo;
    scrBGMManager bgmManager;
    GameManager manager;
    private void Awake()
    {
        manager = transform.root.GetComponent<GameManager>();
        bgmManager = manager.GetComponent<scrBGMManager>();
        playerInfo = manager.playerInfo;
        sldBgm = transform.Find("imgBox").Find("txtBGM").Find("Slider").GetComponent<Slider>();
        sldSE = transform.Find("imgBox").Find("txtSE").Find("Slider").GetComponent<Slider>();
        audio = GetComponent<AudioSource>();
        trKeyFolder = transform.Find("imgBox").Find("txtKey").Find("imgBox").Find("objFolder");
        trKeySel= transform.Find("imgBox").Find("txtKey").Find("imgBox").Find("imgSel");
        trSel = transform.Find("imgBox").Find("imgSel");
    }
    private void OnEnable()
    {  
        colSubSel = trKeySel.GetComponent<Image>().color;
        colSubSel.a = 0;
        trKeySel.GetComponent<Image>().color = colSubSel;
        
        Sel = 0; 
        trSel.position = transform.Find("imgBox").GetChild(Sel + 1).position;
        KeySel = -1;
        SubSel = false;

        string str = "";
        for(int i=0; i<10; i++)
        { 
            switch (i)
            {
                case 0: str = playerInfo.RightMoveKey; break;
                case 1: str = playerInfo.LeftMoveKey; break;
                case 2: str = playerInfo.UpMoveKey; break;
                case 3: str = playerInfo.DownMoveKey; break;
                case 4: str = playerInfo.JumpKey; break;
                case 5: str = playerInfo.AttackKey; break;
                case 6: str = playerInfo.DashKey; break;
                case 7: str = playerInfo.SubmitKey; break;
                case 8: str = playerInfo.CancelKey; break;
                case 9: str = playerInfo.MenuKey; break;
            }
            trKeyFolder.GetChild(i).Find("imgBtn").Find("Text").GetComponent<Text>().text = str;
        }
        Set();
    }
    void Set() 
    {
        sldBgm.value = playerInfo.BGMVol;
        sldSE.value = playerInfo.SEVol;
    }

    public void SetBGMVol()
    {
        playerInfo.BGMVol = sldBgm.value;
        bgmManager.SetBgmVol(playerInfo.BGMVol);
    }
    public void SetSEVol()
    {
        if (playerInfo.SEVol != sldSE.value)
        {
            playerInfo.SEVol = sldSE.value;
            if (CanPlaySE)
            {
                audio.volume = playerInfo.SEVol;
                audio.Play();
                CanPlaySE = false;
                StartCoroutine(this.ResetDelaySE());
            }
        }
    }

    IEnumerator ResetDelaySE()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        CanPlaySE = true;
    }
    private void Update()
    {
        if (!CanChangeKey)
        {
            if (manager.PressRight())
            {
                if (SubSel)
                {
                    switch (Sel)
                    {
                        case 0:
                            if (sldBgm.value < 1)
                            {
                                sldBgm.value += 0.1f;
                                SetBGMVol();
                            }
                            break;
                        case 1:
                            if (sldSE.value < 1)
                            {
                                sldSE.value += 0.1f;
                                SetSEVol();
                            }
                            break;
                        case 2:
                            if (KeySel < 10 - 1)
                            {
                                KeySel++;
                                trKeySel.position = 
                                    trKeyFolder.GetChild(KeySel).position;
                            }
                            break;
                    }
                }
            }
            else if (manager.PressLeft())
            {
                if (SubSel)
                {
                    switch (Sel)
                    {
                        case 0:
                            if (sldBgm.value > 0)
                            {
                                sldBgm.value -= 0.1f;
                                SetBGMVol();
                            }
                            break;
                        case 1:
                            if (sldSE.value > 0)
                            {
                                sldSE.value -= 0.1f;
                                SetSEVol();
                            }
                            break;
                        case 2:
                            if (KeySel > 0)
                            {
                                KeySel--;
                                trKeySel.position = 
                                    trKeyFolder.GetChild(KeySel).position;
                            }
                            break;
                    }
                }
            }
            else if (manager.PressUp())
            {
                if (!SubSel)
                {
                    if (Sel > 0)
                    { 
                        Sel--;
                        trSel.position = transform.Find("imgBox").GetChild(Sel + 1).position;
                    }
                }
                else
                {
                    if (SubSel)
                    {
                        if (KeySel-2 >= 0)
                        {
                            KeySel-=2;
                            trKeySel.position = trKeyFolder.GetChild(KeySel).position;
                        }
                    }
                }
            }
            else if (manager.PressDown())
            {
                if (!SubSel)
                {
                    if (Sel < 3 - 1)
                    { 
                        Sel++;
                        trSel.position = transform.Find("imgBox").GetChild(Sel + 1).position;
                    }
                }
                else
                {
                    if (KeySel + 2 < 10)
                    {
                        KeySel += 2;
                        trKeySel.position = trKeyFolder.GetChild(KeySel).position;
                    }
                }
            }
            else if (manager.PressOk())
            {
                if (SubSel)
                {
                    if(KeySel>-1)
                    {
                        Color col= Color.red;
                        col.a = 0.5f;
                        trKeySel.GetComponent<Image>().color = col;
                        CanChangeKey = true;
                        StartCoroutine(this.CheckChangeKey());
                    }
                }
                else
                {
                    SubSel = true;
                    switch (Sel)
                    {
                        case 0: break;
                        case 1: break;
                        case 2:
                            KeySel = 0;
                            if (colSubSel.a != 0.5f)
                            {
                                colSubSel.a = 0.5f;
                                trKeySel.GetComponent<Image>().color = colSubSel;
                            }
                            trKeySel.position = trKeyFolder.GetChild(KeySel).position;
                            break;
                    }
                }
            }
            else if (manager.PressMenu())
            {
                if (SubSel)
                {
                    SubSel = false;
                    colSubSel.a = 0f;
                    trKeySel.GetComponent<Image>().color = colSubSel;
                }
                else
                {
                    transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
    public void ChangeKey(int Key)
    { 
        KeySel = Key;
        CanChangeKey = true;
        StartCoroutine(this.CheckChangeKey());
    }
    IEnumerator CheckChangeKey()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        string str = "";
        while(true)
        {
            if(CanChangeKey) 
            {
                if(Input.anyKeyDown)
                { 
                    str = Input.inputString;
                    Debug.Log(str);
                    if (str != "")
                    {
                        switch (KeySel)
                        {
                            case 0: playerInfo.RightMoveKey = str; break;
                            case 1: playerInfo.LeftMoveKey = str; break;
                            case 2: playerInfo.UpMoveKey = str; break;
                            case 3: playerInfo.DownMoveKey = str; break;
                            case 4: playerInfo.JumpKey = str; break;
                            case 5: playerInfo.AttackKey = str; break;
                            case 6: playerInfo.DashKey = str; break;
                            case 7: playerInfo.SubmitKey = str; break;
                            case 8: playerInfo.CancelKey = str; break;
                            case 9: playerInfo.MenuKey = str; break;
                        }
                        trKeyFolder.GetChild(KeySel).Find("imgBtn").Find("Text").
                            GetComponent<Text>().text = str;
                        CanChangeKey = false;
                        trKeySel.GetComponent<Image>().color = colSubSel;
                        break;
                    }
                }
            }
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
    }
}
