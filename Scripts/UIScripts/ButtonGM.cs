using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGM : MonoBehaviour
{
    GameMech mech;
    public int buttonSort;
    // Start is called before the first frame update
    void Start()
    {
        mech = GameObject.Find("GameMechUI").GetComponent<GameMech>();
    }
    public void onclick(bool isWord)
    {
        if (isWord)
        {
//            mech.selectWord(buttonSort);
        }
        else
        {
//            mech.selectImage(buttonSort);
        }
    }
}
