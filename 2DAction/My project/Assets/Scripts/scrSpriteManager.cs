using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSpriteManager : MonoBehaviour
{
    public Sprite[] sprItem;
    private void OnEnable()
    {
        sprItem = Resources.LoadAll<Sprite>("Sprites/Item");
    }
}
