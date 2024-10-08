using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrItem : MonoBehaviour
{ 
    [SerializeField] Item item;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector3 posEff = transform.position + (transform.position - collision.transform.position) / 2;
            GameManager.instance.GetComponent<scrObjPool>().CreateGetEff(posEff);
            GameManager.instance.playerInfo.GetItem(item); 
            gameObject.SetActive(false);
        }
    }
}
