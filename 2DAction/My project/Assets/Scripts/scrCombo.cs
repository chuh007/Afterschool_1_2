using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrCombo : MonoBehaviour
{
    int Combo = 0;
    Text txtCombo;
    Animator ani;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        txtCombo = transform.Find("txtCombo").GetComponent<Text>();
    }
    public void PlusCombo()
    {
        StopAllCoroutines();
        if (Combo < 99) Combo++;
        txtCombo.text = Combo.ToString();
        ani.SetTrigger("Reset");
        StartCoroutine(this.ProgressCombo());
    }
    IEnumerator ProgressCombo()
    {
        yield return new WaitForSeconds(0.1f);
        ani.SetBool("Play", true);
        yield return new WaitForSeconds(3);
        ani.SetBool("Play", false);
        yield return new WaitForSeconds(0.5f);
        ResetCombo();
    }
    void ResetCombo()
    {
        Combo = 0;
        txtCombo.text = Combo.ToString();
    }
}
