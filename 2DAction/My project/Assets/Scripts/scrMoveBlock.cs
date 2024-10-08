using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrMoveBlock : MonoBehaviour
{
    [SerializeField] float Spd = 1;
    Transform trDirection;
    Transform trPos;
    [SerializeField] Transform trGoal;
    private void Awake()
    {
        trDirection = transform.Find("objDirection");
        trPos= trDirection.Find("objPos");
    }
    private void OnEnable()
    {
        SetRotation(trGoal);
    }
    private void Update()
    { 
        transform.Translate((trPos.position-transform.position) * Spd * Time.deltaTime);
    }
    void SetRotation(Transform tr)
    {
        float angle = 
            Mathf.Atan2(tr.position.y - transform.position.y, tr.position.x - transform.position.x) 
            * Mathf.Rad2Deg;
        trDirection.localEulerAngles = new Vector3(0, 0, angle );
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform==trGoal)
        {
            trGoal = collision.GetComponent<scrMoveBlockBounce>().trGoal;
            SetRotation(trGoal);
        }
    }
}
