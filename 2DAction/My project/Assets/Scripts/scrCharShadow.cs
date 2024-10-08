using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrCharShadow : MonoBehaviour
{
    [SerializeField] float PlusPos = 0.25f;
    Vector3 pos;
    SpriteRenderer spr;
    Transform trParent;
    RaycastHit2D hitGround;
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        trParent = transform.parent.parent;
    }
    // Update is called once per frame
    void Update()
    {
        hitGround = Physics2D.Raycast(trParent.position, Vector2.down, 10, LayerMask.GetMask("Block"));
        if (hitGround)
        {
            Vector3 collPos = hitGround.point;
            pos = new Vector3(trParent.position.x, collPos.y + PlusPos);
            transform.position = pos;
            float dist = Vector3.SqrMagnitude(trParent.position - collPos);
            transform.localScale = (Vector3.one * 1.2f) * (1 - (dist / 40));
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
    }
}
