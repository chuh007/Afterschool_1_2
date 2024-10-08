using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrGameOverSys : MonoBehaviour
{
    int Sel = 0;
    Transform trFolder;
    [SerializeField] Color[] col = new Color[2];
    Image[] imgBtn = new Image[2];
    private void Awake()
    {
        trFolder = transform.Find("imgBox");

        imgBtn[0] = trFolder.GetChild(1).GetComponent<Image>(); 
        imgBtn[1] = trFolder.GetChild(2).GetComponent<Image>();
         
    }
    private void OnEnable()
    {
        //trFolder.GetChild(1).GetComponent<Button>().enabled = false; 
        //Select(0);
    }

    private void Select(int Index)
    {
        Sel = Index;
        imgBtn[0].color = (Index == 0 ? col[0] : col[1]);
        imgBtn[1].color = (Index == 1 ? col[0] : col[1]);
    }
    public void MouseClick(int Index)
    {
        trFolder.GetChild(1 + Sel).GetComponent<Button>().enabled = true;
        Click(Index);
    }
    void Click(int Index)
    {
        switch (Index)
        {
            case 0:
                GameManager.instance.LoadData();
                GameManager.instance.isEventPlay = false;
                GameManager.instance.SetCamPlayer();
                GameManager.instance.ResetCamBoss();
                gameObject.SetActive(false);
                break; //예 일때 게임 다시 시작
            case 1:
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
                GameManager.instance.Pause(true);
#else

                Application.Quit();
# endif
                break; // 아니오 일 때 게임 끄기 
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (Sel != 0)
            {
                trFolder.GetChild(1).GetComponent<Button>().enabled = false;
                trFolder.GetChild(2).GetComponent<Button>().enabled = true;
                Sel--;
                Select(Sel);
            }
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (Sel != 1)
            {
                trFolder.GetChild(1).GetComponent<Button>().enabled = true;
                trFolder.GetChild(2).GetComponent<Button>().enabled = false;
                Sel++;
                Select(Sel);
            }
        }
        else if (Input.GetButtonDown("Submit"))
        { Click(Sel); }
    }
}
