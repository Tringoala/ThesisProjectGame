using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Codex : MonoBehaviour
{
    [HideInInspector]
    public string codexNameEN;//incodex
    [HideInInspector]
    public string codexNameTR;//inhover
    [HideInInspector]
    public Sprite codexImage;//tick

    public GameObject onhoverGO;

    Text contentText;
    Image contentImage;
    private void Start()
    {
        contentText = gameObject.transform.GetChild(0).GetComponent<Text>();
        contentImage = gameObject.GetComponent<Image>();

        onhoverGO.GetComponentInChildren<Text>().text = codexNameTR;
        onhoverGO.SetActive(false);
    }
    private void Update()
    {
        contentImage.sprite = codexImage;
        contentText.text = codexNameEN;
        onhoverGO.GetComponentInChildren<Text>().text = codexNameTR;
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