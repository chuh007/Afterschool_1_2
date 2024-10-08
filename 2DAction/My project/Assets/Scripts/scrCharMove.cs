using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrCharMove : MonoBehaviour
{
    enum CharKind { Player, NPC, Enemy};
    [SerializeField] CharKind charKind = CharKind.Enemy;
    [SerializeField]bool isSlope = false;
    [HideInInspector] public bool isGround = true;
    [HideInInspector] public int Look = 1;
     public float MoveSpd = 0;
    [SerializeField] float GroundAngle = 0;
    float MaxSlopeAngle = 85;
    Vector2 Perp;
    scrEnemyCtrl enemCtrl;
    Transform trSprite;
    Transform trGround;
    Transform trFront;
    Rigidbody2D rigid;

    private void Awake()
    {
        trGround = transform.Find("objGroundChk");
        trSprite = this.transform.Find("Sprite");
        trFront = trSprite.Find("objFrontChk");
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    { 
        if (charKind == CharKind.Enemy)
        {
            enemCtrl = transform.GetComponent<scrEnemyCtrl>();
        }
    }
    private void Update()
    {
        RaycastHit2D hitGround = 
            Physics2D.Raycast(trGround.position, Vector2.down, 0.5f, 
            LayerMask.GetMask("Ground"));
        RaycastHit2D hitFront =
            Physics2D.Raycast(trFront.position, new Vector2(Look,0), 0.5f,
            LayerMask.GetMask("Ground"));

        if (hitGround || hitFront)
        {
            if(hitFront)
                SlopeCheck(hitFront);
            else
                SlopeCheck(hitGround);
        }

        if (charKind == CharKind.Player)
        {
            if (!isGround && hitGround)
            { GetComponent<scrPlayerCtrl>().ColliderBlock(); } 
        }


        if (charKind == CharKind.NPC)
        {
            if (MoveSpd == 0)
            {
                rigid.constraints =
                  RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rigid.constraints =
                  RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
    void FixedUpdate()
    {
        if (isGround)
        {
            if (isSlope)
            {
                if (GroundAngle < MaxSlopeAngle)
                {
                    rigid.velocity = Perp * MoveSpd * -1f;
                }
                else
                { rigid.velocity = new Vector2(MoveSpd, 0); }
            }
            else
            { rigid.velocity = new Vector2(MoveSpd, rigid.velocity.y); }
        }
        else //공중에 있을 때 
        {
            rigid.velocity = new Vector2(MoveSpd, rigid.velocity.y);
        }
    }
    void SlopeCheck(RaycastHit2D hit)
    {
        Perp = Vector2.Perpendicular(hit.normal).normalized;
        GroundAngle = Vector2.Angle(hit.normal, Vector2.up);
         
        if (GroundAngle <= 10 || GroundAngle >= 170) isSlope = false;
        else isSlope = true;

        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow);
        Debug.DrawLine(hit.point, hit.point + Perp, Color.green);
    }
    public void Flip(int Scale)
    { 
        if (trSprite != null)
        { 
            if (Look != Scale)
            {
                Look = Scale;
                trSprite.localScale = new Vector3(Scale, 1, 0); 
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BounceBlock") // 플레이어는 통과되도록 트리거로, 몬스터는 닿으면 코드를 실행한다 
        {
            if (charKind == CharKind.Enemy && enemCtrl.canDash)
            {
                enemCtrl.CallLookChange();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BounceBlock")
        {
            if (charKind == CharKind.Enemy && enemCtrl.canDash)
            {
                if (!enemCtrl.CanFlip) // 트리거를 빠져나갈때 방향을 바꿀 수 있는 변수 
                {
                    enemCtrl.CanFlip = true;
                }
            }
        }
    }
}
