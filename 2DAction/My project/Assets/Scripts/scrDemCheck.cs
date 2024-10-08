using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrDemCheck : MonoBehaviour
{
    enum CharKind { Player, Enemy, Boss };
    [SerializeField] CharKind charKind = CharKind.Enemy;
    public bool CanDamage = true;
    [SerializeField] int MaxExp = 1;
    [SerializeField] float MaxHp = 3;
    float Hp = 1;
    float HpVar = 0;
    float Width;
    Vector3 posEff;
    scrCombo combo;
    scrCharMove charMove;
    [SerializeField] Image imgHp;
    [SerializeField] Image imgDem;
    PlayerInfo playerInfo;
    Transform trDem;
    Transform trHpEnd;
    Animator ani;
    scrObjPool objPool;
    GameManager manager;
    [SerializeField] GameObject objCanvas;
    private void Awake()
    { 
        charMove = GetComponent<scrCharMove>();
        ani = transform.Find("Sprite").Find("Sprite").GetComponent<Animator>();
        if (charKind == CharKind.Player)
        { }
    }
    void OnEnable()
    {
        CanDamage = true;
        switch (charKind)
        {
            case CharKind.Player:
                break;
            case CharKind.Enemy:
                Hp = MaxHp;
                Width = (imgHp.GetComponent<RectTransform>().sizeDelta.x);
                trHpEnd = imgHp.transform.Find("imgEnd");
                imgHp.fillAmount = 1;
                objCanvas.SetActive(false);
                break;
            case CharKind.Boss:
                Width = (imgHp.GetComponent<RectTransform>().sizeDelta.x);
                trHpEnd = imgHp.transform.Find("imgEnd");
                StartCoroutine(this.setBossHp());
                break;
        }
        if (charKind != CharKind.Player)
        {
            StartCoroutine(this.CheckHP());
        }
    }
    
    IEnumerator setBossHp()
    {
        Hp = MaxHp;
        imgHp.fillAmount = 0; 
        imgDem.fillAmount = imgHp.fillAmount;
        while(imgHp.fillAmount<1)
        {
            imgHp.fillAmount += 0.01f;
            trHpEnd.localPosition =
                new Vector3((Width * -0.5f) + (Width * imgHp.fillAmount), 0, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        imgHp.fillAmount = 1;
        imgDem.fillAmount = imgHp.fillAmount;
    }
    IEnumerator CheckHP()
    { 
        while (true)
        {
            if (imgDem.fillAmount > imgHp.fillAmount)
            {
                imgDem.fillAmount -= HpVar;
                if (imgDem.fillAmount <= 0)
                {
                    if (charKind == CharKind.Boss)
                    {
                        objCanvas.transform.parent.Find("pnlVictory").gameObject.SetActive(true);
                        objPool.BossDestroyEff(transform.position + new Vector3(0, 1, 0));
                    }
                    transform.parent.gameObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        } 
    }
    IEnumerator ResetDamage()
    {
        float Spd = 0;
        float Delay = 0.1f;
        if (manager == null) manager = GameManager.instance;
        if (objPool == null) objPool = manager.GetComponent<scrObjPool>();
        
        playerInfo = manager.playerInfo; 
        if (!manager.isEventPlay)
        { 
            if (charKind == CharKind.Player)
            { 
                if (playerInfo.Hp > 0)
                {
                    objPool.CreateDemEff(posEff);
                    int DemVar = 1;
                    if(trDem!=null) DemVar = trDem.GetComponent<scrDem>().DemVar;
                    playerInfo.Hp-= DemVar;
                    Delay = 0.5f;
                    GetComponent<scrPlayerCtrl>().SetDamage(true);
                    GetComponent<scrPlayerCtrl>().CanDamage = false;
                    if (playerInfo.Hp <= 0)
                    {
                        charMove.MoveSpd = 0;
                           GetComponent<scrPlayerCtrl>().SetDie(true); 
                        manager.isEventPlay = true;
                        objPool.CreateGameOver();
                        CanDamage = true;
                    }
                }
            }
            else
            {
                objPool.CreateAttEff(posEff);
                if (!objCanvas.activeSelf)
                {
                    objCanvas.SetActive(true);
                }
                if (combo == null)
                    combo = manager.transform.Find("Canvas").Find("pnlPlayerInfo").Find("objCombo").
                        GetComponent<scrCombo>();
                combo.PlusCombo();
                if (Hp > 0)
                { 
                    Hp -= playerInfo.Pow;
                    Delay = 0.3f;
                }
                if (Hp<=0) transform.parent.gameObject.SetActive(false); 
                ani.SetBool("Damage", true);
                imgHp.fillAmount = (Hp / MaxHp);
                trHpEnd.localPosition =
                    new Vector3((Width * -0.5f) + (Width * imgHp.fillAmount), 0, 0);
                HpVar = (imgDem.fillAmount - imgHp.fillAmount) * 0.05f;
                if (charKind == CharKind.Enemy)
                {
                    Spd = charMove.MoveSpd;
                    charMove.MoveSpd = 0;
                }
            } 
            yield return new WaitForSeconds(Delay);
            if (ani != null)
            {
                ani.SetBool("Damage", false);
                if (charKind == CharKind.Player)
                { }

            }
            if (charKind == CharKind.Enemy)
                { charMove.MoveSpd = Spd; }
                CanDamage = true;
            if (charKind == CharKind.Player)
            {
                yield return new WaitForSeconds(0.3f);
                GetComponent<scrPlayerCtrl>().CanDamage = true;
            } 
        }
        else yield return null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    { 
        if (collision.gameObject.tag == "Trap")
        {
            if (charKind == CharKind.Player)
            {
                if (GetComponent<scrPlayerCtrl>().CanDamage)
                {
                    trDem = collision.transform;
                    CanDamage = false;
                    posEff = transform.position + (transform.position - collision.transform.position) / 2;
                    StartCoroutine(this.ResetDamage());
                }
            }
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (charKind == CharKind.Player)
            {
                if (GetComponent<scrPlayerCtrl>().CanDamage)
                {
                    trDem = collision.transform;
                      CanDamage = false;
                    posEff = transform.position + (transform.position-collision.transform.position)/2;
                    StartCoroutine(this.ResetDamage());

                }
            }
        }
        else if (collision.gameObject.tag == "Weapon")
        {
            if (charKind != CharKind.Player)
            {
                if (CanDamage)
                {
                    CanDamage = false;
                    posEff = transform.position + (transform.position - collision.transform.position) / 2;
                    StartCoroutine(this.ResetDamage());
                    if(collision.gameObject.name== "objMissile")
                    { collision.gameObject.SetActive(false); }
                }
            }
        }
    }
    private void OnDisable()
    {
        if (charKind != CharKind.Player)
        {
            if (Hp <= 0)
            {
                if (manager == null) manager = GameManager.instance;
                if (objPool == null) objPool = manager.GetComponent<scrObjPool>();
                for (int i=0; i< MaxExp; i++)
                {
                    objPool.CreateExp(transform.position);
                } 
            }
        }
    }

}
