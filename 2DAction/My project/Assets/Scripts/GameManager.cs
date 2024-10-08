using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;


[Serializable]
public class Item 
{ 
    public int Index = 0;
    public int Count = 1;
}

[Serializable]
public class Quest
{
    public int Index = -1;
    public int Progress = 0;
}

[Serializable]
public class PlayerInfo //플레이어 정보
{
    public bool[] OpenBoxIndex = new bool[20];
    public int MaxJump = 2; 
    public int Money = 0;
    public int Level = 1;
    public int MaxHp = 10;
    public int Hp = 10;
    public int Pow = 1;
    public int BGMIndex = 0;
    public float BGMVol = 0.5f;
    public float SEVol = 1f;
    public float MaxExp = 5;
    public float Exp = 0;
    public Vector3 posPlayer;
    public string strSceneName;
    
    public string UpMoveKey="t";
    public string DownMoveKey = "g";
    public string LeftMoveKey = "f";
    public string RightMoveKey = "h";
    public string SubmitKey = "c";
    public string CancelKey = "v";
    public string JumpKey = "v";
    public string AttackKey = "b";
    public string MenuKey = "escape";
    public string DashKey = "b";

    public List<Quest> QuestList = new List<Quest>();
    public List<Item> itemList = new List<Item>();

    #region //레벨 관련 함수
    public void LevelUp()
    {
        Level++;

        MaxHp = Level * 10;
        Hp = MaxHp;
        MaxExp = Level * 5;
        Pow = Level; 
    }
    #endregion
    #region //아이템 관련 함수
    public void GetItem(Item item)
    {
        Item tmp = new Item();
        tmp.Index = item.Index; 
        if (itemList.Count == 0)
        {
            itemList.Add(tmp);
        }
        else
        {
            bool isExists = false;
            int Index = 0;
            for (int i = 0; i < itemList.Count; i++)
            {
                if (item.Index == itemList[i].Index)
                {
                    isExists = true;
                    Index = i;
                    break;
                }
            }
            if (isExists)
            {
                itemList[Index].Count += item.Count;
            }
            else
            {
                itemList.Add(tmp);
            }
        }
    }
    public void ThrowItem(int Index)
    {
        if (itemList.Count > 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (Index == itemList[i].Index)
                {
                    itemList[i].Count--;
                    if (itemList[i].Count <= 0)
                    {
                        itemList.Remove(itemList[i]);
                    }
                    break;
                }
            }
        }
    }
    public void ItemUse(int Index)
    {
        switch (Index)
        {//아이템 효과
            case 0: Hp += 5; break;//체력포션
            case 2: Exp += 10; break;//경험치
        }
    }

    #endregion
    #region //퀘스트 관련 함수

    public void GetQuest(Quest quest)
    {
        Quest tmp = new Quest();
        tmp.Index = quest.Index;
        tmp.Progress = quest.Progress;
        if (QuestList.Count == 0)
        {
            QuestList.Add(tmp);
        }
        else
        {
            bool isExists = false;
            int Index = 0;
            for (int i = 0; i < QuestList.Count; i++)
            {
                if (tmp.Index == QuestList[i].Index)
                {
                    isExists = true;
                    Index = i;
                    break;
                }
            }
            if (isExists)
            {
                QuestList[Index].Progress++;
            }
            else
            {
                QuestList.Add(tmp);
            }
        }
    }
    public int CheckQuest(Quest quest)
    {
        int Index = -1; 
        if (QuestList.Count > 0)
        {  
            for (int i = 0; i < QuestList.Count; i++)
            {
                if (quest.Index == QuestList[i].Index)
                { 
                    Index = i;
                    break;
                }
            } 
        }
        return Index;
    }
    #endregion

}
public class GameManager : MonoBehaviour
{
    bool CanButton = true;
    public bool isEventPlay = false;
    public bool CanMenu = true;
    int Saved = 0;
    Transform trPlayer;
    Transform cam;
    scrBGMManager bgmManager;
    scrSceneManager sceneManager;
    scrObjPool objPool;
    scrMoneySys moneySys;
    GameObject objPause;

    [HideInInspector] public static GameManager instance;
    public PlayerInfo playerInfo = new PlayerInfo();

