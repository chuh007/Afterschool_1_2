using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrQuestSys : MonoBehaviour
{ 
    int Sel = -1;
    int Max = 0;
    [SerializeField] string[] strQuestName;
    [SerializeField] string[] strQuestDesc;
    Color colSel;
    Text txtName;
    Text txtDesc;
    Transform trSel;
    Transform trFolder;
    PlayerInfo playerInfo;
    GameManager manager;
    [SerializeField] GameObject objPrefab;
    private void Awake()
    {
        manager = transform.root.GetComponent<GameManager>();
        playerInfo = manager.playerInfo;
        txtName = transform.Find("imgDesc").Find("txtTitle").GetComponent<Text>();
        txtDesc = transform.Find("imgDesc").Find("txtDesc").GetComponent<Text>();
        trFolder = transform.Find("imgBox").Find("objMask").Find("objFolder");
        trSel = transform.Find("imgBox").Find("objMask").Find("imgSel");
    }
    private void OnEnable()
    {
        Sel = -1;
        colSel = trSel.GetComponent<Image>().color;
        colSel.a = 0;
        trSel.GetComponent<Image>().color = colSel;
        txtName.text = "";
        txtDesc.text = "";
        Set();  
    }
    void Set()
    {
        Max = 0;
        if (playerInfo.QuestList.Count>trFolder.childCount)
        {
            for (int i = trFolder.childCount; i < playerInfo.QuestList.Count; i++)
            {
                GameObject objTmp = Instantiate(objPrefab);
                objTmp.transform.SetParent(trFolder, true);
                objTmp.transform.localScale = Vector3.one;
            }
            Max = trFolder.childCount;
        }
        else if (playerInfo.QuestList.Count < trFolder.childCount)
        {
            for (int i= playerInfo.QuestList.Count; i< trFolder.childCount; i++)
            {
                if (i >= playerInfo.itemList.Count)
                    trFolder.GetChild(i).gameObject.SetActive(false);
                else
                    trFolder.GetChild(i).gameObject.SetActive(true);
            }  
            Max = playerInfo.QuestList.Count;
        } 
        else
        {
            for (int i = 0; i < trFolder.childCount; i++)
            {
                if(!trFolder.GetChild(i).gameObject.activeSelf)
                trFolder.GetChild(i).gameObject.SetActive(true);
            }
            Max = trFolder.childCount;
        }    

        Transform trSel;
        for (int i = 0; i < Max; i++)
        {
            trSel = trFolder.GetChild(i);
            trSel.GetComponent<scrQuestItem>().QuestNum =
                playerInfo.QuestList[i].Index;

            trSel.Find("Text").GetComponent<Text>().text = 
                strQuestName[ playerInfo.QuestList[i].Index ];
        }
        trFolder.GetComponent<RectTransform>().sizeDelta = new Vector2(530, 65*Max);
    }
    public void SetDetail (int Index)
    {
        txtName.text = strQuestName[Index];
        txtDesc.text = strQuestDesc[Index];
    }
    // Update is called once per frame
    void Update()
    {
        if (manager.PressUp())
        {
            if (colSel.a != 0.5f)
            {
                colSel.a = 0.5f;
                trSel.GetComponent<Image>().color = colSel;
            }
            if (Sel > 0) Sel--;
            trSel.position = trFolder.GetChild(Sel).position;
        }
        else if (manager.PressDown())
        {
            if (colSel.a != 0.5f)
            {
                colSel.a = 0.5f;
                trSel.GetComponent<Image>().color = colSel;
            }
            if (Sel < Max - 1) Sel++;
            trSel.position = trFolder.GetChild(Sel).position;
        }
        else if (manager.PressOk())
        {
            SetDetail( trFolder.GetChild(Sel).GetComponent<scrQuestItem>().QuestNum ); 
        } 
        else if (manager.PressMenu())
        {
            transform.parent.gameObject.SetActive(false);
        }
    } 
}
