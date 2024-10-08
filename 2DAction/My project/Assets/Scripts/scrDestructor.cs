using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDestructor : MonoBehaviour
{
    [SerializeField] bool isDestory = false;
    public float Delay = 0;
    private void OnEnable()
    {
        if (Delay > 0)
        {
            if (isDestory) { Destroy(); }
            else Disable();
        }
    }
    public void Destroy()
    {
        if (Delay == 0) Destroy(gameObject);
        else Destroy(gameObject, Delay);
    }
    public void Disable()
    {
        if (Delay == 0) gameObject.SetActive(false);
        else StartCoroutine(this.SetDisable(Delay));
    }

    IEnumerator SetDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
