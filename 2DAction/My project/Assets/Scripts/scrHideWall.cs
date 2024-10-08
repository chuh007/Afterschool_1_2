using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class scrHideWall : MonoBehaviour
{
    Color col;
    SpriteShapeRenderer shape;
    private void Awake()
    {
        shape = GetComponent<SpriteShapeRenderer>();
    }
    private void OnEnable()
    {
        col = shape.color;
        col.a = 1;
        shape.color = col;
    }
    IEnumerator ChangeCol()
    {
        int Count = 0;
        while (Count < 10)
        {
            shape.color = Color.Lerp(shape.color, col, 0.1f);
            Count++;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        shape.color = col;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (col.a == 1)
            {
                col.a = 0;
                StartCoroutine(this.ChangeCol());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (col.a == 0)
            {
                col.a = 1;
                StartCoroutine(this.ChangeCol());
            }
        }
    }
}
