using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSaveBlock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.SaveData();
            gameObject.SetActive(false);
        }
    }
}
