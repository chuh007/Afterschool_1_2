using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scrSceneManager : MonoBehaviour
{
    public string ScName = "scTitle";
    public Vector3 pos = Vector3.zero;
    [SerializeField] Transform trPlayer;
    IEnumerator LoadScene(string sceneName)
    {
        if (sceneName != "scTitle")
        {
            //만약 타이틀 씬이 아닐때 씬 전환 효과를 넣는다.
        }
        yield return new WaitForSeconds(Time.deltaTime);

        if (sceneName != ScName)
        { SceneManager.LoadScene(sceneName, LoadSceneMode.Single); } 

        yield return new WaitForSeconds(Time.deltaTime);

        string strTmp = SceneManager.GetActiveScene().name;
        if (strTmp == "scTitle")
        {
            if (trPlayer.gameObject.activeSelf) trPlayer.gameObject.SetActive(false);
        }
        else
        {
            GetComponent<GameManager>().playerInfo.strSceneName = strTmp;
            if (!trPlayer.gameObject.activeSelf) { trPlayer.gameObject.SetActive(true); }
            trPlayer.position = pos;
            pos.z = -10;
            transform.Find("Main Camera").position = pos;
        }
    }

    public void SceneMove()
    {
        StartCoroutine(LoadScene(ScName));
    }
}
