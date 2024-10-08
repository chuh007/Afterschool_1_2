using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrMissile : MonoBehaviour
{
    [SerializeField]float Spd = 2;
    public Transform trCreate;

    void Update()
    { 
        transform.Translate(Vector2.up * Spd * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Weapon")
        {
            if (trCreate != null && gameObject.tag=="Trap")
            {
                gameObject.tag = "Weapon";
                float Direction = Mathf.Atan2
                    (transform.position.y - trCreate.position.y, transform.position.x - trCreate.position.x) 
                    * Mathf.Rad2Deg;
                transform.localEulerAngles = new Vector3(0, 0, Direction+90); 
            }
        }
    }
}
