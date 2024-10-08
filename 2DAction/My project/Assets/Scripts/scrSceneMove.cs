using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSceneMove : MonoBehaviour
{
    [SerializeField] string ScName = "scHome";
    [SerializeField] Vector3 pos = Vector3.zero;

    public void SceneGoto()
    {
        scrSceneManager load = GameManager.instance.GetComponent<scrSceneManager>();
        load.ScName = ScName;
        load.pos = pos;
        load.SceneMove();
    }
}
