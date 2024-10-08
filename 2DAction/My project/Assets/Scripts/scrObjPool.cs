using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrObjPool : MonoBehaviour
{
    int TalkCount = 0, MaxTalk = 1;
    int MissileCount = 0, MaxMissile = 20;
    int ExpMax = 10, ExpCount = 0;
    int SoundMax = 10,SoundCount = 0;
    int MoneyMax = 10,MoneyCount = 0;
    int MoneyEffMax = 10, MoneyEffCount = 0;
    int AttEffMax = 5, AttEffCount = 0;
    int DemEffMax = 3, DemEffCount = 0;
    int GetEffMax = 10, GetEffCount = 0;
    Camera cam;
    Transform trPrefabFolder;
    Transform trUIPrefabFolder;
    List<GameObject> objMissileList = new List<GameObject>();
    List<GameObject> objMoneyList = new List<GameObject>();
    List<GameObject> objTalkList = new List<GameObject>();
    List<GameObject> objExpList = new List<GameObject>();
    List<GameObject> objSoundList = new List<GameObject>();
    List<GameObject> objGetEffList = new List<GameObject>();
    List<GameObject> objAttEffList = new List<GameObject>();
    List<GameObject> objDemEffList = new List<GameObject>();
    List<GameObject> objMoneyEffList = new List<GameObject>();

    private void Awake()
    {
        cam = transform.Find("Main Camera").GetComponent<Camera>();
        trUIPrefabFolder = transform.Find("Canvas").Find("objPrefabFolder");
        trPrefabFolder = transform.Find("objPrefabFolder");
    }
    public void CreateGetEff(Vector3 pos)
    {
        if (objGetEffList.Count < GetEffMax)
        {
            GameObject objPrefab = Instantiate(Resources.Load("Prefabs/Eff/ptcItemGetEff", typeof(GameObject)) as GameObject); //생성한 것을 리스트에 넣는다
            objGetEffList.Add(objPrefab);
            objGetEffList[GetEffCount].transform.SetParent(trPrefabFolder);
        }
        else
        {
            if (GetEffCount >= GetEffMax) GetEffCount = 0; // 갯수가 맥스보다 크면 인텍스 0으로  
            if (!objGetEffList[GetEffCount].activeSelf) objGetEffList[GetEffCount].SetActive(true);
        }
        objGetEffList[GetEffCount].transform.position = pos;// 해당 위치에 위치시키고  
        GetEffCount++; // 카운트를 늘린다 
    }
    public void CreateAttEff(Vector3 pos)
    {
        if (objAttEffList.Count < AttEffMax)
        {
            GameObject objPrefab = Instantiate(Resources.Load("Prefabs/Eff/ptclAttEff", typeof(GameObject)) as GameObject); //생성한 것을 리스트에 넣는다
            objAttEffList.Add(objPrefab);
            objAttEffList[AttEffCount].transform.SetParent(trPrefabFolder);
        }
        else
        {
            if (AttEffCount >= AttEffMax) AttEffCount = 0; // 갯수가 맥스보다 크면 인텍스 0으로  
            if (!objAttEffList[AttEffCount].activeSelf) objAttEffList[AttEffCount].SetActive(true);
        }
        objAttEffList[AttEffCount].transform.position = pos;// 해당 위치에 위치시키고  
        AttEffCount++; // 카운트를 늘린다 
    }
    public void BossDestroyEff(Vector3 pos)
    {
        if (trPrefabFolder.Find("ptclBossDestroy"))
        {
            trPrefabFolder.Find("ptclBossDestroy").gameObject.SetActive(true);
        }
        else
        {
            GameObject objPrefab = Instantiate(Resources.Load("Prefabs/Eff/ptclBossDestroy", typeof(GameObject)) as GameObject); //생성한 것을 리스트에 넣는다
            objPrefab.name = "ptclBossDestroy";
            objPrefab.transform.SetParent(trPrefabFolder);
        }
        trPrefabFolder.Find("ptclBossDestroy").transform.position = pos;// 해당 위치에 위치시키고   
    }
    public void CreateDemEff(Vector3 pos)
    {
        if (objDemEffList.Count < DemEffMax)
        {
            GameObject objPrefab = Instantiate(Resources.Load("Prefabs/Eff/ptclDemEff", typeof(GameObject)) as GameObject); //생성한 것을 리스트에 넣는다
            objDemEffList.Add(objPrefab);
            objDemEffList[DemEffCount].transform.SetParent(trPrefabFolder);
        }
        else
        {
            if (DemEffCount >= DemEffMax) DemEffCount = 0; // 갯수가 맥스보다 크면 인텍스 0으로  
            if (!objDemEffList[DemEffCount].activeSelf) objDemEffList[DemEffCount].SetActive(true);
        }
        objDemEffList[DemEffCount].transform.position = pos;// 해당 위치에 위치시키고  
        DemEffCount++; // 카운트를 늘린다 
    }
    public void CreateMoneyEff(Vector3 pos)
    {
        if (objMoneyEffList.Count < MoneyEffMax)
        {
            GameObject objPrefab = Instantiate(Resources.Load("Prefabs/Eff/ptclCoinGetEff", typeof(GameObject)) as GameObject); //생성한 것을 리스트에 넣는다
            objMoneyEffList.Add(objPrefab);
            objMoneyEffList[MoneyEffCount].transform.SetParent(trPrefabFolder);
        }
        else
        {
            if (MoneyEffCount >= MoneyEffMax) MoneyEffCount = 0; // 갯수가 맥스보다 크면 인텍스 0으로  
            if (!objMoneyEffList[MoneyEffCount].activeSelf) objMoneyEffList[MoneyEffCount].SetActive(true);
        }
        objMoneyEffList[MoneyEffCount].transform.position = pos;// 해당 위치에 위치시키고  
        MoneyEffCount++; // 카운트를 늘린다 
    }
    public void CreateSound(Vector3 pos, AudioClip clip)
    {
        if (objSoundList.Count < SoundMax)
        {
            GameObject objPrefab = 
                Instantiate(Resources.Load("Prefabs/objSound", typeof(GameObject)) as GameObject);  
            objSoundList.Add(objPrefab);
            objSoundList[SoundCount].transform.SetParent(trPrefabFolder);
        }
        else
        {
            if (SoundCount >= SoundMax) SoundCount = 0; // 갯수가 맥스보다 크면 인텍스 0으로  
            if (!objSoundList[SoundCount].activeSelf) objSoundList[SoundCount].SetActive(true);
        }
        objSoundList[SoundCount].transform.position = pos;// 해당 위치에 위치시키고   
        objSoundList[SoundCount].GetComponent<scrSoundObj>().SetAudioClip(clip);
        SoundCount++; // 카운트를 늘린다 
    }
    public void CreateExp(Vector3 pos)
    {
        if (objExpList.Count < ExpMax)
        {
            GameObject objPrefab = 
                Instantiate(Resources.Load("Prefabs/objExp", typeof(GameObject)) as GameObject);  
            objExpList.Add(objPrefab);
            objExpList[ExpCount].transform.SetParent(trPrefabFolder);
        }
        else
        {
            if (ExpCount >= ExpMax) ExpCount = 0; // 갯수가 맥스보다 크면 인텍스 0으로  
            if (!objExpList[ExpCount].activeSelf) objExpList[ExpCount].SetActive(true);
        }
        objExpList[ExpCount].transform.position = pos;// 해당 위치에 위치시키고
        objExpList[ExpCount].transform.localEulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
        ExpCount++; // 카운트를 늘린다 
    }
    public void CreateMoney(Vector3 pos)
    {
        if (objMoneyList.Count < MoneyMax)
        {
            GameObject objPrefab =
                Instantiate(Resources.Load("Prefabs/objMoney", typeof(GameObject)) as GameObject); //생성한 것을 리스트에 넣는다
            objMoneyList.Add(objPrefab);
            objMoneyList[MoneyCount].transform.SetParent(trPrefabFolder);
        }
        else
        {
            if (MoneyCount >= MoneyMax) MoneyCount = 0; // 갯수가 맥스보다 크면 인텍스 0으로  
            if (!objMoneyList[MoneyCount].activeSelf) objMoneyList[MoneyCount].SetActive(true);
        }
        objMoneyList[MoneyCount].transform.position = pos;
        MoneyCount++; // 카운트를 늘린다 
    }
    public void CreateMissile(Vector3 pos, float Direction, Transform trCreate)
    {
        if (objMissileList.Count < MaxMissile)
        {
            GameObject objPrefab =
                Instantiate(Resources.Load("Prefabs/objMissile", typeof(GameObject)) as GameObject); //생성한 것을 리스트에 넣는다
            objMissileList.Add(objPrefab);
            objMissileList[MissileCount].transform.SetParent(trPrefabFolder);
        }
        else
        {
            if (MissileCount >= MaxMissile) MissileCount = 0; // 갯수가 맥스보다 크면 인텍스 0으로  
            if (!objMissileList[MissileCount].activeSelf) objMissileList[MissileCount].SetActive(true);
        }
        objMissileList[MissileCount].name = "objMissile";
        objMissileList[MissileCount].GetComponent<scrMissile>().trCreate = trCreate;
        objMissileList[MissileCount].transform.position = pos;
        objMissileList[MissileCount].transform.localEulerAngles = new Vector3(0, 0, Direction);
        MissileCount++; // 카운트를 늘린다 
    }
    public void CreateTalk(Vector3 pos,string str)
    {
        if (objTalkList.Count < MaxTalk)
        {
            GameObject objPrefab =
                Instantiate(Resources.Load("Prefabs/imgTalk", typeof(GameObject)) as GameObject);  
            objTalkList.Add(objPrefab);
            objTalkList[TalkCount].transform.SetParent(trUIPrefabFolder, true);
            objTalkList[TalkCount].transform.localScale = Vector3.one;
        }
        else
        {
            if (TalkCount >= MaxTalk) TalkCount = 0; // 갯수가 맥스보다 크면 인텍스 0으로  
            if (!objTalkList[TalkCount].activeSelf) objTalkList[TalkCount].SetActive(true);
        } 
        objTalkList[TalkCount].transform.position = cam.WorldToScreenPoint(pos);
        objTalkList[TalkCount].GetComponent<scrTalkBox>().Sett(str);
        TalkCount++; // 카운트를 늘린다 
    }
    public bool ExistTalk()
    {
        return transform.Find("Canvas").Find("pnlTalkBox").gameObject.activeSelf;
    }
    public void CreateTalk(CharKind[] talk, string[] str, Quest quest)
    {
        Transform trTalkBox = transform.Find("Canvas").Find("pnlTalkBox");
        trTalkBox.GetComponent<scrTalkSys>().Set(talk, str);
        trTalkBox.gameObject.SetActive(true);
        trTalkBox.GetComponent<scrTalkSys>().quest = quest;
    } 
    public void CreateGameOver()
    {
        Transform trBtn = transform.Find("Canvas").Find("pnlGameOver");
        trBtn.gameObject.SetActive(true);
    }
    public bool ExistsShop()
    {
        Transform trBtn = transform.Find("Canvas").Find("pnlShop"); 
        return trBtn.gameObject.activeSelf;
    }
    public void CreateShop()
    {
        Transform trBtn = transform.Find("Canvas").Find("pnlShop");
        trBtn.gameObject.SetActive(true);
    }
    public bool ExistEvntKey()
    {
        return transform.Find("Canvas").Find("pnlPlayerInfo").Find("imgEvntBtn").gameObject.activeSelf;
    }
    public void CreateEvntKey(bool Show, scrEventObj trTarget = null)
    {
        Transform trBtn = transform.Find("Canvas").Find("pnlPlayerInfo").Find("imgEvntBtn");
        trBtn.GetComponent<scrEventKey>().SetTarget(trTarget);
        trBtn.gameObject.SetActive(Show);
    }
    private void OnDisable()// 게임이 꺼질때 리스트 리셋 
    {
        GameObject tmp;
        if (objMoneyList.Count > 0)
        {
            for (int i = 0; i < objMoneyList.Count; i++)
            {
                tmp = objMoneyList[i];
                objMoneyList.Remove(tmp);
                Destroy(tmp);
            }
        }
        if (objTalkList.Count > 0)
        {
            for (int i = 0; i < objTalkList.Count; i++)
            {
                tmp = objTalkList[i];
                objTalkList.Remove(tmp);
                Destroy(tmp);
            }
        }
        if (objMissileList.Count > 0)
        {
            for (int i = 0; i < objMissileList.Count; i++)
            {
                tmp = objMissileList[i];
                objMissileList.Remove(tmp);
                Destroy(tmp);
            }
        }
        if (objExpList.Count > 0)
        {
            for (int i = 0; i < objExpList.Count; i++)
            {
                tmp = objExpList[i];
                objExpList.Remove(tmp);
                Destroy(tmp);
            }
        }
        if (objSoundList.Count > 0)
        {
            for (int i = 0; i < objSoundList.Count; i++)
            {
                tmp = objSoundList[i];
                objSoundList.Remove(tmp);
                Destroy(tmp);
            }
        }
        if (objMoneyEffList.Count > 0)
        {
            for (int i = 0; i < objMoneyEffList.Count; i++)
            {
                tmp = objMoneyEffList[i];
                objMoneyEffList.Remove(tmp);
                Destroy(tmp);
            }
        }
        if (objDemEffList.Count > 0)
        {
            for (int i = 0; i < objDemEffList.Count; i++)
            {
                tmp = objDemEffList[i];
                objDemEffList.Remove(tmp);
                Destroy(tmp);
            }
        }
        if (objAttEffList.Count > 0)
        {
            for (int i = 0; i < objAttEffList.Count; i++)
            {
                tmp = objAttEffList[i];
                objAttEffList.Remove(tmp);
                Destroy(tmp);
            }
        }
        if (objGetEffList.Count > 0)
        {
            for (int i = 0; i < objGetEffList.Count; i++)
            {
                tmp = objGetEffList[i];
                objGetEffList.Remove(tmp);
                Destroy(tmp);
            }
        }
    }
}
