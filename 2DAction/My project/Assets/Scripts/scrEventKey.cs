using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrEventKey : MonoBehaviour
{ 
    Image imgGuage; 
    scrEventObj trTarget;
    Text txt; 
    Camera cam;
    GameManager manager;
    private void Awake()
    {
        imgGuage = transform.Find("imgGuage").GetComponent<Image>();
        manager = transform.root.GetComponent<GameManager>(); 
        txt = transform.Find("Text").GetComponent<Text>();
        cam = manager.transform.Find("Main Camera").GetComponent<Camera>();
    } 
    private void OnEnable()
    {
        txt.text = manager.playerInfo.SubmitKey;
        imgGuage.fillAmount = 0;
    }
    public void SetTarget(scrEventObj tr)
    {
        trTarget = tr;
    }
    void Update()
    {
        if (trTarget != null)
        {
            transform.position =
                cam.WorldToScreenPoint(trTarget.transform.position);

            if (!manager.isEventPlay && !trTarget.Open)
            {
                if (Input.GetButton("Submit"))
                {
                    if (imgGuage.fillAmount < 1)
                    {
                        imgGuage.fillAmount += 0.01f;
                    }
                    else
                    {
                        imgGuage.fillAmount = 0;
                        trTarget.Open = true;
                    }
                }
                else
                {
                    if (imgGuage.fillAmount > 0)
                    {
                        imgGuage.fillAmount -= 0.02f;
                    }
                }
            }
        }
    }
}
