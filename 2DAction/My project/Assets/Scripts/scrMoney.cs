using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrMoney : MonoBehaviour
{
    public int Money = 1;
    CircleCollider2D coll;
    Rigidbody2D rigid;
    Transform trGroundCheck;
    private void Awake()
    {
        trGroundCheck = transform.Find("objGroundChk");
        coll = GetComponent<CircleCollider2D>();
        rigid= GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        rigid.AddForce(new Vector2(Random.Range(-3f, 3f), 6), ForceMode2D.Impulse);
        StartCoroutine(this.ChkGround());  
    }
    IEnumerator ChkGround()
    {
        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(trGroundCheck.position, Vector2.down, 0.5f, 
                LayerMask.GetMask("Ground"));
            if (hit)
            {
                if (coll.isTrigger)
                {
                    coll.isTrigger = false;
                    break;
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);

        }
    } 
    private void OnDisable()
    {
        coll.isTrigger = true;
    }
}
