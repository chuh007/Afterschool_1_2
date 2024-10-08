using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrMenu : MonoBehaviour
{
    int Sel = -1;
    [SerializeField] GameObject[] obj = new GameObject[3];
    GameManager manager;
    private void Awake()
    {
        manager = transform.parent.parent.GetComponent<GameManager>();
    }
    private void OnEnable()
    {
        Sel = -1;
        manager.isEventPlay = true; 
        OpenObj(0);
    }
    public void OpenObj(int Index)
    {
        if(Sel>=0)
            obj[Sel].SetActive(false);
        Sel = Index;
        obj[Sel].SetActive(true);
    }

    private void OnDisable()
    {
        if (Sel >= 0)
            obj[Sel].SetActive(false);
        manager.isEventPlay = false;
    }
}
