using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class scrFakeBlock : MonoBehaviour
{
    Color col;
    SpriteShapeRenderer shape;
    // Start is called before the first frame update
    private void Awake()
    {
        shape = this.GetComponent<SpriteShapeRenderer>();
    }
    private void OnEnable()
    {
        col = shape.color;
        col.a = 1;
        shape.color = col;
    }
    IEnumerator ChangeCol()
    {
        int count = 0;
        while (count < 10)
        {
            shape.color = Color.Lerp(shape.color, col, 0.5f);
            count++;
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if(col.a == 1)
            {
                col.a = 0;
                StartCoroutine(this.ChangeCol());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if (col.a == 0)
            {
                col.a = 1;
                StartCoroutine(this.ChangeCol());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
