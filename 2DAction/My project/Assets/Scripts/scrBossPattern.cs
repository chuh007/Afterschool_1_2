using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrBossPattern : MonoBehaviour
{
    enum AttPtrn { Idle, Attack, Dash, Jump, }
    AttPtrn curPtrn = AttPtrn.Idle;
    [SerializeField] AttPtrn[] allAttPtrn;//다른 보스들도 이전 보스의 패턴을 응용할 수 있기에 패턴 이름을 공용으로
    int AttPtrnIndex = 0;
    int AttPtrnMax = 1;
    scrCharMove charMove;
    Rigidbody2D rigid;
    SpriteRenderer[] spr = new SpriteRenderer[0];
    Transform trTarget;
    Animator ani;
    scrObjPool objPool;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = transform.Find("Sprite").Find("Sprite").GetComponent<Animator>();
        charMove = GetComponent<scrCharMove>();
    }

    private void OnEnable()
    {
        AttPtrnMax = allAttPtrn.Length;
        AttPtrnIndex = 0;
        charMove.Flip(charMove.Look);
    }
    void ChangeColor(bool Ready)
    { 
        if (spr.Length == 0)
        {
            spr = ani.GetComponentsInChildren<SpriteRenderer>();
        }
        for(int i=0;i<spr.Length; i++)
        {
            spr[i].color = Ready ? Color.red : Color.white;
        } 
    }

    public IEnumerator PlayAttPtrn(float Delay = 0.5f)
    {// 패턴 설정 
        if (objPool == null) objPool = GameManager.instance.GetComponent<scrObjPool>();
        if (trTarget == null) trTarget = objPool.transform.Find("objPlayer");
        curPtrn = AttPtrn.Idle;
        StartCoroutine(this.Ptrn());

        yield return new WaitForSeconds(Delay);
        curPtrn = allAttPtrn[AttPtrnIndex];
        AttPtrnIndex++;
        if (AttPtrnIndex >= AttPtrnMax) AttPtrnIndex = 0;
        StartCoroutine(this.Ptrn());
    }
    IEnumerator Ptrn()
    {
        // 패턴 적용
        switch (curPtrn)
        {
            case AttPtrn.Idle:
                SetSpd(0);
                break;
            case AttPtrn.Attack:
                LookPlayer();
                ani.SetFloat("AniSpd", 0);
                ani.SetBool("Attack", true);
                ChangeColor(true);

                yield return new WaitForSeconds(2f);
                ChangeColor(false);
                ani.SetFloat("AniSpd", 1);
                SetSpd(2);

                yield return new WaitForSeconds(1f);
                SetSpd(0);
                ani.SetBool("Attack", false);
                StartCoroutine(this.PlayAttPtrn(2));
                break;
            case AttPtrn.Dash:
                LookPlayer(); 
                ani.SetFloat("AniSpd", 0);
                ani.SetBool("Dash", true);
                ChangeColor(true);

                yield return new WaitForSeconds(2f);
                ChangeColor(false);
                ani.SetFloat("AniSpd", 1);
                SetSpd(4);

                yield return new WaitForSeconds(2f);
                SetSpd(0);
                ani.SetBool("Dash", false);
                StartCoroutine(this.PlayAttPtrn(2));
                break;
            case AttPtrn.Jump:
                LookPlayer(); 
                ani.SetFloat("AniSpd", 0);
                ani.SetBool("Jump", true);
                ChangeColor(true);

                yield return new WaitForSeconds(2f);
                ChangeColor(false);
                ani.SetFloat("AniSpd", 1);
                SetSpd(3);
                rigid.AddForce(new Vector2(charMove.MoveSpd, 30), ForceMode2D.Impulse);

                yield return new WaitForSeconds(2f);
                SetSpd(0);
                ani.SetBool("Jump", false);
                StartCoroutine(this.PlayAttPtrn(2f));
                break;
        }
        yield return null;
    }
    public void LookPlayer()
    {
        if (transform.position.x < trTarget.position.x)
            { charMove.Flip(1); }
        else { charMove.Flip(-1); }
        
    }
    public void SetSpd(float Speed)
    {
        charMove.MoveSpd = Speed * charMove.Look;
    }
}
