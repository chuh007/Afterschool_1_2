using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrEnemyCtrl : MonoBehaviour
{
    public enum AttPattern { Stay, Walk, Dash, Follow , Missile}
    public AttPattern ptrn = AttPattern.Stay;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool CanFlip = true;
    [SerializeField] int MissileMax = 10;
    [SerializeField] float Spd = 1f; //속도 변수  
    float EnemCheckDist = 1f; 
    Rigidbody2D rigid;
    scrDemCheck demChek;
    scrCharMove charMove;
    Animator ani;
    Transform trTarget;
    scrObjPool objPool;
    private void Awake()
    {
        demChek = GetComponent<scrDemCheck>();
        rigid = GetComponent<Rigidbody2D>();
        charMove = transform.GetComponent<scrCharMove>();
        ani = transform.Find("Sprite").Find("Sprite").GetComponent<Animator>();
    }
    private void OnEnable()
    {
        StartCoroutine(this.SetPattern());
    }
    IEnumerator SetPattern()
    {
        yield return new WaitForSeconds(0.1f);
        switch (ptrn)
        {
            case AttPattern.Stay:
                Spd = 0; 
                break;
            case AttPattern.Walk:
                break;
            case AttPattern.Dash:
                SetTarget();
                EnemCheckDist = 100; 
                break; 
            case AttPattern.Follow:
                SetTarget();
                EnemCheckDist = 300;
                break;
            case AttPattern.Missile:
                Spd = 0;
                StartCoroutine(ShootMissile());
                break;
        }
        SetSpd(Spd);//걷기
        yield return null;
    }

    IEnumerator ShootMissile()
    {
        if (objPool == null)
        objPool = GameManager.instance.GetComponent<scrObjPool>();
        float angle = 360 / MissileMax;
        while (true)
        {
            for (int i = 0; i < MissileMax; i++)
            {
                objPool.CreateMissile(transform.position, angle * i, this.transform);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(5f);
        }
        /*angle = 
            Mathf.Atan2(transform.position.y - (trTarget.position.y + 1.5f), transform.position.x - trTarget.position.x) 
            * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0, 0, angle + 90);
        objPool.CreateMissile(transform.position, angle);*/
    }
    void SetTarget()
    {
        if (trTarget == null) trTarget = GameManager.instance.transform.Find("objPlayer");
    }
    void Update()
    {
        if (trTarget != null)
        {
            if (Vector3.SqrMagnitude(trTarget.position - transform.position) <= EnemCheckDist)
            {
                if (canDash)
                {
                    canDash = false;
                    StartCoroutine(this.Dash());
                }
            }
        }

        if (demChek.CanDamage)
        {
            if (charMove.MoveSpd == 0)
            {
                ani.SetFloat("MoveSpd", 0);
                rigid.constraints =
                  RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                ani.SetFloat("MoveSpd", 1);
                rigid.constraints =
                  RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
    IEnumerator Dash()
    {
        if (transform.position.x < trTarget.position.x)
        { 
            charMove.Flip(1);
        }
        else
        { 
            charMove.Flip(-1);
        }
        ani.SetFloat("AniSpd", 0);
        ani.SetBool("Attack",true);
        SetSpd(0);//대기 
        yield return new WaitForSeconds(1);
        ani.SetFloat("AniSpd", 1);
        SetSpd(Spd * 4);//대기
        yield return new WaitForSeconds(3);
        ani.SetBool("Attack", false);
        SetSpd(0);//대기
        yield return new WaitForSeconds(0.5f);
        SetSpd(Spd);//대기
        canDash = true;
    }
    public void SetSpd(float Speed)
    {
        charMove.MoveSpd = Speed * charMove.Look;
    }
    public void CallLookChange()
    { 
        if (CanFlip) 
        {
            CanFlip = false;
            StartCoroutine(this.ChangeLook());
        }
    }
    IEnumerator ChangeLook()
    {
        charMove.MoveSpd = 0;
        yield return new WaitForSeconds(1); 
        charMove.Flip(-charMove.Look);
        charMove.MoveSpd = Spd * charMove.Look;

    }
}
