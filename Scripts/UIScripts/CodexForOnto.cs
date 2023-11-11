using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CodexForOnto : MonoBehaviour
{
    [HideInInspector]
    public string codexNameEN;//incodex
    [HideInInspector]
    public string codexNameTR;//incodex
    [HideInInspector]
    public string codexDesc;//inhover

    public GameObject onhoverGO;

    Text contentText_EN;
    Text contentText_TR;
    private void Start()
    {
        contentText_EN = gameObject.transform.GetChild(0).GetComponent<Text>();
        contentText_TR = gameObject.transform.GetChild(1).GetComponent<Text>();
        onhoverGO.SetActive(false);
    }
    private void Update()
    {
        contentText_EN.text = codexNameEN;
        contentText_TR.text = codexNameTR;
        onhoverGO.GetComponentInChildren<Text>().text = codexDesc;
    }
    private void OnMouseEnter()
    {
        onhoverGO.SetActive(true);
    }
    private void OnMouseExit()
    {
        onhoverGO.SetActive(false);
    }
}