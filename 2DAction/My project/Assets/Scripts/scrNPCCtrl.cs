using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrNPCCtrl : MonoBehaviour
{
    [SerializeField] bool Move = false;
    scrCharMove charMove;
    Animator ani;
    private void Awake()
    {
        charMove = transform.GetComponent<scrCharMove>();
        ani = transform.Find("Sprite").Find("Sprite").GetComponent<Animator>();
    }
    private void OnEnable()
    {
        if (Move)
        { StartCoroutine(this.AutoMove()); }
    }

    IEnumerator AutoMove()
    {
        while (true)
        {
            charMove.Flip(-charMove.Look);
            ani.SetFloat("MoveSpd", 1);
            charMove.MoveSpd = charMove.Look * 1.5f;
            yield return new WaitForSeconds(3);
            ani.SetFloat("MoveSpd", 0);
            charMove.MoveSpd = 0; 
            yield return new WaitForSeconds(1);
        }
    }
}
