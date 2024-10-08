using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrInvenSys : MonoBehaviour
{ 
    int Max = 0;
    int SubSel = 0;
    int Sel = 0;
    int[] itemIndex;
    Color colSel;
    public string[] strItemName;
    public string[] strItemDesc;
    Text txtName;
    Text txtDesc;
    [SerializeField] Sprite sprEmpty;
    Image imgDescItem;
    Transform trFolder;
    Transform trSel;
    Transform trSubSel;
    PlayerInfo playerInfo;
    scrSpriteManager sprManager;
    GameManager manager;
    GameObject objSelBox;
    [SerializeField] GameObject objPrefab;
    private void Awake()
    {
        manager = transform.root.GetComponent<GameManager>();
        sprManager = manager.GetComponent<scrSpriteManager>();
        playerInfo = manager.playerInfo;
        txtName = transform.Find("imgItemDesc").Find("txtTitle").GetComponent<Text>();
        txtDesc = transform.Find("imgItemDesc").Find("txtDesc").GetComponent<Text>();
        imgDescItem= transform.Find("imgItemDesc").Find("imgBox").Find("imgIcon").GetComponent<Image>();
        trFolder = transform.Find("imgItemFolder").Find("objMask").Find("objFolder");
        trSel = transform.Find("imgItemFolder").Find("objMask").Find("imgSel");
        objSelBox = transform.Find("objSelBox").gameObject;
        trSubSel = objSelBox.transform.Find("imgSel");
    }
    private void OnEnable()
    {
        colSel = trSel.GetComponent<Image>().color;
        colSel.a = 0;
        trSel.GetComponent<Image>().color = colSel;
        txtName.text = "";
        txtDesc.text = "";
        imgDescItem.sprite = sprEmpty;
        Set();
    }
    void Set()
    {
        Max = 0;
        if (playerInfo.itemList.Count > trFolder.childCount)
        {
            for (int i = trFolder.childCount; i < playerInfo.itemList.Count; i++)
            {
                GameObject objTmp = Instantiate(objPrefab);
                objTmp.transform.SetParent(trFolder, true);
                objTmp.transform.localScale = Vector3.one;
            }
            Max = trFolder.childCount;
        }
        else if (playerInfo.itemList.Count < trFolder.childCount)
        {
            for (int i = 0; i < trFolder.childCount; i++)
            {
                if (i >= playerInfo.itemList.Count)
                trFolder.GetChild(i).gameObject.SetActive(false);
                else
                    trFolder.GetChild(i).gameObject.SetActive(true);
            }
            Max = playerInfo.itemList.Count;
        }
        else
        {
            for (int i = 0; i < trFolder.childCount; i++)
            {
                if (!trFolder.GetChild(i).gameObject.activeSelf)
                    trFolder.GetChild(i).gameObject.SetActive(true);
            }
            Max = trFolder.childCount;
        }

        if (playerInfo.itemList.Count > 0)
        {
            itemIndex = new int[playerInfo.itemList.Count];
            Transform trSel;
            for (int i = 0; i < Max; i++)
            {
                itemIndex[i] = playerInfo.itemList[i].Index;
                trSel = trFolder.GetChild(i);
                if (!trSel.gameObject.activeSelf)
                    trSel.gameObject.SetActive(true);

                int Index = playerInfo.itemList[i].Index;
                trSel.GetComponent<scrInvenItem>().ItemNum = Index;
                trSel.Find("Image").GetComponent<Image>().sprite =
                    sprManager.sprItem[Index];
                trSel.Find("txtCount").GetComponent<Text>().text =
                    "x " + playerInfo.itemList[i].Count;
            }
        }
        trFolder.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(550, 130 * ((Max / 4)+1));
    }
    public void SetDetail(int Index)
    {
        Sel = Index;
        objSelBox.transform.position = trFolder.GetChild(Sel).position;
        txtName.text = strItemName[Index];
        txtDesc.text = strItemDesc[Index];
        imgDescItem.sprite = sprManager.sprItem[Index];
    }

    public void ItemUse(int Index)
    { 
        switch (Index)
        {
            case 0:
            case 1:
                int Item = itemIndex[Sel];
                if (Index == 0) { playerInfo.ItemUse(Item); }
                playerInfo.ThrowItem(Item);
                Set();
                break;
        }
        objSelBox.SetActive(false);
    }

    public void OpenSelBox()
    {
        if (!objSelBox.activeSelf)
            objSelBox.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    { 
        if (objSelBox.activeSelf)
        {
            if (manager.PressDown())
            {
                if (SubSel < 3)
                {
                    SubSel++;
                    trSubSel.position = objSelBox.transform.Find("imgBox").GetChild(SubSel).position;
                }
            }
            else if (manager.PressUp())
            {
                if (SubSel > 0)
                {
                    SubSel--;
                    trSubSel.position = objSelBox.transform.Find("imgBox").GetChild(SubSel).position;
                }
            }
            else if (manager.PressOk())
            {
                ItemUse(SubSel);
            }
            else if (manager.PressMenu())
            {
                objSelBox.SetActive(false);
            }
        }
        else
        {
            if (manager.PressRight())
            {
                if (colSel.a != 0.5f)
                {
                    colSel.a = 0.5f;
                    trSel.GetComponent<Image>().color = colSel;
                }
                if (Sel < Max - 1)
                {
                    Sel++;
                    trSel.position = trFolder.GetChild(Sel).position;
                }
            }
            else if (manager.PressLeft())
            {
                if (colSel.a != 0.5f)
                {
                    colSel.a = 0.5f;
                    trSel.GetComponent<Image>().color = colSel;
                }
                if (Sel > 0)
                {
                    Sel--;
                    trSel.position = trFolder.GetChild(Sel).position;
                }
            }
            else if (manager.PressOk())
            {
                SubSel = 0;
                SetDetail(Sel);
                OpenSelBox();
            }
            else if (manager.PressMenu())
            {
                transform.parent.gameObject.SetActive(false);
            }
        }
    } 
}
