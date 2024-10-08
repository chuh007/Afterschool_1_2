using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class scrDepth : MonoBehaviour
{
    Transform tr;
    SortingGroup layer;
    private void Awake()
    {
        tr = transform;
        layer = tr.GetComponent<SortingGroup>();
    }
    private void OnEnable()
    {
        layer.sortingOrder = -Mathf.RoundToInt(tr.localPosition.y * 10);
    }
}
