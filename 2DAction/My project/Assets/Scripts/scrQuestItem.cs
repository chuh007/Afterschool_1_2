using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrQuestItem : MonoBehaviour
{
    public int QuestNum = 0;
    scrQuestSys sys;
    public void Detail()
    {
        if (sys == null) sys = 
                transform.parent.parent.parent.parent.GetComponent<scrQuestSys>();
        sys.SetDetail(QuestNum);
    }
}
