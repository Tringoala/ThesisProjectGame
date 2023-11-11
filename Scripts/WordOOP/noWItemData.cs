using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class noWItemData
{
    public int id;
    public string wordTR;
    public string wordEn;
    public string desc;
    public noWItemData(noWItemData w)
    {
        id = w.id;
        wordTR = w.wordTR;
        wordEn = w.wordEn;
        desc = w.desc;
    }
}
