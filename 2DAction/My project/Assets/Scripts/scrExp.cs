using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrExp : MonoBehaviour
{
    [SerializeField]bool Follow = false;
    float angle = 0;
    float Spd = 5;
    Transform trPlayer;
    private void OnEnable()
    {
        StartCoroutine(this.Create());
    }
    IEnumerator Create()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        if (trPlayer == null) trPlayer = GameManager.instance.transform.Find("objPlayer"); 
        while (Spd > 0.5f)
        {
            Spd -= 0.1f;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(0.1f);
        tag = "Exp";
        Follow = true;
        Spd = 6;

    }
    private void Update()
    {
        if (Follow)
        {
            angle = Mathf.Atan2
                (transform.position.y - (trPlayer.position.y + 1.5f), transform.position.x - trPlayer.position.x) 
                * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0, 0, angle + 90);
        }
        transform.Translate(Vector2.up * Spd * Time.deltaTime);
    }
}
