using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrPlayerInfo : MonoBehaviour
{
    int HP = 1;
    int MaxHp = 1;
    float Plus = 0;
    [SerializeField]float Exp = 0;
    [SerializeField]float Etc = 0;
    float HpWidth;
    float ExpWidth;
    Color colHeal = Color.white;
    Image imgHp;
    Image imgDemHp;
    Image imgHealHp;
    Image imgExpGuage;
    Transform trHpEnd;
    Transform trExpEnd;
    RectTransform recHp;
    RectTransform recExp;
    PlayerInfo playerInfo;
    private void Awake()
    {
        recHp = transform.Find("objHpFolder").GetComponent<RectTransform>();
        imgHp = recHp.transform.Find("imgHp").GetComponent<Image>();
        imgDemHp = recHp.transform.Find("imgDem").GetComponent<Image>();
        imgHealHp = recHp.transform.Find("imgHeal").GetComponent<Image>();
        trHpEnd = imgHp.transform.Find("imgEnd");
        recExp = transform.Find("objExp").GetComponent<RectTransform>();
        imgExpGuage = recExp.transform.Find("imgExp").GetComponent<Image>();
        trExpEnd= imgExpGuage.transform.Find("imgEnd");
    }
    void Start()
    { 
        colHeal = imgHealHp.color;
        colHeal.a = 0;
        imgHealHp.color = colHeal;
    }
    private void OnEnable()
    {
        SetInfo();

        HpWidth= (imgHp.GetComponent<RectTransform>().sizeDelta.x);
        ExpWidth = (imgExpGuage.GetComponent<RectTransform>().sizeDelta.x);

        HP = playerInfo.Hp;
        MaxHp = playerInfo.MaxHp;
        imgHp.fillAmount = (float)HP / (float)MaxHp;
        imgExpGuage.fillAmount = 0;
    }
    public void SetInfo()
    {
        playerInfo = transform.root.GetComponent<GameManager>().playerInfo;
    }
    private void Update()
    {
        //경험치
        Exp = (float)(playerInfo.Exp / playerInfo.MaxExp);
        if (Exp > 1 && Etc == 0)
        {
            Etc = Exp - 1;
            Exp = 1;
        }

        if (imgExpGuage.fillAmount < Exp)
        {
            imgExpGuage.fillAmount += 0.01f; 
        }
        if(imgExpGuage.fillAmount>=1)
        {
            if (playerInfo.Exp >= playerInfo.MaxExp)
            {
                playerInfo.Exp -= playerInfo.MaxExp;
                playerInfo.LevelUp();
                imgExpGuage.fillAmount = 0; 
                if (Etc > 0)
                {
                    Exp = Etc;
                    Etc -= playerInfo.MaxExp;
                    if (Etc < 0) Etc = 0;
                } 
            }
        }
        trExpEnd.localPosition =
            new Vector3((ExpWidth * -0.5f) + (ExpWidth * imgExpGuage.fillAmount), 0, 0);


        //체력
        HP = playerInfo.Hp;
        imgHp.fillAmount = (float)HP / (float)MaxHp;

        if (imgDemHp.fillAmount > imgHp.fillAmount)
        {
            if (Plus == 0) 
            { Plus = (imgDemHp.fillAmount - imgHp.fillAmount) * 0.1f; }  
            imgDemHp.fillAmount += -Plus; 
        }
        else
        {
            if (imgDemHp.fillAmount != imgHp.fillAmount)
            {
                imgDemHp.fillAmount = imgHp.fillAmount; 
                Plus = 0; 
            }
        }

        //회복 체력

        if (imgHealHp.fillAmount < imgHp.fillAmount)
        {
            if (Plus == 0) 
            {
                Plus = (imgHealHp.fillAmount - imgHp.fillAmount) * 0.05f;
                colHeal.a = 1; 
                imgHealHp.color = colHeal;
            } 
            else
            {
                colHeal.a = (imgHp.fillAmount - imgHealHp.fillAmount) * 10;  
                imgHealHp.color = colHeal;
            }
            imgHealHp.fillAmount += -Plus; 
        }
        else
        {
            if (imgHealHp.fillAmount != imgHp.fillAmount)
            {
                imgHealHp.fillAmount = imgHp.fillAmount; 
                Plus = 0; 
            }
        }
        trHpEnd.localPosition =
            new Vector3((HpWidth * -0.5f) + (HpWidth * imgHp.fillAmount), 0, 0);
    }
}
