using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NonObjectWord
{
    public int id;
    public string nameTR;
    public string nameEN;
    public string desc;
    public bool isLearned;
    public bool isStudied;
    public bool contained;

    public NonObjectWord(NonObjectWord w)
    {
        id = w.id;
        nameTR = w.nameTR;
        nameEN = w.nameEN;
        desc = w.desc;
        isLearned = w.isLearned;
        isStudied = w.isStudied;
        contained = w.contained;
    }

}