    private void Awake()
    {
        if (instance == null) instance = this;
        moneySys =
            transform.Find("Canvas").Find("pnlPlayerInfo").Find("imgMoney").GetComponent<scrMoneySys>();
        objPause = transform.Find("Canvas").Find("pnlMenu").gameObject;
        trPlayer = transform.Find("objPlayer");
        bgmManager = GetComponent<scrBGMManager>();
        objPool = GetComponent<scrObjPool>();
        cam = transform.Find("Main Camera");
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    { 
    }
    public void Pause(bool Pause)
    {
        OnApplicationPause(Pause);
    }
    void OnApplicationPause(bool pauseStatus)
    {
    }
    #region //버튼 관련 함수 
    IEnumerator ResetButton()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        CanButton = true;
    }
    public bool MoveRight()
    { 
        return Input.GetAxisRaw("Horizontal") > 0
            || Input.GetKey(playerInfo.RightMoveKey);
    }
    public bool MoveLeft()
    {
        return Input.GetAxisRaw("Horizontal") < 0
            || Input.GetKey(playerInfo.LeftMoveKey);
    }
    public bool MoveUp()
    {
        return Input.GetAxisRaw("Vertical") > 0
            || Input.GetKey(playerInfo.UpMoveKey);
    }
    public bool MoveDown()
    {
        return Input.GetAxisRaw("Vertical") < 0
            || Input.GetKey(playerInfo.DownMoveKey);
    }
    public bool PressRight()
    {
        bool Press = false;
        if(CanButton &&
            (Input.GetAxisRaw("Horizontal")> 0
            || Input.GetKey(playerInfo.RightMoveKey)))
        {
            StartCoroutine(this.ResetButton());
            CanButton = false;
            Press = true;
        }
        return Press;
    }
    public bool PressLeft()
    {
        bool Press = false;
        if (CanButton && 
            (Input.GetAxisRaw("Horizontal") < 0
            || Input.GetKey(playerInfo.LeftMoveKey)))
        {
            StartCoroutine(this.ResetButton());
            CanButton = false;
            Press = true;
        }
        return Press;
    }
    public bool PressUp()
    {
        bool Press = false;
        if (CanButton && 
            (Input.GetAxisRaw("Vertical") > 0
            || Input.GetKey(playerInfo.UpMoveKey)))
        {
            StartCoroutine(this.ResetButton());
            CanButton = false;
            Press = true;
        }
        return Press;
    }
    public bool PressDown()
    {
        bool Press = false;
        if(CanButton &&
            (Input.GetAxisRaw("Vertical") < 0
            || Input.GetKey(playerInfo.DownMoveKey)))
        {
            StartCoroutine(this.ResetButton());
            CanButton = false;
            Press = true;
        }
        return Press;
    }

