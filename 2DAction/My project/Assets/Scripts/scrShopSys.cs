using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class ShopItem
{
    public Item item = new Item();
    public int Money = 10;
}

public class scrShopSys : MonoBehaviour
{ 
    int Max = 0;
    int Sel = -1;
    int SubSel = -1; 
    Color colSel;
    Color colSubSel;
    [SerializeField] string strMessage;
    public ShopItem[] ShopItem;
    [SerializeField] Sprite sprEmpty;
    Transform trFolder;
    Transform trSel;
    Transform trSubSel;
    Text txtMessage;
    Text txtDesc;
    Text txtName;
    Image imgDescIcon;
    PlayerInfo playerInfo;
    scrInvenSys invenSys;
    scrSpriteManager sprManager;
    GameManager manager;
    GameObject objMessage;
    [SerializeField] GameObject objPrefab;
    private void Awake()
    {
        trFolder = transform.Find("imgList").Find("objMask").Find("objFolder"); 
        txtDesc = transform.Find("imgDesc").Find("txtDesc").GetComponent<Text>();
        txtName = transform.Find("imgDesc").Find("txtTitle").GetComponent<Text>();
        imgDescIcon = transform.Find("imgDesc").Find("imgBox").Find("imgIcon").GetComponent<Image>();
        manager = transform.root.GetComponent<GameManager>();
        sprManager = manager.GetComponent<scrSpriteManager>();
        playerInfo = manager.playerInfo;
        invenSys = 
            manager.transform.Find("Canvas").Find("pnlMenu").Find("objInven").GetComponent<scrInvenSys>();
        objMessage = transform.Find("pnlMessage").gameObject;
        txtMessage= objMessage.transform.Find("imgBox").Find("txtTitle").GetComponent<Text>();
        trSel = transform.Find("imgSel");
        trSubSel = objMessage.transform.Find("imgBox").Find("imgSel");
    }
    private void OnEnable()
    {
        manager.isEventPlay = true;

        colSel = trSel.GetComponent<Image>().color;
        colSel.a = 0;
        trSel.GetComponent<Image>().color = colSel;

        colSubSel = trSubSel.GetComponent<Image>().color;
        colSubSel.a = 0;
        trSubSel.GetComponent<Image>().color = colSubSel;

        imgDescIcon.sprite = sprEmpty;
        txtName.text = "";
        txtDesc.text = "";
        Set();
    }
    void Set()
    {
        Max = 0;
        int ItemLength = ShopItem.Length;
        if(ItemLength> trFolder.childCount)
        {
            GameObject obj = Instantiate(objPrefab);
            obj.transform.SetParent(trFolder, true);
            obj.transform.localScale = Vector3.one;
            Max = trFolder.childCount;
        }
        else
        {
            for (int i = 0; i < trFolder.childCount; i++)
            {
                if (i >= ItemLength)
                    trFolder.GetChild(i).gameObject.SetActive(false);
                else
                    trFolder.GetChild(i).gameObject.SetActive(true);
            }
            Max = ItemLength;
        }
         
        Transform trSel;
        for (int i = 0; i < trFolder.childCount; i++) 
        { 
            trSel = trFolder.GetChild(i);

            trSel.GetComponent<scrShopItem>().ItemIndex = 
                ShopItem[i].item.Index;

            trSel.Find("imgIcon").GetComponent<Image>().sprite =
                sprManager.sprItem[i];

            trSel.Find("imgIcon").Find("txtName").GetComponent<Text>().text =
                invenSys.strItemName[i];

            trSel.Find("imgMoney").Find("txtMoney").GetComponent<Text>().text= 
                ShopItem[i].Money.ToString();
        }
        trFolder.GetComponent<RectTransform>().sizeDelta =
            new Vector2(730, 90 * Max);
    }

    public void SetItem(int BtnIndex)
    {
        if(Sel != BtnIndex)
        {
            Sel = BtnIndex;
            imgDescIcon.sprite = sprManager.sprItem[BtnIndex];
            txtName.text = invenSys.strItemName[BtnIndex];
            txtDesc.text = invenSys.strItemDesc[BtnIndex]; 
        }
        else
        { 
            if (!objMessage.activeSelf)
            {
                txtMessage.text = invenSys.strItemName[BtnIndex] + "\n" + strMessage;
                objMessage.SetActive(true);
            }
        }
    }
    public void DisableMessage()
    { 
        colSubSel.a = 0;
        trSubSel.GetComponent<Image>().color = colSubSel;
        objMessage.SetActive(false); 
    }
    public void Buy( )
    {
        if(playerInfo.Money>= ShopItem[Sel].Money)
        {
            playerInfo.Money -= ShopItem[Sel].Money;
            ShopItem[Sel].item.Count = 1;
            playerInfo.GetItem(ShopItem[Sel].item);
            DisableMessage();
        }
    } 
    // Update is called once per frame
    void Update()
    {
        if (objMessage.activeSelf)
        {
            if (manager.PressLeft())
            {
                if (SubSel > 0)
                { 
                    SubSel--;
                    trSubSel.position = 
                        objMessage.transform.Find("imgBox").GetChild(SubSel + 1).position;
                }
            }
            else if (manager.PressRight())
            {
                if (SubSel < 1)
                { 
                    SubSel++;
                    trSubSel.position =
                        objMessage.transform.Find("imgBox").GetChild(SubSel + 1).position;
                }
            }
            else if (manager.PressOk())
            {
                switch (SubSel)
                {
                    case 0: Buy(); break;
                    case 1: DisableMessage(); break;
                }
            }
            else if (manager.PressNo())
            { DisableMessage(); }
        }
        else
        {
            if (manager.PressUp())
            {
                if (Sel > 0)
                {
                    if (colSel.a != 0.5f)
                    {
                        colSel.a = 0.5f;
                        trSel.GetComponent<Image>().color = colSel;
                    }
                    SetItem(Sel-1);
                    trSel.position = trFolder.GetChild(Sel).position;
                }
            }
            else if (manager.PressDown())
            {
                if (Sel < Max - 1)
                {
                    if (colSel.a != 0.5f)
                    {
                        colSel.a = 0.5f;
                        trSel.GetComponent<Image>().color = colSel;
                    }
                    SetItem(Sel+1);
                    trSel.position = trFolder.GetChild(Sel).position;
                }
            }
            else if (manager.PressOk())
            {
                if (colSubSel.a != 0.5f)
                {
                    colSubSel.a = 0.5f;
                    trSubSel.GetComponent<Image>().color = colSubSel;
                    SubSel = 0;
                    trSubSel.position =
                        objMessage.transform.Find("imgBox").GetChild(SubSel + 1).position;
                }
                SetItem(Sel);
            }
            else if (manager.PressNo())
            {
                gameObject.SetActive(false);
            }
        }
        
    }

    private void OnDisable()
    {
        manager.isEventPlay = false;
    }
}
