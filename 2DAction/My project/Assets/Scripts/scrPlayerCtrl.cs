using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrPlayerCtrl : MonoBehaviour
{
    enum State { Idle, Run, Jump, Attack, Sit, Dash, Slide, Fallen, Die};
    [SerializeField] State state = State.Idle;
    bool EnterEvnt = false;
    public bool CanDamage = true;
    bool CanThrough = false;
    bool CanSit = true;
    bool CanJump = true;
    bool CanDash = true;
    bool CanAttack = true;
    int AttackCombo = 0;
    int JumpCount = 0; 
    [SerializeField] float Spd = 5;
    [SerializeField] float JumpSpd = 16; //점프 높이  
    Vector3 MoveBlockDist;
    scrDemCheck dem;
    PlayerInfo playerInfo; 
    IEnumerator corResetDash;
    IEnumerator corAttack;
    IEnumerator corAttackEnd;
    Transform trMoveBlock;
    scrCharMove charMove;
    Animator ani;
    BoxCollider2D coll;
    Rigidbody2D rigid;
    scrObjPool objPool;
    Transform trSprite;
    GameManager manager;
    private void Awake()
    {
        manager = transform.root.GetComponent<GameManager>();
        objPool = manager.GetComponent<scrObjPool>();
        playerInfo = manager.playerInfo;
        charMove = GetComponent<scrCharMove>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        dem = GetComponent<scrDemCheck>();
        trSprite = this.transform.Find("Sprite");
        ani = trSprite.Find("Sprite").GetComponent<Animator>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isEventPlay)
        {///////////// 좌우 키를 누를 때 
         //if(Input.GetKey(KeyCode.RightArrow)) 
            if (CanDash && CanSit)
            {
                if (manager.MoveRight()) //오른쪽
                {
                    charMove.MoveSpd = Spd;
                    charMove.Flip(1);
                }
                else if (manager.MoveLeft()) //왼쪽
                {
                    charMove.MoveSpd = -Spd;
                    charMove.Flip(-1);
                }
                else if (manager.ReleaseHorizontal())//두 키 다 떼었을 때 
                {
                    if (CanDash) charMove.MoveSpd = 0;
                }
            }

            ///////////// 상하 키를 누를 때 
            if (manager.MoveDown()) // 아래쪽
            {
                if (CanDash) //대쉬 할 수 있으면
                {
                    if (charMove.isGround) //땅에 있을 때
                    {
                        if (CanSit)
                        {
                            Sit();
                            CanSit = false;
                        }
                    }
                    else if (rigid.velocity.y < 0) //낙하 중일때
                    {
                        AniChange(State.Fallen);
                        SetGravity(1);
                    }
                }
                else // 대쉬 할 수 없으면
                {
                    if (state == State.Dash)
                    {
                        AniChange(State.Idle);
                        CanDash = true;
                        charMove.MoveSpd = 0;
                        StopCoroutine(corResetDash);
                    }
                }
            }
            else if (manager.MoveUp()) //위쪽
            {
                if (charMove.isGround)
                {
                }
                else if (rigid.velocity.y < 0) //낙하 중일때
                {
                    AniChange(State.Fallen);
                    SetGravity(-1);
                }
            }
            else if (manager.ReleaseVertical())  //두 키 다 떼었을 때
            {
                if (!CanSit)
                {
                    CanSit = true;
                    SetSitColl(false);
                }
                SetGravity(0);
                AniChange(State.Fallen);
            }

            ///////////// 점프 키 
            if (!EnterEvnt)
            {
                if (manager.PressJump()) //점프 키를 눌렀을 때
                {
                    if (CanJump && JumpCount < playerInfo.MaxJump)
                    {
                        CanJump = false;
                        Jump();
                    }
                }
                else if (manager.UpJump())//점프 키를 떼었을 때
                {
                    if (JumpCount > 0 && rigid.velocity.y > 0)
                        rigid.velocity = new Vector2(charMove.MoveSpd, -1);
                    if (!CanJump) CanJump = true;
                }

            ///////////// 대쉬 키
            if (manager.PressDash()) //대쉬 키를 눌렀을 때
            {
                if (CanDash)
                {
                    CanDash = false;
                    if (CanSit)
                    {
                        Dash();
                    }
                    else
                    {
                        Slide();
                    }
                }
            }
                if (Input.GetButtonDown("Fire1"))
                {
                    Attack();
                }
            }
        }

        ///////////// 일반 물리 
        if (CanAttack && dem.CanDamage)//
        {
            if (charMove.MoveSpd == 0 && trMoveBlock == null)
            {
                rigid.constraints =
                RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            }
            else
            {
                if (CanDash)
                    rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                else
                {
                    rigid.constraints =
                    RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }
        else
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        ///////////// 이동 블록
        if (trMoveBlock != null)
        {
            if (charMove.MoveSpd == 0)
                transform.position = trMoveBlock.position - MoveBlockDist;
            else
                MoveBlockDist = trMoveBlock.position - transform.position;
        }

        ///////////// 애니메이션 변경
        if (dem.CanDamage)
        {
            AniCheck();
        }
    }
    void Dash()
    {
        AniChange(State.Dash);
        charMove.MoveSpd = charMove.Look * 18;
        corResetDash = this.ResetDashDelay();
         StartCoroutine(corResetDash);
    }

    public void SetDamage(bool die)
    {
        ani.SetBool("Damage", die); 
    }
    void Slide()
    {
        AniChange(State.Slide);
        charMove.MoveSpd = charMove.Look * 20;
        corResetDash = this.ResetDashDelay();
        StartCoroutine(corResetDash);
    }
    IEnumerator ResetDashDelay()
    {
        while (Abs(charMove.MoveSpd) > 10f)
        {
            if (charMove.MoveSpd > 0)
            {
                charMove.MoveSpd -= 0.6f;
            }
            else if (charMove.MoveSpd < 0)
            {
                charMove.MoveSpd += 0.6f;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        charMove.MoveSpd = 0;
        CanSit = true;
        ani.SetBool("Sit", false); 
        ani.SetBool("Dash", false);
        SetSitColl(false);
        yield return new WaitForSeconds(0.1f);
        SetSitColl(false);
        if (charMove.isGround) AniChange(State.Idle); 
        else AniChange(State.Fallen);
        CanDash = true;
    }
    void SetGravity(int Plus) //중력 변경
    {
        float var = 5;

        switch (Plus)
        {
            case -1: var = 1; break;
            case 0: var = 5; break;
            case 1: var = 12; break;
        }
        if (rigid.gravityScale != var) rigid.gravityScale = var;
    }
    public void Attack()
    {
        if (CanAttack)
        {
            CanAttack = false;
            corAttack = this.AttackDelay();
            corAttackEnd = this.ResetAttDelay();
            StopCoroutine(corAttackEnd);
            StartCoroutine(corAttack);
        }
    }
    IEnumerator AttackDelay()
    { 
        AttAni(true);
        ani.SetInteger("AttCombo", AttackCombo); 
        yield return new WaitForSeconds(0.3f);
        AttackCombo++;
        if (AttackCombo > 1) { AttackCombo = 0; }
        CanAttack = true;
        StartCoroutine(corAttackEnd);
    }
    IEnumerator ResetAttDelay()
    {
        yield return new WaitForSeconds((state == State.Run) ? 0.2f : 0.6f);
        ani.SetFloat("MoveSpd", charMove.MoveSpd > 0 ? 1 : 0);
        rigid.velocity = Vector2.down;
        if (CanAttack)
        { 
            AttAni(false);
            AttackCombo = 0;
        }
    }
    void AttAni(bool att)
    {
        ani.SetBool("Attack", att); 
    }

    void AniCheck() //애니메이션 변경
    {
        if (charMove.isGround)
        {
            if (Abs(charMove.MoveSpd) < 0.1f)
            {
                if (CanSit) AniChange(State.Idle);
                else AniChange(State.Sit);
            }
            else
            {
                if (CanDash)
                    AniChange(State.Run);
                else
                {
                    if (CanSit) AniChange(State.Dash);
                    else AniChange(State.Slide);
                }
                    
            }
        }
        else
        { 
            if (rigid.velocity.y <= -1)
            { AniChange(State.Fallen); }
        }
    }

    public void SetDie(bool die)
    {
        ani.SetBool("Die", die);
    }
    float Abs(float a) //절개값
    {
        return a < 0 ? -a : a;
    }

    void Sit() //앉는 함수
    {
        if (CanThrough) //통과하는 블록과 닿으면
        {
            coll.isTrigger = true;
            charMove.isGround = false;
            rigid.velocity = new Vector2(charMove.MoveSpd, -5);
        }
        else //닿지 않았으면
        { 
            if (state != State.Sit)
            {
                AniChange(State.Sit);
                SetSitColl(true);
            }
        }
    }
    void SetSitColl(bool sit) //앉아있을 때 판정박스 크기 변경
    {
        if(sit)
        {
            coll.offset = new Vector2(0, 0.5f);
            coll.size = new Vector2(0.6f, 0.8f);
        }
        else
        {
            coll.offset = new Vector2(0, 1.2f);
            coll.size = new Vector2(0.6f, 2.2f);
        }
    }

    void AniChange(State change) //상태 변경
    {
        if (state != change) state = change;
        switch (change)
        {
            case State.Idle:
                if (ani.GetBool("Jump") && ani.GetFloat("FallSpd") < 0)
                {
                    ani.SetBool("Jump", false); 
                    ani.SetFloat("FallSpd", 0); 
                }
                else
                {
                    if (CanAttack)
                    {
                        if (ani.GetBool("Sit"))
                        {
                            ani.SetBool("Sit", false); 
                        }
                    }
                    ani.SetFloat("MoveSpd", 0); 
                }
                break;
            case State.Run:
                if (CanAttack)
                {
                    if (ani.GetBool("Jump") && ani.GetFloat("FallSpd") < 0)
                    {
                        ani.SetBool("Jump", false); 
                        ani.SetFloat("FallSpd", 0); 
                    }
                    ani.SetFloat("MoveSpd", 1); 
                }

                break;
            case State.Slide:
            case State.Dash:
                if (!ani.GetBool("Dash"))
                {
                    ani.SetBool("Dash", true); 
                }
                break;
            case State.Jump:

                if (!ani.GetBool("Jump"))
                {
                    ani.SetBool("Jump", true); 
                    ani.SetFloat("FallSpd", 1); 
                }
                break;
            case State.Fallen: 
                if (!ani.GetBool("Jump"))
                {
                    ani.SetBool("Jump", true); 
                }
                ani.SetFloat("FallSpd", -1); 
                break;
            case State.Attack:
                if (!ani.GetBool("Attack"))
                {
                    ani.SetBool("Attack", true); 
                }
                break;
            case State.Die:
                if (!ani.GetBool("Die"))
                {
                    ani.SetBool("Die", true); 
                }
                break;
            case State.Sit:
                if (!ani.GetBool("Sit"))
                {
                    ani.SetBool("Sit", true); 
                }
                break;
        }
    }

    public void Jump() //점프 
    {
        AniChange(State.Jump);
        JumpCount++;
        float JSpd = JumpSpd;
        if(JumpCount>1) JSpd= JumpSpd *1.5f;
        rigid.AddForce(new Vector2(charMove.MoveSpd, JSpd), ForceMode2D.Impulse);
        charMove.isGround = false;
    }
    public void ColliderBlock()
    {
        if (ani.GetBool("Jump") && ani.GetFloat("FallSpd") < 0)
        {  
            charMove.isGround = true;
            if (JumpCount > 0) JumpCount = 0;
            ani.SetBool("Jump", false);
            rigid.velocity = new Vector2(0, -2);//---
            CanJump = true;
            SetGravity(0);
            AniChange(State.Idle);
        }
    }

    public void CallEvntBtn(bool Create, scrEventObj target)
    {
        EnterEvnt = Create;
        objPool.CreateEvntKey(Create, target);

    }
    private void OnTriggerEnter2D(Collider2D collision) // 영역충돌
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "ThroughBlock")
        { 
            if (coll.isTrigger) coll.isTrigger = false;
        }
        else if (collision.gameObject.tag == "Door")
        {
            collision.GetComponent<scrSceneMove>().SceneGoto();
        }
        else if (collision.gameObject.tag == "Exp")
        { 
            playerInfo.Exp++;
            Vector3 posEff = transform.position + (transform.position - collision.transform.position) / 2; 
            objPool.CreateGetEff(posEff);
            collision.gameObject.SetActive(false);
        }
    }
     
    private void OnCollisionEnter2D(Collision2D collision) //판정 충돌
    {
        if (collision.gameObject.tag == "Ground"|| collision.gameObject.tag=="ThroughBlock")
        {
            if (ani.GetBool("Jump") && ani.GetFloat("FallSpd") <= 0)
            {
                if (collision.gameObject.tag == "ThroughBlock") //통과 블록일 때
                { if (!CanThrough) CanThrough = true; }

            }
            if (collision.gameObject.GetComponent<scrMoveBlock>() != null) //이동 블록일때
            {
                if (trMoveBlock == null) trMoveBlock = collision.transform;
                MoveBlockDist = trMoveBlock.position - transform.position;
            }
            else if (trMoveBlock != null) trMoveBlock = null;
        }
        if (collision.gameObject.tag == "Money")
        {
            objPool.CreateMoneyEff(collision.contacts[0].point);
            manager.SetMoney(collision.transform.GetComponent<scrMoney>().Money); 
            collision.gameObject.SetActive(false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (collision.gameObject.GetComponent<scrMoveBlock>() != null) //이동 블록일때
            {
                if (trMoveBlock != null) trMoveBlock = null;
            }
        }
    }
}