    public bool PressOk()
    {
        return (Input.GetButtonDown("Submit"))
            || Input.GetKeyDown(playerInfo.SubmitKey);
    }
    public bool UpOK()
    {
        return (Input.GetButtonUp("Submit"))
            || Input.GetKeyUp(playerInfo.SubmitKey);
    }
    public bool PressNo()
    {
        return (Input.GetButtonDown("Cancel"))
            || Input.GetKeyDown(playerInfo.CancelKey);
    }
    public bool UpNo()
    {
        return (Input.GetButtonUp("Cancel"))
            || Input.GetKeyUp(playerInfo.CancelKey);
    }
    public bool PressMenu()
    {
        return (Input.GetButtonDown("Menu"))
            || Input.GetKeyDown(playerInfo.MenuKey);
    }
    public bool UpMenu()
    {
        return (Input.GetButtonUp("Menu"))
            || Input.GetKeyUp(playerInfo.MenuKey);
    }
    public bool PressJump()
    {
        return (Input.GetButtonDown("Jump"))
            || Input.GetKeyDown(playerInfo.JumpKey);
    }
    public bool UpJump()
    {
        return (Input.GetButtonUp("Jump"))
            || Input.GetKeyUp(playerInfo.JumpKey);
    }
    public bool PressDash()
    {
        return (Input.GetButtonDown("Dash"))
            || Input.GetKeyDown(playerInfo.DashKey);
    }
    public bool UpDash()
    {
        return (Input.GetButtonUp("Dash"))
            || Input.GetKeyUp(playerInfo.DashKey);
    }
    public bool ReleaseHorizontal()
    {
        return Input.GetAxisRaw("Horizontal") == 0
            || Input.GetKeyUp(playerInfo.RightMoveKey)
            || Input.GetKeyUp(playerInfo.LeftMoveKey);
    }
    public bool ReleaseVertical()
    {
        return Input.GetAxisRaw("Vertical") == 0
            || Input.GetKeyUp(playerInfo.UpMoveKey)
            || Input.GetKeyUp(playerInfo.DownMoveKey);
    }
    #endregion
    public void SetMoney(int money)
    {
        moneySys.PlusMoney(money);
    }
    public void SetCamPlayer()
    {
        cam.GetComponent<scrFollowCam>().SetPlayer();
    }
    public void ResetCamBoss()
    {
        cam.GetComponent<scrFollowCam>().SetResetBoss();
    }
    public void CamRelease(Vector3 pos) // 플레이어 공격, 피격 외에도 이벤트 연출을 위해 따로 빼낸다
    {
        cam.GetComponent<scrFollowCam>().PosSet = pos;
        cam.GetComponent<scrFollowCam>().SetReset();
    }
    // Update is called once per frame
    void Update()
    {
        if (!objPool.ExistsShop())
        {
            if (PressMenu())
            {
                if (CanMenu)
                {
                    CanMenu = false;
                    if (!objPause.activeSelf) objPause.SetActive(true);
                }
            }
            else if (UpMenu())
            {
                if (!CanMenu) CanMenu = true;
            }
        }
    }
    public void SaveData() //저장
    {
        PlayerPrefs.SetInt("Saved", 1);
        {
            playerInfo.posPlayer = trPlayer.position;
            playerInfo.strSceneName = SceneManager.GetActiveScene().name;
            string FileName = "/PlayerInfo.sav";

#if UNITY_ANDROID || UNITY_IOS
           string directoryPath = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ? Application.persistentDataPath : Application.dataPath);
            
            
            /*DirectoryInfo directoryPath = new DirectoryInfo(Application.dataPath+"/SaveFolder");

            if (!directoryPath.Exists)
            {
                directoryPath.Create();
            }

                File.Create(directoryPath + FileName).Close();*/
                File.WriteAllText(directoryPath + FileName, JsonUtility.ToJson(playerInfo));
#else //UNITY_EDITOR_WIN || UNITY_EDITOR_64 || UNITY_EDITOR || UNITY_STANDALONE_WIN

            DirectoryInfo directoryPath = new DirectoryInfo(Application.dataPath + "/SaveFolder");

            if (!directoryPath.Exists)
            {
                directoryPath.Create();
            }

            File.WriteAllText(directoryPath + FileName, JsonUtility.ToJson(playerInfo));

#endif 
            Debug.Log(directoryPath + " 저장 완료");
        }
    }

    public void LoadData() //불러오기 
    { 
        Saved = PlayerPrefs.GetInt("Saved");
        if (Saved == 1)
        {
            string FileName = "/PlayerInfo.sav";

#if UNITY_ANDROID|| UNITY_IOS

           string directoryPath = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ? Application.persistentDataPath : Application.dataPath);
                
            // DirectoryInfo directoryPath = new DirectoryInfo(Application.dataPath+"/SaveFolder");

                string str2 = File.ReadAllText(directoryPath + FileName);
                WWW www = new WWW(directoryPath);
                while (!www.isDone) { }
                string dataAsJson = www.text;
            playerInfo = JsonUtility.FromJson<PlayerInfo>(str2);

                transform.Find("Canvas").Find("pnlInfo").Find("objHpUI").GetComponent< scrPlayerHp >().SetInfo();
                sceneManager.ScName = playerInfo.strSceneName;
                sceneManager.pos = playerInfo.posPlayer;
                sceneManager.SceneMove();
                trPlayer.GetComponent<scrPlayerCtrl>().SetDie(false);
                GetComponent<scrBGMManager>().SetChangeBgm(playerInfo.BGMIndex);

#else //UNITY_EDITOR_WIN || UNITY_EDITOR_64 || UNITY_EDITOR || UNITY_STANDALONE_WIN

            DirectoryInfo directoryPath = new DirectoryInfo(Application.dataPath + "/SaveFolder");
            if (File.Exists(directoryPath + FileName))
            {
                string str2 = File.ReadAllText(directoryPath + FileName);
                playerInfo = JsonUtility.FromJson<PlayerInfo>(str2);

                sceneManager = GetComponent<scrSceneManager>();
                sceneManager.ScName = playerInfo.strSceneName;
                sceneManager.pos = playerInfo.posPlayer;
                sceneManager.SceneMove();
                transform.Find("Canvas").Find("pnlPlayerInfo").Find("objHpUI").GetComponent<scrPlayerInfo>().SetInfo();
                transform.Find("objPlayer").GetComponent<scrPlayerCtrl>().SetDie(false);
            }

#endif  
        }
    }
}
